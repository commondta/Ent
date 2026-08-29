using System.Net;

namespace HRMS_Web.Extensions
{
    /// <summary>
    /// Reverse proxy: everything under <c>/lims</c> on this host is forwarded to the LIMS
    /// application (Laravel) so the whole ERP lives on ONE url and ONE cookie jar. The
    /// <c>erp_sso</c> cookie issued by the PMS login rides along with every request, which
    /// is how LIMS knows who the user is without a second login.
    /// Configuration: <c>Erp:LimsUpstream</c> (e.g. http://127.0.0.1:8000) and
    /// <c>Erp:LimsPrefix</c> (default /lims). Dependency-free (HttpClient only).
    /// </summary>
    public static class LimsProxyMiddleware
    {
        private static readonly HashSet<string> HopByHop = new(StringComparer.OrdinalIgnoreCase)
        {
            "Connection", "Keep-Alive", "Proxy-Authenticate", "Proxy-Authorization", "TE", "Trailer",
            "Transfer-Encoding", "Upgrade", "Host", "Accept-Encoding", "Content-Length"
        };

        public static IApplicationBuilder UseLimsProxy(this IApplicationBuilder app, IConfiguration cfg)
        {
            var upstream = cfg["Erp:LimsUpstream"];
            var prefix = cfg["Erp:LimsPrefix"] ?? "/lims";
            if (string.IsNullOrWhiteSpace(upstream)) return app;
            var upstreamUri = new Uri(upstream.TrimEnd('/'));

            var handler = new SocketsHttpHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.None,
                ConnectTimeout = TimeSpan.FromSeconds(10),
            };
            var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(5) };

            app.Map(prefix, branch => branch.Run(async ctx =>
            {
                var req = ctx.Request;
                var target = new UriBuilder(upstreamUri)
                {
                    Path = req.Path.HasValue ? req.Path.Value : "/",
                    Query = req.QueryString.HasValue ? req.QueryString.Value!.TrimStart('?') : ""
                }.Uri;

                using var msg = new HttpRequestMessage(new HttpMethod(req.Method), target);

                if (req.ContentLength > 0 || req.Headers.ContainsKey("Transfer-Encoding") ||
                    (!HttpMethods.IsGet(req.Method) && !HttpMethods.IsHead(req.Method) && !HttpMethods.IsDelete(req.Method) && !HttpMethods.IsOptions(req.Method)))
                {
                    msg.Content = new StreamContent(req.Body);
                    foreach (var h in req.Headers)
                        if (h.Key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase) && !HopByHop.Contains(h.Key))
                            msg.Content.Headers.TryAddWithoutValidation(h.Key, h.Value.ToArray());
                }
                foreach (var h in req.Headers)
                {
                    if (HopByHop.Contains(h.Key) || h.Key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase)) continue;
                    msg.Headers.TryAddWithoutValidation(h.Key, h.Value.ToArray());
                }
                msg.Headers.Host = upstreamUri.Authority;
                var originalHost = req.Host.Value;
                msg.Headers.TryAddWithoutValidation("X-Forwarded-Host", originalHost);
                msg.Headers.TryAddWithoutValidation("X-Forwarded-Proto", req.Scheme);
                msg.Headers.TryAddWithoutValidation("X-Forwarded-Prefix", prefix);
                msg.Headers.TryAddWithoutValidation("X-Forwarded-For", ctx.Connection.RemoteIpAddress?.ToString() ?? "");
                msg.Headers.TryAddWithoutValidation("X-Real-IP", ctx.Connection.RemoteIpAddress?.ToString() ?? "");

                HttpResponseMessage resp;
                try
                {
                    resp = await client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead, ctx.RequestAborted);
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    ctx.Response.StatusCode = 502;
                    ctx.Response.ContentType = "text/html; charset=utf-8";
                    await ctx.Response.WriteAsync("<!doctype html><meta charset=utf-8><title>LIMS unavailable</title><body style=\"font-family:Inter,Segoe UI,Arial,sans-serif;background:#fff;color:#111;padding:48px\"><h2 style=\"margin:0 0 8px\">Land Information Management System is not reachable</h2><p style=\"color:#4D4D4D\">The application service is not running. Start it and retry.</p><p><a href=\"/Apps\" style=\"color:#111\">&larr; Back to My Home</a></p></body>");
                    return;
                }

                using (resp)
                {
                    ctx.Response.StatusCode = (int)resp.StatusCode;
                    foreach (var h in resp.Headers)
                    {
                        if (HopByHop.Contains(h.Key)) continue;
                        ctx.Response.Headers[h.Key] = h.Value.ToArray();
                    }
                    foreach (var h in resp.Content.Headers)
                    {
                        if (HopByHop.Contains(h.Key)) continue;
                        ctx.Response.Headers[h.Key] = h.Value.ToArray();
                    }
                    // Absolute redirects to the internal upstream host come back onto this host under the prefix
                    if (ctx.Response.Headers.TryGetValue("Location", out var loc) && loc.Count > 0)
                    {
                        var l = loc[0] ?? "";
                        var internalRoot = upstreamUri.GetLeftPart(UriPartial.Authority);
                        if (l.StartsWith(internalRoot, StringComparison.OrdinalIgnoreCase))
                            ctx.Response.Headers["Location"] = prefix + l.Substring(internalRoot.Length);
                    }
                    ctx.Response.Headers.Remove("Transfer-Encoding");
                    await resp.Content.CopyToAsync(ctx.Response.Body, ctx.RequestAborted);
                }
            }));
            return app;
        }
    }
}

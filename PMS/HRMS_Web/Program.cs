using B_DB_Context;
using HRMS_Web;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs.SMSDTO;
using HRMS_Web.Services.AlertService;
using HRMS_Web.Services.BusinessServices;
using HRMS_Web.Services.BusinessServicesInterFace;
using HRMS_Web.Services.NotificationService;
using HRMS_Web.Services.PhotoService;
using HRMS_Web.Services.SMSService;
using HRMS_Web.Services.UploaderService;
using HRMS_Web.Services.ErpPlatform;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataBase_Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.CommandTimeout(180) 
    )
);


builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(9);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.Configure<SmsApiSettings>(builder.Configuration.GetSection("SmsApiSettings"));
builder.Services.AddHttpClient<ISMSService, SMSService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IUploaderService, UploaderService>();
builder.Services.AddTransient<IFeatures, BFeatures>();
builder.Services.AddTransient<IDealerCategory, BDealerCategory>();
builder.Services.AddTransient<IDealer, BDealer>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddScoped<ErpPlatformService>();   // ERP platform: central identity / SSO sessions


// � Normal login key
var loginKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Key"]));

// � Reset?flow key
var resetCfg = builder.Configuration.GetSection("ResetJwtSettings");
var resetKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(resetCfg["Key"]));

// � Two?factor key
var twoFaCfg = builder.Configuration.GetSection("TwoFactorJwtSettings");
var twoFaKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(twoFaCfg["Key"]));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "LoginScheme";
    options.DefaultChallengeScheme = "LoginScheme";
})

.AddJwtBearer("LoginScheme", opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = loginKey,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
})

.AddJwtBearer("ResetScheme", opt =>
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = resetKey,
        ValidateIssuer = true,
        ValidIssuer = resetCfg["Issuer"],
        ValidateAudience = true,
        ValidAudience = resetCfg["Audience"],
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = JwtRegisteredClaimNames.Sub
    };
    opt.SaveToken = true;
})

.AddJwtBearer("TwoFactorScheme", opt =>
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = twoFaKey,
        ValidateIssuer = true,
        ValidIssuer = twoFaCfg["Issuer"],
        ValidateAudience = true,
        ValidAudience = twoFaCfg["Audience"],
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = JwtRegisteredClaimNames.Sub
    };
    opt.SaveToken = true;
});


builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy("RequireResetScope", policy =>
        policy.AddAuthenticationSchemes("ResetScheme")
              .RequireAuthenticatedUser()
              .RequireClaim("scope", "reset"));

    options.AddPolicy("Require2FAScope", policy =>
        policy.AddAuthenticationSchemes("TwoFactorScheme")
              .RequireAuthenticatedUser()
              .RequireClaim("scope", "2fa"));
});

var app = builder.Build();

// Navigation/form registry (AI file.xlsx Instructions §5) — create + seed once, backfill
// permission rows for restored hidden forms. Idempotent; failure leaves legacy state intact.
using (var seedScope = app.Services.CreateScope())
{
    HRMS_Web.Extensions.NavigationRegistrySeeder.EnsureSeeded(seedScope.ServiceProvider);
}

// SECURITY: baseline response headers on everything, uploaded attachments included —
// stop MIME sniffing, framing by other origins, and referrer leakage.
// No CSP yet: the legacy views rely heavily on inline scripts.
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["Referrer-Policy"] = "same-origin";
    await next();
});

// ERP platform: the Land Information Management System is served on THIS host under /lims
// (reverse proxy to the Laravel app); the erp_sso cookie set at login rides along.
app.UseLimsProxy(app.Configuration);
// Payroll Management (ASP.NET MVC 5 on IIS Express) is served on THIS host under /payroll the same way.
app.UsePayrollProxy(app.Configuration);

var attachmentsFolderPath = app.Configuration.GetValue<string>("Attachments:AttachmentsFolderPath");

if (!Directory.Exists(attachmentsFolderPath))
{
    Directory.CreateDirectory(attachmentsFolderPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(attachmentsFolderPath),
    RequestPath = "/attachments"
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
AppSessionExtensions.Configure(app.Services.GetService<IHttpContextAccessor>());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

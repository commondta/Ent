namespace HRMS_Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class LoginViewModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ErrorMsg { get; set; }
        /// <summary>"Remember me" on the login form: keep the central SSO cookie across browser restarts (else it ends with the browser session).</summary>
        public bool RememberMe { get; set; }


    }
    public class LoginRequestModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

}
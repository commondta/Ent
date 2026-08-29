namespace HRMS_Web.Services.AlertService
{
    public interface IAlertService
    {
        Task<bool> PushAlert(int formId, string narration);
        bool GetNDC();
    }
}

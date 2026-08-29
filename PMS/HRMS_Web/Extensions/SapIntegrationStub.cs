using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models.DTOs.SAPDTO;

namespace HRMS_Web.Controllers.api
{
    /// <summary>
    /// Local stand-in for <c>SapIntegrationController</c>, compiled only when the build is
    /// made without SAP (<c>SapIntegration != true</c>, the default).
    ///
    /// The real controller depends on the SAP DI API, a COM component that must be installed
    /// and registered on the machine, and that only the .NET Framework MSBuild can resolve.
    /// That is why the solution built on exactly one PC. This file lets everything else
    /// compile and run anywhere.
    ///
    /// Every method is a no-op that returns an explicit "not available" result rather than a
    /// fake success, so nothing silently believes a document was posted to SAP. The real file
    /// is untouched on disk and comes back with /p:SapIntegration=true.
    ///
    /// Temporary. Task #35 replaces this with one interface plus a proper fake.
    /// </summary>
    public class SapIntegrationController
    {
        private const string Unavailable =
            "SAP integration is not available in this build. The SAP DI API is not installed " +
            "on this machine, so no document was posted.";

        private readonly DataBase_Context _db;

        public SapIntegrationController(DataBase_Context db) => _db = db;

        private static Response_Result NotAvailable() => new Response_Result
        {
            code = (int)ResponseCode.Error,
            message = Unavailable,
            data = null,
            secondData = null
        };

        public Response_Result MemberPosting(MemberProfile member) => NotAvailable();

        public Response_Result MemberUpdate(MemberProfile member) => NotAvailable();

        public Response_Result UpdatePropertyInMemberProfile(StockCreation member) => NotAvailable();

        public Response_Result UpdateMemberProfileToAddContactPerson(int stockId, int memberId) => NotAvailable();

        public Response_Result AddSAPStock(int stockID) => NotAvailable();

        public Response_Result AddServiceTypeInvoiceProcessingCharges(Booking booking, bool isUpdate) => NotAvailable();

        public Response_Result AddServiceTypeInvoiceBookingSchedule(Booking booking, bool isUpdate) => NotAvailable();

        public Response_Result PostingStandAloneARInvoice(StandAlone genralAdjustment) => NotAvailable();

        public Response_Result CancelInvoicesByChallan(string challanNo) => NotAvailable();

        public Response_Result PostingTransferRecieptSellerARInvoice(TransferReceiptProcessing transferReceipt) => NotAvailable();

        public Response_Result PostingTransferRecieptBuyerARInvoice(TransferReceiptProcessing transferReceipt) => NotAvailable();

        public Response_Result PostingARInvoiceForFileVerificationRequest(FileVerificationRequest file) => NotAvailable();

        public ApiResponse<object> PostingDemarcationARInvoice(NewDemarcationRequest dto) => new ApiResponse<object>
        {
            Code = ResponseCode.Error,
            Message = Unavailable,
            Data = null
        };
    }
}

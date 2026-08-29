using Microsoft.AspNetCore.Mvc;
using HRMS_Web.Models.DTOs.SAPDTO;
using B_Utility.Common;
using B_DB_Model;
using B_DB_Context;
using B_Utility.BLL;
using Microsoft.EntityFrameworkCore;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Extensions;
using Microsoft.VisualBasic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SapIntegrationController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;

        private const string SAP_SECURITY_KEY = "s3cR#T-9F2k!xQ7@LmP8vZ1#uN6rT4wY";

        public SapIntegrationController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        // SECURITY (#6): was [AllowAnonymous] — an unauthenticated caller could execute any
        // stored DynamicQueries template. Now requires the controller's [Authorize].
        [HttpPost]
        [Route("GenerateDynamicReport")]
        public IActionResult GenerateDynamicReport([FromBody] QueryRequest request)
        {
            try
            {
                var queryTemplate = _db.DynamicQueries.FirstOrDefault(x => x.Id == request.QueryId);

                if (queryTemplate == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "SQL template not found in DynamicQueries table",
                        Data = null
                    });
                }

                string rawQuery = queryTemplate.SqlQuery;

                foreach (var param in request.Parameters)
                {
                    var safeValue = param.Value?.Replace("'", "''");
                    rawQuery = rawQuery.Replace("{" + param.Key + "}", $"'{safeValue}'");
                }

                if (queryTemplate.Type == "SAP")
                {
                    SAPOperationDb sapconnection = new SAPOperationDb(_db);
                    sapconnection.ConnectToCompany();

                    if (sapconnection._a != 0)
                    {
                        var error = sapconnection.Ocomp.GetLastErrorDescription();
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Error,
                            Message = $"SAP Connection Error: {error}",
                            Data = null
                        });
                    }

                    var orecord = (SAPbobsCOM.Recordset)sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    orecord.DoQuery(rawQuery);

                    var resultList = new List<Dictionary<string, string>>();
                    while (!orecord.EoF)
                    {
                        var row = new Dictionary<string, string>();
                        for (int i = 0; i < orecord.Fields.Count; i++)
                        {
                            var field = orecord.Fields.Item(i);
                            row[field.Name] = Convert.ToString(field.Value);
                        }
                        resultList.Add(row);
                        orecord.MoveNext();
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Report generated successfully (SAP)",
                        Data = resultList
                    });
                }

                else if (queryTemplate.Type == "SQL/PMS")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = rawQuery;

                        cmd.CommandTimeout = 0;

                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        var resultList = new List<Dictionary<string, object>>();

                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            resultList.Add(row);
                        }

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Report generated successfully (SQL/PMS)",
                            Data = resultList
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Error,
                    Message = "Unsupported query type",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllDepartments")]

        public IActionResult GetAllDepartments()
        {


            List<Departs> Departments = new List<Departs>();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"Code\", T0.\"Name\" FROM OUDP T0";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            Departs Department = new Departs();

                            Department.Id = orecord.Fields.Item("Code").Value;
                            Department.Name = orecord.Fields.Item("Name").Value;
                            Departments.Add(Department);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = Departments
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllProjects")]

        public IActionResult GetAllProjects()
        {


            List<Projects> ProjectList = new List<Projects>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"PrjCode\", T0.\"PrjName\" FROM OPRJ T0 WHERE T0.\"Active\" ='Y'";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            Projects project = new Projects();

                            project.PrjCode = orecord.Fields.Item("PrjCode").Value;
                            project.PrjName = orecord.Fields.Item("PrjName").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetDemarcationClearanceData")]
        public IActionResult GetDemarcationClearanceData(string registrationNo)
        {


            List<TownPlanningCleareanceDTO> ProjectList = new List<TownPlanningCleareanceDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                var accounts = _db.SAPOperations.FirstOrDefault().TownPlanningClearanceCommaSepratedGLs;

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string str =
                    "SELECT " +
                    " T1.\"Dscription\", " +

                    " T1.\"DocEntry\", " +
                    " T0.\"PaidToDate\", " +
                    " T1.\"AcctCode\", " +
                    " T1.\"LineNum\", " +
                    " T0.\"Project\", " +
                    " T2.\"AcctName\", " +
                    " T0.\"DocNum\", " +
                    " T2.\"AccntntCod\", " +
                    " T0.\"DocTotal\", " +
                    " (T0.\"DocTotal\" - T0.\"PaidToDate\") AS \"BalanceDue\", " +
                    " ( " +
                    "   SELECT STRING_AGG(A.\"DocNum\", ',') " +
                    "   FROM \"ORCT\" A " +
                    "   INNER JOIN \"RCT2\" B ON A.\"DocEntry\" = B.\"DocNum\" " +
                    //"   WHERE B.\"InvType\" = '13' " +
                    "   AND B.\"DocEntry\" = T0.\"DocEntry\" " +
                    " ) AS \"ReceiptNum\" " +

                    "FROM \"OINV\" T0 " +
                    "INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                    "INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" " +

                    "WHERE " +
                    " T0.\"Project\" = '" + registrationNo + "' " +
                    " AND T0.\"CANCELED\" = 'N' " +
                    " AND T1.\"AcctCode\" IN (" + accounts + ")";


                    orecord.DoQuery(str);

                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            TownPlanningCleareanceDTO project = new TownPlanningCleareanceDTO();

                            project.ChargeType = orecord.Fields.Item("Dscription").Value;
                            project.DocEntry = orecord.Fields.Item("DocEntry").Value;
                            project.LineNum = orecord.Fields.Item("LineNum").Value;
                            project.AcctCode = orecord.Fields.Item("AcctCode").Value;
                            project.Project = orecord.Fields.Item("Project").Value;
                            project.AcctName = orecord.Fields.Item("AcctName").Value;
                            project.DocNum = orecord.Fields.Item("DocNum").Value;
                            project.AccntntCod = orecord.Fields.Item("AccntntCod").Value == null ? "" : orecord.Fields.Item("AccntntCod").Value;
                            project.DocTotal = orecord.Fields.Item("DocTotal").Value == null
                            ? 0
                            : Convert.ToDecimal(orecord.Fields.Item("DocTotal").Value);

                            project.PaidToDate = orecord.Fields.Item("PaidToDate").Value == null
                                ? 0
                                : Convert.ToDecimal(orecord.Fields.Item("PaidToDate").Value);
                            project.BalanceDue = Convert.ToDecimal(orecord.Fields.Item("BalanceDue").Value);

                            project.ReceiptNum = orecord.Fields.Item("ReceiptNum").Value == null ? "" : orecord.Fields.Item("ReceiptNum").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against registration No " + registrationNo,
                            Data = null
                        });
                    }
                }
                else
                {
                    var exc = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:";

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = exc,
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetCleareancePaidData")]
        public IActionResult GetCleareancePaidData(string registrationNo, string? cnic = null)
        {
            try
            {
                //var id = HttpContext.Session.GetString("ID");
                //if (!_db.StockCreations.Any(x => x.RegistrationNo == registrationNo && x.MemberProfileId == Convert.ToInt32(id)))
                //{
                //    return BadRequest("InvalidRequest.");
                //}

                // =========================
                // SECURITY CHECK
                // =========================
                var headerKey = Request.Headers["X-SAP-KEY"].ToString();

                if (string.IsNullOrEmpty(headerKey) || headerKey != SAP_SECURITY_KEY)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = "Invalid security key",
                        Data = null
                    });
                }
                if (!string.IsNullOrEmpty(cnic))
                {
                    if (!_db.StockCreations.Any(x => x.RegistrationNo == registrationNo && x.MemberProfile.Cnic == cnic))
                    {
                        return BadRequest("Invalid Request.");
                    }
                }

                    SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string query = $@"
SELECT
    f.""DocEntry"",
    f.""InvoiceNo"",
    f.""DueDate"",
    f.""Account Code"",
    f.""Account Name"",
    f.""Description"",
    f.""InvoiceAmount"",
    f.""BalanceOwed"",
    f.""SettleDocNum"",
    f.""SettleDate"",
    f.""ReceivedAmount"",
    f.""Adjustment"",
    f.""BalanceDue"",
    f.""Payment_Method"",
    f.""Surcharge"",

    GREATEST(
        CASE 
            WHEN f.""Surcharge"" <= 0 THEN 0
            WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
            WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
            ELSE 0
        END, 0
    ) AS ""Waiver Paid"",

    GREATEST(
        CASE 
            WHEN f.""Surcharge"" <= 0 THEN 0
            WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
            ELSE LEAST(
                f.""Surcharge"" - 
                CASE 
                    WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                        THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                    ELSE 0
                END,
                (f.""Waiver"" + f.""Surcharge Payment"") - f.""RunningSurcharge"" + f.""Surcharge""
            )
        END, 0
    ) AS ""Surcharge Payment Paid"",

    (
        (f.""BalanceDue"" + f.""Surcharge"") -
        (
            GREATEST(
                CASE
                    WHEN f.""Surcharge"" <= 0 THEN 0
                    WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
                    WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                        THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                    ELSE 0
                END, 0
            )
            +
            GREATEST(
                CASE 
                    WHEN f.""Surcharge"" <= 0 THEN 0
                    WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
                    ELSE LEAST(
                        f.""Surcharge"" -
                        CASE
                            WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                                THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                            ELSE 0
                        END,
                        (f.""Waiver"" + f.""Surcharge Payment"") - f.""RunningSurcharge"" + f.""Surcharge""
                    )
                END, 0
            )
        )
    ) AS ""Total Amount Due""

FROM (
    SELECT base.*,
        SUM(base.""Surcharge"") OVER (
            ORDER BY base.""Order"", base.""DocEntry"", base.SortOrder
            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
        ) AS ""RunningSurcharge""
    FROM (
        SELECT
            ""DocEntry"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceNum"" ELSE NULL END AS ""InvoiceNo"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""DueDate"" ELSE NULL END AS ""DueDate"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Account Code"" ELSE NULL END AS ""Account Code"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Account Name"" ELSE NULL END AS ""Account Name"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Description"" ELSE NULL END AS ""Description"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceAmount"" ELSE NULL END AS ""InvoiceAmount"",
            ""BalanceOwed"",
            ""SettleDocNum"",
            ""SettleDate"",
            ""ReceivedAmount"",
            ""Adjustment"",
            ""BalanceDue"",
            ""Payment_Method"",

            IFNULL(
                (SELECT FLOOR(SUM(x.""Surcharge""))
                 FROM ""Surcharge_Invoice2"" x
                 WHERE x.""DocEntry"" = T.""DocEntry""
                   AND x.""CardCode"" = T.""CardCode""
                   AND x.""SettleDocNum"" = T.""SettleDocNum""),
                0
            ) AS ""Surcharge"",

            IFNULL(
                (SELECT C.""U_waiv""
                 FROM ""OCRD"" C
                 WHERE C.""CardCode"" = T.""CardCode""),
                0
            ) AS ""Waiver"",

            IFNULL(
                (SELECT SUM(P.""Payment"")
                 FROM ""Surcharge_Payments"" P
                 WHERE P.""CardCode"" = T.""CardCode""),
                0
            ) AS ""Surcharge Payment"",

            SortOrder,
            ""Order""

        FROM (
            SELECT
                A.""CardCode"",
                A.""DocEntry"",
                A.""InvoiceNum"",
                A.""DueDate"",
                A.""Account Code"",
                A.""Account Name"",
                A.""Description"",
                A.""InvoiceAmount"",
                A.""BalanceOwed"",
                A.""SettleDocNum"",
                A.""SettleDate"",
                A.""ReceivedAmount"",
                A.""Adjustment"",
                A.""BalanceDue"",
                A.""RN"" AS SortOrder,
                0 AS IsSummary,
                A.""Order"",
                A.""Payment_Method""
            FROM ""Invoice_Summary"" A
            WHERE A.""CardCode"" = '{registrationNo.Trim()}'
              AND EXISTS (
                    SELECT 1
                    FROM ""Invoice_Summary"" P
                    WHERE P.""DocEntry"" = A.""DocEntry""
                      AND P.""CardCode"" = A.""CardCode""
                      AND (
                        P.""ReceivedAmount"" > 0
                        OR COALESCE(P.""Adjustment"", 0) > 0
                        OR P.""BalanceOwed"" > P.""BalanceDue""
                      )
                )

            UNION ALL

            SELECT
                S.""CardCode"",
                S.""DocEntry"",
                0 AS ""InvoiceNum"",
                NULL AS ""DueDate"",
                NULL AS ""Account Code"",
                NULL AS ""Account Name"",
                'Balance Owed' AS ""Description"",
                0 AS ""InvoiceAmount"",
                MAX(S.""BalanceDue"") AS ""BalanceOwed"",
                0 AS ""SettleDocNum"",
                NULL AS ""SettleDate"",
                0 AS ""ReceivedAmount"",
                0 AS ""Adjustment"",
                MAX(S.""BalanceDue"") AS ""BalanceDue"",
                9999 AS SortOrder,
                1 AS IsSummary,
                MAX(S.""Order"") AS ""Order"",
                MAX(S.""Payment_Method"") AS ""Payment_Method""
            FROM ""Invoice_Summary"" S
            WHERE S.""CardCode"" = '{registrationNo.Trim()}'
              AND S.""RN"" = (
                    SELECT MAX(X.""RN"")
                    FROM ""Invoice_Summary"" X
                    WHERE X.""DocEntry"" = S.""DocEntry""
                )
              AND S.""BalanceDue"" > 0
              AND EXISTS (
                    SELECT 1
                    FROM ""Invoice_Summary"" P
                    WHERE P.""DocEntry"" = S.""DocEntry""
                      AND P.""CardCode"" = S.""CardCode""
                      AND (
                        P.""ReceivedAmount"" > 0
                        OR COALESCE(P.""Adjustment"", 0) > 0
                        OR P.""BalanceOwed"" > P.""BalanceDue""
                      )
                )
            GROUP BY S.""DocEntry"", S.""CardCode""
        ) T
    ) base
) f
ORDER BY f.""Order"", f.""DocEntry"", f.SortOrder";


                    orecord.DoQuery(query);

                    List<InvoiceSurchargeReportDto> rawList = new List<InvoiceSurchargeReportDto>();

                    while (!orecord.EoF)
                    {
                        var dto = new InvoiceSurchargeReportDto
                        {
                            DocEntry = Convert.ToInt32(orecord.Fields.Item("DocEntry").Value),

                            InvoiceNo = orecord.Fields.Item("InvoiceNo").Value?.ToString(),

                            DueDate = ParseSapDate(orecord.Fields.Item("DueDate").Value),

                            AccountCode = orecord.Fields.Item("Account Code").Value?.ToString(),

                            AccountName = orecord.Fields.Item("Account Name").Value?.ToString(),

                            Description = orecord.Fields.Item("Description").Value?.ToString(),

                            InvoiceAmount = SafeDecimal(orecord.Fields.Item("InvoiceAmount").Value),

                            BalanceOwed = SafeDecimal(orecord.Fields.Item("BalanceOwed").Value),

                            SettleDocNum = orecord.Fields.Item("SettleDocNum").Value == null
                                ? (int?)null
                                : Convert.ToInt32(orecord.Fields.Item("SettleDocNum").Value),

                            SettleDate = ParseSapDate(orecord.Fields.Item("SettleDate").Value),

                            ReceivedAmount = SafeDecimal(orecord.Fields.Item("ReceivedAmount").Value),

                            Adjustment = SafeDecimal(orecord.Fields.Item("Adjustment").Value),

                            BalanceDue = SafeDecimal(orecord.Fields.Item("BalanceDue").Value),

                            PaymentMethod = orecord.Fields.Item("Payment_Method").Value?.ToString(),

                            Surcharge = SafeDecimal(orecord.Fields.Item("Surcharge").Value),

                            WaiverPaid = SafeDecimal(orecord.Fields.Item("Waiver Paid").Value),

                            SurchargePaymentPaid = SafeDecimal(orecord.Fields.Item("Surcharge Payment Paid").Value),

                            TotalAmountDue = SafeDecimal(orecord.Fields.Item("Total Amount Due").Value)
                        };

                        rawList.Add(dto);
                        orecord.MoveNext();
                    }



                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "",
                        Data = rawList
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription() + " Local System Error",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetCleareanceData")]
        public IActionResult GetCleareanceData(string registrationNo, int? memberProfileId = null)
        {
            try
            {
                //var id = HttpContext.Session.GetString("ID");
                //if (!_db.StockCreations.Any(x => x.RegistrationNo == registrationNo && x.MemberProfileId == Convert.ToInt32(id)))
                //{
                //    return BadRequest("InvalidRequest.");
                //}

                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string query = $@"
SELECT
    f.""DocEntry"",
    f.""InvoiceNo"",
    f.""DueDate"",
    f.""Account Code"",
    f.""Account Name"",
    f.""Description"",
    f.""InvoiceAmount"",
    f.""BalanceOwed"",
    f.""SettleDocNum"",
    f.""SettleDate"",
    f.""ReceivedAmount"",
    f.""Adjustment"",
    f.""BalanceDue"",
    f.""Payment_Method"",
    f.""Surcharge"",

    GREATEST(
        CASE 
            WHEN f.""Surcharge"" <= 0 THEN 0
            WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
            WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
            ELSE 0
        END, 0
    ) AS ""Waiver Paid"",

    GREATEST(
        CASE 
            WHEN f.""Surcharge"" <= 0 THEN 0
            WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
            ELSE LEAST(
                f.""Surcharge"" - 
                CASE 
                    WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                        THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                    ELSE 0
                END,
                (f.""Waiver"" + f.""Surcharge Payment"") - f.""RunningSurcharge"" + f.""Surcharge""
            )
        END, 0
    ) AS ""Surcharge Payment Paid"",

    (
        (f.""BalanceDue"" + f.""Surcharge"") -
        (
            GREATEST(
                CASE
                    WHEN f.""Surcharge"" <= 0 THEN 0
                    WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
                    WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                        THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                    ELSE 0
                END, 0
            )
            +
            GREATEST(
                CASE 
                    WHEN f.""Surcharge"" <= 0 THEN 0
                    WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
                    ELSE LEAST(
                        f.""Surcharge"" -
                        CASE
                            WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                                THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                            ELSE 0
                        END,
                        (f.""Waiver"" + f.""Surcharge Payment"") - f.""RunningSurcharge"" + f.""Surcharge""
                    )
                END, 0
            )
        )
    ) AS ""Total Amount Due""

FROM (
    SELECT base.*,
        SUM(base.""Surcharge"") OVER (
            ORDER BY base.""Order"", base.""DocEntry"", base.SortOrder
            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
        ) AS ""RunningSurcharge""
    FROM (
        SELECT
            ""DocEntry"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceNum"" ELSE NULL END AS ""InvoiceNo"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""DueDate"" ELSE NULL END AS ""DueDate"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Account Code"" ELSE NULL END AS ""Account Code"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Account Name"" ELSE NULL END AS ""Account Name"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Description"" ELSE NULL END AS ""Description"",
            CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceAmount"" ELSE NULL END AS ""InvoiceAmount"",
            ""BalanceOwed"",
            ""SettleDocNum"",
            ""SettleDate"",
            ""ReceivedAmount"",
            ""Adjustment"",
            ""BalanceDue"",
            ""Payment_Method"",

            IFNULL(
                (SELECT FLOOR(SUM(x.""Surcharge""))
                 FROM ""Surcharge_Invoice2"" x
                 WHERE x.""DocEntry"" = T.""DocEntry""
                   AND x.""CardCode"" = T.""CardCode""
                   AND x.""SettleDocNum"" = T.""SettleDocNum""),
                0
            ) AS ""Surcharge"",

            IFNULL(
                (SELECT C.""U_waiv""
                 FROM ""OCRD"" C
                 WHERE C.""CardCode"" = T.""CardCode""),
                0
            ) AS ""Waiver"",

            IFNULL(
                (SELECT SUM(P.""Payment"")
                 FROM ""Surcharge_Payments"" P
                 WHERE P.""CardCode"" = T.""CardCode""),
                0
            ) AS ""Surcharge Payment"",

            SortOrder,
            ""Order""

        FROM (
            SELECT
                A.""CardCode"",
                A.""DocEntry"",
                A.""InvoiceNum"",
                A.""DueDate"",
                A.""Account Code"",
                A.""Account Name"",
                A.""Description"",
                A.""InvoiceAmount"",
                A.""BalanceOwed"",
                A.""SettleDocNum"",
                A.""SettleDate"",
                A.""ReceivedAmount"",
                A.""Adjustment"",
                A.""BalanceDue"",
                A.""RN"" AS SortOrder,
                0 AS IsSummary,
                A.""Order"",
                A.""Payment_Method""
            FROM ""Invoice_Summary"" A
            WHERE A.""CardCode"" = '{registrationNo.Trim()}'

            UNION ALL

            SELECT
                S.""CardCode"",
                S.""DocEntry"",
                0 AS ""InvoiceNum"",
                NULL AS ""DueDate"",
                NULL AS ""Account Code"",
                NULL AS ""Account Name"",
                'Balance Owed' AS ""Description"",
                0 AS ""InvoiceAmount"",
                MAX(S.""BalanceDue"") AS ""BalanceOwed"",
                0 AS ""SettleDocNum"",
                NULL AS ""SettleDate"",
                0 AS ""ReceivedAmount"",
                0 AS ""Adjustment"",
                MAX(S.""BalanceDue"") AS ""BalanceDue"",
                9999 AS SortOrder,
                1 AS IsSummary,
                MAX(S.""Order"") AS ""Order"",
                MAX(S.""Payment_Method"") AS ""Payment_Method""
            FROM ""Invoice_Summary"" S
            WHERE S.""CardCode"" = '{registrationNo.Trim()}'
              AND S.""RN"" = (
                    SELECT MAX(X.""RN"")
                    FROM ""Invoice_Summary"" X
                    WHERE X.""DocEntry"" = S.""DocEntry""
                )
              AND S.""BalanceDue"" > 0
              AND EXISTS (
                    SELECT 1
                    FROM ""Invoice_Summary"" P
                    WHERE P.""DocEntry"" = S.""DocEntry""
                      AND (P.""ReceivedAmount"" > 0 OR COALESCE(P.""Adjustment"",0) > 0)
                )
            GROUP BY S.""DocEntry"", S.""CardCode""
        ) T
    ) base
) f
ORDER BY f.""Order"", f.""DocEntry"", f.SortOrder";


                    orecord.DoQuery(query);

                    List<InvoiceSurchargeReportDto> rawList = new List<InvoiceSurchargeReportDto>();

                    while (!orecord.EoF)
                    {
                        var dto = new InvoiceSurchargeReportDto
                        {
                            DocEntry = Convert.ToInt32(orecord.Fields.Item("DocEntry").Value),

                            InvoiceNo = orecord.Fields.Item("InvoiceNo").Value?.ToString(),

                            DueDate = ParseSapDate(orecord.Fields.Item("DueDate").Value),

                            AccountCode = orecord.Fields.Item("Account Code").Value?.ToString(),

                            AccountName = orecord.Fields.Item("Account Name").Value?.ToString(),

                            Description = orecord.Fields.Item("Description").Value?.ToString(),

                            InvoiceAmount = SafeDecimal(orecord.Fields.Item("InvoiceAmount").Value),

                            BalanceOwed = SafeDecimal(orecord.Fields.Item("BalanceOwed").Value),

                            SettleDocNum = orecord.Fields.Item("SettleDocNum").Value == null
                                ? (int?)null
                                : Convert.ToInt32(orecord.Fields.Item("SettleDocNum").Value),

                            SettleDate = ParseSapDate(orecord.Fields.Item("SettleDate").Value),

                            ReceivedAmount = SafeDecimal(orecord.Fields.Item("ReceivedAmount").Value),

                            Adjustment = SafeDecimal(orecord.Fields.Item("Adjustment").Value),

                            BalanceDue = SafeDecimal(orecord.Fields.Item("BalanceDue").Value),

                            PaymentMethod = orecord.Fields.Item("Payment_Method").Value?.ToString(),

                            Surcharge = SafeDecimal(orecord.Fields.Item("Surcharge").Value),

                            WaiverPaid = SafeDecimal(orecord.Fields.Item("Waiver Paid").Value),

                            SurchargePaymentPaid = SafeDecimal(orecord.Fields.Item("Surcharge Payment Paid").Value),

                            TotalAmountDue = SafeDecimal(orecord.Fields.Item("Total Amount Due").Value)
                        };

                        rawList.Add(dto);
                        orecord.MoveNext();
                    }



                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "",
                        Data = rawList
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription() + " Local System Error",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetCleareanceSummary")]
        public IActionResult GetCleareanceSummary(string registrationNo, int memberProfileId)
        {
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription() + " Local System Error",
                        Data = null
                    });
                }

                SAPbobsCOM.Recordset orecord =
                    sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string query = $@"
SELECT 
    q.""Account Code"",
    (SELECT x.""AcctName"" 
     FROM OACT x 
     WHERE x.""AcctCode"" = q.""Account Code"") AS ""Charge Type"",

    SUM(q.""InvoiceAmount"") AS ""Balance Owed"",
    SUM(q.""ReceivedAmount"") AS ""Received Amount"",
    SUM(q.""Adjustment"") AS ""Adjustment Amount"",

    SUM(q.""InvoiceAmount"") - (SUM(q.""ReceivedAmount"") + SUM(q.""Adjustment"")) AS ""Balance Due"",
    SUM(q.""Surcharge"") AS ""Surcharge"",
    SUM(q.""Waiver Paid"") AS ""WaiverOff"",
    SUM(q.""Surcharge Payment Paid"") AS ""Surcharge Recieved Applied"",

    (
        (SUM(q.""InvoiceAmount"") - (SUM(q.""ReceivedAmount"") + SUM(q.""Adjustment""))) 
        + SUM(q.""Surcharge"")
    )
    - (SUM(q.""Waiver Paid"") + SUM(q.""Surcharge Payment Paid"")) AS ""Total Amount Due""

FROM
(
    SELECT 
        f.""Account Code"",
        f.""Account Name"",
        f.""InvoiceAmount"",
        f.""ReceivedAmount"",
        f.""Adjustment"",
        f.""Surcharge"",

        GREATEST(
            CASE 
                WHEN f.""Surcharge"" <= 0 THEN 0
                WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
                WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                    THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                ELSE 0
            END, 0
        ) AS ""Waiver Paid"",

        GREATEST(
            CASE 
                WHEN f.""Surcharge"" <= 0 THEN 0
                WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
                ELSE LEAST(
                    f.""Surcharge"" - 
                    CASE 
                        WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                            THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                        ELSE 0
                    END,
                    (f.""Waiver"" + f.""Surcharge Payment"") 
                    - f.""RunningSurcharge"" + f.""Surcharge""
                )
            END, 0
        ) AS ""Surcharge Payment Paid"",

        (
            (f.""BalanceDue"" + f.""Surcharge"") 
            - (
                GREATEST(
                    CASE 
                        WHEN f.""Surcharge"" <= 0 THEN 0
                        WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN f.""Surcharge""
                        WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                            THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                        ELSE 0
                    END, 0
                )
                +
                GREATEST(
                    CASE 
                        WHEN f.""Surcharge"" <= 0 THEN 0
                        WHEN f.""RunningSurcharge"" <= f.""Waiver"" THEN 0
                        ELSE LEAST(
                            f.""Surcharge"" -
                            CASE 
                                WHEN f.""RunningSurcharge"" - f.""Surcharge"" < f.""Waiver""
                                    THEN f.""Waiver"" - (f.""RunningSurcharge"" - f.""Surcharge"")
                                ELSE 0
                            END,
                            (f.""Waiver"" + f.""Surcharge Payment"") 
                            - f.""RunningSurcharge"" + f.""Surcharge""
                        )
                    END, 0
                )
            )
        ) AS ""Total Amount Due""

    FROM
    (
        SELECT base.*,
            SUM(base.""Surcharge"") OVER (
                ORDER BY base.""Order"", base.""DocEntry"", base.SortOrder
                ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
            ) AS ""RunningSurcharge""
        FROM
        (
            SELECT 
                ""DocEntry"",
                CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceNum"" ELSE NULL END AS ""InvoiceNo"",
                CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""DueDate"" ELSE NULL END AS ""DueDate"",
                ""Account Code"",
                ""Account Name"",
                CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Description"" ELSE NULL END AS ""Description"",
                CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceAmount"" ELSE NULL END AS ""InvoiceAmount"",
                ""BalanceOwed"",
                ""SettleDocNum"",
                ""SettleDate"",
                ""ReceivedAmount"",
                ""Adjustment"",
                ""BalanceDue"",
                ""Payment_Method"",

                IFNULL(
                    (SELECT ROUND(SUM(x.""Surcharge""), 0)
                     FROM ""Surcharge_Invoice2"" x
                     WHERE x.""DocEntry"" = T.""DocEntry""
                       AND x.""CardCode"" = T.""CardCode""
                       AND x.""SettleDocNum"" = T.""SettleDocNum""),
                    0
                ) AS ""Surcharge"",

                IFNULL(
                    (SELECT C.""U_waiv"" FROM ""OCRD"" C WHERE C.""CardCode"" = T.""CardCode""),
                    0
                ) AS ""Waiver"",

                IFNULL(
                    (SELECT SUM(P.""Payment"") 
                     FROM ""Surcharge_Payments"" P 
                     WHERE P.""CardCode"" = T.""CardCode""),
                    0
                ) AS ""Surcharge Payment"",

                SortOrder,
                ""Order""

            FROM
            (
                SELECT 
                    A.""CardCode"",
                    A.""DocEntry"",
                    A.""InvoiceNum"",
                    A.""DueDate"",
                    A.""Account Code"",
                    A.""Account Name"",
                    A.""Description"",
                    A.""InvoiceAmount"",
                    A.""BalanceOwed"",
                    A.""SettleDocNum"",
                    A.""SettleDate"",
                    A.""ReceivedAmount"",
                    A.""Adjustment"",
                    A.""BalanceDue"",
                    A.""RN"" AS SortOrder,
                    0 AS IsSummary,
                    A.""Order"",
                    A.""Payment_Method""
                FROM ""Invoice_Summary"" A
                WHERE A.""CardCode"" = '{registrationNo.Trim()}'

                UNION ALL

                SELECT 
                    S.""CardCode"",
                    S.""DocEntry"",
                    0 AS ""InvoiceNum"",
                    NULL AS ""DueDate"",
                    S.""Account Code"",
                    S.""Account Name"",
                    'Balance Owed' AS ""Description"",
                    0 AS ""InvoiceAmount"",
                    MAX(S.""BalanceDue"") AS ""BalanceOwed"",
                    0 AS ""SettleDocNum"",
                    NULL AS ""SettleDate"",
                    0 AS ""ReceivedAmount"",
                    0 AS ""Adjustment"",
                    MAX(S.""BalanceDue"") AS ""BalanceDue"",
                    9999 AS SortOrder,
                    1 AS IsSummary,
                    MAX(S.""Order"") AS ""Order"",
                    MAX(S.""Payment_Method"") AS ""Payment_Method""
                FROM ""Invoice_Summary"" S
                WHERE S.""CardCode"" = '{registrationNo.Trim()}'
                  AND S.""RN"" = (
                        SELECT MAX(X.""RN"") 
                        FROM ""Invoice_Summary"" X 
                        WHERE X.""DocEntry"" = S.""DocEntry""
                  )
                  AND S.""BalanceDue"" > 0
                  AND EXISTS (
                        SELECT 1 
                        FROM ""Invoice_Summary"" P 
                        WHERE P.""DocEntry"" = S.""DocEntry"" 
                          AND (P.""ReceivedAmount"" > 0 OR COALESCE(P.""Adjustment"",0) > 0)
                  )
                GROUP BY 
                    S.""DocEntry"",
                    S.""CardCode"",
                    S.""Account Code"",
                    S.""Account Name""
            ) T
        ) base
    ) f
) q
WHERE q.""Account Code"" IS NOT NULL
GROUP BY q.""Account Code"";
";

                orecord.DoQuery(query);

                List<ClearanceSummaryDto> summaryList = new();

                while (!orecord.EoF)
                {
                    var dto = new ClearanceSummaryDto
                    {
                        AccountCode = orecord.Fields.Item("Account Code").Value?.ToString(),

                        ChargeType = orecord.Fields.Item("Charge Type").Value?.ToString(),

                        BalanceOwed = SafeDecimal(orecord.Fields.Item("Balance Owed").Value),

                        ReceivedAmount = SafeDecimal(orecord.Fields.Item("Received Amount").Value),

                        AdjustmentAmount = SafeDecimal(orecord.Fields.Item("Adjustment Amount").Value),

                        BalanceDue = SafeDecimal(orecord.Fields.Item("Balance Due").Value),

                        Surcharge = SafeDecimal(orecord.Fields.Item("Surcharge").Value),

                        WaiverOff = SafeDecimal(orecord.Fields.Item("WaiverOff").Value),

                        SurchargeReceivedApplied =
                            SafeDecimal(orecord.Fields.Item("Surcharge Recieved Applied").Value),

                        TotalAmountDue =
                            SafeDecimal(orecord.Fields.Item("Total Amount Due").Value)
                    };

                    summaryList.Add(dto);
                    orecord.MoveNext();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "",
                    Data = summaryList
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        private static decimal SafeDecimal(object value)
        {
            if (value == null)
                return 0m;

            // SAP sometimes returns double.NaN
            if (value is double d)
                return double.IsNaN(d) ? 0m : Convert.ToDecimal(d);

            if (value is float f)
                return float.IsNaN(f) ? 0m : Convert.ToDecimal(f);

            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0m;
            }
        }


        private DateTime? ParseSapDate(object value)
        {
            if (DateTime.TryParse(value?.ToString(), out var parsedDate))
            {
                // SAP dummy date (30-12-1899) should be treated as null
                if (parsedDate == new DateTime(1899, 12, 30))
                    return null;

                return parsedDate;
            }
            return null;
        }

        [HttpGet]
        [Route("GetCleareanceDatas")]
        public IActionResult GetCleareanceDatas(string registrationNo, int memberProfileId)
        {
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string query = $@"
                SELECT
                    CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceNo"" ELSE NULL END AS ""InvoiceNo"",
                    --CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""DocDueDate"" ELSE NULL END AS ""DueDate"",
                    ""DocDueDate"" as  ""DueDate"",
                    CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""AcctCode"" ELSE NULL END AS ""Account Code"",
                    CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""AcctName"" ELSE NULL END AS ""Account Name"",
                    CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""Dscription"" ELSE NULL END AS ""Description"",
                    CASE WHEN IsSummary = 0 AND SortOrder = 1 THEN ""InvoiceAmount"" ELSE NULL END AS ""InvoiceAmount"",
                    ""BalanceOwed"", ""SettleDocNum"", ""SettleDate"", ""ReceivedAmount"", ""Adjustment"", ""BalanceDue""
                FROM (
                    SELECT ""DocEntry"", ""InvoiceNo"", ""DocDueDate"", ""AcctCode"", ""AcctName"", ""Dscription"", ""InvoiceAmount"",
                        CASE 
                            WHEN RowNum = 1 THEN ""InvoiceAmount""
                            ELSE LAG(""InvoiceAmount"" - BalanceDue) OVER (PARTITION BY ""DocEntry"" ORDER BY RowNum)
                        END AS ""BalanceOwed"",
                        ""SettleDocNum"", ""SettleDate"", ""ReceivedAmount"", ""Adjustment"",
                        ""InvoiceAmount"" - BalanceDue AS ""BalanceDue"",
                        RowNum AS SortOrder, 0 AS IsSummary, ""Order""
                    FROM V_INVOICE
                    WHERE ""CardCode"" = '{registrationNo.Trim()}'

                    UNION ALL

                    SELECT ""DocEntry"", NULL, NULL, NULL, NULL, NULL, MAX(""InvoiceAmount""),
                        MAX(""InvoiceAmount"") - (SUM(""ReceivedAmount"") + SUM(COALESCE(""Adjustment"", 0))) AS ""BalanceOwed"",
                        NULL, NULL, NULL, NULL,
                        MAX(""InvoiceAmount"") - (SUM(""ReceivedAmount"") + SUM(COALESCE(""Adjustment"", 0))) AS ""BalanceDue"",
                        9999, 1, MAX(""Order"")
                    FROM (
                        SELECT DISTINCT ""DocEntry"", ""InvoiceAmount"", ""ReceivedAmount"", COALESCE(""Adjustment"", 0) AS ""Adjustment"", BalanceDue, ""Order""
                        FROM V_INVOICE
                        WHERE ""CardCode"" = '{registrationNo.Trim()}'
                    ) AS DistinctRows
                    GROUP BY ""DocEntry"", ""InvoiceAmount""
                    HAVING MAX(""InvoiceAmount"") - (SUM(""ReceivedAmount"") + SUM(""Adjustment"")) > 0
                ) AS S
                ORDER BY ""Order"", ""DocEntry"", SortOrder";

                    orecord.DoQuery(query);

                    List<NewClearanceDTO> rawList = new List<NewClearanceDTO>();

                    while (!orecord.EoF)
                    {
                        var dto = new NewClearanceDTO
                        {
                            InvoiceNo = orecord.Fields.Item("InvoiceNo").Value?.ToString(),
                            DueDate = ParseSapDate(orecord.Fields.Item("DueDate").Value),
                            AccountCode = orecord.Fields.Item("Account Code").Value?.ToString(),
                            AccountName = orecord.Fields.Item("Account Name").Value?.ToString(),
                            Description = orecord.Fields.Item("Description").Value?.ToString(),
                            InvoiceAmount = Convert.ToDecimal(orecord.Fields.Item("InvoiceAmount").Value ?? 0),
                            BalanceOwed = Convert.ToDecimal(orecord.Fields.Item("BalanceOwed").Value ?? 0),
                            SettleDocNum = orecord.Fields.Item("SettleDocNum").Value?.ToString(),
                            SettleDate = ParseSapDate(orecord.Fields.Item("SettleDate").Value),
                            ReceivedAmount = Convert.ToDecimal(orecord.Fields.Item("ReceivedAmount").Value ?? 0),
                            Adjustment = Convert.ToDecimal(orecord.Fields.Item("Adjustment").Value ?? 0),
                            BalanceDue = Convert.ToDecimal(orecord.Fields.Item("BalanceDue").Value ?? 0)
                        };

                        rawList.Add(dto);
                        orecord.MoveNext();
                    }

                    decimal surchargePaid = GetPaidSurcharge(registrationNo, sapconnection.Ocomp);
                    var surchargeListDTO = GetSurchargeList();
                    var surchargeList = surchargeListDTO.Select(x => new SurchargeSetup
                    {
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        TotalSurCharge = (double?)x.SurchargeAmount
                    }).ToList();

                    var waiveOffTotal = GetWaveOffAginstRegNo(registrationNo);

                    var finalList = CalculateClearanceData(rawList, surchargeList, waiveOffTotal, surchargePaid);

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "",
                        Data = finalList
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription() + " Local System Error",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        private List<NewClearanceDTO> CalculateClearanceData(List<NewClearanceDTO> rawData, List<SurchargeSetup> surchargeList, decimal waiveOffTotal, decimal surchargePaid)
        {
            var currentDate = DateTime.Now;
            var results = new List<NewClearanceDTO>();

            foreach (var dto in rawData)
            {
                //decimal surchargeRate = FindSurchargeRate(dto.DueDate, surchargeList);
                decimal surcharge = 0;
                decimal waiveOffApplied = 0;
                decimal surchargePaidApplied = 0;

                if (dto.DueDate < currentDate)
                {
                    var settleDate = dto.SettleDate ?? currentDate;
                    var daysLate = (settleDate - dto.DueDate.Value).Days;

                    if (daysLate > 0)
                    {
                        var balance = (dto.ReceivedAmount > 0 && dto.ReceivedAmount < dto.BalanceOwed) ? dto.ReceivedAmount : dto.BalanceOwed;
                        surcharge = CalculateTieredSurcharge(dto.DueDate.Value, settleDate, balance, surchargeList);
                        //surcharge = ((surchargeRate / 100m) * dto.BalanceOwed / 365m) * daysLate;

                        if (waiveOffTotal > 0)
                        {
                            waiveOffApplied = Math.Min(surcharge, waiveOffTotal);
                            waiveOffTotal -= waiveOffApplied;
                        }
                        if (surchargePaid > 0)
                        {
                            decimal remaingSurcharge = surcharge - waiveOffApplied;
                            surchargePaidApplied = Math.Min(remaingSurcharge, surchargePaid);
                            surchargePaid -= surchargePaidApplied;
                        }
                    }
                }

                dto.Surcharge = Math.Round(surcharge, 2);
                dto.WaiveOffApplied = waiveOffApplied;
                dto.SurchargeReceivedApplied = surchargePaidApplied;
                dto.TotalAmountReceivable = dto.InvoiceAmount + ((dto.Surcharge - dto.WaiveOffApplied) - dto.SurchargeReceivedApplied);
                dto.FinalBalance = dto.BalanceDue + ((dto.Surcharge - dto.WaiveOffApplied) - dto.SurchargeReceivedApplied) - dto.Adjustment;

                results.Add(dto);
            }

            return results;
        }

        private decimal CalculateTieredSurcharge(DateTime dueDate, DateTime settleDate, decimal balance, List<SurchargeSetup> ranges)
        {
            decimal total = 0;

            foreach (var r in ranges.OrderBy(x => x.FromDate))
            {
                DateTime rangeStart = r.FromDate.Value.AddDays(-1);
                DateTime rangeEnd = r.ToDate.Value;

                DateTime overlapStart = dueDate > rangeStart ? dueDate : rangeStart;
                DateTime overlapEnd = settleDate < rangeEnd ? settleDate : rangeEnd;

                if (overlapStart < overlapEnd)
                {
                    int days = (overlapEnd - overlapStart).Days;

                    if (days > 0)
                    {
                        decimal rate = (decimal)(r.TotalSurCharge ?? 0);
                        decimal surchargePart = ((rate / 100m) * balance / 365m) * days;
                        total += surchargePart;
                    }
                }
            }

            return total;
        }


        private decimal FindSurchargeRate(DateTime? dueDate, List<SurchargeSetup> surchargeList)
        {
            if (dueDate == null) return 0;
            var item = surchargeList.FirstOrDefault(s => dueDate >= s.FromDate && dueDate <= s.ToDate);
            return (decimal)(item?.TotalSurCharge ?? 0);
        }

        private decimal GetWaveOffAginstRegNo(string regNo)
        {
            if (regNo == null) return 0;
            var item = _db.StockCreations.Where(x => x.RegistrationNo == regNo).Select(x => x.DiscountPercent).FirstOrDefault();
            return (decimal)(item ?? 0);
        }


        private List<SurchargeSetupDTO> GetSurchargeList()
        {
            var surchargeList = _db.SurchargeSetups
                                   .Where(x => !x.IsDeleted)
                                   .Select(x => new SurchargeSetupDTO
                                   {
                                       Id = x.Id,
                                       FromDate = (DateTime)x.FromDate,
                                       ToDate = (DateTime)x.ToDate,
                                       SurchargeAmount = (decimal)x.TotalSurCharge
                                   })
                                   .ToList();

            return surchargeList;
        }



        //[HttpGet]
        //[Route("GetCleareanceData")]

        //public IActionResult GetCleareanceData(string registrationNo, int memberProfileId)
        //{


        //    List<CleareanceDTO> ProjectList = new List<CleareanceDTO>();
        //    try
        //    {

        //        SAPOperationDb sapconnection = new SAPOperationDb(_db);

        //        sapconnection.ConnectToCompany();
        //        if (sapconnection._a == 0)
        //        {
        //            SAPbobsCOM.Recordset orecord = null;
        //            orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //            string str = "SELECT T1.\"DocEntry\",T0.\"U_InvCat\",T0.\"DocDueDate\",T1.\"Dscription\" as  \"Comments\", T1.\"AcctCode\",T1.\"LineTotal\",T0.\"Project\", T2.\"AcctName\",T0.\"DocNum\",  T2.\"AccntntCod\", T0.\"DocTotal\", " +
        //                         " (T0.\"DocTotal\" - T0.\"PaidToDate\") AS \"BalanceDue\", T0.\"PaidToDate\", (Select STRING_AGG(a.\"DocNum\", ',' ORDER BY b.\"DocEntry\")" +
        //                         " from orct a INNER JOIN rct2 b " +
        //                         " on a.\"DocEntry\" = b.\"DocNum\" where b.\"InvType\" = '13' and " +
        //                         " T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptNum\" " +
        //                         ", (SELECT STRING_AGG(CAST(a.\"DocDate\" AS DATE), ',' ORDER BY b.\"DocEntry\") \r\nfrom orct a\r\n INNER JOIN rct2 b  on a.\"DocEntry\" = b.\"DocNum\" \r\nwhere b.\"InvType\" = '13' and  T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptDate\"  " +
        //                         " FROM \"OINV\" T0 " +
        //                         " INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
        //                         " INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" " +
        //                         "  INNER JOIN \"OCRD\" T4 on T4.\"CardCode\"=T0.\"CardCode\" " +
        //                         " WHERE T0.\"CardCode\" = '" + registrationNo.Trim() + "' and T0.\"CANCELED\"='N'" +
        //                         " ORDER BY \r\n " +
        //                         "   CASE WHEN T0.\"U_InvCat\"='Booking' THEN 0 ELSE 1 END,\r\n  " +
        //                         "  T0.\"DocNum\" ASC ";
        //            orecord.DoQuery(str);
        //            if (orecord.RecordCount > 0)
        //            {
        //                int i = 0;
        //                for (i = 0; i < orecord.RecordCount; i++)
        //                {
        //                    CleareanceDTO project = new CleareanceDTO();

        //                    project.DocEntry = orecord.Fields.Item("DocEntry").Value;
        //                    project.DocDueDate = orecord.Fields.Item("DocDueDate").Value;
        //                    project.AcctCode = orecord.Fields.Item("AcctCode").Value;
        //                    project.Remarks = orecord.Fields.Item("Comments").Value;
        //                    project.Project = orecord.Fields.Item("Project").Value;
        //                    project.InvCat = orecord.Fields.Item("U_InvCat").Value;
        //                    project.AcctName = orecord.Fields.Item("AcctName").Value;
        //                    project.DocNum = orecord.Fields.Item("DocNum").Value;
        //                    project.AccntntCod = orecord.Fields.Item("AccntntCod").Value == null ? "" : orecord.Fields.Item("AccntntCod").Value;
        //                    project.DocTotal = orecord.Fields.Item("LineTotal").Value;
        //                    project.BalanceDue = orecord.Fields.Item("BalanceDue").Value;
        //                    project.TotalRecieved = orecord.Fields.Item("PaidToDate").Value;
        //                    project.ReceiptNum = orecord.Fields.Item("ReceiptNum").Value == null ? "" : orecord.Fields.Item("ReceiptNum").Value;
        //                    project.ReceiptDate = orecord.Fields.Item("ReceiptDate").Value == null ? "" : orecord.Fields.Item("ReceiptDate").Value;
        //                    ProjectList.Add(project);
        //                    orecord.MoveNext();

        //                }
        //                //responce.Data = customer_Details;
        //                //return responce;
        //                return Ok(new ApiResponse<object>
        //                {
        //                    Code = ResponseCode.Success,
        //                    Message = "",
        //                    Data = ProjectList
        //                });
        //            }
        //            else
        //            {
        //                return Ok(new ApiResponse<object>
        //                {
        //                    Code = ResponseCode.NotFound,
        //                    Message = "No Sap Record Available Against registration No " + registrationNo,
        //                    Data = null
        //                });
        //            }
        //        }
        //        else
        //        {


        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.Error,
        //                Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
        //                Data = null
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

        //    }
        //}

        [HttpGet]
        [Route("GetFinanacials")]
        public IActionResult GetFinanacials(string registrationNo, int memberProfileId)
        {
            List<CleareanceDTO> ProjectList = new List<CleareanceDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T1.\"DocEntry\",T0.\"U_InvCat\",T0.\"DocDueDate\",T1.\"Dscription\" as  \"Comments\", T1.\"AcctCode\",T1.\"LineTotal\",T0.\"Project\", T2.\"AcctName\",T0.\"DocNum\",  T2.\"AccntntCod\", T0.\"DocTotal\", " +
                                 " (T0.\"DocTotal\" - T0.\"PaidToDate\") AS \"BalanceDue\", T0.\"PaidToDate\", (Select STRING_AGG(a.\"DocNum\", ',' ORDER BY b.\"DocEntry\")" +
                                 " from orct a INNER JOIN rct2 b " +
                                 " on a.\"DocEntry\" = b.\"DocNum\" where b.\"InvType\" = '13' and " +
                                 " T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptNum\" " +
                                 ", (SELECT STRING_AGG(CAST(a.\"DocDate\" AS DATE), ',' ORDER BY b.\"DocEntry\") \r\nfrom orct a\r\n INNER JOIN rct2 b  on a.\"DocEntry\" = b.\"DocNum\" \r\nwhere b.\"InvType\" = '13' and  T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptDate\"  " +
                                 " FROM \"OINV\" T0 " +
                                 " INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                                 " INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" " +
                                 "  INNER JOIN \"OCRD\" T4 on T4.\"CardCode\"=T0.\"CardCode\" " +
                                 " WHERE T0.\"CardCode\" = '" + registrationNo.Trim() + "' and T0.\"CANCELED\"='N'" +
                                 " ORDER BY \r\n " +
                                 "   CASE WHEN T0.\"U_InvCat\"='Booking' THEN 0 ELSE 1 END,\r\n  " +
                                 "  T0.\"DocNum\" ASC ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            CleareanceDTO project = new CleareanceDTO();

                            project.DocEntry = orecord.Fields.Item("DocEntry").Value;
                            project.DocDueDate = orecord.Fields.Item("DocDueDate").Value;
                            project.AcctCode = orecord.Fields.Item("AcctCode").Value;
                            project.Remarks = orecord.Fields.Item("Comments").Value;
                            project.Project = orecord.Fields.Item("Project").Value;
                            project.InvCat = orecord.Fields.Item("U_InvCat").Value;
                            project.AcctName = orecord.Fields.Item("AcctName").Value;
                            project.DocNum = orecord.Fields.Item("DocNum").Value;
                            project.AccntntCod = orecord.Fields.Item("AccntntCod").Value == null ? "" : orecord.Fields.Item("AccntntCod").Value;
                            project.DocTotal = orecord.Fields.Item("LineTotal").Value;
                            project.BalanceDue = orecord.Fields.Item("BalanceDue").Value;
                            project.TotalRecieved = orecord.Fields.Item("PaidToDate").Value;
                            project.ReceiptNum = orecord.Fields.Item("ReceiptNum").Value == null ? "" : orecord.Fields.Item("ReceiptNum").Value;
                            project.ReceiptDate = orecord.Fields.Item("ReceiptDate").Value == null ? "" : orecord.Fields.Item("ReceiptDate").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against registration No " + registrationNo,
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }


        [HttpGet]
        [Route("GetFinanacialsData")]

        public IActionResult GetFinanacialsData(int memberProfileId)
        {


            List<CleareanceDTO> ProjectList = new List<CleareanceDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T1.\"DocEntry\",T0.\"U_InvCat\",T0.\"DocDueDate\", T1.\"AcctCode\",T1.\"LineTotal\",T0.\"Project\", T2.\"AcctName\",T0.\"DocNum\",  T2.\"AccntntCod\", T0.\"DocTotal\", " +
                                 " (T0.\"DocTotal\" - T0.\"PaidToDate\") AS \"BalanceDue\", T0.\"PaidToDate\", (Select STRING_AGG(a.\"DocNum\", ',' ORDER BY b.\"DocEntry\")" +
                                 " from orct a INNER JOIN rct2 b " +
                                 " on a.\"DocEntry\" = b.\"DocNum\" where b.\"InvType\" = '13' and " +
                                 " T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptNum\" " +
                                 " FROM \"OINV\" T0 " +
                                 " INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                                 " INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" " +
                                 "  INNER JOIN \"OCRD\" T4 on T4.\"CardCode\"=T0.\"CardCode\" " +
                                 " WHERE  T0.\"CANCELED\"='N' and T4.\"U_PMSID\"=" + memberProfileId + " ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            CleareanceDTO project = new CleareanceDTO();

                            project.DocEntry = orecord.Fields.Item("DocEntry").Value;
                            project.DocDueDate = orecord.Fields.Item("DocDueDate").Value;
                            project.AcctCode = orecord.Fields.Item("AcctCode").Value;
                            project.Project = orecord.Fields.Item("Project").Value;
                            project.InvCat = orecord.Fields.Item("U_InvCat").Value;
                            project.AcctName = orecord.Fields.Item("AcctName").Value;
                            project.DocNum = orecord.Fields.Item("DocNum").Value;
                            project.AccntntCod = orecord.Fields.Item("AccntntCod").Value == null ? "" : orecord.Fields.Item("AccntntCod").Value;
                            project.DocTotal = orecord.Fields.Item("LineTotal").Value;
                            project.BalanceDue = orecord.Fields.Item("BalanceDue").Value;
                            project.TotalRecieved = orecord.Fields.Item("PaidToDate").Value;
                            project.ReceiptNum = orecord.Fields.Item("ReceiptNum").Value == null ? "" : orecord.Fields.Item("ReceiptNum").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against User Id " + memberProfileId,
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetRepurchaseData")]

        public IActionResult GetRepurchaseData(string registrationNo, int MemberID)
        {


            List<CleareanceDTO> ProjectList = new List<CleareanceDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T1.\"DocEntry\",T0.\"PaidToDate\", T1.\"AcctCode\", T1.\"LineNum\",T0.\"Project\", T2.\"AcctName\",T0.\"DocNum\",  T2.\"AccntntCod\", T0.\"DocTotal\", " +
                                 " (T0.\"DocTotal\" - T0.\"PaidToDate\") AS \"BalanceDue\", (Select STRING_AGG(a.\"DocNum\", ',' ORDER BY b.\"DocEntry\")" +
                                 " from orct a INNER JOIN rct2 b " +
                                 " on a.\"DocEntry\" = b.\"DocNum\" where b.\"InvType\" = '13' and " +
                                 " T0.\"DocEntry\" = b.\"DocEntry\") AS \"ReceiptNum\" " +
                                 " FROM \"OINV\" T0 " +
                                 " INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                                 " INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" " +
                                 " INNER JOIN  \"OCRD\" T4 on T4.\"CardCode\"=T0.\"CardCode\" " +
                                 " WHERE T0.\"Project\" = '" + registrationNo + "' and T0.\"CANCELED\"='N' and T4.\"U_PMSID\"=" + MemberID + " and " +
        " T2.\"AcctName\" in('Sui Gas Dev Charges', 'Excess Area Fine', 'Prime Location Fine', 'Booking Fee', 'Proportional Dev Charges', 'Development Charges') ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            CleareanceDTO project = new CleareanceDTO();

                            project.DocEntry = orecord.Fields.Item("DocEntry").Value;
                            project.LineNum = orecord.Fields.Item("LineNum").Value;
                            project.AcctCode = orecord.Fields.Item("AcctCode").Value;
                            project.Project = orecord.Fields.Item("Project").Value;
                            project.AcctName = orecord.Fields.Item("AcctName").Value;
                            project.DocNum = orecord.Fields.Item("DocNum").Value;
                            project.AccntntCod = orecord.Fields.Item("AccntntCod").Value == null ? "" : orecord.Fields.Item("AccntntCod").Value;
                            project.DocTotal = orecord.Fields.Item("DocTotal").Value;
                            project.BalanceDue = orecord.Fields.Item("BalanceDue").Value;
                            project.PaidToDate = orecord.Fields.Item("PaidToDate").Value;
                            project.ReceiptNum = orecord.Fields.Item("ReceiptNum").Value == null ? "" : orecord.Fields.Item("ReceiptNum").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against registration No " + registrationNo,
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        private decimal GetPaidSurcharge(string registrationNo, SAPbobsCOM.Company company)
        {
            try
            {
                decimal paidSurcharge = 0;
                SAPbobsCOM.Recordset orecord = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string query =
                    "SELECT SUM(OINV.\"DocTotal\") AS \"TotalInvoiceAmount\" " +
                    "FROM OINV " +
                    "INNER JOIN INV1 ON OINV.\"DocEntry\" = INV1.\"DocEntry\" " +
                    "INNER JOIN RCT2 ON OINV.\"DocEntry\" = RCT2.\"DocEntry\" AND RCT2.\"InvType\" = '13' " +
                    "INNER JOIN ORCT ON RCT2.\"DocNum\" = ORCT.\"DocEntry\" " +
                    "WHERE OINV.\"DocTotal\" > 0 " +
                    "AND INV1.\"AcctCode\" = 'R102010002' " +
                    "AND OINV.\"CardCode\" = '" + registrationNo.Trim() + "' " +
                    "AND ORCT.\"Canceled\" <> 'Y'";

                orecord.DoQuery(query);

                if (orecord.RecordCount > 0)
                {
                    paidSurcharge = Convert.ToDecimal(orecord.Fields.Item("TotalInvoiceAmount").Value);
                }

                return paidSurcharge;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        [HttpGet]
        [Route("GetSingleInvoice")]
        public IActionResult GetSingleInvoice(int docNum)
        {
            try
            {

                SAPBillingDb sapconnection = new SAPBillingDb(_db);
                SingleInvoice invoice = new SingleInvoice();
                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"DocEntry\",T0.\"DocNum\", T0.\"DocDate\", T0.\"DocDueDate\", T0.\"CardCode\", T0.\"CardName\", T0.\"DocTotal\",T0.\"PaidToDate\",  T0.\"DocTotal\"- T0.\"PaidToDate\" as \"BalanceDue\" ,T0.\"Project\",T0.\"NumAtCard\" FROM OINV T0 WHERE  T0.\"DocNum\"='" + docNum + "'";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {


                            invoice.DocNum = orecord.Fields.Item("DocNum").Value != null ? Convert.ToString(orecord.Fields.Item("DocNum").Value) : "";
                            invoice.DocEntry = orecord.Fields.Item("DocEntry").Value != null ? Convert.ToString(orecord.Fields.Item("DocEntry").Value) : "";
                            invoice.DocDate = orecord.Fields.Item("DocDate").Value != null ? Convert.ToString(orecord.Fields.Item("DocDate").Value) : "";
                            invoice.DocDueDate = orecord.Fields.Item("DocDueDate").Value != null ? Convert.ToString(orecord.Fields.Item("DocDueDate").Value) : "";
                            invoice.RegistrationNum = orecord.Fields.Item("Project").Value != null ? Convert.ToString(orecord.Fields.Item("Project").Value) : "";
                            invoice.PropertyNum = orecord.Fields.Item("NumAtCard").Value != null ? Convert.ToString(orecord.Fields.Item("NumAtCard").Value) : "";
                            invoice.CardCode = orecord.Fields.Item("CardCode").Value != null ? Convert.ToString(orecord.Fields.Item("CardCode").Value) : "";
                            invoice.CardName = orecord.Fields.Item("CardName").Value != null ? Convert.ToString(orecord.Fields.Item("CardName").Value) : "";
                            invoice.DocTotal = orecord.Fields.Item("DocTotal").Value != null ? Convert.ToString(orecord.Fields.Item("DocTotal").Value) : "";
                            invoice.TotalPaid = orecord.Fields.Item("PaidToDate").Value != null ? Convert.ToString(orecord.Fields.Item("PaidToDate").Value) : "";
                            invoice.BalanceDue = orecord.Fields.Item("BalanceDue").Value != null ? Convert.ToString(orecord.Fields.Item("BalanceDue").Value) : "";
                            invoice.Details = GetSingleInvoiceDetailsForMobileApp(docNum);


                            orecord.MoveNext();

                        }

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = invoice
                        }); ;
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        private List<InvoiceDetail> GetSingleInvoiceDetailsForMobileApp(int docNum)
        {
            try
            {

                SAPBillingDb sapconnection = new SAPBillingDb(_db);
                List<InvoiceDetail> listofInvoices = new List<InvoiceDetail>();
                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"DocNum\", T0.\"DocEntry\", T1.\"LineNum\", T1.\"Project\", T1.\"Price\", T1.\"LineTotal\", T1.\"AcctCode\", T2.\"AcctName\" FROM OINV T0  INNER JOIN INV1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN OACT T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" WHERE T0.\"DocNum\"  =" + docNum + "";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            InvoiceDetail invoice = new InvoiceDetail();

                            invoice.LineNum = orecord.Fields.Item("LineNum").Value != null ? Convert.ToString(orecord.Fields.Item("LineNum").Value) : "";
                            invoice.LineTotal = orecord.Fields.Item("LineTotal").Value != null ? Convert.ToString(orecord.Fields.Item("LineTotal").Value) : "";
                            invoice.AccountCode = orecord.Fields.Item("AcctCode").Value != null ? Convert.ToString(orecord.Fields.Item("AcctCode").Value) : "";
                            invoice.ChargeName = orecord.Fields.Item("AcctName").Value != null ? Convert.ToString(orecord.Fields.Item("AcctName").Value) : "";
                            invoice.RegistrationNum = orecord.Fields.Item("Project").Value != null ? Convert.ToString(orecord.Fields.Item("Project").Value) : "";

                            listofInvoices.Add(invoice);
                            orecord.MoveNext();

                        }

                        return listofInvoices;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        [HttpGet]
        [Route("GetAllInvoicesAgainstCategory")]
        public IActionResult GetAllInvoicesAgainstCategory(string pmsId, string categoryCode)
        {

            ////        //                    InvoiceList.Add(invoice);
            ////        //                    orecord.MoveNext();

            List<Invoice> InvoiceList = new List<Invoice>();
            try
            {

                SAPBillingDb sapconnection = new SAPBillingDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT Distinct T0.\"DocNum\",T0.\"NumAtCard\",T0.\"DocEntry\", T0.\"DocDate\", T0.\"DocDueDate\",T0.\"Project\",T3.\"U_PropertyNo\", T0.\"CardCode\", T0.\"CardName\", T0.\"DocTotal\"- T0.\"PaidToDate\" as \"BalanceDue\", T2.\"AcctName\"  FROM OINV T0 INNER JOIN INV1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN OACT T2 ON  T1.\"AcctCode\" = T2.\"AcctCode\" INNER JOIN OPRJ T3 ON T0.\"Project\" = T3.\"PrjCode\"  " +
" INNER JOIN OCRD T4 ON T4.\"CardCode\" = T0.\"CardCode\" " +
" WHERE T0.\"DocStatus\" = 'O' and T4.\"U_PMSID\" = '" + pmsId + "'  and T2.\"AcctCode\" = '" + categoryCode + "' and T1.\"LineNum\" = 0";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            Invoice invoice = new Invoice();

                            invoice.DocNum = orecord.Fields.Item("DocNum").Value != null ? Convert.ToString(orecord.Fields.Item("DocNum").Value) : "";
                            invoice.DocEntry = orecord.Fields.Item("DocEntry").Value != null ? Convert.ToString(orecord.Fields.Item("DocEntry").Value) : "";
                            invoice.DocDate = orecord.Fields.Item("DocDate").Value != null ? Convert.ToString(orecord.Fields.Item("DocDate").Value) : "";
                            invoice.DocDueDate = orecord.Fields.Item("DocDueDate").Value != null ? Convert.ToString(orecord.Fields.Item("DocDueDate").Value) : "";
                            invoice.Project = orecord.Fields.Item("Project").Value != null ? Convert.ToString(orecord.Fields.Item("Project").Value) : "";

                            var block = _db.StockCreations.Where(x => x.RegistrationNo == invoice.Project).Select(x => x.Block).FirstOrDefault();
                            invoice.Block = _commonBLL.GetBlockName(Convert.ToInt32(block));
                            invoice.U_PropertyNo = orecord.Fields.Item("NumAtCard").Value != null ? Convert.ToString(orecord.Fields.Item("NumAtCard").Value) : "";
                            invoice.CardCode = orecord.Fields.Item("CardCode").Value != null ? Convert.ToString(orecord.Fields.Item("CardCode").Value) : "";
                            invoice.CardName = orecord.Fields.Item("CardName").Value != null ? Convert.ToString(orecord.Fields.Item("CardName").Value) : "";
                            invoice.InvoiceCategory = orecord.Fields.Item("AcctName").Value != null ? Convert.ToString(orecord.Fields.Item("AcctName").Value) : "";
                            invoice.BalanceDue = orecord.Fields.Item("BalanceDue").Value != null ? Convert.ToString(orecord.Fields.Item("BalanceDue").Value) : "";


                            InvoiceList.Add(invoice);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = InvoiceList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {

                    ////            List<Invoice> InvoiceList = new List<Invoice>();
                    ////            try
                    ////            {

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetTotalInvoiceCount")]
        public IActionResult GetTotalInvoiceCount(string pmsId)
        {
            try
            {
                SAPBillingDb sapconnection = new SAPBillingDb(_db);
                string count;
                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "Select count(*) as \"totalcount\" from ( SELECT Distinct T0.\"DocNum\",T0.\"DocEntry\", T0.\"DocDate\", T0.\"DocDueDate\",T0.\"Project\",T3.\"U_PropertyNo\", T0.\"CardCode\", T0.\"CardName\", T0.\"DocTotal\"- T0.\"PaidToDate\" as \"Balance Due\", T2.\"AccntntCod\" FROM OINV T0 INNER JOIN INV1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN OACT T2 ON  T1.\"AcctCode\" = T2.\"AcctCode\"  INNER JOIN OPRJ T3 ON T0.\"Project\" = T3.\"PrjCode\" INNER JOIN OCRD T4 on T0.\"CardCode\" = T4.\"CardCode\" WHERE  T4.\"U_PMSID\"='" + pmsId + "' and T0.\"DocStatus\" ='O' and T1.\"LineNum\"=0)";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        count = orecord.Fields.Item("totalcount").Value != null ? Convert.ToString(orecord.Fields.Item("totalcount").Value) : "";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = count
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = "0"
                        });
                    }
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [Route("GetAllInvoiceCategories")]
        public IActionResult GetAllInvoiceCategories(string pmsId)
        {


            List<InvoiceCategory> InvoiceList = new List<InvoiceCategory>();
            try
            {

                SAPBillingDb sapconnection = new SAPBillingDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT Distinct  T2.\"AcctCode\" ,T2.\"AcctName\", (Select count(*)  from " +
" ( " +
" SELECT Distinct T10.\"DocNum\", T10.\"DocEntry\", T10.\"DocDate\", T10.\"DocDueDate\", T10.\"Project\", T13.\"U_PropertyNo\", T10.\"CardCode\", T10.\"CardName\", T10.\"DocTotal\" - T10.\"PaidToDate\" as \"Balance Due\", T12.\"AccntntCod\" FROM OINV  T10 INNER JOIN INV1 T11 ON T10.\"DocEntry\" = T11.\"DocEntry\" INNER JOIN OACT T12 ON  T11.\"AcctCode\" = T12.\"AcctCode\"  INNER JOIN OPRJ T13 ON T10.\"Project\" = T13.\"PrjCode\" " +
"  INNER JOIN OCRD T14 on T10.\"CardCode\" = T14.\"CardCode\" " +
"  WHERE T14.\"U_PMSID\" = " + pmsId + " and T10.\"DocStatus\" = 'O' and T11.\"AcctCode\" = T2.\"AcctCode\" " +
" ) " +
" ) as \"totalcount\" " +
"  FROM OINV T0 INNER JOIN INV1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN OACT T2 ON T1.\"AcctCode\" = T2.\"AcctCode\"  INNER JOIN OPRJ T3 ON T0.\"Project\" = T3.\"PrjCode\" INNER JOIN OCRD T4 on T0.\"CardCode\" = T4.\"CardCode\" WHERE T0.\"DocStatus\" = 'O' and T4.\"U_PMSID\" = " + pmsId + "  ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            InvoiceCategory invoice = new InvoiceCategory();

                            invoice.CategoryCode = orecord.Fields.Item("AcctCode").Value != null ? Convert.ToString(orecord.Fields.Item("AcctCode").Value) : "";
                            invoice.CategoryName = orecord.Fields.Item("AcctName").Value != null ? Convert.ToString(orecord.Fields.Item("AcctName").Value) : "";
                            invoice.Count = orecord.Fields.Item("totalcount").Value != null ? Convert.ToString(orecord.Fields.Item("totalcount").Value) : "";

                            InvoiceList.Add(invoice);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = InvoiceList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllItemGroups")]

        public IActionResult GetAllItemGroups()
        {


            List<Departs> Departments = new List<Departs>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT Distinct T1.\"ItmsGrpCod\", T1.\"ItmsGrpNam\",T1.\"U_ItemCustodian\" FROM OITM T0  INNER JOIN OITB T1 ON T0.\"ItmsGrpCod\" = T1.\"ItmsGrpCod\"";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            Departs Department = new Departs();

                            Department.Id = orecord.Fields.Item("ItmsGrpCod").Value;
                            Department.Name = orecord.Fields.Item("ItmsGrpNam").Value;
                            Department.CustodianId = orecord.Fields.Item("U_ItemCustodian").Value == null ? "" : Convert.ToString(orecord.Fields.Item("U_ItemCustodian").Value);
                            Departments.Add(Department);
                            orecord.MoveNext();

                        }
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = Departments
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllItems")]

        public IActionResult GetAllItems(int groupcode)
        {


            List<ItemMaster> Items = new List<ItemMaster>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T1.\"ItemCode\", T1.\"ItemName\", T1.\"ItmsGrpCod\", T1.\"LastPurPrc\", T1.\"DfltWH\" from OITM T1 where T1.\"ItmsGrpCod\" = " + groupcode + "  and T1.\"validFor\"='Y'  ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            ItemMaster Item = new ItemMaster();

                            Item.ItemCode = Convert.ToString(orecord.Fields.Item("ItemCode").Value);
                            Item.ItemName = Convert.ToString(orecord.Fields.Item("ItemName").Value);
                            Items.Add(Item);
                            orecord.MoveNext();

                        }
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = Items
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetItemDetails")]

        public IActionResult GetItemDetails(string itemCode)
        {

            ////                    return Ok(new ApiResponse<object>
            ////                    {
            ////                        Code = ResponseCode.Error,
            ////                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
            ////                        Data = null
            ////                    });
            ////                }
            ////            }
            ////            catch (Exception ex)
            ////            {
            ////                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            List<ItemMaster> Items = new List<ItemMaster>();
            try
            {
                ItemMaster Item = new ItemMaster();
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"ItemCode\", T0.\"ItemName\", T0.\"IUoMEntry\", T0.\"InvntryUom\", T1.\"UomCode\", T0.\"LastPurPrc\", T0.\"DfltWH\" FROM OITM T0  INNER JOIN OUOM T1 ON T0.\"IUoMEntry\" = T1.\"UomEntry\" where T0.\"ItemCode\"='" + itemCode + "'";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {


                            Item.ItemCode = Convert.ToString(orecord.Fields.Item("ItemCode").Value);
                            Item.ItemName = Convert.ToString(orecord.Fields.Item("ItemName").Value);
                            Item.UomCode = Convert.ToString(orecord.Fields.Item("UomCode").Value);
                            Item.LastPurPrc = Convert.ToString(orecord.Fields.Item("LastPurPrc").Value);
                            Item.DfltWH = Convert.ToString(orecord.Fields.Item("DfltWH").Value);
                            orecord.MoveNext();

                        }
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = Item
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllWhse")]

        public IActionResult GetAllWhse()
        {


            List<WareHouse> wareHouses = new List<WareHouse>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"WhsCode\", T0.\"WhsName\" FROM OWHS T0";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            WareHouse Item = new WareHouse();

                            Item.WhsCode = Convert.ToString(orecord.Fields.Item("WhsCode").Value);
                            Item.WhsName = Convert.ToString(orecord.Fields.Item("WhsName").Value);

                            wareHouses.Add(Item);
                            orecord.MoveNext();

                        }
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = wareHouses
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {

                    ////                            Item.ItemCode = Convert.ToString(orecord.Fields.Item("ItemCode").Value);
                    ////                            Item.ItemName = Convert.ToString(orecord.Fields.Item("ItemName").Value);
                    ////                            Item.UomCode = Convert.ToString(orecord.Fields.Item("UomCode").Value);
                    ////                            Item.LastPurPrc = Convert.ToString(orecord.Fields.Item("LastPurPrc").Value);
                    ////                            Item.DfltWH = Convert.ToString(orecord.Fields.Item("DfltWH").Value);
                    ////                            orecord.MoveNext();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllUoms")]

        public IActionResult GetAllUoms()
        {


            List<Models.DTOs.SAPDTO.UOM> UOMs = new List<Models.DTOs.SAPDTO.UOM>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT Distinct T1.\"UomCode\", T1.\"UomName\" FROM OITM T0  INNER JOIN OUOM T1 ON T0.\"PUoMEntry\" = T1.\"UomEntry\" ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            Models.DTOs.SAPDTO.UOM Item = new Models.DTOs.SAPDTO.UOM();

                            Item.UomCode = Convert.ToString(orecord.Fields.Item("UomCode").Value);
                            Item.UomName = Convert.ToString(orecord.Fields.Item("UomName").Value);

                            UOMs.Add(Item);
                            orecord.MoveNext();

                        }
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = UOMs
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "",
                            Data = null
                        });
                    }
                }
                else
                {

                    ////                            wareHouses.Add(Item);
                    ////                            orecord.MoveNext();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }



        [HttpPost]
        [Route("MemberPosting")]
        public Response_Result MemberPosting(MemberProfile member)
        {
            Response_Result response = new Response_Result();
            double DocTotal = 0;
            bool result = false;
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.BusinessPartners oBusinessPartner = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    oBusinessPartner.CardName = member.MemberName;

                    var operationsDetails = _db.SAPOperations.FirstOrDefault();
                    oBusinessPartner.Series = Convert.ToInt32(operationsDetails.CustomerSeries);
                    oBusinessPartner.UserFields.Fields.Item("Fax").Value = member.RelationshipWith;
                    oBusinessPartner.UserFields.Fields.Item("Cellular").Value = member.Mobile;
                    oBusinessPartner.UserFields.Fields.Item("VatIdUnCmp").Value = member.Cnic;
                    //oBusinessPartner.UserFields.Fields.Item("Phone1").Value = member.Phone;
                    //oBusinessPartner.UserFields.Fields.Item("LicTradNum").Value = member.NTNNo;
                    // oBusinessPartner.UserFields.Fields.Item("Address").Value = member.CurrentAddress;
                    oBusinessPartner.UserFields.Fields.Item("DebPayAcct").Value = operationsDetails.MemberAccountCode;
                    oBusinessPartner.UserFields.Fields.Item("U_PMSID").Value = member.Id;
                    int finalresult = oBusinessPartner.Add();
                    if (finalresult == 0)
                    {
                        var DocEntry = sapconnection.Ocomp.GetNewObjectKey();
                        MemberProfile memberProfile = _db.MemberProfile.Where(x => x.Id == member.Id).FirstOrDefault();
                        if (memberProfile != null)
                        {
                            memberProfile.DocNum = DocEntry;
                            memberProfile.SapPosting = true;
                            memberProfile.DocEntry = Convert.ToString(DocEntry);
                            _db.Entry(memberProfile).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                        response.message = "Sap Posting Successfull With CardCode : " + DocEntry;
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
                else
                {
                    string message = sapconnection.Ocomp.GetLastErrorDescription();
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = message;
                }





                ////                    foreach (var items in tbl_MDN.DemandNoteItems)
                ////                    {
                ////                        oPurchaseRequest.Lines.ItemCode = items.ItemNo;
                ////                        oPurchaseRequest.Lines.FreeText = items.Remarks;
                ////                        oPurchaseRequest.Lines.WarehouseCode = items.Whse;
                ////                        oPurchaseRequest.Lines.ProjectCode = items.PrjCode;
                ////                        oPurchaseRequest.Lines.Price = Convert.ToDouble(items.InfoPrice);
                ////                        oPurchaseRequest.Lines.UnitPrice = Convert.ToDouble(items.InfoPrice);
                ////                        oPurchaseRequest.Lines.RequiredDate = (DateTime)items.RequiredDate;
                ////                        oPurchaseRequest.Lines.Quantity = Convert.ToDouble(items.RequiredQuantity);
                ////                        oPurchaseRequest.Lines.Add();

            }

            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }

        public Response_Result UpdatePropertyInMemberProfile(StockCreation member)
        {
            Response_Result response = new Response_Result();
            double DocTotal = 0;
            bool result = false;
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.BusinessPartners oBusinessPartner = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    //SAPbobsCOM.Recordset orecord = null;
                    //orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    //string str = "SELECT T0.\"DocEntry\" from OCRD T0 where T0.\"CardCode\"= '"+member.RegistrationNo+"'";
                    //orecord.DoQuery(str);
                    //var DocEntry = orecord.Fields.Item("DocEntry").Value;
                    oBusinessPartner.GetByKey(member.RegistrationNo);
                    oBusinessPartner.CardName = member.PropertyNo;

                    int finalresult = oBusinessPartner.Update();
                    if (finalresult == 0)
                    {
                        SAPbobsCOM.Recordset orecordForProjectUpdate = null;
                        orecordForProjectUpdate = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        string projectupdatequery = "Update OPRJ T0 set T0.\"U_Property\"='" + member.PropertyNo + "' where T0.\"PrjCode\"='" + member.RegistrationNo + "'";
                        orecordForProjectUpdate.DoQuery(projectupdatequery);

                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
                else
                {
                    string message = sapconnection.Ocomp.GetLastErrorDescription();
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = message;
                }



            }

            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }

        public Response_Result UpdateMemberProfileToAddContactPerson(int stockId, int memberId)
        {
            Response_Result response = new Response_Result();
            double DocTotal = 0;
            bool result = false;
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.BusinessPartners oBusinessPartner = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                    var RegistrationNo = _db.StockCreations.Where(i => i.ID == stockId).Select(i => i.RegistrationNo).FirstOrDefault();
                    //    SapCardNameAndCardCode businessPartner = GetSapCardNameAndCardCodeForOperation((int)m);

                    oBusinessPartner.GetByKey(RegistrationNo);

                    var memberName = _db.MemberProfile.Where(i => i.Id == memberId).Select(i => i.MemberName).FirstOrDefault();
                    oBusinessPartner.ContactEmployees.Name = memberName;
                    oBusinessPartner.ContactEmployees.Add();



                    int finalresult = oBusinessPartner.Update();
                    if (finalresult == 0)
                    {


                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
                else
                {
                    string message = sapconnection.Ocomp.GetLastErrorDescription();
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = message;
                }



            }

            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }
        public Response_Result MemberUpdate(MemberProfile member)
        {
            Response_Result response = new Response_Result();
            double DocTotal = 0;
            bool result = false;
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.BusinessPartners oBusinessPartner = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    oBusinessPartner.GetByKey(member.DocEntry);
                    oBusinessPartner.CardName = member.MemberName;

                    var operationsDetails = _db.SAPOperations.FirstOrDefault();
                    oBusinessPartner.Series = Convert.ToInt32(operationsDetails.CustomerSeries);
                    oBusinessPartner.UserFields.Fields.Item("Fax").Value = member.RelationshipWith;
                    oBusinessPartner.UserFields.Fields.Item("Cellular").Value = member.Mobile;
                    oBusinessPartner.UserFields.Fields.Item("VatIdUnCmp").Value = member.Cnic;
                    oBusinessPartner.UserFields.Fields.Item("Phone1").Value = member.Phone;
                    oBusinessPartner.UserFields.Fields.Item("LicTradNum").Value = member.NTNNo;
                    oBusinessPartner.UserFields.Fields.Item("Address").Value = member.CurrentAddress;
                    oBusinessPartner.UserFields.Fields.Item("DebPayAcct").Value = operationsDetails.MemberAccountCode;
                    //      oBusinessPartner.UserFields.Fields.Item("U_PMSID").Value = member.Id;
                    int finalresult = oBusinessPartner.Update();
                    if (finalresult == 0)
                    {
                        var DocEntry = sapconnection.Ocomp.GetNewObjectKey();

                        response.message = "Sap Updation Successfull With CardCode : " + DocEntry;
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
                else
                {
                    string message = sapconnection.Ocomp.GetLastErrorDescription();
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = message;
                }

                ////                }
                ////            }
                ////            catch (Exception exception3)
                ////            {
                ////                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                ////                response.message = exception3.Message + response.message;
                ////            }
                ////            return response;
                ////        }




                ////            try
                ////            {
                ////                SAPBillingDb sapconnection = new SAPBillingDb(_db);

            }

            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }
        [HttpPost]
        [Route("PostPurchaseRequest")]
        public Response_Result PostPurchaseRequest(DemandNote tbl_MDN)
        {
            Response_Result response = new Response_Result();
            double DocTotal = 0;
            bool result = false;
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Documents oPurchaseRequest = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);
                    //      var demandNoteId=
                    oPurchaseRequest.Requester = "manager";
                    oPurchaseRequest.RequesterName = "manager";
                    //oPurchaseRequest.DocDueDate = (DateTime)tbl_MDN.ValidUntill;
                    oPurchaseRequest.RequriedDate = (DateTime)tbl_MDN.RequiredDate;
                    var user1 = _db.PMSUser.Where(i => i.EMP_CODE == tbl_MDN.ManagerId).FirstOrDefault();
                    var user2 = _db.PMSUser.Where(i => i.EMP_CODE == tbl_MDN.CustodianId).FirstOrDefault();
                    oPurchaseRequest.UserFields.Fields.Item("U_HODName").Value = user1 != null ? user1.EMP_FULL_NAME : "";
                    oPurchaseRequest.UserFields.Fields.Item("U_Ctdn").Value = user2 != null ? user2.EMP_FULL_NAME : "";
                    oPurchaseRequest.UserFields.Fields.Item("U_Dept").Value = tbl_MDN.Deparment;
                    oPurchaseRequest.UserFields.Fields.Item("U_Req").Value = tbl_MDN.RequesterName;
                    oPurchaseRequest.UserFields.Fields.Item("U_PMSDoc").Value = tbl_MDN.Id;

                    oPurchaseRequest.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

                    ////                    return BP;
                    ////                }
                    ////            }
                    ////            catch (Exception ex)
                    ////            {
                    ////                return BP;

                    foreach (var items in tbl_MDN.DemandNoteItems)
                    {
                        oPurchaseRequest.Lines.ItemCode = items.ItemNo;
                        oPurchaseRequest.Lines.FreeText = items.Remarks;
                        oPurchaseRequest.Lines.WarehouseCode = items.Whse;
                        oPurchaseRequest.Lines.ProjectCode = items.PrjCode;
                        oPurchaseRequest.Lines.Price = Convert.ToDouble(items.InfoPrice);
                        oPurchaseRequest.Lines.UnitPrice = Convert.ToDouble(items.InfoPrice);
                        oPurchaseRequest.Lines.RequiredDate = (DateTime)items.RequiredDate;
                        oPurchaseRequest.Lines.Quantity = Convert.ToDouble(items.RequiredQuantity);
                        oPurchaseRequest.Lines.Add();

                    }
                    int finalresult = oPurchaseRequest.Add();
                    if (finalresult == 0)
                    {
                        var DocEntry = sapconnection.Ocomp.GetNewObjectKey();
                        SAPbobsCOM.Recordset recordSetDocEntry = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        var DocNumQuery = "select b.\"DocNum\" from OPRQ b  where b.\"DocEntry\"='" + DocEntry + "'";
                        recordSetDocEntry.DoQuery(DocNumQuery);
                        if (recordSetDocEntry.RecordCount > 0)
                        {
                            DemandNote postedDemandNote = _db.DemandNote.Where(x => !x.IsDeleted && x.Id == tbl_MDN.Id)
                                                                     .FirstOrDefault();
                            if (postedDemandNote != null)
                            {
                                postedDemandNote.DocEntry = Convert.ToString(DocEntry);
                                postedDemandNote.SapPosting = true;
                                postedDemandNote.DocNum = Convert.ToString(recordSetDocEntry.Fields.Item("DocNum").Value);
                                _db.Entry(postedDemandNote).State = EntityState.Modified;
                                _db.SaveChanges();

                            }

                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Document Post Successfully with document No: " + postedDemandNote.DocNum;
                        }
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }

        public SapCardNameAndCardCode GetSapCardNameAndCardCode(int id)
        {
            SapCardNameAndCardCode BP = new SapCardNameAndCardCode();

            ////                    oPurchaseRequest.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

            ////////                    return BP;
            ////////                }
            ////////            }
            ////////            catch (Exception ex)
            ////////            {
            ////////                return BP;

            try
            {
                SAPBillingDb sapconnection = new SAPBillingDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"CardCode\", T0.\"CardName\" FROM OCRD T0 WHERE T0.\"U_PMSID\"=" + id + "";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {

                            BP.CardCode = orecord.Fields.Item("CardCode").Value;
                            BP.CardName = orecord.Fields.Item("CardName").Value;

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return BP;
                    }
                    else
                    {
                        return BP;
                    }
                }
                else
                {


                    return BP;
                }
            }
            catch (Exception ex)
            {
                return BP;

            }
        }
        public int GetCntctInternalNum(int pmsID, string ContactName, SAPOperationDb sapconnection)
        {
            try
            {
                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"CntctCode\" FROM OCPR T0  INNER JOIN OCRD T1 ON T0.\"CardCode\" = T1.\"CardCode\" WHERE T1.\"U_PMSID\" = '" + pmsID + "' and T0.\"Name\"= '" + ContactName + "'";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int cntCode = orecord.Fields.Item("CntctCode").Value;
                        return cntCode;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public Response_Result PostARInvoice(BillPrintDTO bill, decimal billSurchargePercentage)
        {
            Response_Result response = new Response_Result();
            SAPOperationDb sap = new SAPOperationDb(_db);
            List<int> createdDocEntries = new List<int>();

            sap.ConnectToCompany();

            try
            {
                if (sap._a != 0)
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "SAP Connection Failed";
                    return response;
                }

                var invoice = (SAPbobsCOM.Documents)
                    sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                invoice.CardCode = bill.RegistrationNo;
                invoice.DocDate = Convert.ToDateTime(bill.DocDate);
                invoice.DocDueDate = Convert.ToDateTime(bill.DueDate);
                invoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                invoice.Comments = bill.Remarks;
                invoice.Project = bill.RegistrationNo;
                invoice.UserFields.Fields.Item("U_BillSurchargePerc").Value = billSurchargePercentage.ToString();
                invoice.UserFields.Fields.Item("U_BillMnth").Value = bill.BillMonth.ToString();
                invoice.UserFields.Fields.Item("U_BillReferenceNo").Value = $"{bill.RegistrationNo}-{bill.BillMonth}";


                bool hasLines = false;

                foreach (var item in bill.MeterBillWithFixedCharges)
                {
                    if (item.Rate == null || item.Rate <= 0)
                        continue;

                    string accountCode = item.SAPAccount;

                    if (string.IsNullOrEmpty(accountCode))
                        throw new Exception("Some GL accounts are missing");

                    invoice.Lines.ItemDescription = item.ChargeType;
                    invoice.Lines.AccountCode = accountCode;
                    invoice.Lines.Price = Convert.ToDouble(item.Rate);
                    invoice.Lines.UnitPrice = Convert.ToDouble(item.Rate);
                    invoice.Lines.ProjectCode = bill.RegistrationNo;
                    //invoice.Lines.VatGroup = item.SaleTax;

                    invoice.Lines.Add();

                    hasLines = true;
                }

                if (!hasLines)
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "No valid rows (NetAmount > 0 required)";
                    return response;
                }

                // ✅ POST
                int res = invoice.Add();

                if (res != 0)
                    throw new Exception(sap.Ocomp.GetLastErrorDescription());

                int docEntry = int.Parse(sap.Ocomp.GetNewObjectKey());
                createdDocEntries.Add(docEntry);

                response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                response.message = "Invoice posted successfully";
            }
            catch (Exception ex)
            {
                // 🔥 ROLLBACK (same as your new API)
                foreach (var docEntry in createdDocEntries)
                {
                    try
                    {
                        var inv = (SAPbobsCOM.Documents)
                            sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                        if (inv.GetByKey(docEntry))
                            inv.Cancel();
                    }
                    catch { }
                }

                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = ex.Message + " (Invoice cancelled)";
            }

            return response;
        }


        [HttpPost]
        [Route("BillPostingInSAP")] // IT Tower
        public IActionResult BillPostingInSAP(ListForPrintDTO dto)
        {
            try
            {
                List<BillPrintDTO> billPrintDTO = new List<BillPrintDTO>();

                if (dto == null || !dto.BillList.Any())
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "No bills to post",
                        Data = billPrintDTO
                    });
                }

                decimal billSurchargePercentage = _db.SAPOperations.FirstOrDefault()?.BillDiscountPercentage ?? 0;

                foreach (var item in dto.BillList)
                {
                    BillPrintDTO bill = new BillPrintDTO();

                    bill = _db.StockCreations.Where(x => x.RegistrationNo == item.RegistrationNo)
                                                      .Select(x => new BillPrintDTO
                                                      {
                                                          StockId = x.ID,
                                                          RegistrationNo = x.RegistrationNo,
                                                          PropertyNo = x.PropertyNo,
                                                          Area = x.ActualSize,
                                                          UOM = x.ActualSizeUnit,
                                                          MemberId = x.MemberProfile.Id,
                                                          MemberName = x.MemberProfile.MemberName,
                                                          Address = x.MemberProfile.PermanentAddress,
                                                          MobileNo = x.MemberProfile.Mobile,
                                                          WhatsAppNo = x.MemberProfile.WhatsAppNo,
                                                          DueDate = dto.DueDate,
                                                          BillMonth = dto.BillMonth,
                                                          DocDate = dto.DocDate
                                                      })
                                                      .FirstOrDefault();

                    TanantDetail tanantDetail = _db.TanantDetail.Where(t => t.IsActive == true &&
                                                                       t.StockCreationID == bill.StockId
                                                                      )
                                                                .FirstOrDefault();
                    SAPOperations sapOperations = _db.SAPOperations.Where(x => !x.IsDeleted).FirstOrDefault();
                    bill.ArInvoiceSeries = Convert.ToInt32(sapOperations.CustomerSeries);
                    SaleTax saletaxDeatail = _db.SaleTax.Where(x => !x.IsDeleted).FirstOrDefault();
                    if (decimal.TryParse(saletaxDeatail?.TaxCode, out var taxValue))
                    {
                        bill.SaleTax = taxValue;
                    }
                    else
                    {
                        bill.SaleTax = null;
                    }

                    bill.TenantMember = tanantDetail?.Name == "" ? "N/A" : tanantDetail?.Name;
                    bill.TenantMobileNo = tanantDetail?.Mobile == "" || tanantDetail?.Mobile == null ? "N/A" : tanantDetail?.Mobile;
                    bill.TenantMobileNo = tanantDetail?.Mobile == null || tanantDetail?.Mobile == "" ? "N/A" : tanantDetail?.Mobile;

                    bill.FixedChargeBillWHApplied = _db.FixedChargeBillWHApplied
                                                       .Where(x => !x.IsDeleted &&
                                                         x.RegistrationNo == item.RegistrationNo &&
                                                         x.Month == dto.BillMonth)
                                                       .Distinct()
                                                       .ToList();
                    bill.WTaxMapDTOPropertywise = bill.FixedChargeBillWHApplied
                                                      .Where(x => x.Month == dto.BillMonth &&
                                                             x.RegistrationNo == item.RegistrationNo)
                                                      .GroupBy(x => new { x.TaxCode, x.WHPercentage })
                                                      .Select(g => new WTaxMapDTOPropertywise
                                                      {
                                                          RegistrationNo = g.First().RegistrationNo,
                                                          Month = g.First().Month,
                                                          TaxCode = g.Key.TaxCode,
                                                          NetAmount = g.First().NetAmount,
                                                          WHPercentage = g.Key.WHPercentage,
                                                          Amount = g.Sum(x => x.Amount)
                                                      }).ToList();

                    if (dto.BillFor == "All" || dto.BillFor == "Electricity")
                    {
                        bill.MeterBillGenerationDetail = _db.MeterBillGenerationDetail
                                                            .Where(x => !x.IsDeleted &&
                                                              x.RegistrationNo == item.RegistrationNo &&
                                                              x.MeterBillGeneration.Month == dto.BillMonth)
                                                            .Distinct()
                                                            .ToList();

                        bill.GrandMeterWTaxAmount = bill.MeterBillGenerationDetail.Sum(x => x.WTaxAmount);
                        bill.GrandMeterBillAmount = bill.MeterBillGenerationDetail.Sum(x => x.NetAmount);
                    }

                    if (dto.BillFor == "Constructed" || dto.BillFor == "Fixed Dues")
                    {
                        bill.FixedChargeBillDetail = _db.FixedChargeBillDetail
                                                     .Where(x => !x.IsDeleted &&
                                                       x.FixedChargeBill.Month == dto.BillMonth &&
                                                       x.FixedChargeBill.RegistrationNo == item.RegistrationNo)
                                                     .Distinct()
                                                     .ToList();

                        bill.GrandFixedWTaxAmount = bill.FixedChargeBillDetail.Sum(x => x.WTaxAmountLine);
                        bill.GrandFixedBillAmount = bill.FixedChargeBillDetail.Sum(x => x.NetAmount);
                    }

                    bill.BillBeforeDueDate = bill.GrandFixedBillAmount + bill.GrandFixedWTaxAmount + bill.GrandMeterBillAmount + bill.GrandMeterWTaxAmount;
                    bill.SurchargeAfterDueDate = bill.BillBeforeDueDate * billSurchargePercentage / 100;
                    bill.BillAfterDueDate = bill.BillBeforeDueDate + bill.SurchargeAfterDueDate;

                    //Mapping

                    var result = new List<MeterBillWithFixedCharge>();


                    // Map MeterBillGenerationDetail
                    foreach (var meterBill in bill.MeterBillGenerationDetail)
                    {
                        var meterBillWithFixedCharge = new MeterBillWithFixedCharge
                        {
                            ChargeType = "Electricity",
                            SAPAccount = meterBill.SapAccount,
                            RegistrationNo = meterBill.RegistrationNo,
                            MeterNo = meterBill.MeterNo,
                            SaleTax = meterBill.SaleTaxAmount == 0 ? "S2" : bill.SaleTax.ToString(),
                            CurrentReading = meterBill.CurrentReading,
                            PreviousReading = meterBill.PreviousReading,
                            ReadingDate = meterBill.CreatedOn,
                            Uom = bill.UOM,
                            Rate = meterBill.PerUnitRate,
                            Quantity = meterBill.TotalUnitConsumed,
                        };
                        result.Add(meterBillWithFixedCharge);
                    }

                    // Map FixedChargeBillDetail
                    foreach (var fixedChargeBill in bill.FixedChargeBillDetail)
                    {
                        var fixedChargeBillWithFixedCharge = new MeterBillWithFixedCharge
                        {
                            ChargeType = fixedChargeBill.Description,
                            SAPAccount = fixedChargeBill.SapAccount,
                            RegistrationNo = bill.RegistrationNo,
                            MeterNo = "",
                            SaleTax = fixedChargeBill.SaleTaxAmount == 0 ? "S2" : bill.SaleTax.ToString(),
                            CurrentReading = "",
                            PreviousReading = "",
                            ReadingDate = fixedChargeBill.CreatedOn,
                            Uom = bill.UOM,
                            Rate = fixedChargeBill.Amount,
                            Quantity = fixedChargeBill.Unit
                        };
                        result.Add(fixedChargeBillWithFixedCharge);
                    }

                    // Sort the results by registration number
                    result = result.OrderBy(x => x.RegistrationNo).ToList();
                    bill.MeterBillWithFixedCharges = result;

                    if (bill != null)
                    {
                        Response_Result sapresult = new SapIntegrationController(_db).PostARInvoice(bill, billSurchargePercentage);
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Sap Posting Successfull",
                    Data = billPrintDTO
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("AddSAPStock")]
        public Response_Result AddSAPStock(int stockID)
        {
            Response_Result response = new Response_Result();

            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    var stock = _db.StockCreations.Where(x => x.ID == stockID && x.Is_StockCreationApproved == true).FirstOrDefault();
                    if (stock != null)
                    {
                        SAPbobsCOM.CompanyService oCmpSrv;
                        SAPbobsCOM.ProjectsService ProjectService;
                        oCmpSrv = sapconnection.Ocomp.GetCompanyService();
                        ProjectService = oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.ProjectsService);
                        SAPbobsCOM.Project oProject;
                        SAPbobsCOM.ProjectsParams ProjectParams;
                        oProject = ProjectService.GetDataInterface(SAPbobsCOM.ProjectsServiceDataInterfaces.psProject);
                        oProject.Code = stock.RegistrationNo;
                        oProject.Name = stock.RegistrationNo;

                        oProject.UserFields.Item("U_Property").Value = stock.PropertyNo == null ? "" : stock.PropertyNo;
                        if (stock.Project != null)
                        {
                            oProject.UserFields.Item("U_MainPrjctName").Value = _commonBLL.GetProjectName(Convert.ToInt32(stock.Project));
                        }
                        if (stock.Phase != null)
                        {
                            oProject.UserFields.Item("U_Phase").Value = _commonBLL.GetPhaseName(Convert.ToInt32(stock.Phase));
                        }
                        if (stock.Block != null)
                        {
                            oProject.UserFields.Item("U_Block").Value = _commonBLL.GetBlockName(Convert.ToInt32(stock.Block));
                        }
                        if (stock.Floor != null && stock.Floor != "")
                        {
                            oProject.UserFields.Item("U_SubProject").Value = _commonBLL.GetFloorName(Convert.ToInt32(stock.Floor));
                        }

                        oProject.Active = SAPbobsCOM.BoYesNoEnum.tYES;

                        ProjectService.AddProject(oProject);

                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        if (message != "")
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;

                        }
                        else
                        {
                            Response_Result responsefromCreatingmember = MemberPostingForStock(stock);

                            if (responsefromCreatingmember.code == 0)
                            {
                                response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                                response.message = "Stock Created In Sap";
                            }


                        }

                        // response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    }

                    else
                    {
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                        response.message = "Stock Not Approved";
                    }
                }
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }
        public Response_Result MemberPostingForStock(StockCreation stock)
        {
            Response_Result response = new Response_Result();

            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.BusinessPartners oBusinessPartner = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    oBusinessPartner.Series = 1;
                    oBusinessPartner.GroupCode = 116;
                    oBusinessPartner.CardCode = stock.RegistrationNo;
                    oBusinessPartner.CardName = stock.PropertyNo;
                    //oBusinessPartner.EmailAddress = stock.RegistrationNo;

                    int phaseid = Convert.ToInt32(stock.Phase);
                    int blockid = Convert.ToInt32(stock.Block);
                    int projectid = Convert.ToInt32(stock.Project);
                    var phaseName = _commonBLL.GetPhaseName(phaseid);
                    var blockName = _commonBLL.GetBlockName(blockid);
                    var projectName = _commonBLL.GetProjectName(projectid);
                    oBusinessPartner.UserFields.Fields.Item("IntrntSite").Value = phaseName;
                    oBusinessPartner.UserFields.Fields.Item("E_Mail").Value = projectName;
                    oBusinessPartner.Password = blockName;
                    int finalresult = oBusinessPartner.Add();
                    if (finalresult == 0)
                    {

                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                    }
                }
                else
                {
                    string message = sapconnection.Ocomp.GetLastErrorDescription();
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = message;
                }



            }

            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }
            return response;
        }



        public SapCardNameAndCardCode GetSapCardNameAndCardCodeForOperation(int id)
        {
            SapCardNameAndCardCode BP = new SapCardNameAndCardCode();


            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();

                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"CardCode\", T0.\"CardName\" FROM OCRD T0 WHERE T0.\"U_PMSID\"=" + id + "";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {

                            BP.CardCode = orecord.Fields.Item("CardCode").Value;
                            BP.CardName = orecord.Fields.Item("CardName").Value;

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return BP;
                    }
                    else
                    {
                        return BP;
                    }
                }
                else
                {

                    ////        //            }
                    ////        //        }
                    ////        //        else
                    ////        //        {
                    ////        //            string message = sapconnection.Ocomp.GetLastErrorDescription();
                    ////        //            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    ////        //            response.message = message;
                    ////        //        }

                    return BP;
                }
            }
            catch (Exception ex)
            {
                return BP;

            }
        }


        public Response_Result AddServiceTypeInvoiceProcessingCharges(Booking booking, bool isUpdate)
        {
            Response_Result response = new Response_Result();
            var registrationNo = "";
            var taxCode = "";

            foreach (var item in booking.BookingProcessingCharges)
            {
                try
                {
                    int finalresult = -1;

                    SAPOperationDb sapconnection = new SAPOperationDb(_db);

                    sapconnection.ConnectToCompany();
                    if (sapconnection._a == 0)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = DateTime.Now;
                        var stock = _db.StockCreations.Where(i => i.ID == booking.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();


                        var member = _db.MemberProfile.Where(i => i.Id == (int)booking.MemberProfileId).FirstOrDefault();


                        oInvoice.CardCode = stock.RegistrationNo;
                        oInvoice.TrackingNumber = Convert.ToString(booking.MemberProfileId);
                        // oInvoice.ContactPersonCode = member.MemberName;
                        if (stock != null)
                        {
                            registrationNo = stock.RegistrationNo;
                            oInvoice.Project = registrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }
                        oInvoice.UserFields.Fields.Item("U_PMSID").Value = booking.Id;
                        oInvoice.UserFields.Fields.Item("U_InvCat").Value = "Booking Processing Charges";
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        var acctcode = "";

                        if (item.ChargeTypeId != null)
                        {
                            acctcode = _commonBLL.GetSapAccountByChargeTypeId((int)item.ChargeTypeId);
                        }
                        oInvoice.Lines.ItemDescription = item.Remarks;
                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = registrationNo;
                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {

                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Processing Charges Posted In Sap Successfully";
                            if (isUpdate == false)
                            {
                                var bookingObject = _db.Booking.Find(booking.Id);
                                bookingObject.isBookingProcessingChargesPostedInSap = true;
                                bookingObject.BookingProcessingChargesErrorMsg = response.message;
                                _db.Entry(bookingObject).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                            else
                            {
                                var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                                backlog.BookingChargePosted = true;
                                _db.Update(backlog);
                                _db.SaveChanges();
                            }

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                            if (isUpdate == false)
                            {
                                BookingBackLog backlog = new BookingBackLog();
                                backlog.BookingId = booking.Id;
                                backlog.ErrorMessage = response.message;
                                backlog.BookingChargeId = item.Id;
                                backlog.BookingChargePosted = false;
                                backlog.BookingType = 1;
                                backlog.CreatedOn = DateTime.Now;
                                _db.Add(backlog);
                                _db.SaveChanges();
                            }
                            else
                            {
                                var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                                backlog.ErrorMessage = response.message;
                                _db.Update(backlog);
                                _db.SaveChanges();
                            }

                        }
                    }

                    else
                    {
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = "Connection Failed";
                        if (isUpdate == false)
                        {
                            BookingBackLog backlog = new BookingBackLog();
                            backlog.BookingId = booking.Id;
                            backlog.StockId = (int)booking.StockCreationId;
                            backlog.BookingChargeId = item.Id;
                            backlog.BookingChargePosted = false;
                            backlog.BookingType = 1;
                            backlog.ErrorMessage = response.message;
                            backlog.CreatedOn = DateTime.Now;
                            _db.Add(backlog);
                            _db.SaveChanges();
                        }
                        else
                        {
                            var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                            backlog.ErrorMessage = response.message;
                            _db.Update(backlog);
                            _db.SaveChanges();
                        }

                    }
                }
                catch (Exception exception3)
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                    response.message = exception3.Message + response.message;
                    if (isUpdate == false)
                    {
                        BookingBackLog backlog = new BookingBackLog();
                        backlog.BookingId = booking.Id;
                        backlog.StockId = (int)booking.StockCreationId;
                        backlog.BookingChargeId = item.Id;
                        backlog.BookingChargePosted = false;
                        backlog.BookingType = 1;
                        backlog.ErrorMessage = response.message;
                        backlog.CreatedOn = DateTime.Now;
                        _db.Add(backlog);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                        backlog.ErrorMessage = response.message;
                        _db.Update(backlog);
                        _db.SaveChanges();
                    }
                }
            }

            return response;
        }

        public Response_Result AddServiceTypeInvoiceBookingSchedule(Booking booking, bool isUpdate)
        {
            Response_Result response = new Response_Result();
            var registrationNo = "";
            var taxCode = "";

            foreach (var item in booking.BookingSchedulePaymentPlanDetail)
            {
                try
                {

                    SAPOperationDb sapconnection = new SAPOperationDb(_db);

                    sapconnection.ConnectToCompany();
                    if (sapconnection._a == 0)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = (DateTime)booking.CreatedOn;
                        oInvoice.DocDueDate = (DateTime)item.DueDate;
                        SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)booking.MemberProfileId);
                        var member = _db.MemberProfile.Where(i => i.Id == (int)booking.MemberProfileId).FirstOrDefault();

                        var stock = _db.StockCreations.Where(i => i.ID == booking.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        oInvoice.CardCode = stock.RegistrationNo;
                        // oInvoice.ContactPersonCode = member.MemberName;
                        if (stock != null)
                        {
                            registrationNo = stock.RegistrationNo;
                            oInvoice.Project = registrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }
                        oInvoice.TrackingNumber = Convert.ToString(booking.MemberProfileId);
                        oInvoice.UserFields.Fields.Item("U_PMSID").Value = booking.Id;
                        oInvoice.UserFields.Fields.Item("U_InvCat").Value = "Booking";
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        var totalamount = 0;
                        //      oInvoice.Lines.AccountCode = operationsDetails.BookingAccount;
                        var acctCode = _commonBLL.GetSapAccountByChargeTypeId((int)item.ChargeTypeId);
                        oInvoice.Lines.AccountCode = acctCode;
                        oInvoice.Lines.ItemDescription = item.Remarks;
                        oInvoice.Lines.ProjectCode = registrationNo;
                        oInvoice.Lines.UnitPrice = Convert.ToDouble((double)item.Amount);
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Booking Schedule Posted In Sap Successfully";
                            if (isUpdate == false)
                            {
                                var bookingObject = _db.Booking.Find(booking.Id);
                                bookingObject.isBookingPaymentSchedulePostedInSap = true;
                                bookingObject.BookingPaymentScheduleErrorMsg = response.message;
                                _db.Entry(bookingObject).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                            else
                            {
                                var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                                backlog.BookingChargePosted = true;
                                _db.Update(backlog);
                                _db.SaveChanges();
                            }

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                            //var bookingObject = _db.Booking.Find(booking.Id);
                            //bookingObject.isBookingPaymentSchedulePostedInSap = false;
                            //bookingObject.BookingPaymentScheduleErrorMsg = response.message;
                            //_db.Entry(bookingObject).State = EntityState.Modified;
                            //_db.SaveChanges();
                            if (isUpdate == false)
                            {
                                BookingBackLog backlog = new BookingBackLog();
                                backlog.BookingId = booking.Id;
                                backlog.StockId = (int)booking.StockCreationId;
                                backlog.BookingChargeId = item.Id;
                                backlog.BookingChargePosted = false;
                                backlog.BookingType = 2;
                                backlog.ErrorMessage = response.message;
                                backlog.CreatedOn = DateTime.Now;
                                _db.Add(backlog);
                                _db.SaveChanges();
                            }
                            else
                            {
                                var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                                backlog.ErrorMessage = response.message;
                                _db.Update(backlog);
                                _db.SaveChanges();
                            }
                        }
                    }

                    else
                    {
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = "Connection Failed";
                        if (isUpdate == false)
                        {
                            BookingBackLog backlog = new BookingBackLog();
                            backlog.BookingId = booking.Id;
                            backlog.StockId = (int)booking.StockCreationId;
                            backlog.BookingChargeId = item.Id;
                            backlog.BookingChargePosted = false;
                            backlog.BookingType = 2;
                            backlog.ErrorMessage = response.message;
                            backlog.CreatedOn = DateTime.Now;
                            _db.Add(backlog);
                            _db.SaveChanges();
                        }
                        else
                        {
                            var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                            backlog.ErrorMessage = response.message;
                            _db.Update(backlog);
                            _db.SaveChanges();
                        }
                    }
                }
                catch (Exception exception3)
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                    response.message = exception3.Message + response.message;
                    if (isUpdate == false)
                    {
                        BookingBackLog backlog = new BookingBackLog();
                        backlog.BookingId = booking.Id;
                        backlog.StockId = (int)booking.StockCreationId;
                        backlog.BookingChargeId = item.Id;
                        backlog.BookingChargePosted = false;
                        backlog.BookingType = 2;
                        backlog.ErrorMessage = response.message;
                        backlog.CreatedOn = DateTime.Now;
                        _db.Add(backlog);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var backlog = _db.BookingBackLog.Where(x => x.BookingChargeId == item.Id).FirstOrDefault();
                        backlog.ErrorMessage = response.message;
                        _db.Update(backlog);
                        _db.SaveChanges();
                    }
                }
            }

            return response;
        }


        [HttpGet]
        [Route("PostingBackLogBooking")]
        public Response_Result PostingBackLogBooking(int id, int bookingtype)
        {
            Response_Result response = new Response_Result();
            var message = "";
            try
            {
                var bookingdata = _db.BookingBackLog.Where(x => x.BookingId == id && x.BookingChargePosted == false).ToList();
                foreach (var item in bookingdata)
                {
                    if (bookingtype == 2)
                    {
                        var bookingWithData = _db.Booking
                                                     //  .Include(x => x.BookingSchedulePaymentPlanDetail.Where(y=>y.Id ==item.BookingChargeId))
                                                     .Where(x => x.Id == id).FirstOrDefault();

                        bookingWithData.BookingSchedulePaymentPlanDetail = _db.BookingSchedulePaymentPlanDetail.Where(x => x.Id == item.BookingChargeId).ToList();
                        Response_Result response_ResultBookingSchedule = new SapIntegrationController(_db).AddServiceTypeInvoiceBookingSchedule(bookingWithData, true);
                        message = response_ResultBookingSchedule.message;


                    }
                    if (bookingtype == 1)
                    {
                        var bookingWithData = _db.Booking
                                                     //  .Include(x => x.BookingProcessingCharges.Where(y => y.Id == item.BookingChargeId))
                                                     .Where(x => x.Id == id).FirstOrDefault();

                        bookingWithData.BookingProcessingCharges = _db.BookingProcessingCharges.Where(x => x.Id == item.BookingChargeId).ToList();
                        Response_Result response_ResultBookingSchedule = new SapIntegrationController(_db).AddServiceTypeInvoiceProcessingCharges(bookingWithData, true);
                        message = response_ResultBookingSchedule.message;


                    }
                }



                response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                response.message = message;
                return response;

            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }

        }

        public Response_Result GlobalInvoicePosting(InvoicePostingDTO invoicePostingDTO)
        {
            Response_Result response = new Response_Result();
            var registrationNo = "";
            var taxCode = "";
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {

                    foreach (var item in invoicePostingDTO.Details)
                    {
                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = invoicePostingDTO.DueDate;
                        oInvoice.DocDueDate = invoicePostingDTO.DueDate;
                        var stock = _db.StockCreations.Where(i => i.ID == invoicePostingDTO.StockId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)stock.MemberProfileId);
                        if (cardNameAndCardCode != null)
                        {
                            oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        }
                        if (stock != null)
                        {
                            registrationNo = stock.RegistrationNo;
                            oInvoice.Project = registrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }
                        var asd = _commonBLL.GetGlobalChargeName(invoicePostingDTO.ChargeSetUpId);
                        oInvoice.UserFields.Fields.Item("U_InvCat").Value = asd;
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        var acctcode = "";

                        if (item.ChargeID != null)
                        {
                            acctcode = _commonBLL.GetSapAccountByGlobalChargeDetail((int)item.ChargeID);
                            oInvoice.Lines.AccountCode = acctcode;
                        }
                        oInvoice.Lines.VatGroup = "S2";
                        oInvoice.Lines.ProjectCode = registrationNo;
                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Posted In Sap Successfully";
                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                        }
                    }
                }
                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";
                }
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
            }



            return response;
        }


        [HttpGet]
        [Route("GetAllInvoicesProjects")]
        public IActionResult GetAllInvoicesProjects()
        {


            List<RegistrationAgainstInvoice> ProjectList = new List<RegistrationAgainstInvoice>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT Distinct T0.\"Project\",T0.\"CardName\", T0.\"NumAtCard\" as \"PropertyNo\" FROM \"OINV\" T0  INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" where T0.\"CANCELED\"='N'";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            RegistrationAgainstInvoice project = new RegistrationAgainstInvoice();

                            project.Registration = orecord.Fields.Item("Project").Value;
                            project.Property = orecord.Fields.Item("PropertyNo").Value;
                            project.Member = orecord.Fields.Item("CardName").Value;

                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available ",
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetAllInvoicesAgainstRegistration")]
        public IActionResult GetAllInvoicesAgainstRegistration(string registrationNo)
        {


            List<InvoiceRecordDTO> ProjectList = new List<InvoiceRecordDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "Select distinct X.\"DocEntry\",X.\"DocNum\",X.\"DocDate\",X.\"CardName\",X.\"DocTotal\" from (SELECT T1.\"DocEntry\", T1.\"AcctCode\",T0.\"Project\", T2.\"AcctName\",T0.\"DocNum\",T0.\"CardName\",  T0.\"DocDate\",T2.\"AccntntCod\", T0.\"DocTotal\",(T0.\"DocTotal\"-T0.\"PaidToDate\") AS \"Balance Due\", (Select STRING_AGG (a.\"DocNum\",',' ORDER BY b.\"DocEntry\") from orct a INNER JOIN rct2 b on a.\"DocEntry\"=b.\"DocNum\" where T0.\"DocEntry\" =b.\"DocEntry\") AS \"ReceiptNum\" FROM \"OINV\" T0  INNER JOIN \"INV1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" INNER JOIN \"OACT\" T2 ON T1.\"AcctCode\" = T2.\"AcctCode\" where T0.\"CANCELED\"='N') as X  WHERE X.\"Project\"='" + registrationNo + "' ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            InvoiceRecordDTO project = new InvoiceRecordDTO();

                            project.DocEntry = orecord.Fields.Item("DocEntry").Value;
                            project.CardName = orecord.Fields.Item("CardName").Value;
                            project.DocTotal = orecord.Fields.Item("DocTotal").Value;
                            project.DocDate = Convert.ToString(orecord.Fields.Item("DocDate").Value);

                            project.DocNum = orecord.Fields.Item("DocNum").Value;
                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList.OrderByDescending(x => x.DocNum).ToList()
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against registration No " + registrationNo,
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }

        [HttpGet]
        [Route("GetSingleInvoiceDetails")]
        public IActionResult GetSingleInvoiceDetails(int DocEntry)
        {


            List<SingleInvoiceChargeDTO> ProjectList = new List<SingleInvoiceChargeDTO>();
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    SAPbobsCOM.Recordset orecord = null;
                    orecord = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string str = "SELECT T0.\"AcctCode\",T0.\"LineTotal\",T0.\"LineNum\",T2.\"AcctName\", T0.\"Price\", T1.\"DocNum\",T0.\"DocEntry\", T1.\"NumAtCard\" as \"RegistrationNo\", " +
" ifnull(sum(T3.\"LineTotal\"), 0) as \"CreditMemo\" " +
" FROM INV1 T0 " +
" INNER JOIN OINV T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
" INNER JOIN OACT T2 ON T0.\"AcctCode\" = T2.\"AcctCode\" " +
" left join rin1 T3 on T0.\"DocEntry\" = T3.\"BaseEntry\" and T0.\"LineNum\" = T3.\"BaseLine\" and T3.\"BaseType\" = '13' " +
" WHERE T0.\"DocEntry\" = " + DocEntry + " " +
" group by T0.\"AcctCode\", T0.\"LineTotal\", T0.\"LineNum\", T2.\"AcctName\", T0.\"Price\", T1.\"DocNum\", T0.\"DocEntry\", T1.\"NumAtCard\" ";
                    orecord.DoQuery(str);
                    if (orecord.RecordCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < orecord.RecordCount; i++)
                        {
                            SingleInvoiceChargeDTO project = new SingleInvoiceChargeDTO();

                            project.LineNum = orecord.Fields.Item("LineNum").Value;
                            project.AcctCode = orecord.Fields.Item("AcctCode").Value;
                            project.AcctName = orecord.Fields.Item("AcctName").Value;
                            project.CreditMemo = orecord.Fields.Item("CreditMemo").Value;
                            project.Price = orecord.Fields.Item("Price").Value;

                            ProjectList.Add(project);
                            orecord.MoveNext();

                        }
                        //responce.Data = customer_Details;
                        //return responce;
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "",
                            Data = ProjectList
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "No Sap Record Available Against Doc Entry " + DocEntry,
                            Data = null
                        });
                    }
                }
                else
                {


                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription().ToString() + "Local System Error:",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));

            }
        }




        public Response_Result PostingCreditNoteAndARInvoice(GenralAdjustment genralAdjustment)
        {
            Response_Result response = new Response_Result();
            string fullmessage = "";
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    var negativedata = genralAdjustment.GenralAdjustmentCharges.Where(x => x.Adjustment < 0).Count();
                    var positivedata = genralAdjustment.GenralAdjustmentCharges.Where(x => x.Adjustment > 0).Count();
                    if (negativedata > 0)
                    {
                        SAPbobsCOM.Documents oCreditNote = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        int docEntry = Convert.ToInt32(genralAdjustment.InvoiceNo);
                        oInvoice.GetByKey(docEntry);

                        oCreditNote.DocDate = oInvoice.DocDate;
                        oCreditNote.DocDueDate = oInvoice.DocDueDate;
                        oCreditNote.CardCode = oInvoice.CardCode;
                        oCreditNote.ContactPersonCode = oInvoice.ContactPersonCode;
                        oCreditNote.NumAtCard = oInvoice.NumAtCard;

                        oCreditNote.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        foreach (var item in genralAdjustment.GenralAdjustmentCharges.Where(x => x.Adjustment < 0).ToList())
                        {
                            oCreditNote.Lines.BaseEntry = Convert.ToInt32(genralAdjustment.InvoiceNo);
                            oCreditNote.Lines.BaseType = 13;
                            oCreditNote.Lines.BaseLine = item.LineNum;
                            oCreditNote.Lines.AccountCode = item.SapAccount;
                            oCreditNote.Lines.ProjectCode = genralAdjustment.RegistrationNo;
                            oCreditNote.Lines.UnitPrice = ((double)item.Adjustment) * -1;
                            oCreditNote.Lines.Add();

                        }
                        int finalresult = oCreditNote.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Credit Note Posted IN SAP Successfully";
                            fullmessage = response.message;

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                            fullmessage = response.message;
                        }
                    }
                    if (positivedata > 0)
                    {
                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = DateTime.Now;
                        var stock = _db.StockCreations.Where(i => i.ID == genralAdjustment.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)genralAdjustment.MemberProfileId);
                        if (cardNameAndCardCode != null)
                        {
                            oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        }
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }

                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        foreach (var item in genralAdjustment.GenralAdjustmentCharges.Where(x => x.Adjustment > 0).ToList())
                        {

                            oInvoice.Lines.AccountCode = item.SapAccount;
                            oInvoice.Lines.ProjectCode = genralAdjustment.RegistrationNo;

                            oInvoice.Lines.UnitPrice = (double)item.Adjustment;
                            oInvoice.Lines.Add();

                        }
                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            var message = "Ar Invoice Posted Successfully";
                            response.message = message + " " + fullmessage;


                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message + " " + fullmessage;

                        }
                    }

                }
                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + " " + response.message + "" + fullmessage;
                return response;
            }
        }


        public Response_Result PostingCreditNoteForRepurchase(RePurchase repurchase)
        {
            Response_Result response = new Response_Result();
            string fullmessage = "";
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.Documents oCreditNote = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                    oCreditNote.DocDate = DateTime.Now;
                    var stock = _db.StockCreations.Where(i => i.ID == repurchase.StockCreationId).FirstOrDefault();
                    var operationsDetails = _db.SAPOperations.FirstOrDefault();
                    SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)stock.MemberProfileId);
                    if (cardNameAndCardCode != null)
                    {
                        oCreditNote.CardCode = cardNameAndCardCode.CardCode;
                    }
                    var registrationNo = "";
                    if (stock != null)
                    {
                        registrationNo = stock.RegistrationNo;
                        oCreditNote.Project = registrationNo;
                        oCreditNote.NumAtCard = stock.PropertyNo;
                    }

                    oCreditNote.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    var financeCount = repurchase.RePurchaseFinanceDetail.Count();
                    var deductionAmount = Convert.ToDouble(repurchase.DeductionAmount);
                    var creditnoteAmount = deductionAmount / financeCount;
                    foreach (var item in repurchase.RePurchaseFinanceDetail)
                    {

                        //oCreditNote.Lines.BaseEntry = Convert.ToInt32(item.DocEntry);
                        //oCreditNote.Lines.BaseType = 13;
                        //oCreditNote.Lines.BaseLine = Convert.ToInt32(item.LineNum);
                        oCreditNote.Lines.AccountCode = item.SapAccount;
                        oCreditNote.Lines.ProjectCode = registrationNo;
                        oCreditNote.Lines.UnitPrice = ((double)creditnoteAmount);
                        oCreditNote.Lines.Add();

                    }
                    int finalresult = oCreditNote.Add();
                    if (finalresult == 0)
                    {
                        // Response_Result responseForArInvoice = new Response_Result();
                        Response_Result responseForArInvoice = PostingArInvoiceAfterPostingCreditNoteForRepurchase(repurchase);
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                        response.message = "Credit Note Posted IN SAP Successfully";
                        fullmessage = response.message;

                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                        fullmessage = response.message;
                    }


                }
                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + " " + response.message + "" + fullmessage;
                return response;
            }
        }


        public Response_Result PostingArInvoiceAfterPostingCreditNoteForRepurchase(RePurchase repurchase)
        {
            Response_Result response = new Response_Result();
            string fullmessage = "";
            try
            {

                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {

                    SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    oInvoice.DocDate = DateTime.Now;
                    var stock = _db.StockCreations.Where(i => i.ID == repurchase.StockCreationId).FirstOrDefault();
                    var operationsDetails = _db.SAPOperations.FirstOrDefault();
                    SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)stock.MemberProfileId);
                    if (cardNameAndCardCode != null)
                    {
                        oInvoice.CardCode = cardNameAndCardCode.CardCode;
                    }
                    var registrationNo = "";
                    if (stock != null)
                    {
                        registrationNo = stock.RegistrationNo;
                        oInvoice.Project = registrationNo;
                        oInvoice.NumAtCard = stock.PropertyNo;
                    }

                    oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    var deductionAmount = Convert.ToDouble(repurchase.DeductionAmount);

                    oInvoice.Lines.AccountCode = operationsDetails.RepurchaseDeductionAccount;
                    oInvoice.Lines.ProjectCode = registrationNo;
                    oInvoice.Lines.UnitPrice = deductionAmount;
                    oInvoice.Lines.Add();

                    int finalresult = oInvoice.Add();
                    if (finalresult == 0)
                    {
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                        response.message = "Ar Invoice For Deduction Posted Successfully";
                        fullmessage = response.message;

                    }
                    else
                    {
                        string message = sapconnection.Ocomp.GetLastErrorDescription();
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = message;
                        fullmessage = response.message;
                    }


                }
                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + " " + response.message + "" + fullmessage;
                return response;
            }
        }


        public Response_Result PostingStandAloneARInvoice(StandAlone genralAdjustment)
        {
            Response_Result response = new Response_Result();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();
                foreach (var item in genralAdjustment.StandAloneCharges)
                {
                    if (sapconnection._a == 0)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = (DateTime)genralAdjustment.DocumentDate;
                        oInvoice.DocDueDate = (DateTime)item.DueDate;
                        var stock = _db.StockCreations.Where(i => i.ID == genralAdjustment.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        //SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)genralAdjustment.MemberProfileId);
                        //if (cardNameAndCardCode != null)
                        //{
                        //    oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        //}
                        oInvoice.CardCode = stock.RegistrationNo;
                        oInvoice.TrackingNumber = Convert.ToString(genralAdjustment.MemberProfileId);
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        oInvoice.UserFields.Fields.Item("U_ChN").Value = genralAdjustment.ChallanNo;
                        oInvoice.UserFields.Fields.Item("U_ChDate").Value = DateTime.Now.Date;
                        oInvoice.UserFields.Fields.Item("U_DDBankAC").Value = !string.IsNullOrEmpty(genralAdjustment.BankAccountDD) ? genralAdjustment.BankAccountDD : string.Empty;
                        oInvoice.Lines.ItemDescription = item.Remarks;
                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = genralAdjustment.RegistrationNo;
                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Ar Invoice Posted Successfully";
                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                        }
                    }

                    else
                    {
                        response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                        response.message = "Connection Failed";

                    }
                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }
        }


        public Response_Result CancelInvoicesByChallan(string challanNo)
        {
            Response_Result response = new Response_Result();

            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    response.code = (int)Global_Utility.ResponseCode.error;
                    response.message = "SAP Connection Failed";
                    return response;
                }

                SAPbobsCOM.Recordset rs =
                    (SAPbobsCOM.Recordset)sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string query = $@"
                                 SELECT 
                                     ""DocEntry""
                                 FROM ""OINV""
                                 WHERE IFNULL(TO_NVARCHAR(""U_ChN""), '') = '{challanNo}'
                                   AND ""DocStatus"" = 'O'
                                 ";

                rs.DoQuery(query);

                if (rs.RecordCount == 0)
                {
                    response.code = (int)Global_Utility.ResponseCode.succcess;
                    response.message = "No open invoices found for this challan";
                    return response;
                }


                while (!rs.EoF)
                {
                    int docEntry = Convert.ToInt32(rs.Fields.Item("DocEntry").Value);

                    SAPbobsCOM.Documents oInvoice =
                        (SAPbobsCOM.Documents)sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    if (oInvoice.GetByKey(docEntry))
                    {
                        if (oInvoice.DocumentStatus == SAPbobsCOM.BoStatus.bost_Open)
                        {
                            SAPbobsCOM.Documents cancelDoc = oInvoice.CreateCancellationDocument();
                            int res = cancelDoc.Add();

                            if (res != 0)
                            {
                                sapconnection.Ocomp.GetLastError(out int errCode, out string errMsg);
                                throw new Exception($"SAP Cancel Error (DocEntry {docEntry}): {errMsg}");
                            }
                        }
                    }

                    rs.MoveNext();
                }

                response.code = (int)Global_Utility.ResponseCode.succcess;
                response.message = "All related invoices cancelled successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.code = (int)Global_Utility.ResponseCode.exception;
                response.message = ex.Message;
                return response;
            }
        }

        [HttpPost]
        [Route("PostScheduleInSap")]
        public IActionResult PostScheduleInSap(ScheduleDTO dto)
        {
            List<int> createdDocEntries = new List<int>();

            SAPOperationDb sap = new SAPOperationDb(_db);
            sap.ConnectToCompany();

            try
            {
                if (sap._a != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "SAP Connection Failed"
                    });
                }

                var stock = _db.StockCreations
                    .FirstOrDefault(x => x.ID == dto.StockCreationId);

                if (stock == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Stock not found"
                    });
                }

                //if (_db.Inovices.Any(x => x.StockCreationId == dto.StockCreationId))
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.BadRequest,
                //        Message = "Schedule already exist"
                //    });
                //}

                foreach (var item in dto.PaymentPlan)
                {
                    string accountCode = string.Empty;

                    if (item.ChargeTypeId != null)
                    {
                        accountCode = _commonBLL
                            .GetSapAccountByChargeTypeId(Convert.ToInt32(item.ChargeTypeId));
                    }

                    if (string.IsNullOrEmpty(accountCode))
                        throw new Exception("Some GL accounts are missing");

                    var invoice = (SAPbobsCOM.Documents)
                        sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    invoice.CardCode = stock.RegistrationNo;
                    invoice.DocDate = (DateTime)dto.PostingDate;
                    invoice.DocDueDate = item.DueDate ?? DateTime.Now;
                    invoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    invoice.Comments = dto.Remarks;

                    invoice.Lines.ItemDescription = item.PaymentType;
                    invoice.Lines.AccountCode = accountCode;
                    invoice.Lines.UnitPrice = (double)item.NetAmount;
                    invoice.Lines.ProjectCode = stock.RegistrationNo;

                    invoice.Lines.Add();

                    int res = invoice.Add();

                    if (res != 0)
                        throw new Exception(sap.Ocomp.GetLastErrorDescription());

                    // ✅ Get newly created DocEntry
                    int docEntry = int.Parse(sap.Ocomp.GetNewObjectKey());
                    createdDocEntries.Add(docEntry);

                    // save in local db.
                    Inovice inovice = new Inovice();
                    inovice.FormName = "Booking";
                    inovice.PostingDate = dto.PostingDate;
                    inovice.DueDate = item.DueDate;
                    inovice.ChargeType = item.PaymentType;
                    inovice.ChargeTypeId = Convert.ToInt32(item.ChargeTypeId);
                    inovice.SapAccount = accountCode;
                    inovice.Amount = (double?)item.NetAmount;
                    inovice.Remarks = item.PaymentFor;
                    inovice.MemberProfileId = dto.MemberProfileId;
                    inovice.StockCreationId = dto.StockCreationId;
                    inovice.LastModifiedUserName = dto.LastModifiedUserName;
                    inovice.CreatedOn = DateTime.Now;
                    inovice.IsActive = true;
                    inovice.IsSapPosting = true;
                    inovice.IsDeleted = false;
                    inovice.InvoiceNo = Convert.ToString(docEntry);

                    _db.Inovices.Add(inovice);
                }

                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "All invoices posted successfully"
                });
            }
            catch (Exception ex)
            {
                // 🔥 ROLLBACK: Cancel all created invoices
                foreach (var docEntry in createdDocEntries)
                {
                    try
                    {
                        var inv = (SAPbobsCOM.Documents)
                            sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                        if (inv.GetByKey(docEntry))
                        {
                            int cancelRes = inv.Cancel();

                            if (cancelRes != 0)
                            {
                                // optional: log failure but continue rollback
                                string err = sap.Ocomp.GetLastErrorDescription();
                            }
                        }
                    }
                    catch
                    {
                        // optional: log
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.BadRequest,
                    Message = ex.Message + "(All previous invoices cancelled)"
                });
            }
        }


        public ApiResponse<object> PostingDemarcationARInvoice(NewDemarcationRequest dto)
        {
            List<int> createdDocEntries = new List<int>();

            SAPOperationDb sap = new SAPOperationDb(_db);
            sap.ConnectToCompany();

            try
            {
                if (sap._a != 0)
                {
                    return new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "SAP Connection Failed"
                    };
                }

                var stock = _db.StockCreations
                    .FirstOrDefault(x => x.ID == dto.StockCreationId);

                if (stock == null)
                {
                    return new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Stock not found"
                    };
                }

                foreach (var item in dto.NewDemarcationRequestDetail)
                {
                    string accountCode = string.Empty;

                    if (item.ChargeTypeId != null)
                    {
                        accountCode = _commonBLL
                            .GetSapAccountByChargeTypeId(Convert.ToInt32(item.ChargeTypeId));
                    }

                    if (string.IsNullOrEmpty(accountCode))
                        throw new Exception("Some GL accounts are missing");

                    var invoice = (SAPbobsCOM.Documents)
                        sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    invoice.CardCode = stock.RegistrationNo;
                    invoice.DocDate = DateTime.Now;
                    invoice.DocDueDate = dto.DueDate ?? DateTime.Now;
                    invoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                    invoice.Lines.ItemDescription = item.ChargeName;
                    invoice.Lines.AccountCode = accountCode;
                    invoice.Lines.UnitPrice = (double)item.Rate;
                    invoice.Lines.ProjectCode = stock.RegistrationNo;

                    invoice.Lines.Add();

                    int res = invoice.Add();

                    if (res != 0)
                        throw new Exception(sap.Ocomp.GetLastErrorDescription());

                    int docEntry = int.Parse(sap.Ocomp.GetNewObjectKey());
                    createdDocEntries.Add(docEntry);
                }

                return new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "All invoices posted successfully",
                    Data = createdDocEntries
                };
            }
            catch (Exception ex)
            {
                // rollback
                foreach (var docEntry in createdDocEntries)
                {
                    try
                    {
                        var inv = (SAPbobsCOM.Documents)
                            sap.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                        if (inv.GetByKey(docEntry))
                        {
                            int cancelRes = inv.Cancel();

                            if (cancelRes != 0)
                            {
                                string err = sap.Ocomp.GetLastErrorDescription();
                            }
                        }
                    }
                    catch { }
                }

                return new ApiResponse<object>
                {
                    Code = ResponseCode.BadRequest,
                    Message = ex.Message + " (All previous invoices cancelled)"
                };
            }
        }

        public Response_Result PostingTransferRecieptSellerARInvoice(TransferReceiptProcessing transferReceipt)
        {
            Response_Result response = new Response_Result();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {

                    foreach (var item in transferReceipt.GovtSellerCharges)
                    {
                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = transferReceipt.CreatedOn;
                        var stock = _db.StockCreations.Where(i => i.ID == transferReceipt.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        //SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)transferReceipt.SellerId);
                        //if (cardNameAndCardCode != null)
                        //{
                        //    oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        //}
                        oInvoice.CardCode = stock.RegistrationNo;
                        oInvoice.TrackingNumber = Convert.ToString(transferReceipt.SellerId);
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }

                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        oInvoice.UserFields.Fields.Item("U_ChN").Value = transferReceipt.ChallanNoSellerTaxes;
                        oInvoice.UserFields.Fields.Item("U_ChDate").Value = DateTime.Now.Date;
                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = transferReceipt.RegistrationNo;

                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Ar Invoice Posted Successfully";

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                        }
                    }
                }

                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }
        }


        public Response_Result PostingTransferRecieptBuyerARInvoice(TransferReceiptProcessing transferReceipt)
        {
            Response_Result response = new Response_Result();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    List<GovtBuyerCharges> buyercharges;

                    buyercharges = transferReceipt.GovtBuyerCharges.ToList();

                    foreach (var item in buyercharges)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = transferReceipt.CreatedOn;
                        oInvoice.DocDueDate = DateTime.Now.AddYears(1);
                        var stock = _db.StockCreations.Where(i => i.ID == transferReceipt.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        //SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)transferReceipt.BuyerId);
                        //if (cardNameAndCardCode != null)
                        //{
                        //    oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        //}
                        oInvoice.CardCode = stock.RegistrationNo;
                        oInvoice.TrackingNumber = Convert.ToString(transferReceipt.BuyerId);
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }

                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        oInvoice.UserFields.Fields.Item("U_ChN").Value = transferReceipt.ChallanNoBuyerTaxes;
                        oInvoice.UserFields.Fields.Item("U_ChDate").Value = DateTime.Now.Date;
                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = transferReceipt.RegistrationNo;

                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            string message = sapconnection.Ocomp.GetLastErrorDescription();


                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;

                        }
                    }
                }

                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }
        }


        public Response_Result PostingARInvoiceForFileRequest(FileDocDupRequest file)
        {
            Response_Result response = new Response_Result();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {
                    foreach (var item in file.FileDocDupRequestedCharges)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = DateTime.Now;
                        var stock = _db.StockCreations.Where(i => i.ID == file.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)file.MemberProfileId);
                        if (cardNameAndCardCode != null)
                        {
                            oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        }
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }

                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;


                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = stock.RegistrationNo;
                        // oInvoice.UserFields.Fields.Item("U_InvCat").Value = "Booking Processing Charges";
                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Ar Invoice Posted Successfully";

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                        }
                    }
                }

                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }
        }

        public Response_Result PostingARInvoiceForFileVerificationRequest(FileVerificationRequest file)
        {
            Response_Result response = new Response_Result();
            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);

                sapconnection.ConnectToCompany();
                if (sapconnection._a == 0)
                {

                    foreach (var item in file.FileVerificationRequestCharges)
                    {

                        SAPbobsCOM.Documents oInvoice = sapconnection.Ocomp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        oInvoice.DocDate = DateTime.Now;
                        var stock = _db.StockCreations.Where(i => i.ID == file.StockCreationId).FirstOrDefault();
                        var operationsDetails = _db.SAPOperations.FirstOrDefault();
                        SapCardNameAndCardCode cardNameAndCardCode = GetSapCardNameAndCardCodeForOperation((int)file.MemberProfileId);
                        if (cardNameAndCardCode != null)
                        {
                            oInvoice.CardCode = cardNameAndCardCode.CardCode;
                        }
                        if (stock != null)
                        {
                            oInvoice.Project = stock.RegistrationNo;
                            oInvoice.NumAtCard = stock.PropertyNo;
                        }

                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;

                        oInvoice.Lines.AccountCode = item.SapAccount;
                        oInvoice.Lines.ProjectCode = stock.RegistrationNo;

                        oInvoice.Lines.UnitPrice = (double)item.Amount;
                        oInvoice.Lines.Add();


                        int finalresult = oInvoice.Add();
                        if (finalresult == 0)
                        {
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                            response.message = "Ar Invoice Posted Successfully";

                        }
                        else
                        {
                            string message = sapconnection.Ocomp.GetLastErrorDescription();
                            response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                            response.message = message;
                        }
                    }
                }

                else
                {
                    response.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                    response.message = "Connection Failed";

                }
                return response;
            }
            catch (Exception exception3)
            {
                response.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response.message = exception3.Message + response.message;
                return response;
            }
        }

        #region Mobile app Maint Bills

        [AllowAnonymous]
        [HttpGet]
        [Route("GetBillList")]
        //[ValidateSession]
        public IActionResult GetBillList(string? registrationNo = null, string? ConsumerNo = null)
        {
            try
            {
                var headerKey = Request.Headers["X-SAP-KEY"].ToString();

                if (string.IsNullOrEmpty(headerKey) || headerKey != SAP_SECURITY_KEY)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = "Invalid security key",
                        Data = null
                    });
                }

                if (string.IsNullOrWhiteSpace(registrationNo) && int.TryParse(ConsumerNo, out int consumerId))
                {
                    registrationNo = _db.StockCreations
                        .Where(x => x.ID == consumerId)
                        .Select(x => x.RegistrationNo)
                        .FirstOrDefault();
                }

                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription(),
                        Data = null
                    });
                }

                SAPbobsCOM.Recordset orecord =
                    (SAPbobsCOM.Recordset)sapconnection.Ocomp.GetBusinessObject(
                        SAPbobsCOM.BoObjectTypes.BoRecordset);


                string query = $@"
SELECT
    a.""Project"" AS ""RegistrationNo"",
    MIN(a.""DocDueDate"") AS ""DocDueDate"",
    a.""U_BillMnth"" AS ""BillMonth"",
    SUM(a.""DocTotal"") AS ""Bill"",
    SUM(a.""PaidToDate"") AS ""Paid"",
    SUM(a.""DocTotal"" - a.""PaidToDate"") AS ""Outstanding"",
    (
        SELECT IFNULL(SUM(x.""LineTotal"" + x.""VatSum""),0)
        FROM INV1 x
        INNER JOIN OINV y
            ON x.""DocEntry"" = y.""DocEntry""
        WHERE y.""U_BillReferenceNo"" = a.""U_BillReferenceNo""
          AND x.""AcctCode"" = 'R102010003'
    ) AS ""Surcharge"",
    CASE
        WHEN SUM(a.""PaidToDate"") = 0 THEN 'Not Paid'
        WHEN SUM(a.""PaidToDate"") >= SUM(a.""DocTotal"") THEN 'Paid'
        ELSE 'Partially Paid'
    END AS ""Status""
FROM OINV a
WHERE a.""CANCELED"" = 'N'
  AND IFNULL(a.""U_BillReferenceNo"", '') <> ''
  AND  a.""Project"" = '{registrationNo}'
GROUP BY
    a.""Project"",
    a.""U_BillMnth"",
    a.""U_BillReferenceNo""
ORDER BY
    MIN(a.""DocDueDate"") DESC";

                orecord.DoQuery(query);

                List<BillListDTO> result = new List<BillListDTO>();

                while (!orecord.EoF)
                {
                    result.Add(new BillListDTO
                    {
                        RegistrationNo = orecord.Fields.Item("RegistrationNo").Value?.ToString(),

                        DueDate = orecord.Fields.Item("DocDueDate").Value == null
                            ? null
                            : Convert.ToDateTime(orecord.Fields.Item("DocDueDate").Value),

                        BillMonth = orecord.Fields.Item("BillMonth").Value?.ToString(),

                        Bill = orecord.Fields.Item("Bill").Value == null
                            ? 0
                            : Convert.ToDecimal(orecord.Fields.Item("Bill").Value),

                        Paid = orecord.Fields.Item("Paid").Value == null
                            ? 0
                            : Convert.ToDecimal(orecord.Fields.Item("Paid").Value),

                        Outstanding = orecord.Fields.Item("Outstanding").Value == null
                            ? 0
                            : Convert.ToDecimal(orecord.Fields.Item("Outstanding").Value),

                        Surcharge = orecord.Fields.Item("Surcharge").Value == null
                            ? 0
                            : Convert.ToDecimal(orecord.Fields.Item("Surcharge").Value),

                        Status = orecord.Fields.Item("Status").Value?.ToString()
                    });

                    orecord.MoveNext();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DownloadBill")]
        //[ValidateSession]
        public IActionResult DownloadBill(string registrationNo, string billMonth, string? billFor = "Constructed")
        {
            try
            {
                //var headerKey = Request.Headers["X-SAP-KEY"].ToString();

                //if (string.IsNullOrEmpty(headerKey) || headerKey != SAP_SECURITY_KEY)
                //{
                //    return Unauthorized(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.Error,
                //        Message = "Invalid security key",
                //        Data = null
                //    });
                //}

                decimal billSurchargePercentage = _db.SAPOperations.FirstOrDefault()?.BillDiscountPercentage ?? 0;

                if (string.IsNullOrEmpty(registrationNo))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Registration No is required",
                        Data = null
                    });
                }

                // =========================
                // LOCAL DB (UNCHANGED)
                // =========================
                var bill = _db.StockCreations
                    .Where(x => x.RegistrationNo == registrationNo)
                    .Select(x => new BillPrintDTO
                    {
                        StockId = x.ID,
                        RegistrationNo = x.RegistrationNo,
                        BillPrintRegistrationNo = x.BillPrintRegistrationNo,
                        PropertyNo = string.IsNullOrEmpty(x.BillPrintPropertyNo)
                            ? x.PropertyNo
                            : x.BillPrintPropertyNo,
                        BillPrintPropertyNo = x.BillPrintPropertyNo,
                        Area = x.ActualSize,
                        UOM = x.ActualSizeUnit,
                        Size = $"{x.ActualSize} {x.ActualSizeUnit}",
                        MemberId = x.MemberProfile.Id,
                        MemberName = string.IsNullOrEmpty(x.BillPrintName)
                            ? x.MemberProfile.MemberName
                            : x.BillPrintName,
                        Address = string.IsNullOrEmpty(x.BillPrintAddress)
                            ? x.MemberProfile.PermanentAddress
                            : x.BillPrintAddress,
                        MobileNo = x.MemberProfile.Mobile,
                        WhatsAppNo = x.MemberProfile.WhatsAppNo,
                        DueDate = DateTime.Now.Date,
                        DocDate = DateTime.Now.AddDays(10).Date,
                        BillMonth = billMonth,
                        MaintenceAdvanceBillPaid = x.MaintenceAdvanceBillPaid
                    })
                    .FirstOrDefault();

                if (bill == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "No record found",
                        Data = null
                    });
                }

                var billResult = GetFixedArrearsAndAdvanceByRegistrationNo(registrationNo, billMonth);

                bill.Arrears = billResult.Arrears;
                bill.Advance = (int)Convert.ToInt64(billResult.AdvancePayment);
                bill.MaintenceAdvanceBillPaid = (int)Convert.ToInt64(billResult.AdvancePayment);

                string blockName = GetBlock(bill.RegistrationNo);
                bill.PropertyNo = string.IsNullOrEmpty(bill.BillPrintPropertyNo)
                    ? $"{blockName}-{bill.PropertyNo}"
                    : bill.BillPrintPropertyNo;

                var tenant = _db.TanantDetail
                    .FirstOrDefault(t => t.IsActive && t.StockCreationID == bill.StockId);

                bill.TenantMember = tenant?.Name ?? "N/A";
                bill.TenantMobileNo = tenant?.Mobile ?? "N/A";

               
                // =========================
                // SAP CONNECTION
                // =========================
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription(),
                        Data = null
                    });
                }

                var orecord = (SAPbobsCOM.Recordset)sapconnection.Ocomp
                    .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // =========================
                // SINGLE OPTIMIZED SAP QUERY
                // =========================
                string query = $@"
SELECT
    a.""Project"" AS ""Project"",
    a.""U_BillReferenceNo"" AS ""BillReferenceNo"",

    /* Only unpaid / partially paid invoices */
   (
    SELECT STRING_AGG(TO_NVARCHAR(d.""DocEntry""), ',')
    FROM OINV d
    WHERE d.""CANCELED"" = 'N'
      AND d.""Project"" = a.""Project""
      AND IFNULL(d.""U_BillReferenceNo"", '') <> ''
      AND (
            d.""U_BillMnth"" = a.""U_BillMnth""
            OR (d.""U_BillMnth"" < a.""U_BillMnth""
                AND (d.""DocTotal"" - d.""PaidToDate"") > 0)
          )
) AS ""DocEntries"",

    MIN(a.""DocDate"") AS ""DocDate"",
    MIN(a.""DocDueDate"") AS ""DocDueDate"",
    SUM(a.""DocTotal"") AS ""Bill"",
    MIN(a.""Comments"") AS ""Remarks"",

    /* ARREARS (Only Outstanding Previous Bills) */
    (
        SELECT IFNULL(SUM(x.""DocTotal"" - x.""PaidToDate""), 0)
        FROM OINV x
        WHERE x.""CANCELED"" = 'N'
          AND IFNULL(x.""U_BillReferenceNo"", '') <> ''
          AND x.""Project"" = a.""Project""
          AND x.""U_BillMnth"" < a.""U_BillMnth""
          AND (x.""DocTotal"" - x.""PaidToDate"") > 0
    ) AS ""Arrears"",

    /* CHARGES */
    (
        SELECT IFNULL(
            STRING_AGG(
                b.""Dscription"" || '||' ||
                TO_NVARCHAR(b.""LineTotal"" + b.""VatSum""),
                '##'
            ),
            ''
        )
        FROM INV1 b
        INNER JOIN OINV a2
            ON a2.""DocEntry"" = b.""DocEntry""
        WHERE a2.""Project"" = a.""Project""
          AND a2.""U_BillMnth"" = a.""U_BillMnth""
          AND a2.""CANCELED"" = 'N'
    ) AS ""Charges"",

    /* HISTORY (Last 6 Months) */
    (
        SELECT IFNULL(
            STRING_AGG(
                h.""U_BillMnth"" || '||' ||
                TO_NVARCHAR(h.""Amount"") || '||' ||
                TO_NVARCHAR(h.""Pending"") || '||' ||
                h.""Status"",
                '##'
            ),
            ''
        )
        FROM
        (
            SELECT
                h.""U_BillMnth"",
                SUM(h.""DocTotal"") AS ""Amount"",
                SUM(h.""DocTotal"" - h.""PaidToDate"") AS ""Pending"",
                CASE
                    WHEN SUM(h.""DocTotal"" - h.""PaidToDate"") = 0 THEN 'Paid'
                    WHEN SUM(h.""PaidToDate"") = 0 THEN 'Unpaid'
                    ELSE 'Partially Paid'
                END AS ""Status""
            FROM OINV h
            WHERE h.""CANCELED"" = 'N'
              AND h.""Project"" = a.""Project""
              AND IFNULL(h.""U_BillReferenceNo"", '') <> ''
              AND TO_DATE(h.""U_BillMnth"" || '-01')
                    BETWEEN ADD_MONTHS(TO_DATE(a.""U_BillMnth"" || '-01'), -6)
                        AND ADD_MONTHS(TO_DATE(a.""U_BillMnth"" || '-01'), -1)
            GROUP BY h.""U_BillMnth""
        ) h
    ) AS ""History""

FROM OINV a
WHERE a.""CANCELED"" = 'N'
  AND a.""Project"" = '{registrationNo}'
  AND a.""U_BillMnth"" = '{billMonth}'
  AND IFNULL(a.""U_BillReferenceNo"", '') <> ''

GROUP BY
    a.""Project"",
    a.""U_BillMnth"",
    a.""U_BillReferenceNo"";
";

                orecord.DoQuery(query);

                if (orecord.EoF)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "No SAP record found",
                        Data = null
                    });
                }

                // =========================
                // READ SAP RESULT
                // =========================
                decimal sapBill = Convert.ToDecimal(orecord.Fields.Item("Bill").Value);
                decimal sapArrears = (decimal)bill.Arrears;
                string remarks = orecord.Fields.Item("Remarks").Value?.ToString();
                string billReferenceNo = orecord.Fields.Item("BillReferenceNo").Value?.ToString();
                string docEntriesRaw = orecord.Fields.Item("DocEntries").Value?.ToString();
                string[] docEntries = string.IsNullOrEmpty(docEntriesRaw)
                    ? new string[0]
                    : docEntriesRaw.Split(',', StringSplitOptions.RemoveEmptyEntries);
                DateTime docDate = Convert.ToDateTime(orecord.Fields.Item("DocDate").Value);
                DateTime dueDate = Convert.ToDateTime(orecord.Fields.Item("DocDueDate").Value);

                string chargesRaw = orecord.Fields.Item("Charges").Value?.ToString();
                string historyRaw = orecord.Fields.Item("History").Value?.ToString();

                bill.DocDate = docDate;
                bill.DueDate = dueDate;
                bill.Remarks = remarks;


                // =========================
                // PARSE CHARGES
                // =========================
                var charges = new List<DownloadBillChargeDTO>();

                if (!string.IsNullOrWhiteSpace(chargesRaw))
                {
                    var rows = chargesRaw.Split("##", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var r in rows)
                    {
                        var p = r.Split("||");
                        if (p.Length >= 2)
                        {
                            charges.Add(new DownloadBillChargeDTO
                            {
                                ChargeType = p[0],
                                Amount = decimal.Parse(p[1])
                            });
                        }
                    }
                }

                // =========================
                // PARSE HISTORY
                // =========================
                var previousBills = new List<PreviousBillDetailDTO>();

                if (!string.IsNullOrWhiteSpace(historyRaw))
                {
                    var rows = historyRaw.Split("##", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var r in rows)
                    {
                        var p = r.Split("||");

                        if (p.Length >= 4)
                        {
                            previousBills.Add(new PreviousBillDetailDTO
                            {
                                Month = DateTime.Parse(p[0]).ToString("MMM-yyyy"),
                                TotalAmount = Convert.ToInt32(decimal.Parse(p[1])),
                                PendingAmount = Convert.ToInt32(decimal.Parse(p[2])),
                                Status = p[3]
                            });
                        }
                    }
                }

                // =========================
                // BILL CALCULATION (UNCHANGED LOGIC)
                // =========================
                int currentBill = (int)Math.Round(sapBill);

                bill.Arrears = bill.Arrears;
                bill.CurrentBill = currentBill;
                bill.BillBeforeDueDate = currentBill;
                bill.BillAfterDueDate = currentBill;

                int arrears = (int)bill.Arrears;
                int advance = bill.MaintenceAdvanceBillPaid;
                int remaingArrears = arrears;

                if (advance > 0)
                {
                    if (advance >= arrears)
                    {
                        bill.MaintenceAdvanceBillPaid = advance - arrears;
                        remaingArrears = 0;
                    }
                    else
                    {
                        bill.MaintenceAdvanceBillPaid = arrears - advance;
                        remaingArrears = arrears - advance;
                    }
                }

                bill.CurrentBill = currentBill;

                int surchangeableBill = currentBill;

                if (bill.MaintenceAdvanceBillPaid > 0)
                {
                    if (bill.MaintenceAdvanceBillPaid >= currentBill)
                    {
                        bill.MaintenceAdvanceBillPaid -= currentBill;
                        bill.BillBeforeDueDate = 0 + remaingArrears;

                        surchangeableBill = 0;
                    }
                    else
                    {
                        surchangeableBill = currentBill - bill.MaintenceAdvanceBillPaid;
                        bill.BillBeforeDueDate = currentBill - bill.MaintenceAdvanceBillPaid + remaingArrears;
                        bill.MaintenceAdvanceBillPaid = 0;
                    }
                }
                else
                {
                    surchangeableBill = currentBill;
                    bill.BillBeforeDueDate = currentBill + remaingArrears;
                }


                bill.SurchargeAfterDueDate = (int)(surchangeableBill * billSurchargePercentage / 100);
                bill.BillAfterDueDate = bill.BillBeforeDueDate + bill.SurchargeAfterDueDate;


                bill.MaintenceAdvanceBillPaid = bill.MaintenceAdvanceBillPaid * (-1);

                bill.QrString = "qwertystring";
                bill.BillMonth = DateTime.Parse(billMonth + "-01").ToString("MMM-yyyy");

                // =========================
                // RESPONSE
                // =========================
                var result = new DownloadBillDTO
                {
                    BillReferenceNo = billReferenceNo,
                    DocEntry = docEntries,
                    MemberId = bill.StockId,
                    MemberName = bill.MemberName,
                    Address = bill.Address,
                    PropertyNo = bill.PropertyNo,
                    RegistrationNo = !string.IsNullOrEmpty(bill.BillPrintRegistrationNo) ? bill.BillPrintRegistrationNo : bill.RegistrationNo,
                    DueDate = bill.DueDate,
                    DocDate = bill.DocDate,
                    BillMonth = bill.BillMonth,
                    Arrears = bill.Arrears,
                    CurrentBill = bill.CurrentBill,
                    BillBeforeDueDate = bill.BillBeforeDueDate,
                    BillAfterDueDate = bill.BillAfterDueDate,
                    Advance = bill.Advance,
                    Balance = bill.MaintenceAdvanceBillPaid,
                    Remarks = bill.Remarks,
                    Charges = charges,
                    PreviousBills = previousBills
                };

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private (decimal Arrears, decimal AdvancePayment) GetFixedArrearsAndAdvanceByRegistrationNo(string registrationNo, string billMonth)
        {
            decimal arrears = 0;
            decimal advancePayment = 0;
            SAPOperationDb sap = new SAPOperationDb(_db);
            sap.ConnectToCompany();
            try
            {
                if (sap._a != 0)
                    return (0, 0);

                string query = $@"
    SELECT
        SUM(""Arrears"") AS ""Arrears"",
        SUM(""Advance Payment"") AS ""Advance Payment""
    FROM
    (
        SELECT
            IFNULL(SUM(A.""DocTotal"" - A.""PaidToDate""), 0) AS ""Arrears"",
            0 AS ""Advance Payment""
        FROM ""DHA_LIVE"".""OINV"" A
        WHERE
            A.""CANCELED"" = 'N'
            AND A.""Project"" = '{registrationNo}'
            AND IFNULL(A.""U_BillReferenceNo"", '') <> ''
            AND A.""U_BillMnth"" < '{billMonth}'
            AND (A.""DocTotal"" - A.""PaidToDate"") > 0
        UNION ALL
        SELECT
            0 AS ""Arrears"",
            IFNULL(SUM(RC.""OpenBal""), 0) AS ""Advance Payment""
        FROM ""DHA_LIVE"".""ORCT"" RC
        WHERE
            RC.""PayNoDoc"" = 'Y'
            AND RC.""Canceled"" = 'N'
            AND IFNULL(RC.""U_Source"", '') <> ''
            AND RC.""OpenBal"" > 0
            AND RC.""CardCode"" = '{registrationNo}'
    ) X";

                SAPbobsCOM.Recordset rs =
                    (SAPbobsCOM.Recordset)sap.Ocomp.GetBusinessObject(
                        SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);
                if (!rs.EoF)
                {
                    arrears = Convert.ToDecimal(
                        rs.Fields.Item("Arrears").Value ?? 0);
                    advancePayment = Convert.ToDecimal(
                        rs.Fields.Item("Advance Payment").Value ?? 0);
                }
            }
            catch
            {
                arrears = 0;
                advancePayment = 0;
            }
            finally
            {
                if (sap.Ocomp != null && sap.Ocomp.Connected)
                    sap.Ocomp.Disconnect();
            }
            return (arrears, advancePayment);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("MarkInvoicesPaid")]
        public IActionResult MarkInvoicesPaid([FromBody] MarkPaidRequest request)
        {
            try
            {
                // =========================
                // SECURITY CHECK
                // =========================
                var headerKey = Request.Headers["X-SAP-KEY"].ToString();

                if (string.IsNullOrEmpty(headerKey) || headerKey != SAP_SECURITY_KEY)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = "Invalid security key",
                        Data = null
                    });
                }

                if (request?.DocEntries == null || request.DocEntries.Length == 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "DocEntries required",
                        Data = null
                    });
                }

                // =========================
                // SAP CONNECTION
                // =========================
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = sapconnection.Ocomp.GetLastErrorDescription(),
                        Data = null
                    });
                }

                var company = sapconnection.Ocomp;

                var updated = new List<int>();
                var failed = new List<string>();

                // =========================
                // CREATE PAYMENT OBJECT (ONCE)
                // =========================
                SAPbobsCOM.Payments payment =
                    (SAPbobsCOM.Payments)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                payment.DocDate = DateTime.Now;
                payment.TransferDate = DateTime.Now;
                payment.TransferSum = 0;
                payment.CashSum = 0;

                bool hasAny = false;
                string cardCode = null;

                // =========================
                // LOOP INVOICES
                // =========================
                foreach (var doc in request.DocEntries)
                {
                    if (!int.TryParse(doc, out int docEntry))
                    {
                        failed.Add($"Invalid DocEntry: {doc}");
                        continue;
                    }

                    try
                    {
                        SAPbobsCOM.Recordset rs =
                            (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        rs.DoQuery($@"
                    SELECT ""CardCode"", ""DocTotal"" - ""PaidToDate"" AS ""Balance""
                    FROM OINV
                    WHERE ""DocEntry"" = {docEntry}");

                        if (rs.EoF)
                        {
                            failed.Add($"Not found: {docEntry}");
                            continue;
                        }

                        string invCardCode = rs.Fields.Item("CardCode").Value.ToString();
                        decimal balance = Convert.ToDecimal(rs.Fields.Item("Balance").Value);

                        // set CardCode once (SAP requirement: same customer per payment)
                        if (cardCode == null)
                            cardCode = invCardCode;

                        if (cardCode != invCardCode)
                        {
                            failed.Add($"DocEntry {docEntry}: different customer, cannot combine payment");
                            continue;
                        }

                        payment.CardCode = cardCode;

                        // =========================
                        // ADD INVOICE TO PAYMENT
                        // =========================
                        payment.Invoices.DocEntry = docEntry;
                        payment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                        payment.Invoices.SumApplied = (double)balance;

                        payment.Invoices.Add();

                        updated.Add(docEntry);
                        hasAny = true;
                    }
                    catch (Exception ex)
                    {
                        failed.Add($"{docEntry}: {ex.Message}");
                    }
                }

                // =========================
                // ADD PAYMENT TO SAP
                // =========================
                if (!hasAny)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = "No valid invoices to process",
                        Data = new { updated, failed }
                    });
                }

                int result = payment.Add();

                if (result != 0)
                {
                    company.GetLastError(out int code, out string msg);

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = msg,
                        Data = new { updated, failed }
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Payment successfully posted",
                    Data = new
                    {
                        Updated = updated,
                        Failed = failed
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        private string GetBlock(string? registrationNo)
        {
            string blockId = _db.StockCreations.FirstOrDefault(x => x.RegistrationNo == registrationNo).Block;

            return _db.Blocks.FirstOrDefault(x => x.ID == Convert.ToInt32(blockId)).Description ?? "N/A";
        }

        #endregion

        [HttpGet]
        [Route("CancelInovices")]
        public Response_Result CancelInvoices(string cancelInvoices)
        {
            Response_Result response = new Response_Result();

            if (string.IsNullOrEmpty(cancelInvoices))
            {
                response.code = (int)Global_Utility.ResponseCode.error;
                response.message = "DocEntry numbers are required";
                return response;
            }

            using var transaction = _db.Database.BeginTransaction();

            try
            {
                SAPOperationDb sapconnection = new SAPOperationDb(_db);
                sapconnection.ConnectToCompany();

                if (sapconnection._a != 0)
                {
                    response.code = (int)Global_Utility.ResponseCode.error;
                    response.message = "SAP Connection Failed";
                    return response;
                }

                // 1️⃣ Convert comma separated values into list
                var docEntries = cancelInvoices
                                    .Split(',')
                                    .Select(x => x.Trim())
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .ToList();

                int successCount = 0;
                int failCount = 0;

                foreach (var entry in docEntries)
                {
                    SAPbobsCOM.Recordset rs = null;
                    SAPbobsCOM.Documents invoice = null;

                    try
                    {
                        rs = (SAPbobsCOM.Recordset)sapconnection.Ocomp
                            .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        string query = $@"
                            SELECT ""DocEntry""
                            FROM ""OINV""
                            WHERE ""DocEntry"" = '{entry}'
                            AND ""DocStatus"" = 'O'
                        ";

                        rs.DoQuery(query);

                        if (rs.RecordCount == 0)
                        {
                            failCount++;
                            continue;
                        }

                        int docEntry = Convert.ToInt32(rs.Fields.Item("DocEntry").Value);

                        invoice = (SAPbobsCOM.Documents)sapconnection.Ocomp
                            .GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                        if (invoice.GetByKey(docEntry))
                        {
                            if (invoice.DocumentStatus == SAPbobsCOM.BoStatus.bost_Open)
                            {
                                SAPbobsCOM.Documents cancelDoc = invoice.CreateCancellationDocument();

                                int res = cancelDoc.Add();

                                if (res != 0)
                                {
                                    sapconnection.Ocomp.GetLastError(out int errCode, out string errMsg);
                                    failCount++;
                                }
                                else
                                {
                                    successCount++;
                                }
                            }
                            else
                            {
                                failCount++;
                            }
                        }
                    }
                    catch
                    {
                        failCount++;
                    }
                    finally
                    {
                        if (rs != null)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

                        if (invoice != null)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(invoice);
                    }
                }

                _db.SaveChanges();
                transaction.Commit();

                response.code = (int)Global_Utility.ResponseCode.succcess;
                response.message = $"Completed. Success: {successCount}, Failed: {failCount}";

                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                response.code = (int)Global_Utility.ResponseCode.exception;
                response.message = ex.Message;

                return response;
            }
        }

    }
}

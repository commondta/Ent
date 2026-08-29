using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Dynamic;
using DataLayer;
using Payroll_HCC.Filters;
using Payroll_HCC.Infrastructure;

namespace Payroll_HCC.Controllers
{
    [Payroll_HCC.Filters.AdminAuthorize]
    public class TransactionController : Controller
    {
        static string con_string = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string conStringHCC = ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString;
        PaymentsAndDeductions paymentsAndDeductionsObj = new PaymentsAndDeductions(con_string);
        List<PaymentsAndDeductions2Model> padList;
        PayPeriod payPeriodObj = new PayPeriod(con_string);
        EmployeeCategoryMaster empCategMasterObj = new EmployeeCategoryMaster(con_string);
        PayrollProcess payrollProcessObj = new PayrollProcess(con_string);
        PayElements payElementObj = new PayElements(con_string);
        Company compObj = new Company(conStringHCC);

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    ViewBag.Company = compObj.getAll();
        //}

        // GET: Transaction
        public ActionResult MonthlyAttendanceSheet()
        {
            return View();
        }

        public ActionResult MonthlyAddition()
        {
            return View();
        }

        public ActionResult MonthlyDeduction()
        {
            return View();
        }
        
        public ActionResult PayrollProcess()
        {
            ViewBag.EmployeeCategories = empCategMasterObj.getCategories();
            ViewData["PayPeriod"] = payPeriodObj.getAll();
            ViewBag.PayElements = payElementObj.getCodeDescription();
            return View();
        }

        [HttpPost]
        public JsonResult getPayrollProcess()
        {
            return Json(payrollProcessObj.getPayrollProcess(), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Payroll.Processing", PermissionAction.Create)]
        [HttpPost]
        public JsonResult PayrollProcessCreate(PayrollProcessParentModel parentData, List<IDictionary<string, object>> childData)
        {
            childData[0].Remove("payroll-process-table_length");
            var result = payrollProcessObj.Insert(parentData, childData);
            string title = "Payroll run " + parentData.PayMonth + " (" + parentData.EmployeeType + ", " + parentData.PayPeriod + ")";
            App.Log("Process", "Payroll.Processing", title + " - " + childData.Count + " employees");
            // Approval mechanism: every payroll run is queued for approval (unless Approval Setup switched it off).
            try
            {
                if (App.Approvals.RequiresApproval("PayrollRun"))
                App.Approvals.Submit("PayrollRun", parentData.DocumentNo ?? title, title,
                    "Document " + parentData.DocumentNo + " dated " + parentData.DocumentDate.ToString("dd MMM yyyy") + "; " + childData.Count + " employees; status " + parentData.Status,
                    App.CurrentUsername);
            }
            catch (Exception ex) { FileLogger.Error("Could not queue payroll run for approval.", ex); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public void PayrollProcessUpdate(PayrollProcessParentModel parentData, List<IDictionary<string, object>> childData)
        //{
        //    childData[0].Remove("payroll-process-table_length");
        //    List<PayrollProcessChildModel> childIds = new List<PayrollProcessChildModel>();
        //    childIds = payrollProcessObj.Insert(parentData, childData);
        //}

        [RequirePermission("Payroll.Processing", PermissionAction.Delete)]
        [HttpPost]
        public void PayrollProcessDelete(string id)
        {
            App.Log("Delete", "Payroll.Processing", "Deleted payroll run #" + id);
            payrollProcessObj.Delete(id);
        }

        [RequirePermission("Payroll.Processing", PermissionAction.Edit)]
        [HttpPost]
        public void PayrollProcessUpdate(PayrollProcessParentModel parentData, List<IDictionary<string, object>> childData)
        {
            App.Log("Update", "Payroll.Processing", "Updated payroll run " + (parentData == null ? "" : parentData.DocumentNo));
            childData[0].Remove("payroll-process-table_length");
            payrollProcessObj.Update(parentData, childData);
        }

        [HttpPost]
        public JsonResult CalculatePayPayrollProcess(DateTime fromDate, DateTime toDate)
        {
            return Json(payrollProcessObj.CalculatePay(fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPayElements()
        {
            return Json(payElementObj.getDescription(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyAttendanceSheet()
        {
            return View();
        }

        public ActionResult LeaveApplication()
        {
            return View();
        }

        public ActionResult OvertimeProcess()
        {
            return View();
        }

        public ActionResult LoanApplication()
        {
            return View();
        }

        public ActionResult LeaveSettlement()
        {
            return View();
        }

        public ActionResult PaymentsAndDeductions()
        {
            ViewData["PayPeriod"] = payPeriodObj.getAll();
            ViewBag.paymentsAndDeductionsParent = paymentsAndDeductionsObj.getAllParent();
            ViewBag.paymentsAndDeductionsChild = paymentsAndDeductionsObj.getAllChild();
            return View();
        }
        
        //[HttpPost]
        //public void PaymentsAndDeductionsCreate(PaymentsAndDeductions2Model parentData, PaymentsAndDeductions2Model[] childData)
        //{
        //    List<PaymentsAndDeductions2Model> childIds = new List<PaymentsAndDeductions2Model>();
        //    paymentsAndDeductionsObj.InsertParent(parentData);
        //    Insert(parentData, childData);
        //    return Json(childIds, JsonRequestBehavior.AllowGet);
        //}

        //[RequirePermission("Payroll.PaymentsDeductions", PermissionAction.Create)]
        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase data, string PayPeriod, DateTime DocumentDate, string Status)
        {
            App.Log("Import", "Payroll.PaymentsDeductions", "Imported payments/deductions for " + PayPeriod + " from " + (data == null ? "(no file)" : data.FileName));
            padList = new List<PaymentsAndDeductions2Model>();
            PaymentsAndDeductions2Model parentData = new PaymentsAndDeductions2Model();
            parentData.PayPeriod = PayPeriod;
            parentData.DocumentDate = DocumentDate;
            parentData.Status = Status;

            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file == null || file.ContentLength == 0)
                return Json("Error occured while importing file.", JsonRequestBehavior.AllowGet);

            try
            {
                var rows = Infrastructure.ExcelImport.ReadFirstSheet(file.InputStream, skipHeaderRow: true);
                int parentId = paymentsAndDeductionsObj.InsertParent(parentData);

                foreach (var row in rows)
                {
                    PaymentsAndDeductions2Model obj = new PaymentsAndDeductions2Model();
                    obj.ParentID = parentId;
                    Func<int, string> col = i => row.Length > i ? row[i] : "";
                    obj.PayrollName = col(0);
                    obj.EmployeeName = col(1);
                    obj.EmployeeID = Convert.ToInt16(col(2));
                    obj.PayrollPayElement = col(3);
                    obj.TransactionType = col(4);
                    obj.EffectiveDate = Convert.ToDateTime(col(5));
                    obj.EndDate = Convert.ToDateTime(col(6));
                    obj.Recurrence = col(7);
                    obj.Amount = float.Parse(col(8));
                    obj.Currency = col(9);
                    obj.Comments = col(10);
                    padList.Add(obj);
                    paymentsAndDeductionsObj.InsertChild(obj);
                }
                paymentsAndDeductionsObj.UpdateEmpSalaryDetail(padList);
                return Json("File succesfully imported!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Infrastructure.FileLogger.Error("Payments & deductions Excel import failed.", ex);
                return Json("Error occured while importing file.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OvertimeFormula()
        {
            return View();
        }

        public ActionResult HourlyWages()
        {
            return View();
        }

        public static dynamic getDynamicObject()
        {
            PayElements payElementsObject = new PayElements(con_string);

            var PayrollProcessChildModel = new ExpandoObject() as IDictionary<string, object>;
            List<string> descriptions = new List<string>();
            descriptions = payElementsObject.getDescription();

            foreach (string description in descriptions)
            {
                PayrollProcessChildModel.Add(description, "123");
            }
            

            return PayrollProcessChildModel;
        }
    }
}

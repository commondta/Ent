using BusinessLayer;
using DataLayer;
using Newtonsoft.Json;
using Payroll_HCC.Models;
using Payroll_HCC.Filters;
using Payroll_HCC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    [Payroll_HCC.Filters.AdminAuthorize]
    public class MasterController : Controller
    {
        static string con_string = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string conStringHCC = ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString;
        Departmentsetup DepartmentsetupObj = new Departmentsetup(con_string);
        PayElements payElementsObj = new PayElements(con_string);
        EmployeeCategoryMaster employeeCategMasterObj = new EmployeeCategoryMaster(con_string);
        FormulaMaster formulaMasterObj = new FormulaMaster(con_string);
        PayPeriod payPeriodObj = new PayPeriod(con_string);
        Employees employeeObj = new Employees(con_string);
        TaxFormulaCalculation taxFormulaCalcObj = new TaxFormulaCalculation(con_string);
        Company compObj = new Company(conStringHCC);

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // ViewBag.Company = compObj.getAll();
            compObj.setActive();
        }

        [RequirePermission("Employees.Companies", PermissionAction.Edit)]
        [HttpPost]
        public void CompanySwitch(string id)
        {
            App.Log("Update", "Employees.Companies", "Switched active company to #" + id);
            compObj.Switch(id);
        }

        public ActionResult Company()
        {
            ViewBag.Company = compObj.getAll();
            return View();
        }

        [RequirePermission("Employees.Companies", PermissionAction.Create)]
        [HttpPost]
        public void CompanyCreate(CompanyModel company)
        {
            App.Log("Create", "Employees.Companies", "Created company " + (company == null ? "" : company.CompanyName));
            compObj.Insert(company);
        }

        [RequirePermission("Employees.Companies", PermissionAction.Delete)]
        [HttpPost]
        public void CompanyDelete(string id)
        {
            App.Log("Delete", "Employees.Companies", "Deleted company #" + id + " (and its database)");
            compObj.Delete(id);
        }

        public ActionResult DepartmentSetup()
        {
            ViewBag.DepartmentDetail = DepartmentsetupObj.getAll();
            return View();
        }

        [HttpPost]
        public JsonResult DepartmentSave(DepartmentSetupModel model)
        {
            bool isNew = model.id == 0;
            if (!App.Can("Employees.Departments", isNew ? PermissionAction.Create : PermissionAction.Edit))
                return Json(new { ok = false, message = "You do not have permission to " + (isNew ? "add" : "edit") + " departments." });
            if (model == null || string.IsNullOrWhiteSpace(model.DepartmentName)) return Json(new { ok = false, message = "Department name is required." });
            model.DepartmentName = model.DepartmentName.Trim();
            if (DepartmentsetupObj.Exists(model.DepartmentName, model.id)) return Json(new { ok = false, message = "A department with that name already exists." });
            if (isNew) DepartmentsetupObj.Insert(model); else DepartmentsetupObj.Update(model);
            App.Log(isNew ? "Create" : "Update", "Employees.Departments", (isNew ? "Added" : "Updated") + " department " + model.DepartmentName);
            return Json(new { ok = true, message = "Department " + (isNew ? "added." : "updated.") });
        }

        [RequirePermission("Employees.Departments", PermissionAction.Delete)]
        [HttpPost]
        public JsonResult DepartmentDelete(int id)
        {
            DepartmentsetupObj.Delete(id);
            App.Log("Delete", "Employees.Departments", "Deleted department #" + id);
            return Json(new { ok = true, message = "Department deleted." });
        }

        [RequirePermission("Employees.Departments", PermissionAction.Create)]
        public JsonResult UploadFile(HttpPostedFileBase data)
        {
            App.Log("Import", "Employees.Departments", "Imported departments from " + (data == null ? "(no file)" : data.FileName));
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file == null || file.ContentLength == 0)
                return Json("Can't Import", JsonRequestBehavior.AllowGet);

            try
            {
                // Parse first, replace second - a bad file must not wipe existing rows.
                var rows = Infrastructure.ExcelImport.ReadFirstSheet(file.InputStream, skipHeaderRow: true);

                DepartmentsetupObj.delete();
                foreach (var row in rows)
                {
                    DepartmentSetupModel obj = new DepartmentSetupModel();
                    obj.DepartmentName = row.Length > 0 ? row[0] : "";
                    obj.Description = row.Length > 1 ? row[1] : "";
                    if (!string.IsNullOrWhiteSpace(obj.DepartmentName))
                        DepartmentsetupObj.Insert(obj);
                }
                return Json("Succesfully Import", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Infrastructure.FileLogger.Error("Department Excel import failed.", ex);
                return Json("Can't Import", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EOBIsetup()
        {
            return View();
        }

        public ActionResult Employees()
        {
            ViewBag.Employees = employeeObj.getAll();
            ViewBag.PayElementsCfl = payElementsObj.getCfl_Employees();
            return View();
        }

        public JsonResult GetEmployees()
        {
            return Json(employeeObj.getAll(), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Employees.Profiles", PermissionAction.Create)]
        [HttpPost]
        public JsonResult EmployeesCreate(EmployeeDetailModel employeeDetail, SalaryDetailModel[] salaryDetail)
        {
            App.Log("Create", "Employees.Profiles", "Created employee " + (employeeDetail == null ? "" : employeeDetail.EmployeeNumber + " " + employeeDetail.PayrollName));
            return Json(employeeObj.Insert(employeeDetail, salaryDetail), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Employees.Profiles", PermissionAction.Edit)]
        [HttpPost]
        public void EmployeesUpdate(EmployeeDetailModel employeeDetail, SalaryDetailModel[] salaryDetail)
        {
            App.Log("Update", "Employees.Profiles", "Updated employee " + (employeeDetail == null ? "" : employeeDetail.EmployeeNumber + " " + employeeDetail.PayrollName));
            employeeObj.Update(employeeDetail, salaryDetail);
        }

        [RequirePermission("Employees.Profiles", PermissionAction.Delete)]
        [HttpPost]
        public void EmployeesDelete(string id)
        {
            App.Log("Delete", "Employees.Profiles", "Deleted employee #" + id);
            employeeObj.Delete(id);
        }

        [HttpPost]
        public JsonResult EmployeesGetFormula(string formulaMasterName)
        {
            return Json(formulaMasterObj.getFormula(formulaMasterName), JsonRequestBehavior.AllowGet);
        }


        public ActionResult EmployeeCategoryMaster()
        {
            ViewData["EmployeeCategoryMaster"] = employeeCategMasterObj.getAll();
            return View();
        }
        [RequirePermission("Employees.Categories", PermissionAction.Create)]
        [HttpPost]
        public JsonResult EmployeeCategoryMaster(EmployeeCategoryMasterModel obj)
        {
            App.Log("Update", "Employees.Categories", "Saved employee category");
            employeeCategMasterObj.Insert(obj);
            string data = employeeCategMasterObj.getLastID();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Employees.Categories", PermissionAction.Delete)]
        public void employeeCategoryMasterDelete(string id)
        {
            App.Log("Delete", "Employees.Categories", "Deleted employee category #" + id);
            employeeCategMasterObj.Delete(id);
        }

        public ActionResult PayPeriod()
        {
            ViewData["PayPeriod"] = payPeriodObj.getAll();
            return View();
        }

        [RequirePermission("Setup.PayrollPeriods", PermissionAction.Create)]
        [HttpPost]
        public JsonResult PayPeriodCreate(PayPeriodModel data)
        {
            App.Log("Create", "Setup.PayrollPeriods", "Created payroll period " + (data == null ? "" : data.Name));
            return Json(payPeriodObj.Insert(data), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Setup.PayrollPeriods", PermissionAction.Edit)]
        [HttpPost]
        public void PayPeriodUpdate(PayPeriodModel data)
        {
            App.Log("Update", "Setup.PayrollPeriods", "Updated payroll period " + (data == null ? "" : data.Name));
            payPeriodObj.Update(data);
        }

        [RequirePermission("Setup.PayrollPeriods", PermissionAction.Delete)]
        [HttpPost]
        public void PayPeriodDelete(string id)
        {
            App.Log("Delete", "Setup.PayrollPeriods", "Deleted payroll period #" + id);
            payPeriodObj.Delete(id);
        }

        public ActionResult BonusMaster()
        {
            return View();
        }

        public ActionResult FormulaMaster()
        {
            ViewBag.FormulMasterParent = formulaMasterObj.getAllParent();
            ViewBag.FormulMasterChild = formulaMasterObj.getAllChild();
            ViewBag.EmployeeCategories = employeeCategMasterObj.getCategories();
            ViewBag.PayElementsCfl = payElementsObj.getCfl();
            return View();
        }

        [RequirePermission("Setup.PayrollFormulas", PermissionAction.Create)]
        [HttpPost]
        public JsonResult FormulaMasterCreate(FormulaMasterParentModel parentData, FormulaMasterChildModel[] childData)
        {
            App.Log("Create", "Setup.PayrollFormulas", "Created payroll formula");
            List<FormulaMasterChildModel> childIds = new List<FormulaMasterChildModel>();
            childIds = formulaMasterObj.Insert(parentData, childData);
            return Json(childIds, JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Setup.PayrollFormulas", PermissionAction.Edit)]
        [HttpPost]
        public void FormulaMasterUpdate(FormulaMasterParentModel parentData, FormulaMasterChildModel[] childData)
        {
            App.Log("Update", "Setup.PayrollFormulas", "Updated payroll formula");
            formulaMasterObj.Update(parentData, childData);
        }

        public ActionResult LeaveMaster()
        {
            return View();
        }

        public ActionResult LoanMaster()
        {
            return View();
        }

        public ActionResult TaxFormulaCalculation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTaxFormulaCalculation()
        {
            return Json(taxFormulaCalcObj.getAll(), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Setup.PayrollTaxRules", PermissionAction.Create)]
        [HttpPost]
        public JsonResult TaxFormulaCalcCreate(TaxFormulaCalculationParentModel parentData, TaxFormulaCalculationChildModel[] childData)
        {
            App.Log("Create", "Setup.PayrollTaxRules", "Created tax rule");
            return Json(taxFormulaCalcObj.Insert(parentData, childData), JsonRequestBehavior.AllowGet);
        }

        [RequirePermission("Setup.PayrollTaxRules", PermissionAction.Edit)]
        [HttpPost]
        public void TaxFormulaCalcUpdate(TaxFormulaCalculationParentModel parent, TaxFormulaCalculationChildModel[] child)
        {
            App.Log("Update", "Setup.PayrollTaxRules", "Updated tax rule");
            //child[0].Remove("payroll-process-table_length");
            taxFormulaCalcObj.Update(parent, child);
        }

        public ActionResult PayElements()
        {
            ViewBag.PayElements = payElementsObj.getAll();
            return View();
        }

        [RequirePermission("Setup.PayElements", PermissionAction.Delete)]
        public void PayElementsDelete(PayElementsModel payElement)
        {
            App.Log("Delete", "Setup.PayElements", "Deleted pay element");
            payElementsObj.Delete(payElement);
        }

        [RequirePermission("Setup.PayElements", PermissionAction.Create)]
        [HttpPost]
        public JsonResult PayElements(PayElementsModel obj)
        {
            App.Log("Update", "Setup.PayElements", "Saved pay element");
            //payElementsObj.Insert(obj);
            //var data = payElementsObj.getAll();
            return Json(payElementsObj.Insert(obj), JsonRequestBehavior.AllowGet);
        }


        public ActionResult OvertimeMaster()
        {
            return View();
        }


    }
}

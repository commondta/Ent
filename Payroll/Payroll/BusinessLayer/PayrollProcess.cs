using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Dynamic;

namespace BusinessLayer
{
    public class PayrollProcess
    {
        SqlConnection sql_connection;
        Database database;
        string query;
        string connectionString;
        Double taxableSalary;
        Employees employeeObj;
        List<SqlParameter> pendingParameters;

        public PayrollProcess(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
            connectionString = con_string;
            taxableSalary = 0;
            employeeObj = new Employees(con_string);
        }

        public string getElementsVal(IDictionary<string, object> rowData)
        {
            string elements = "";
            for (int i = 3; i < rowData.Count - 3; i++)
            {
                elements += rowData.Keys.ElementAt(i) + ", ";
            }
            return elements;
        }

        public string getInsertQuery(List<IDictionary<string, object>> childData)
        {
            string query = "";
            if (pendingParameters == null)
            {
                pendingParameters = new List<SqlParameter>();
            }
            foreach (var data in childData)
            {
                string elementNames = "";
                string elementVals = "";

                //taxableSalary = employeeObj.getTxbleSlry(Convert.ToInt32(data["EmployeeID"].ToString()));

                foreach (var element in data)
                {
                    if(element.Key != "id" &&
                       element.Key != "EmployeeID" &&
                       element.Key != "Name" &&
                       element.Key != "IncomeTax" &&
                       element.Key != "TotalDeduction" &&
                       element.Key != "NetSalary" &&
                       element.Key != "TaxableSalary")
                    {
                        elementNames += "[" + element.Key.Replace("]", "]]") + "], "; // identifier, sanitized (']' escaped so the key cannot break out of the brackets)
                        string paramName = "@p" + pendingParameters.Count;
                        elementVals += paramName + ", ";
                        pendingParameters.Add(new SqlParameter(paramName, (object)element.Value ?? DBNull.Value));
                    }
                }
                elementNames = elementNames.Substring(0, elementNames.Length - 2);
                elementVals = elementVals.Substring(0, elementVals.Length - 2);

                string pEmployeeID = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pEmployeeID, (object)Convert.ToInt32(data["EmployeeID"])));
                string pName = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pName, (object)data["Name"].ToString() ?? DBNull.Value));
                string pIncomeTax = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pIncomeTax, (object)float.Parse(data["IncomeTax"].ToString())));
                string pTotalDeduction = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pTotalDeduction, (object)float.Parse(data["TotalDeduction"].ToString())));
                string pNetSalary = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pNetSalary, (object)float.Parse(data["NetSalary"].ToString())));
                string pTaxableSalary = "@p" + pendingParameters.Count;
                pendingParameters.Add(new SqlParameter(pTaxableSalary, (object)float.Parse(data["TaxableSalary"].ToString())));

                query += "INSERT INTO PayrollProcessChild (ParentID, EmployeeID, Name, IncomeTax, TotalDeduction, NetSalary, TaxableSalary, " + elementNames + ") " +
                        "VALUES (@Parent_ID, " + pEmployeeID + ", " + pName + ", " + pIncomeTax + ", " + pTotalDeduction + ", " + pNetSalary + ", " + pTaxableSalary + ", " + elementVals + ") ";
            }
            return query;
        }

        public List<PayrollProcessChildModel> Insert(PayrollProcessParentModel parentData, List<IDictionary<string, object>> childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            pendingParameters = parameters;
            parameters.Add(new SqlParameter("@EmployeeType", (object)parentData.EmployeeType ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PayPeriod", (object)parentData.PayPeriod ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PayMonth", (object)parentData.PayMonth ?? DBNull.Value));
            parameters.Add(new SqlParameter("@FromDate", (object)parentData.FromDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@ToDate", (object)parentData.ToDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DocumentDate", (object)parentData.DocumentDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@Status", (object)parentData.Status ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostJE", (object)parentData.PostJE ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostingDate", (object)parentData.PostingDate ?? DBNull.Value));
            query = @"DECLARE @Parent_ID INT " +
                    "BEGIN TRAN " +
                        "INSERT INTO PayrollProcessParent (EmployeeType, PayPeriod, PayMonth, FromDate, ToDate, DocumentDate, Status, PostJE, PostingDate) " +
                        "VALUES (@EmployeeType, @PayPeriod, @PayMonth, @FromDate, @ToDate, @DocumentDate, @Status, @PostJE, @PostingDate) " +
                        "SET @Parent_ID = SCOPE_IDENTITY() " +
                        getInsertQuery(childData) +
                    "COMMIT TRAN";
            database.Set(query, parameters.ToArray());
            pendingParameters = null;

            query = "SELECT TOP (@Count) id, ParentID FROM PayrollProcessChild ORDER BY id DESC";
            DataTable dt = database.Get(query, new SqlParameter("@Count", (object)childData.Count));
            List<PayrollProcessChildModel> idslist = new List<PayrollProcessChildModel>();
            PayrollProcessChildModel ids;
            for (int i = 0; i < childData.Count; i++)
            {
                ids = new PayrollProcessChildModel();
                ids.id = Convert.ToInt16(dt.Rows[i][0]);
                ids.ParentID = Convert.ToInt16(dt.Rows[i][1]);
                idslist.Add(ids);
            }

            return idslist;
        }

        public List<IDictionary<string, object>> CalculatePay(DateTime payrollProcessFromDate, DateTime payrollProcessToDate)
        {
            
            List<EmployeeDetailModel> employeesList = new List<EmployeeDetailModel>();
            List<IDictionary<string, object>> payrollProcessObjList = new List<IDictionary<string, object>>();
            var payrollProcessObj = new ExpandoObject() as IDictionary<string, object>;
            employeesList = employeeObj.getPayrollProcess();
            Double totalDeductions;
            Double totalAddition;

            foreach (EmployeeDetailModel employee in employeesList)
            {
                payrollProcessObj = new ExpandoObject() as IDictionary<string, object>;
                payrollProcessObj.Add("EmployeeID", employee.id);
                payrollProcessObj.Add("Name", employee.LegalFirstName + " " + employee.LegalLastName);

                PayElements payElementObj = new PayElements(connectionString);
                List<PayElementsModel> payElementsModelObj = payElementObj.getAll();

                taxableSalary = 0;
                totalDeductions = 0;
                totalAddition = 0;

                foreach (SalaryDetailModel salaryDetail in employee.SalaryDetail)
                {
                    foreach (PayElementsModel payElement in payElementsModelObj)
                    {
                        // Make dictionary of all fields of 'Code', 'Amount' from Salary tab
                        // [{ Key = [CodeValue], Value = [AmountValue] } ... ]
                        if (payElement.PayElementCode == salaryDetail.Code)
                        {
                            payrollProcessObj.Add(payElement.Description, salaryDetail.Amount);
                            if (payElement.PayElementType == "Deduction")
                            {
                                totalDeductions += Convert.ToDouble(salaryDetail.Amount);
                            }
                            if (payElement.PayElementCode == "GrossSalary")
                            {
                                totalAddition = Convert.ToDouble(salaryDetail.Amount);
                            }
                        }
                    }
                    if (salaryDetail.Tax == true)
                    {
                        taxableSalary += Convert.ToDouble(salaryDetail.Amount);
                    }
                }

                Int32 remMonths;
                Int32 currMonth = Convert.ToInt32(payrollProcessFromDate.ToString("MM"));

                if (currMonth > 6)
                {
                    remMonths = 13 - currMonth + 6;
                }
                else
                {
                    remMonths = 7 - currMonth;
                }

                Double yrlyIncomeTax = 0;
                string prevTxbleSlry = "0";
                string prevIncmTax = "0";

                if (taxableSalary != 0)
                {
                    query = "SELECT FromDate, ToDate " +
                            "FROM TaxFormulaCalculationParent " +
                            "WHERE @FromDate >= FromDate AND @ToDate <= ToDate;";
                    DataTable dt = database.Get(query,
                        new SqlParameter("@FromDate", (object)payrollProcessFromDate.ToString("yyyy-MM-dd")),
                        new SqlParameter("@ToDate", (object)payrollProcessToDate.ToString("yyyy-MM-dd")));

                    DateTime taxYearFrom = Convert.ToDateTime(dt.Rows[0][0].ToString());
                    DateTime taxYearTo = Convert.ToDateTime(dt.Rows[0][1].ToString());

                    query = "SELECT SUM(TaxableSalary), SUM(IncomeTax) " +
                            "FROM PayrollProcessChild " +
                            "WHERE EmployeeID = 7 AND ParentID IN " +
                            "( " +
                              "SELECT DocumentNo " +
                              "FROM PayrollProcessParent " +
                              "WHERE FromDate >= @TaxYearFrom AND ToDate <= @TaxYearTo " +
                            ")";

                    dt = database.Get(query,
                        new SqlParameter("@TaxYearFrom", (object)taxYearFrom.ToString("yyyy-MM-dd")),
                        new SqlParameter("@TaxYearTo", (object)taxYearTo.ToString("yyyy-MM-dd")));
                    
                    if (dt.Rows[0][0].ToString() != "") 
                    {
                        prevTxbleSlry = dt.Rows[0][0].ToString();
                        prevIncmTax = dt.Rows[0][1].ToString();
                    }

                    Double remMthsTxbleIncm = taxableSalary * remMonths;
                    Double yrlytxbleIncm = remMthsTxbleIncm + Convert.ToDouble(prevTxbleSlry);

                    query = "DECLARE @remMthsTxbleIncm INT = @YrlyTxbleIncm " +
                            "SELECT ((@remMthsTxbleIncm - LowerAmount) * Percentage / 100) + FixedAmount " +
                                "FROM TaxFormulaCalculationChild " +
                                "WHERE @remMthsTxbleIncm > LowerAmount AND @remMthsTxbleIncm <= HigherAmount AND ParentId = " +
                                "(SELECT Code FROM TaxFormulaCalculationParent " +
                                "WHERE @FromDate >= FromDate AND @ToDate <= ToDate) ";
                    dt = database.Get(query,
                        new SqlParameter("@YrlyTxbleIncm", (object)yrlytxbleIncm),
                        new SqlParameter("@FromDate", (object)payrollProcessFromDate.ToString("yyyy-MM-dd")),
                        new SqlParameter("@ToDate", (object)payrollProcessToDate.ToString("yyyy-MM-dd")));

                    yrlyIncomeTax = Convert.ToDouble(dt.Rows[0][0]);
                }
                
                Double mthlyIncomeTax = Math.Round( (yrlyIncomeTax - Convert.ToDouble(prevIncmTax) ) / remMonths);
                totalDeductions = mthlyIncomeTax + totalDeductions;


                payrollProcessObj.Add("IncomeTax", mthlyIncomeTax.ToString());
                payrollProcessObj.Add("TotalDeduction", totalDeductions.ToString());
                payrollProcessObj.Add("NetSalary", (totalAddition - totalDeductions).ToString());
                payrollProcessObj.Add("TaxableSalary", taxableSalary);
                payrollProcessObjList.Add(payrollProcessObj);
            }
            return payrollProcessObjList;
        }

        public List<List<IDictionary<string, object>>> getPayrollProcess()
        {
            query = "SELECT * FROM PayrollProcessParent";
            DataTable dt = database.Get(query);

            List<List<IDictionary<string, object>>> payrollProcess = new List<List<IDictionary<string, object>>>();
            List<IDictionary<string, object>> parentList = new List<IDictionary<string, object>>();

            var parent = new ExpandoObject() as IDictionary<string, object>;



            foreach (DataRow row in dt.Rows)
            {
                parent = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i != 6)
                    {
                        parent.Add(dt.Columns[i].ColumnName, row[i].ToString());
                    }
                    else
                    {
                        parent.Add("DocumentDate", Convert.ToDateTime(row[6].ToString()));
                    }
                }

                parentList.Add(parent);
            }
            payrollProcess.Add(parentList);

            query = "SELECT * FROM PayrollProcessChild";
            dt = database.Get(query);


            List<IDictionary<string, object>> childList = new List<IDictionary<string, object>>();

            var child = new ExpandoObject() as IDictionary<string, object>;
            
            int colCount = dt.Columns.Count;

            foreach (DataRow row in dt.Rows)
            {
                child = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < colCount; i++)
                {
                    child.Add(dt.Columns[i].ColumnName, row[i].ToString());
                }
                childList.Add(child);
            }

            payrollProcess.Add(childList);

            return payrollProcess;
        }
        
        public void Delete(string id)
        {
            query = "DELETE FROM PayrollProcessChild WHERE ParentID=@id " +
                    "DELETE FROM PayrollProcessParent WHERE DocumentNo=@id";
            database.Set(query, new SqlParameter("@id", (object)id ?? DBNull.Value));
        }

        public string getChildUpdateQuery(List<IDictionary<string, object>> childData)
        {
            string query = "";
            int count;
            Int32 id = 0;
            if (pendingParameters == null)
            {
                pendingParameters = new List<SqlParameter>();
            }

            foreach (var item in childData)
            {
                count = 0;

                foreach (var element in item)
                {
                    if (element.Key != "id")
                    {
                        if (count == 0)
                        {
                            query += "UPDATE PayrollProcessChild SET ";
                        }

                        string paramName = "@p" + pendingParameters.Count;
                        if (element.Key == "EmployeeID")
                        {
                            query += "[" + element.Key + "]=" + paramName;
                            pendingParameters.Add(new SqlParameter(paramName, (object)Convert.ToInt32(element.Value)));
                        }
                        else if (element.Key == "Name")
                        {
                            query += "[" + element.Key + "]=" + paramName;
                            pendingParameters.Add(new SqlParameter(paramName, (object)element.Value.ToString() ?? DBNull.Value));
                        }
                        else
                        {
                            query += "[" + element.Key.Replace("]", "]]") + "]=" + paramName; // identifier, sanitized (']' escaped so the key cannot break out of the brackets)
                            pendingParameters.Add(new SqlParameter(paramName, (object)float.Parse(element.Value.ToString())));
                        }

                        if (count++ != item.Count - 2)
                        {
                            query += ", ";
                        }
                        else
                        {
                            string idParamName = "@p" + pendingParameters.Count;
                            pendingParameters.Add(new SqlParameter(idParamName, (object)id));
                            query += " WHERE id=" + idParamName + " ";
                        }
                    }
                    else
                    {
                        id = Convert.ToInt32(element.Value);
                    }
                }
            }

            return query;
        }

        public void Update(PayrollProcessParentModel parentData, List<IDictionary<string, object>> childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            pendingParameters = parameters;
            query = getChildUpdateQuery(childData) +
                    "UPDATE PayrollProcessParent " +
                     "SET EmployeeType=@EmployeeType, PayPeriod=@PayPeriod, PayMonth=@PayMonth, FromDate=@FromDate, ToDate=@ToDate, DocumentDate=@DocumentDate, Status=@Status WHERE DocumentNo=@DocumentNo";
            parameters.Add(new SqlParameter("@EmployeeType", (object)parentData.EmployeeType.ToString() ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PayPeriod", (object)parentData.PayPeriod.ToString() ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PayMonth", (object)parentData.PayMonth.ToString() ?? DBNull.Value));
            parameters.Add(new SqlParameter("@FromDate", (object)parentData.FromDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@ToDate", (object)parentData.ToDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DocumentDate", (object)parentData.DocumentDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@Status", (object)parentData.Status ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DocumentNo", (object)Convert.ToInt32(parentData.DocumentNo)));
            database.Set(query, parameters.ToArray());
            pendingParameters = null;
        }
    }
}

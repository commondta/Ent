using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data;
using Payroll_HCC.Models;

namespace BusinessLayer
{
    public class PaymentsAndDeductions
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public PaymentsAndDeductions(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public int InsertParent(PaymentsAndDeductions2Model obj)
        {
            query = @"INSERT INTO PaymentsAndDeductionsParent (PayPeriod, DocumentDate, Status) " +
                     "VALUES (@PayPeriod, @DocumentDate, @Status) " +
                     "SELECT IDENT_CURRENT('PaymentsAndDeductionsParent')";
            DataTable dt = database.Get(query,
                new SqlParameter("@PayPeriod", (object)obj.PayPeriod ?? DBNull.Value),
                new SqlParameter("@DocumentDate", (object)obj.DocumentDate ?? DBNull.Value),
                new SqlParameter("@Status", (object)obj.Status ?? DBNull.Value));

            return Convert.ToInt16(dt.Rows[0][0]);
        }

        public void InsertChild(PaymentsAndDeductions2Model obj)
        {
            query = @"INSERT INTO PaymentsAndDeductionsChild (ParentID, PayrollName, EmployeeName, EmployeeID, PayrollPayElement, TransactionType, EffectiveDate, EndDate, Recurrence, Amount, Currency, Comments) " +
                       "VALUES (@ParentID, @PayrollName, @EmployeeName, @EmployeeID, @PayrollPayElement, @TransactionType, @EffectiveDate, @EndDate, @Recurrence, @Amount, @Currency, @Comments)";
            database.Set(query,
                new SqlParameter("@ParentID", (object)obj.ParentID ?? DBNull.Value),
                new SqlParameter("@PayrollName", (object)obj.PayrollName ?? DBNull.Value),
                new SqlParameter("@EmployeeName", (object)obj.EmployeeName ?? DBNull.Value),
                new SqlParameter("@EmployeeID", (object)obj.EmployeeID ?? DBNull.Value),
                new SqlParameter("@PayrollPayElement", (object)obj.PayrollPayElement ?? DBNull.Value),
                new SqlParameter("@TransactionType", (object)obj.TransactionType ?? DBNull.Value),
                new SqlParameter("@EffectiveDate", (object)obj.EffectiveDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)obj.EndDate ?? DBNull.Value),
                new SqlParameter("@Recurrence", (object)obj.Recurrence ?? DBNull.Value),
                new SqlParameter("@Amount", (object)obj.Amount ?? DBNull.Value),
                new SqlParameter("@Currency", (object)obj.Currency ?? DBNull.Value),
                new SqlParameter("@Comments", (object)obj.Comments ?? DBNull.Value));
        }

        public List<PaymentsAndDeductions2Model> getAllParent()
        {
            query = "SELECT * FROM PaymentsAndDeductionsParent";
            DataTable dt = database.Get(query);

            return dataTableToListParent(dt);
        }

        public List<PaymentsAndDeductions2Model> getAllChild()
        {
            query = "SELECT * FROM PaymentsAndDeductionsChild";
            DataTable dt = database.Get(query);

            return dataTableToListChild(dt);
        }

        private List<PaymentsAndDeductions2Model> dataTableToListChild(DataTable dt)
        {
            List<PaymentsAndDeductions2Model> list = new List<PaymentsAndDeductions2Model>();
            PaymentsAndDeductions2Model obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new PaymentsAndDeductions2Model();
                obj.id = Convert.ToInt32(row[0]);
                obj.ParentID = Convert.ToInt32(row[1]);
                obj.PayrollName = row[2].ToString();
                obj.EmployeeName = row[3].ToString();
                obj.EmployeeID = Convert.ToInt32(row[4]);
                obj.PayrollPayElement = row[5].ToString();
                obj.TransactionType = row[6].ToString();
                obj.EffectiveDate = Convert.ToDateTime(row[7].ToString());
                obj.EndDate = Convert.ToDateTime(row[8].ToString());
                obj.Recurrence = row[9].ToString();
                obj.Amount = Convert.ToInt32(row[10]);
                obj.Currency = row[11].ToString();
                obj.Comments = row[12].ToString();

                list.Add(obj);
            }

            return list;
        }

        private List<PaymentsAndDeductions2Model> dataTableToListParent(DataTable dt)
        {
            List<PaymentsAndDeductions2Model> list = new List<PaymentsAndDeductions2Model>();
            PaymentsAndDeductions2Model obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new PaymentsAndDeductions2Model();
                obj.DocumentNo = Convert.ToInt32(row[0]);
                obj.PayPeriod = row[1].ToString();
                obj.DocumentDate = Convert.ToDateTime(row[2].ToString());
                obj.Status = row[3].ToString();

                list.Add(obj);
            }

            return list;
        }

        public void UpdateEmpSalaryDetail(List<PaymentsAndDeductions2Model> padList)
        {
            query = "";
            //List<FormulaMasterChildModel> fMasterObj;

            List<SqlParameter> parameters = new List<SqlParameter>();
            int p = 0;
            foreach (var item in padList)
            {
                //if (item.PayrollPayElement == "GrossPay")
                //{
                    query += "UPDATE SalaryDetail SET Amount=@p" + p + " " +
                             "WHERE employeeId=@p" + (p + 1) + " AND Code=@p" + (p + 2) + "; ";
                    parameters.Add(new SqlParameter("@p" + p, (object)item.Amount ?? DBNull.Value));
                    parameters.Add(new SqlParameter("@p" + (p + 1), (object)item.EmployeeID ?? DBNull.Value));
                    parameters.Add(new SqlParameter("@p" + (p + 2), (object)item.PayrollPayElement ?? DBNull.Value));
                    p += 3;
                //}
            }
            database.Set(query, parameters.ToArray());

            //query = "SELECT PayCode, Percentages " +
            //             "FROM FormulaMasterChild " +
            //             "WHERE ParentID = 21;";

            //DataTable dt = database.Get(query);
            //fMasterObj = dtToList(dt);

            //query = "";
            //foreach (var padListItem in padList)
            //{
            //    foreach (var fMasterItem in fMasterObj)
            //    {
            //        query += "UPDATE SalaryDetail SET Amount=" + padListItem.Amount + "*" + fMasterItem.Percentages + "/100 " +
            //        "WHERE Code = '" + fMasterItem.PayCode + "' AND employeeId=" + padListItem.EmployeeID + "; ";
            //    }
            //    query += "UPDATE SalaryDetail " + 
            //             "SET Amount=(SELECT SUM( CONVERT( float, Amount ) ) FROM SalaryDetail WHERE Code!='GrossPay' AND employeeId='" + padListItem.EmployeeID + "') WHERE employeeId = '" + padListItem.EmployeeID + "' AND Code = '" + padListItem.PayrollPayElement + "' ";
            //}

            //database.Set(query);
        }

        public double findPercentByPayCode(List<FormulaMasterChildModel> fMasterObj, string PayCode)
        {
            double percent = 0.0;
            foreach (var item in fMasterObj)
            {
                if (item.PayCode == PayCode)
                {
                    percent = Convert.ToDouble(item.Percentages.ToString());
                    break;
                }
            }

            return percent;
        }

        public List<FormulaMasterChildModel> dtToList(DataTable dt)
        {
            List<FormulaMasterChildModel> fMasterList = new List<FormulaMasterChildModel>();
            FormulaMasterChildModel fMaster;

            foreach (DataRow row in dt.Rows)
            {
                fMaster = new FormulaMasterChildModel();
                fMaster.PayCode = row[0].ToString();
                fMaster.Percentages = row[1].ToString();

                fMasterList.Add(fMaster);
            }

            return fMasterList;
        }
    }
}

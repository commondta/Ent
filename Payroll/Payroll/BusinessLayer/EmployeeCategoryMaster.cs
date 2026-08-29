using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EmployeeCategoryMaster
    {
        SqlConnection sql_connection;
        DataTable dt;
        Database database;
        string query;

        public EmployeeCategoryMaster(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public void Insert(EmployeeCategoryMasterModel obj)
        {
            query = @"INSERT INTO EmployeeCategoryMaster (EmployeeCategoryCode, EmployeeCategoryName, AccountCode, Remarks) VALUES (@EmployeeCategoryCode, @EmployeeCategoryName, @AccountCode, @Remarks)";
            database.Set(query,
                new SqlParameter("@EmployeeCategoryCode", (object)obj.EmployeeCategoryCode ?? DBNull.Value),
                new SqlParameter("@EmployeeCategoryName", (object)obj.EmployeeCategoryName ?? DBNull.Value),
                new SqlParameter("@AccountCode", (object)obj.AccountCode ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)obj.Remarks ?? DBNull.Value));
        }

        public List<EmployeeCategoryMasterModel> getAll()
        {
            List<EmployeeCategoryMasterModel> list = new List<EmployeeCategoryMasterModel>();
            query = @"SELECT * FROM EmployeeCategoryMaster";
            dt = database.Get(query);

            foreach (DataRow row in dt.Rows)
            {
                EmployeeCategoryMasterModel obj = new EmployeeCategoryMasterModel();
                obj.id = row[0].ToString();
                obj.EmployeeCategoryCode = row[1].ToString();
                obj.EmployeeCategoryName = row[2].ToString();
                obj.AccountCode = row[3].ToString();
                obj.Remarks = row[4].ToString();

                list.Add(obj);
            }
            return list;
        }

        public List<EmployeeCategoryMasterModel> getPayrollProcessCfl()
        {
            List<EmployeeCategoryMasterModel> list = new List<EmployeeCategoryMasterModel>();
            query = @"SELECT EmployeeCategoryName FROM EmployeeCategoryMaster";
            dt = database.Get(query);

            foreach (DataRow row in dt.Rows)
            {
                EmployeeCategoryMasterModel obj = new EmployeeCategoryMasterModel();
                obj.EmployeeCategoryName = row[0].ToString();

                list.Add(obj);
            }
            return list;
        }

        public string getLastID()
        {
            query = "SELECT TOP 1 id FROM EmployeeCategoryMaster ORDER BY id DESC";
            DataTable dt = database.Get(query);

            return dt.Rows[0][0].ToString();
        }

        public string[] getCategories()
        {
            query = "SELECT EmployeeCategoryName FROM EmployeeCategoryMaster";
            DataTable dt = database.Get(query);

            return dataTableToArray(dt);
        }

        public string[] dataTableToArray(DataTable dt)
        {
            string[] categ = new string[dt.Rows.Count];

            foreach (DataRow item in dt.Rows)
            {
                categ[dt.Rows.IndexOf(item)] = item[0].ToString();
            }

            return categ;
        }

        public void Delete(string id)
        {
            query = "DELETE FROM EmployeeCategoryMaster WHERE id=@id";
            database.Set(query, new SqlParameter("@id", (object)id ?? DBNull.Value));
        }
    }
}

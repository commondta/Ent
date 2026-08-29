using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class Departmentsetup
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public Departmentsetup(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public List<DepartmentSetupModel> getAll()
        {
            query = "SELECT * FROM DepartmentSetup";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        private List<DepartmentSetupModel> dataTableToList(DataTable dt)
        {
            List<DepartmentSetupModel> list = new List<DepartmentSetupModel>();
            DepartmentSetupModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new DepartmentSetupModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.DepartmentName = row[1].ToString();
                obj.Description = row[2].ToString();

                list.Add(obj);
            }
            return list;
        }

        public void Insert(DepartmentSetupModel obj)
        {
            query = @"INSERT INTO DepartmentSetup (DepartmentName, Description) VALUES (@DepartmentName, @Description)";
            database.Set(query,
                new SqlParameter("@DepartmentName", (object)obj.DepartmentName ?? DBNull.Value),
                new SqlParameter("@Description", (object)obj.Description ?? DBNull.Value));
        }
        public bool Exists(string name, int exceptId)
        {
            DataTable dt = database.Get("SELECT COUNT(*) FROM DepartmentSetup WHERE DepartmentName = @n AND id <> @id",
                new SqlParameter("@n", name), new SqlParameter("@id", exceptId));
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public void Update(DepartmentSetupModel obj)
        {
            database.Set("UPDATE DepartmentSetup SET DepartmentName = @DepartmentName, Description = @Description WHERE id = @id",
                new SqlParameter("@DepartmentName", (object)obj.DepartmentName ?? DBNull.Value),
                new SqlParameter("@Description", (object)obj.Description ?? DBNull.Value),
                new SqlParameter("@id", obj.id));
        }

        public void Delete(int id)
        {
            database.Set("DELETE FROM DepartmentSetup WHERE id = @id", new SqlParameter("@id", id));
        }

        public void delete()
        {
            //query to delete previous data from table
            query = @"DELETE FROM DepartmentSetup";
            database.Set(query);
        }
    }
}

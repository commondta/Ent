using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payroll_HCC.Models;
using System.Data;

namespace BusinessLayer
{
    public class LeaveMaster
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public LeaveMaster(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        //public void Insert(FormulaMasterModel obj)
        //{
        //    query = @"DECLARE @Parent_ID INT " + 
        //            "BEGIN TRAN " + 
        //                "INSERT INTO FormulaMasterParent (EmployeeCategory, Name) " +
        //                "VALUES ('" + obj.EmployeeCategory + "', '" + obj.Name + "') " +
        //                "SET @Parent_ID = SCOPE_IDENTITY() " +
        //                "INSERT INTO FormulaMasterChild (ParentID, PayCode, AmtHigherLimit, AmtLowerLimit, Percentages, Remarks) " +
        //                "VALUES (@Parent_ID, '" + obj.PayCode + "', '" + obj.AmtHigherLimit + "', '" + obj.AmtLowerLimit + "', '" + obj.Percentages + "', '" + obj.Remarks + "') " +
        //            "COMMIT TRAN";
        //    database.Set(query);
        //}

        //public void Update(FormulaMasterModel obj)
        //{
        //    query = @"UPDATE FormulaMasterChild " + 
        //             "SET  PayCode='" + obj.PayCode + "', AmtHigherLimit='" + obj.AmtHigherLimit + "', AmtLowerLimit='" + obj.AmtLowerLimit + "', Percentages='" + obj.Percentages + "', Remarks='" + obj.Remarks + "' WHERE id=" + obj.id + 
        //             " UPDATE FormulaMasterParent " +
        //             "SET EmployeeCategory='" + obj.EmployeeCategory + "', Name='" + obj.Name + "' WHERE id = (SELECT ParentID from FormulaMasterChild WHERE id=" + obj.id + ")";
        //    database.Set(query);
        //}

        //public List<FormulaMasterModel> getAll()
        //{
        //    query = "SELECT * FROM FormulaMasterChild";
        //    DataTable dt = database.Get(query);

        //    return dataTableToList(dt);
        //}

        //private List<FormulaMasterModel> dataTableToList(DataTable dt)
        //{
        //    List<FormulaMasterModel> list = new List<FormulaMasterModel>();
        //    FormulaMasterModel obj;
        //    FormulaMasterParentModel objParent;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        obj = new FormulaMasterModel();
        //        objParent = new FormulaMasterParentModel();
        //        obj.id = Convert.ToInt32(row[0]);
        //        objParent = getParent(Convert.ToInt32(row[1]));
        //        obj.EmployeeCategory = objParent.EmployeeCategory;
        //        obj.Name = objParent.Name;
        //        obj.PayCode = row[2].ToString();
        //        obj.AmtHigherLimit = row[3].ToString();
        //        obj.AmtLowerLimit = row[4].ToString();
        //        obj.Percentages = row[5].ToString();
        //        obj.Remarks = row[6].ToString();

        //        list.Add(obj);
        //    }

        //    return list;
        //}

        //private FormulaMasterParentModel getParent(int id)
        //{
        //    query = "SELECT EmployeeCategory, Name FROM FormulaMasterParent WHERE id=" + id;
        //    DataTable dt = database.Get(query);
        //    FormulaMasterParentModel obj = new FormulaMasterParentModel();
        //    obj.EmployeeCategory = dt.Rows[0][0].ToString();
        //    obj.Name = dt.Rows[0][1].ToString();

        //    return obj;
        //}

        //public string getLastID()
        //{
        //    query = "SELECT TOP 1 id FROM FormulaMasterChild ORDER BY id DESC";
        //    DataTable dt = database.Get(query);

        //    return dt.Rows[0][0].ToString();
        //}
    }
}

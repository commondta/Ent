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
    public class FormulaMaster
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public FormulaMaster(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public string getInsertQuery(FormulaMasterChildModel[] childData)
        {
            string query = "";
            for (int i = 0; i < childData.Length; i++)
            {
                int p = i * 5;
                query += "INSERT INTO FormulaMasterChild (ParentID, PayCode, AmtHigherLimit, AmtLowerLimit, Percentages, Remarks) " +
                        "VALUES (@Parent_ID, @p" + p + ", @p" + (p + 1) + ", @p" + (p + 2) + ", @p" + (p + 3) + ", @p" + (p + 4) + ") ";
            }
            return query;
        }

        private SqlParameter[] getInsertParameters(FormulaMasterChildModel[] childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < childData.Length; i++)
            {
                int p = i * 5;
                parameters.Add(new SqlParameter("@p" + p, (object)childData[i].PayCode ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 1), (object)childData[i].AmtHigherLimit ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 2), (object)childData[i].AmtLowerLimit ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 3), (object)childData[i].Percentages ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 4), (object)childData[i].Remarks ?? DBNull.Value));
            }
            return parameters.ToArray();
        }

        public List<FormulaMasterChildModel> Insert(FormulaMasterParentModel parentData, FormulaMasterChildModel[] childData)
        {
            query = @"DECLARE @Parent_ID INT " +
                    "BEGIN TRAN " +
                        "INSERT INTO FormulaMasterParent (EmployeeCategory, Name) " +
                        "VALUES (@EmployeeCategory, @Name) " +
                        "SET @Parent_ID = SCOPE_IDENTITY() " +
                        getInsertQuery(childData) +
                    "COMMIT TRAN";
            List<SqlParameter> insertParameters = new List<SqlParameter>();
            insertParameters.Add(new SqlParameter("@EmployeeCategory", (object)parentData.EmployeeCategory ?? DBNull.Value));
            insertParameters.Add(new SqlParameter("@Name", (object)parentData.Name ?? DBNull.Value));
            insertParameters.AddRange(getInsertParameters(childData));
            database.Set(query, insertParameters.ToArray());

            query = "SELECT TOP (@TopCount) id, ParentID FROM FormulaMasterChild ORDER BY id DESC";
            DataTable dt = database.Get(query, new SqlParameter("@TopCount", childData.Length));
            List<FormulaMasterChildModel> list = new List<FormulaMasterChildModel>();
            FormulaMasterChildModel childIds;
            for (int i = 0; i < childData.Length; i++)
            {
                childIds = new FormulaMasterChildModel();
                childIds.id = Convert.ToInt16(dt.Rows[i][0]);
                childIds.ParentID = Convert.ToInt16(dt.Rows[i][1]);
                list.Add(childIds);
            }
            
            return list;
        }

        public string getUpdateQuery(FormulaMasterChildModel[] childData, int ParentId)
        {
            string query = "";
            for (int i = 0; i < childData.Length; i++)
            {
                int p = i * 6;
                if (childData[i].id != 0)
                {
                    query += "UPDATE FormulaMasterChild SET PayCode=@p" + p + ", AmtHigherLimit=@p" + (p + 1) + ", AmtLowerLimit=@p" + (p + 2) + ", Percentages=@p" + (p + 3) + ", Remarks=@p" + (p + 4) + " WHERE id=@p" + (p + 5) + " ";
                }
                else
                {
                    query += "INSERT INTO FormulaMasterChild (ParentID, PayCode, AmtHigherLimit, AmtLowerLimit, Percentages, Remarks) " +
                        "VALUES (@p" + (p + 5) + ", @p" + p + ", @p" + (p + 1) + ", @p" + (p + 2) + ", @p" + (p + 3) + ", @p" + (p + 4) + ") ";
                }
            }
            return query;
        }

        private SqlParameter[] getUpdateParameters(FormulaMasterChildModel[] childData, int ParentId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < childData.Length; i++)
            {
                int p = i * 6;
                parameters.Add(new SqlParameter("@p" + p, (object)childData[i].PayCode ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 1), (object)childData[i].AmtHigherLimit ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 2), (object)childData[i].AmtLowerLimit ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 3), (object)childData[i].Percentages ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 4), (object)childData[i].Remarks ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + (p + 5), childData[i].id != 0 ? childData[i].id : ParentId));
            }
            return parameters.ToArray();
        }

        public void Update(FormulaMasterParentModel parentData, FormulaMasterChildModel[] childData)
        {
            query = @"" +
                     getUpdateQuery(childData, parentData.id) +
                     "UPDATE FormulaMasterParent " +
                     "SET EmployeeCategory=@EmployeeCategory, Name=@Name WHERE id=@ParentId";
            List<SqlParameter> updateParameters = new List<SqlParameter>(getUpdateParameters(childData, parentData.id));
            updateParameters.Add(new SqlParameter("@EmployeeCategory", (object)parentData.EmployeeCategory ?? DBNull.Value));
            updateParameters.Add(new SqlParameter("@Name", (object)parentData.Name ?? DBNull.Value));
            updateParameters.Add(new SqlParameter("@ParentId", parentData.id));
            database.Set(query, updateParameters.ToArray());
        }

        public List<FormulaMasterParentModel> getAllParent()
        {
            query = "SELECT * FROM FormulaMasterParent";
            DataTable dt = database.Get(query);

            return dataTableToListParent(dt);
        }

        public List<FormulaMasterChildModel> getAllChild()
        {
            query = "SELECT * FROM FormulaMasterChild";
            DataTable dt = database.Get(query);

            return dataTableToListChild(dt);
        }

        private List<FormulaMasterParentModel> dataTableToListParent(DataTable dt)
        {
            List<FormulaMasterParentModel> list = new List<FormulaMasterParentModel>();
            FormulaMasterParentModel obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new FormulaMasterParentModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.EmployeeCategory = row[1].ToString();
                obj.Name = row[2].ToString();

                list.Add(obj);
            }

            return list;
        }

        private List<FormulaMasterChildModel> dataTableToListChild(DataTable dt)
        {
            List<FormulaMasterChildModel> list = new List<FormulaMasterChildModel>();
            FormulaMasterChildModel obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new FormulaMasterChildModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.ParentID = Convert.ToInt32(row[1]);
                obj.PayCode = row[2].ToString();
                obj.AmtHigherLimit = row[3].ToString();
                obj.AmtLowerLimit = row[4].ToString();
                obj.Percentages = row[5].ToString();
                obj.Remarks = row[6].ToString();

                list.Add(obj);
            }

            return list;
        }

        public List<FormulaMasterChildModel> getFormula(string formulaMasterName)
        {
            query = "SELECT PayCode, Percentages FROM FormulaMasterChild" +
                    " WHERE ParentID=(SELECT id FROM FormulaMasterParent WHERE Name=@Name)";
            DataTable dt = database.Get(query, new SqlParameter("@Name", (object)formulaMasterName ?? DBNull.Value));

            return dataTableToList(dt);
        }

        private List<FormulaMasterChildModel> dataTableToList(DataTable dt)
        {
            List<FormulaMasterChildModel> list = new List<FormulaMasterChildModel>();
            FormulaMasterChildModel obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new FormulaMasterChildModel();
                obj.PayCode = row[0].ToString();
                obj.Percentages = row[1].ToString();

                list.Add(obj);
            }

            return list;
        }
    }
}

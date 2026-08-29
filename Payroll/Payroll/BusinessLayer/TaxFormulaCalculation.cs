using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payroll_HCC.Models;
using System.Data;
using DataLayer;

namespace BusinessLayer
{
    public class TaxFormulaCalculation
    {
        SqlConnection sql_connection;
        Database database;
        string query;
        List<SqlParameter> pendingParameters;

        public TaxFormulaCalculation(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public string getInsertQuery(TaxFormulaCalculationChildModel[] childData)
        {
            string query = "";
            if (pendingParameters == null)
            {
                pendingParameters = new List<SqlParameter>();
            }
            for (int i = 0; i < childData.Length; i++)
            {
                int p = pendingParameters.Count;
                pendingParameters.Add(new SqlParameter("@p" + p, (object)childData[i].LowerAmount ?? DBNull.Value));
                pendingParameters.Add(new SqlParameter("@p" + (p + 1), (object)childData[i].HigherAmount ?? DBNull.Value));
                pendingParameters.Add(new SqlParameter("@p" + (p + 2), (object)childData[i].Percentage ?? DBNull.Value));
                pendingParameters.Add(new SqlParameter("@p" + (p + 3), (object)childData[i].FixedAmount ?? DBNull.Value));
                pendingParameters.Add(new SqlParameter("@p" + (p + 4), (object)childData[i].OtherAmount ?? DBNull.Value));
                pendingParameters.Add(new SqlParameter("@p" + (p + 5), (object)childData[i].Remarks ?? DBNull.Value));
                query += "INSERT INTO TaxFormulaCalculationChild (ParentID, LowerAmount, HigherAmount, Percentage, FixedAmount, OtherAmount, Remarks) " +
                        "VALUES (@Parent_ID, " +
                                "@p" + p + ", " +
                                "@p" + (p + 1) + ", " +
                                "@p" + (p + 2) + ", " +
                                "@p" + (p + 3) + ", " +
                                "@p" + (p + 4) + ", " +
                                "@p" + (p + 5) + ") ";
            }
            return query;
        }

        public List<TaxFormulaCalculationChildModel> Insert(TaxFormulaCalculationParentModel parentData, TaxFormulaCalculationChildModel[] childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            pendingParameters = parameters;
            parameters.Add(new SqlParameter("@FromDate", (object)parentData.FromDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@ToDate", (object)parentData.ToDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DocumentDate", (object)parentData.DocumentDate ?? DBNull.Value));
            query = @"DECLARE @Parent_ID INT " +
                    "BEGIN TRAN " +
                        "INSERT INTO TaxFormulaCalculationParent (FromDate, ToDate, DocumentDate) " +
                        "VALUES (@FromDate, @ToDate, @DocumentDate) " +
                        "SET @Parent_ID = SCOPE_IDENTITY() " +
                        getInsertQuery(childData) +
                    "COMMIT TRAN";
            database.Set(query, parameters.ToArray());
            pendingParameters = null;

            query = "SELECT TOP (@Count) id, ParentID FROM TaxFormulaCalculationChild ORDER BY id DESC";
            DataTable dt = database.Get(query, new SqlParameter("@Count", (object)childData.Length));
            List<TaxFormulaCalculationChildModel> list = new List<TaxFormulaCalculationChildModel>();
            TaxFormulaCalculationChildModel childIds;
            for (int i = childData.Length - 1; i >= 0; i--)
            {
                childIds = new TaxFormulaCalculationChildModel();
                childIds.id = Convert.ToInt16(dt.Rows[i][0]);
                childIds.ParentId = Convert.ToInt16(dt.Rows[i][1]);
                list.Add(childIds);
            }

            return list;
        }

        public string getUpdateQuery(TaxFormulaCalculationChildModel[] childData, int ParentId)
        {
            string query = "";
            if (pendingParameters == null)
            {
                pendingParameters = new List<SqlParameter>();
            }
            for (int i = 0; i < childData.Length; i++)
            {
                int p = pendingParameters.Count;
                if (childData[i].id != 0)
                {
                    pendingParameters.Add(new SqlParameter("@p" + p, (object)childData[i].LowerAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 1), (object)childData[i].HigherAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 2), (object)childData[i].Percentage ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 3), (object)childData[i].FixedAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 4), (object)childData[i].OtherAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 5), (object)childData[i].Remarks ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 6), (object)childData[i].id));
                    query += "UPDATE TaxFormulaCalculationChild SET LowerAmount=@p" + p + ", HigherAmount=@p" + (p + 1) + ", Percentage=@p" + (p + 2) + ", FixedAmount=@p" + (p + 3) + ", OtherAmount=@p" + (p + 4) + ", Remarks=@p" + (p + 5) + " WHERE id=@p" + (p + 6) + "; ";
                }
                else
                {
                    pendingParameters.Add(new SqlParameter("@p" + p, (object)ParentId));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 1), (object)childData[i].LowerAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 2), (object)childData[i].HigherAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 3), (object)childData[i].Percentage ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 4), (object)childData[i].FixedAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 5), (object)childData[i].OtherAmount ?? DBNull.Value));
                    pendingParameters.Add(new SqlParameter("@p" + (p + 6), (object)childData[i].Remarks ?? DBNull.Value));
                    query += "INSERT INTO TaxFormulaCalculationChild (ParentID, LowerAmount, HigherAmount, Percentage, FixedAmount, OtherAmount, Remarks) " +
                        "VALUES (@p" + p + ", " +
                                "@p" + (p + 1) + ", " +
                                "@p" + (p + 2) + ", " +
                                "@p" + (p + 3) + ", " +
                                "@p" + (p + 4) + ", " +
                                "@p" + (p + 5) + ", " +
                                "@p" + (p + 6) + ") ";
                }
            }
            return query;
        }

        public void Update(TaxFormulaCalculationParentModel parentData, TaxFormulaCalculationChildModel[] childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            pendingParameters = parameters;
            query = @"" +
                     getUpdateQuery(childData, parentData.Code) +
                     "UPDATE TaxFormulaCalculationParent " +
                     "SET FromDate=@FromDate, ToDate=@ToDate, DocumentDate=@DocumentDate WHERE Code=@Code";
            parameters.Add(new SqlParameter("@FromDate", (object)parentData.FromDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@ToDate", (object)parentData.ToDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DocumentDate", (object)parentData.DocumentDate ?? DBNull.Value));
            parameters.Add(new SqlParameter("@Code", (object)parentData.Code));
            database.Set(query, parameters.ToArray());
            pendingParameters = null;
        }

        public List<TaxFormulaCalculationParentModel> getAll()
        {
            query = "SELECT * FROM TaxFormulaCalculationParent";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        //public List<FormulaMasterChildModel> getAllChild()
        //{
        //    query = "SELECT * FROM FormulaMasterChild";
        //    DataTable dt = database.Get(query);

        //    return dataTableToListChild(dt);
        //}

        private List<TaxFormulaCalculationParentModel> dataTableToList(DataTable dtParent)
        {
            List<TaxFormulaCalculationParentModel> list = new List<TaxFormulaCalculationParentModel>();
            TaxFormulaCalculationParentModel obj;
            DataTable dtChild;
            foreach (DataRow row in dtParent.Rows)
            {
                obj = new TaxFormulaCalculationParentModel();
                obj.Code = Convert.ToInt32(row[0]);
                obj.FromDate = row[1].ToString();
                obj.ToDate = row[2].ToString();
                obj.DocumentDate = row[3].ToString();

                query = "SELECT * FROM TaxFormulaCalculationChild WHERE ParentId=@ParentId";
                dtChild = database.Get(query, new SqlParameter("@ParentId", (object)obj.Code));
                obj.Child = dataTableToListChild(dtChild);
                list.Add(obj);
            }

            return list;
        }

        private List<TaxFormulaCalculationChildModel> dataTableToListChild(DataTable dt)
        {
            List<TaxFormulaCalculationChildModel> list = new List<TaxFormulaCalculationChildModel>();
            TaxFormulaCalculationChildModel obj;

            foreach (DataRow row in dt.Rows)
            {
                obj = new TaxFormulaCalculationChildModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.ParentId = Convert.ToInt32(row[1]);
                obj.LowerAmount = Convert.ToInt32(row[2].ToString());
                obj.HigherAmount = Convert.ToInt32(row[3].ToString());
                obj.Percentage = Convert.ToDouble(row[4].ToString());
                obj.FixedAmount = Convert.ToInt32(row[5].ToString());
                obj.OtherAmount = Convert.ToInt32(row[6].ToString());
                obj.Remarks = row[7].ToString();

                list.Add(obj);
            }

            return list;
        }

        //public List<FormulaMasterChildModel> getFormula(string formulaMasterName)
        //{
        //    query = "SELECT PayCode, Percentages FROM FormulaMasterChild" +
        //            " WHERE ParentID=(SELECT id FROM FormulaMasterParent WHERE Name='" + formulaMasterName + "')";
        //    DataTable dt = database.Get(query);

        //    return dataTableToList(dt);
        //}

        //private List<FormulaMasterChildModel> dataTableToList(DataTable dt)
        //{
        //    List<FormulaMasterChildModel> list = new List<FormulaMasterChildModel>();
        //    FormulaMasterChildModel obj;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        obj = new FormulaMasterChildModel();
        //        obj.PayCode = row[0].ToString();
        //        obj.Percentages = row[1].ToString();

        //        list.Add(obj);
        //    }

        //    return list;
        //}
    }
}

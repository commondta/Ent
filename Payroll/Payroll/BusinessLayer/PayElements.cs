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
    public class PayElements
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public PayElements(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public void Delete(PayElementsModel payElement)
        {
            query = "DELETE FROM PayElements WHERE id=@id " +
                    "ALTER TABLE PayrollProcessChild DROP COLUMN [" + SanitizeIdentifier(payElement.PayElementCode) + "]"; // identifier, sanitized
            database.Set(query,
                new SqlParameter("@id", (object)payElement.id ?? DBNull.Value));
        }

        // Validates a dynamic column identifier: allows only letters, digits, underscore and space,
        // rejecting anything that could break out of the surrounding [brackets].
        private static string SanitizeIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException("Identifier cannot be null or empty.", "identifier");
            foreach (char c in identifier)
            {
                if (!(char.IsLetterOrDigit(c) || c == '_' || c == ' '))
                    throw new ArgumentException("Invalid character in identifier: " + identifier, "identifier");
            }
            return identifier;
        }

        //public List<ProductModel> getAll()
        //{
        //    DataTable dt = database.Command_Get();
        //    return dataTableToList(dt);
        //}

        public string Insert(PayElementsModel obj)
        {
            query = @"INSERT INTO PayElements (PayElementCode, Description, Type, PayElementType, Amount, EffectiveDate, Taxable) VALUES (@PayElementCode, @Description, @Type, @PayElementType, @Amount, @EffectiveDate, @Taxable) " +
                     "ALTER TABLE PayrollProcessChild ADD [" + SanitizeIdentifier(obj.PayElementCode) + "] float; " + // identifier, sanitized
                     "SELECT SCOPE_IDENTITY()";
            DataTable dt = database.Get(query,
                new SqlParameter("@PayElementCode", (object)obj.PayElementCode ?? DBNull.Value),
                new SqlParameter("@Description", (object)obj.Description ?? DBNull.Value),
                new SqlParameter("@Type", (object)obj.Type ?? DBNull.Value),
                new SqlParameter("@PayElementType", (object)obj.PayElementType ?? DBNull.Value),
                new SqlParameter("@Amount", (object)obj.Amount ?? DBNull.Value),
                new SqlParameter("@EffectiveDate", (object)obj.EffectiveDate ?? DBNull.Value),
                new SqlParameter("@Taxable", (object)obj.Taxable ?? DBNull.Value));
            return dt.Rows[0][0].ToString();
        }

        public List<PayElementsModel> getCfl()
        {
            query = "SELECT id, PayElementCode, Description, Type FROM PayElements";
            DataTable dt = database.Get(query);

            return dataTableToList_cfl(dt);
        }

        public List<PayElementsModel> getCfl_Employees()
        {
            query = "SELECT id, PayElementCode, Description, Type, EffectiveDate FROM PayElements";
            DataTable dt = database.Get(query);

            return dataTableToList_cflEmployees(dt);
        }

        public List<PayElementsModel> getAll()
        {
            query = "SELECT * FROM PayElements";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        public List<string> getDescription()
        {
            query = "SELECT Description FROM PayElements";
            DataTable dt = database.Get(query);

            return dataTableToListDescription(dt);
        }

        public List<PayElementsModel> getCodeDescription()
        {
            query = "SELECT PayElementCode, Description FROM PayElements";
            DataTable dt = database.Get(query);

            List<PayElementsModel> list = new List<PayElementsModel>();
            PayElementsModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new PayElementsModel();
                obj.PayElementCode = row[0].ToString();
                obj.Description = row[1].ToString();

                list.Add(obj);
            }

            return list;
        }

        private List<string> dataTableToListDescription(DataTable dt)
        {
            List<string> list = new List<string>();
            string desription;
            foreach (DataRow row in dt.Rows)
            {
                desription = row[0].ToString();
                list.Add(desription);
            }

            return list;
        }

        //public ProductModel getProduct(string id)
        //{
        //    key_id = id;
        //    query = "SELECT * FROM products WHERE id=" + Convert.ToInt32(id);
        //    DataTable dt = database.Command_Get(query);

        //    return dataTableToList(dt)[0];
        //}

        //public void Update(ProductModel obj)
        //{
        //    query = "UPDATE products SET product_name='" + obj.product_name +
        //                                        "', code='" + obj.code +
        //                                        "', foreign_name='" + obj.foreign_name +
        //                                        "', type='" + obj.type +
        //                                        "', uom_group='" + obj.uom_group +
        //                                        "', price='" + obj.price +
        //                                        "', barcode='" + obj.barcode +
        //                                        "', description='" + obj.description +
        //                                        "', item_group='" + obj.item_group +
        //                                        "', price_list='" + obj.price_list + "' WHERE id=" + obj.Id;


        //    database.Command_Set(query);
        //}

        private List<PayElementsModel> dataTableToList_cfl(DataTable dt)
        {
            List<PayElementsModel> list = new List<PayElementsModel>();
            PayElementsModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new PayElementsModel();
                obj.id = row[0].ToString();
                obj.PayElementCode = row[1].ToString();
                obj.Description = row[2].ToString();
                obj.Type = row[3].ToString();

                list.Add(obj);
            }

            return list;
        }

        private List<PayElementsModel> dataTableToList_cflEmployees(DataTable dt)
        {
            List<PayElementsModel> list = new List<PayElementsModel>();
            PayElementsModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new PayElementsModel();
                obj.id = row[0].ToString();
                obj.PayElementCode = row[1].ToString();
                obj.Description = row[2].ToString();
                obj.Type = row[3].ToString();
                obj.EffectiveDate = Convert.ToDateTime(row[4].ToString());

                list.Add(obj);
            }

            return list;
        }

        private List<PayElementsModel> dataTableToList(DataTable dt)
        {
            List<PayElementsModel> list = new List<PayElementsModel>();
            PayElementsModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new PayElementsModel();
                obj.id = row[0].ToString();
                obj.PayElementCode = row[1].ToString();
                obj.Description = row[2].ToString();
                obj.Type = row[3].ToString();
                obj.PayElementType = row[4].ToString();
                obj.Amount = Convert.ToInt32(row[5]);
                obj.EffectiveDate = Convert.ToDateTime(row[6].ToString());
                obj.Taxable = row[7].ToString();

                list.Add(obj);
            }

            return list;
        }

        //public string getLastID()
        //{
        //    query = "SELECT TOP 1 id FROM products ORDER BY id DESC";
        //    DataTable dt = database.Get(query);

        //    return dt.Rows[0][0].ToString();
        //}
    }
}

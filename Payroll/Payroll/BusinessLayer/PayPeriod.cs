using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataLayer;

namespace BusinessLayer
{
    public class PayPeriod
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public PayPeriod(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public string Insert(PayPeriodModel obj)
        {
            query = @"INSERT INTO PayPeriod (LocationProjectSite, PayPeriodCodeMonth, Name, FromDate, ToDate, PayMonth, NoOfWorkingDays, NoOfFridays, NoOfHolidays, MaximumNormalOTHoursMonth, MaximumWorkingHoursMonth, Remarks) VALUES (@LocationProjectSite, @PayPeriodCodeMonth, @Name, @FromDate, @ToDate, @PayMonth, @NoOfWorkingDays, @NoOfFridays, @NoOfHolidays, @MaximumNormalOTHoursMonth, @MaximumWorkingHoursMonth, @Remarks)";
            database.Set(query,
                new SqlParameter("@LocationProjectSite", (object)obj.LocationProjectSite ?? DBNull.Value),
                new SqlParameter("@PayPeriodCodeMonth", (object)obj.PayPeriodCodeMonth ?? DBNull.Value),
                new SqlParameter("@Name", (object)obj.Name ?? DBNull.Value),
                new SqlParameter("@FromDate", (object)obj.FromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object)obj.ToDate ?? DBNull.Value),
                new SqlParameter("@PayMonth", (object)obj.PayMonth ?? DBNull.Value),
                new SqlParameter("@NoOfWorkingDays", (object)obj.NoOfWorkingDays ?? DBNull.Value),
                new SqlParameter("@NoOfFridays", (object)obj.NoOfFridays ?? DBNull.Value),
                new SqlParameter("@NoOfHolidays", (object)obj.NoOfHolidays ?? DBNull.Value),
                new SqlParameter("@MaximumNormalOTHoursMonth", (object)obj.MaximumNormalOTHoursMonth ?? DBNull.Value),
                new SqlParameter("@MaximumWorkingHoursMonth", (object)obj.MaximumWorkingHoursMonth ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)obj.Remarks ?? DBNull.Value));
            query = "SELECT TOP 1 id FROM PayPeriod ORDER BY id DESC";
            return database.Get(query).Rows[0][0].ToString();
        }

        public List<PayPeriodModel> getAll()
        {
            query = "SELECT * FROM PayPeriod";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        public void Update(PayPeriodModel obj)
        {
            query = "UPDATE PayPeriod SET LocationProjectSite=@LocationProjectSite" +
                                                ", PayPeriodCodeMonth=@PayPeriodCodeMonth" +
                                                ", Name=@Name" +
                                                ", FromDate=@FromDate" +
                                                ", ToDate=@ToDate" +
                                                ", PayMonth=@PayMonth" +
                                                ", NoOfWorkingDays=@NoOfWorkingDays" +
                                                ", NoOfFridays=@NoOfFridays" +
                                                ", NoOfHolidays=@NoOfHolidays" +
                                                ", MaximumNormalOTHoursMonth=@MaximumNormalOTHoursMonth" +
                                                ", MaximumWorkingHoursMonth=@MaximumWorkingHoursMonth" +
                                                ", Remarks=@Remarks" +
                                                " WHERE id=@id";

            database.Set(query,
                new SqlParameter("@LocationProjectSite", (object)obj.LocationProjectSite ?? DBNull.Value),
                new SqlParameter("@PayPeriodCodeMonth", (object)obj.PayPeriodCodeMonth ?? DBNull.Value),
                new SqlParameter("@Name", (object)obj.Name ?? DBNull.Value),
                new SqlParameter("@FromDate", (object)obj.FromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object)obj.ToDate ?? DBNull.Value),
                new SqlParameter("@PayMonth", (object)obj.PayMonth ?? DBNull.Value),
                new SqlParameter("@NoOfWorkingDays", (object)obj.NoOfWorkingDays ?? DBNull.Value),
                new SqlParameter("@NoOfFridays", (object)obj.NoOfFridays ?? DBNull.Value),
                new SqlParameter("@NoOfHolidays", (object)obj.NoOfHolidays ?? DBNull.Value),
                new SqlParameter("@MaximumNormalOTHoursMonth", (object)obj.MaximumNormalOTHoursMonth ?? DBNull.Value),
                new SqlParameter("@MaximumWorkingHoursMonth", (object)obj.MaximumWorkingHoursMonth ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)obj.Remarks ?? DBNull.Value),
                new SqlParameter("@id", (object)obj.id ?? DBNull.Value));
        }

        private List<PayPeriodModel> dataTableToList(DataTable dt)
        {
            List<PayPeriodModel> list = new List<PayPeriodModel>();
            PayPeriodModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new PayPeriodModel();
                obj.id = Convert.ToInt16(row[0]);
                obj.LocationProjectSite = row[1].ToString();
                obj.PayPeriodCodeMonth = row[2].ToString();
                obj.Name = row[3].ToString();
                obj.FromDate = Convert.ToDateTime(row[4].ToString());
                obj.ToDate = Convert.ToDateTime(row[5].ToString());
                obj.PayMonth = row[6].ToString();
                obj.NoOfWorkingDays = Convert.ToInt16(row[7]);
                obj.NoOfFridays = Convert.ToInt16(row[8].ToString());
                obj.NoOfHolidays = Convert.ToInt16(row[9].ToString());
                obj.MaximumNormalOTHoursMonth = Convert.ToInt16(row[10]);
                obj.MaximumWorkingHoursMonth = Convert.ToInt16(row[11].ToString());
                obj.Remarks = row[12].ToString();

                list.Add(obj);
            }

            return list;
        }

        public void Delete(string id)
        {
            query = "DELETE FROM PayPeriod WHERE id=@id";
            database.Set(query,
                new SqlParameter("@id", (object)id ?? DBNull.Value));
        }
    }
}

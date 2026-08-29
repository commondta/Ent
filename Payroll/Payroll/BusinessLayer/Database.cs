using System;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class Database
    {
        readonly string connection_string;

        public Database(string connection_string)
        {
            this.connection_string = connection_string;
        }

        /// <summary>
        /// Executes a non-query command. Always pass user-supplied values as SqlParameters,
        /// never by string concatenation.
        /// </summary>
        public void Set(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a query and returns the results. Always pass user-supplied values as
        /// SqlParameters, never by string concatenation.
        /// </summary>
        public DataTable Get(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter sql_da = new SqlDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                sql_da.Fill(dt);
                return dt;
            }
        }
    }
}

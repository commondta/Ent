using DataLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class Account
    {
        readonly Database database;

        public Account(string connectionString)
        {
            database = new Database(connectionString);
        }

        public Boolean isValid(AccountModel account)
        {
            if (account == null || string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Passwd))
                return false;

            DataTable dt = database.Get(
                "SELECT Password FROM Account WHERE Username = @Username",
                new SqlParameter("@Username", account.Username));

            if (dt.Rows.Count != 1)
                return false;

            string stored = dt.Rows[0][0].ToString();
            return PasswordHasher.Verify(account.Passwd, stored);
        }
    }
}

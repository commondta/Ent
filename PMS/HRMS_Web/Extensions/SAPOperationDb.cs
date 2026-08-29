using B_DB_Context;
using B_DB_Model;
using SAPbobsCOM;

namespace HRMS_Web.Extensions
{
    public class SAPOperationDb
    {
        private DataBase_Context _db;
        private SAPOperations _connectionDb;

        public SAPOperationDb(DataBase_Context db)
        { 
            _db = db;
            _connectionDb = _db.SAPOperations.FirstOrDefault();
        }

        public SAPbobsCOM.Company Ocomp { get; } = new Company();
        public int _a = -1;

        public void ConnectToCompany() 
        {
            try
            {
                // using var oCompScope = new ComObjectScope(Ocomp);
                string result = Ocomp.GetLastErrorDescription();

                if (_a != 0 && string.IsNullOrEmpty(result))
                {
                    string servername = _connectionDb.Server;
                    Ocomp.Server = _connectionDb.Server;
                    Ocomp.CompanyDB = _connectionDb.DBName;
                    Ocomp.UserName = _connectionDb.SAPUser;
                    Ocomp.Password = _connectionDb.SAPPassword;
                    Ocomp.LicenseServer = "192.168.109.6:40000";
                    Ocomp.language = BoSuppLangs.ln_English;
                    Ocomp.DbServerType = BoDataServerTypes.dst_HANADB;
                    _a = Ocomp.Connect();

                    if (_a == -116)
                    {
                        _a = -1;
                        Ocomp.Disconnect();
                        ConnectToCompany();
                    }
                }
                else if (result == "you are already connected to a company")
                {
                    Ocomp.Disconnect();
                    _a = -1;
                    ConnectToCompany();
                }
                else if (_a == 100000085)
                {
                    Ocomp.Disconnect();
                    _a = -1;
                    ConnectToCompany();
                }
                else if (result == "error during sbo user authentication")
                {
                    _a = -1;
                    Ocomp.Disconnect();
                    ConnectToCompany();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}


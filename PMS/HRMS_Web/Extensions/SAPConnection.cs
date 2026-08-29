using Microsoft.Extensions.Configuration;
using SAPbobsCOM;

namespace hrms_web.extensions
{
    public class sapconnection
    {
        private readonly IConfiguration _iconfiguration;
        public sapconnection(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public static SAPbobsCOM.Company ocomp = new Company();
        public static int a = -1;
        public static int systemnumber = 1;
        public static void ConnectToCompany()
        {
            try
            {
                //var configuration = webconfigurationmanager.openwebconfiguration("~");
                string result = ocomp.GetLastErrorDescription();
                if (a != 0 && result == "")
                {

                    string servername = "NDB@192.168.12.32:30013";
                    ocomp.Server = servername;
                    ocomp.CompanyDB = "UDPVT_TEST";
                    ocomp.UserName = "manager";
                    ocomp.Password = "B1admin@";
                    ocomp.LicenseServer = "192.168.12.32:40000";
                    ocomp.language = BoSuppLangs.ln_English;
                    ocomp.DbServerType = BoDataServerTypes.dst_HANADB;
                    a = ocomp.Connect();
                    if (a == -116)
                    {
                        a = -1;
                        ocomp.Disconnect();
                        ConnectToCompany();
                    }

                }
                else if (result == "you are already connected to a company")
                {
                    ocomp.Disconnect();
                    a = -1;
                    ConnectToCompany();
                }
                else if (a == 100000085)
                {
                    ocomp.Disconnect();
                    a = -1;
                    ConnectToCompany();
                }
                else if (result == "error during sbo user authentication")
                {
                    a = -1;
                    ocomp.Disconnect();
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

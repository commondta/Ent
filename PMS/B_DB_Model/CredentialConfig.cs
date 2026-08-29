using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class CredentialConfig : BaseModel
    {
        // ===================== SMTP Fields =====================
        public string SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpEncryptedPassword { get; set; }
        public string SmtpEncryptionType { get; set; } // SSL / TLS / None
        public string FromEmail { get; set; }

        // ===================== Telecard SMS Fields =====================
        public string TelecardApiUsername { get; set; }
        public string TelecardEncryptedPassword { get; set; }
        public string SenderMask { get; set; }

        // ===================== Dealer API Fields =====================
        public string DealerApiUsername { get; set; }
        public string DealerApiEncryptedPassword { get; set; }

    }

}

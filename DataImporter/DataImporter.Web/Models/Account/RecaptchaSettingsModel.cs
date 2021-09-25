using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Models.Account
{
    public class RecaptchaSettingsModel
    {
        public string Recaptcha_Site_Key { get; set; }
        public string Recaptcha_Secret_Key { get; set; }
    }
}

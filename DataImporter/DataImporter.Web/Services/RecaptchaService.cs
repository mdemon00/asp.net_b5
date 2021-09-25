using DataImporter.Web.Models.Account;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataImporter.Web.Services
{
    public class RecaptchaService
    {
        private RecaptchaSettingsModel _settings;

        public RecaptchaService(IOptions<RecaptchaSettingsModel> settings)
        {
            _settings = settings.Value;
        }

        public virtual async Task<RecaptchaResponse> RecaptchaVerify(string token)
        {
            var data = new RecaptchaData
            {
                response = token,
                secret = _settings.Recaptcha_Secret_Key
            };

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(clientHandler);

            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={data.secret}&response={data.response}");

            var fileteredResponse = JsonSerializer.Deserialize<RecaptchaResponse>(response);

            return fileteredResponse;
        }
    }
}

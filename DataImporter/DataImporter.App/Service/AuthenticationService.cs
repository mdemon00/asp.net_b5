using DataImporter.App.Entitties;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataImporter.App.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;

        public AuthenticationService(
            IHttpService httpService
        )
        {
            _httpService = httpService;
        }

        public async Task<string> GetToken(string email, string password)
        {
            var token = await _httpService.Get($"api/token?email={email}&password={password}");
            return token;
        }
    }
}

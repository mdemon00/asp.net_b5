using System.Threading.Tasks;

namespace DataImporter.App.Service
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(string user, string pass);
    }
}
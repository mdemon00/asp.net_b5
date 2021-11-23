using System.Threading.Tasks;

namespace DataImporter.App.Service
{
    public interface IHttpService
    {
        Task<string> Get(string uri);
    }
}
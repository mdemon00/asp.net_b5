using SocialNetwork.Registering.BuisnessObjects;
using System.Collections.Generic;

namespace SocialNetwork.Registering.Services
{
    public interface IPhotoService
    {
        (IList<Photo> records, int total, int totalDisplay) GetPhotos(int pageIndex, int pageSize, string searchText, string sortText);
        void CreatePhoto(Photo photo);
        Photo GetPhoto(int id);
        void UpdatePhoto(Photo photo);
        void DeletePhoto(int id);
    }
}

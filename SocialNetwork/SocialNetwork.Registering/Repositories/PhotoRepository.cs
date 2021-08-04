using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Registering.Contexts;
using SocialNetwork.Registering.Entites;

namespace SocialNetwork.Registering.Repositories
{
    public class PhotoRepository : Repository<Photo, int>, IPhotoRepository
    {
        public PhotoRepository(IRegisteringContext context) : base((DbContext)context)
        {

        }
    }
}

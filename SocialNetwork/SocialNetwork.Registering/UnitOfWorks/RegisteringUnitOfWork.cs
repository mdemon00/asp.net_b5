using SocialNetwork.Data;
using SocialNetwork.Registering.Contexts;
using SocialNetwork.Registering.Repositories;

using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Registering.UnitOfWorks
{
    public class RegisteringUnitOfWork : UnitOfWork, IRegisteringUnitOfWork
    {
        public IMemberRepository Members { get; private set; }
        public IPhotoRepository Photos { get; private set; }

        public RegisteringUnitOfWork(IRegisteringContext context,
            IMemberRepository members, IPhotoRepository photos) : base((DbContext)context)
        {
            Members = members;
            Photos = photos;
        }
    }
}

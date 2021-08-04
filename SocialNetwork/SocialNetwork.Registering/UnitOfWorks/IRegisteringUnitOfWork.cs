using SocialNetwork.Data;
using SocialNetwork.Registering.Repositories;

namespace SocialNetwork.Registering.UnitOfWorks
{
    public interface IRegisteringUnitOfWork : IUnitOfWork
    {
        IMemberRepository Members { get; }
        IPhotoRepository Photos { get; }
    }
}

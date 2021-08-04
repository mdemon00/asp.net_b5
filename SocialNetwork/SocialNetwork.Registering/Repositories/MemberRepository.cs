using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Registering.Contexts;
using SocialNetwork.Registering.Entites;

namespace SocialNetwork.Registering.Repositories
{

    public class MemberRepository : Repository<Member, int>, IMemberRepository
    {
        public MemberRepository(IRegisteringContext context) : base((DbContext)context)
        {

        }
    }
}

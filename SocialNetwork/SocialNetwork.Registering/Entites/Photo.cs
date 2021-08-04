using SocialNetwork.Data;

namespace SocialNetwork.Registering.Entites
{
    public class Photo : IEntity<int>
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string PhotoFileName { get; set; }
    }
}

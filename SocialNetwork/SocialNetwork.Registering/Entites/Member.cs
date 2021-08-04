using SocialNetwork.Data;
using System;

namespace SocialNetwork.Registering.Entites
{
    public class Member : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Address { get; set; }
    }
}

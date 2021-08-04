using Autofac;
using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class CreatePhotoModel
    {
        [Required, Range(1, 100000)]
        public int MemberId { get; set; }

        [Required, MaxLength(50, ErrorMessage = "PhotoFileName should be less than 50 charcaters")]
        public string PhotoFileName { get; set; }

        private readonly IPhotoService _photoService;
        public CreatePhotoModel()
        {
            _photoService = Startup.AutofacContainer.Resolve<IPhotoService>();
        }

        public CreatePhotoModel(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        internal void CreatePhoto()
        {
            var photo = new Photo
            {
                MemberId = MemberId,
                PhotoFileName = PhotoFileName
            };

            _photoService.CreatePhoto(photo);
        }
    }
}

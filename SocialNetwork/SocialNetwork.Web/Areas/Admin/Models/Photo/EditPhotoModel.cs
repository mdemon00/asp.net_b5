using Autofac;
using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class EditPhotoModel
    {
        [Required, Range(1, 100000)]
        public int? Id { get; set; }

        [Required, Range(1, 100000)]
        public int? MemberId { get; set; }

        [Required, MaxLength(50, ErrorMessage = "PhotoFileName should be less than 50 charcaters")]
        public string PhotoFileName { get; set; }

        private readonly IPhotoService _photoService;
        public EditPhotoModel()
        {
            _photoService = Startup.AutofacContainer.Resolve<IPhotoService>();
        }

        public EditPhotoModel(IPhotoService photoService)
        {
            _photoService = photoService;
        }


        public void LoadModelData(int id)
        {
            var photo = _photoService.GetPhoto(id);
            MemberId = photo?.MemberId;
            PhotoFileName = photo.PhotoFileName;
        }

        internal void Update()
        {
            var photo = new Photo
            {
                Id = Id.HasValue ? Id.Value : 0,
                MemberId = MemberId.HasValue ? MemberId.Value : 0,
                PhotoFileName = PhotoFileName
            };
            _photoService.UpdatePhoto(photo);
        }
    }
}

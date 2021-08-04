using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Exceptions;
using SocialNetwork.Registering.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Registering.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IRegisteringUnitOfWork _registeringUnitOfWork;

        public PhotoService(IRegisteringUnitOfWork registeringUnitOfWork)
        {
            _registeringUnitOfWork = registeringUnitOfWork;
        }

        public void CreatePhoto(Photo photo)
        {
            if (photo == null)
                throw new InvalidParameterException("Photo was not provided");

            _registeringUnitOfWork.Photos.Add(new Entites.Photo
            {
                MemberId = photo.MemberId,
                PhotoFileName = photo.PhotoFileName
            });

            _registeringUnitOfWork.Save();
        }

        public void DeletePhoto(int id)
        {
            _registeringUnitOfWork.Photos.Remove(id);
            _registeringUnitOfWork.Save();
        }

        public Photo GetPhoto(int id)
        {
            var photo = _registeringUnitOfWork.Photos.GetById(id);

            if (photo == null) return null;

            return new Photo
            {
                Id = photo.Id,
                MemberId = photo.MemberId,
                PhotoFileName = photo.PhotoFileName
            };
        }

        public (IList<Photo> records, int total, int totalDisplay) GetPhotos(int pageIndex, int pageSize, string searchText, string sortText)
        {

            int value = 0;
            int.TryParse(searchText, out value); // determine whether a string represents a numeric value

            var photoData = _registeringUnitOfWork.Photos.GetDynamic(value == 0 ? null : x => x.MemberId.ToString().Contains(value.ToString()),
            sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from photo in photoData.data
                              select new Photo
                              {
                                  Id = photo.Id,
                                  MemberId = photo.MemberId,
                                  PhotoFileName = photo.PhotoFileName
                              }).ToList();
            return (resultData, photoData.total, photoData.totalDisplay);
        }

        public void UpdatePhoto(Photo photo)
        {
            if (photo == null)
                throw new InvalidOperationException("Photo is missing");

            var photoEntity = _registeringUnitOfWork.Photos.GetById(photo.Id);

            if (photoEntity != null)
            {
                photoEntity.MemberId = photo.MemberId;
                photoEntity.PhotoFileName = photo.PhotoFileName;

                _registeringUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find photo");
        }
    }
}

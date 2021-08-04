using Autofac;
using SocialNetwork.Registering.Services;
using SocialNetwork.Web.Models;
using System.Linq;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class PhotoListModel
    {
        private readonly IPhotoService _photoService;
        public PhotoListModel()
        {
            _photoService = Startup.AutofacContainer.Resolve<IPhotoService>();
        }

        public PhotoListModel(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        internal object GetPhotos(DataTablesAjaxRequestModel dataTableModel)
        {
            var data = _photoService.GetPhotos(dataTableModel.PageIndex,
            dataTableModel.PageSize,
            dataTableModel.SearchText,
            dataTableModel.GetSortText(new string[] { "MemberId", "PhotoFileName" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.MemberId.ToString(),
                                record.PhotoFileName,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _photoService.DeletePhoto(id);
        }
    }
}

using AutoMapper;
using EO = DataImporter.Importing.Entities;
using BO = DataImporter.Importing.BusinessObjects;

namespace DataImporter.Importing.Profiles
{
    public class ImportingProfile : Profile
    {
        public ImportingProfile()
        {
            CreateMap<EO.Group, BO.Group>().ReverseMap();
            CreateMap<EO.Column, BO.Column>().ReverseMap();
            CreateMap<EO.Row, BO.Row>().ReverseMap();
            CreateMap<EO.Cell, BO.Cell>().ReverseMap();
        }
    }
}

using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO = LibraryManagementSystem.Reading.Entites;
using BO = LibraryManagementSystem.Reading.BuisnessObjects;

namespace LibraryManagementSystem.Reading.Profiles
{
    public class ReadingProfile : Profile
    {
        public ReadingProfile()
        {
            CreateMap<EO.Book, BO.Book>().ReverseMap();
            CreateMap<EO.Author, BO.Author>().ReverseMap();
        }
    }
}

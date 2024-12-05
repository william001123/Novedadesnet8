using AutoMapper;
using WebAPI_PDF.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI_PDF.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            // Definir los mapeos entre las clases
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }

    }
}

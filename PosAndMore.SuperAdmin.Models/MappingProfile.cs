using PosAndMore.SuperAdmin.Models.DbModels;
using PosAndMore.SuperAdmin.Models.DtoModels;
using Riok.Mapperly.Abstractions;
using AutoMapper;
 
namespace PosAndMore.SuperAdmin.Models
{
    [Mapper]
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
          
            CreateMap<Firmalar,FirmalarDto>(MemberList.Destination);
            CreateMap<Il,IlDto>(MemberList.Destination);
            CreateMap<Ilce, IlceDto>(MemberList.Destination);

            CreateMap<FirmalarDto, Firmalar>(MemberList.Destination);
            CreateMap<IlDto, Il>(MemberList.Destination);
            CreateMap<IlceDto, Ilce>(MemberList.Destination);
        }

     

         
    }
}

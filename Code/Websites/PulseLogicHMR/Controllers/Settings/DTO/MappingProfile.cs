using AutoMapper;
using PulseLogicHMR.ServerModel.BillingModels;

namespace PulseLogicHMR.Controllers.Settings.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BillSchemeDTO, BillingSchemeModel>();
        }
    }
}

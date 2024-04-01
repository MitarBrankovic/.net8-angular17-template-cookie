using AutoMapper;
using TemplateBackend.Application.Features.ApplicationUsers.Commands;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
using TemplateBackend.Application.Features.ApplicationUsers.Queries;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Infrastructure;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => src.Theme))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

        CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
    }
}

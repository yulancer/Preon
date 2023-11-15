using AutoMapper;
using Preon.Solver.Contracts.Models;
using Preon.Solver.Integration.Contracts;

namespace Preon.Solver.Integration.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonModel, PersonContract>(MemberList.Destination);
    }
}
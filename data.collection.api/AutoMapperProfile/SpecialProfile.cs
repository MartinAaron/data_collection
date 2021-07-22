using AutoMapper;

namespace data.collection.api.AutoMapperProfile
{
    public class SpecialProfile : Profile
    {
        public SpecialProfile()
        {
            // CreateMap<Professor, ProfessorDto>()
            //     .ForMember(x => x.SexText, opt => opt.MapFrom(x => EnumHelper.GetEnumValue(x.Sex)))
            //     .ForMember(x => x.LevelText, opt => opt.MapFrom<ProfessorLevelResolver>());
            //
            // CreateMap<Professor_VideoNumber, VideoNumberDto>()
            //     .ForMember(x => x.StatusText, opt => opt.MapFrom(x => EnumHelper.GetEnumValue(x.Status)))
            //     .ForMember(d => d.ProfessorName,
            //         opt => opt.MapFrom<ProfessorNameResolver>());
            //
            // CreateMap<Meeting_Reservation, MeetingReservationResult>()
            //     .ForMember(x => x.UserNames, opt => opt.MapFrom<UserNamesResolver>());
            //
            // CreateMap<Professor_Meeting, ProfessorMeetingResult>()
            //     .ForMember(dest => dest.FileNames,
            //         opt => opt.MapFrom<ProfessorMeetingValueResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Files, "file")))
            //     .ForMember(dest => dest.InsideUsers,
            //         opt => opt.MapFrom<ProfessorMeetingValueResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Users, "user")))
            //     .ForMember(dest => dest.ProfessorNames,
            //         opt => opt.MapFrom<ProfessorMeetingValueResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Professors, "professor")));
            //
            // CreateMap<Professor_Consultation, ProfessorConsultationResult>()
            //     .ForMember(dest => dest.FileNames,
            //         opt => opt.MapFrom<ProfessorConsultationMemberResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Files, "file")))
            //     .ForMember(dest => dest.InsideUsers,
            //         opt => opt.MapFrom<ProfessorConsultationMemberResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Users, "user")))
            //     .ForMember(dest => dest.ProfessorNames,
            //         opt => opt.MapFrom<ProfessorConsultationMemberResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Professors, "professor")))
            //     .ForMember(dest => dest.ClientNames,
            //         opt => opt.MapFrom<ProfessorConsultationMemberResolver, Tuple<List<string>, string>>(src =>
            //             new Tuple<List<string>, string>(src.Clients, "client")));
            //
            // CreateMap<Base_Role, Base_RoleInfoDTO>();
            //
            // CreateMap<Meeting_Files, MeetingFilesResult>();
        }
    }
}
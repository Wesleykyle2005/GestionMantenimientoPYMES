using SharedModels.ModelsDTO.Empresa;
using SharedModels.ModelsDTO.Usuario;
using SharedModels.ModelsDTO.Equipo;
using SharedModels.ModelsDTO.Mantenimiento;
using AutoMapper;
using SharedModels.Models;

namespace API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // Empresa
        CreateMap<Empresa, EmpresaCreateDTO>().ReverseMap();
        CreateMap<Empresa, EmpresaUpdateDTO>().ReverseMap();
        CreateMap<Empresa, EmpresaResponseDTO>().ReverseMap();

        // Usuario
        CreateMap<UsuarioCreateDTO, Usuario>()
            .ForMember(dest => dest.Contraseña, opt => opt.Ignore());

        CreateMap<UsuarioUpdateDTO, Usuario>()
            .ForMember(dest => dest.Contraseña, opt => opt.Ignore());

        CreateMap<Usuario, UsuarioResponseDTO>().ReverseMap();
        CreateMap<Usuario, UsuarioEmpresaDTO>();

        // Equipo
        CreateMap<Equipo, EquipoCreateDTO>().ReverseMap();
        CreateMap<Equipo, EquipoUpdateDTO>().ReverseMap();
        CreateMap<Equipo, EquipoResponseDTO>().ReverseMap();

        // Mantenimiento
        CreateMap<Mantenimiento, MantenimientoCreateDTO>().ReverseMap();
        CreateMap<Mantenimiento, MantenimientoUpdateDTO>().ReverseMap();
        CreateMap<Mantenimiento, MantenimientoResponseDTO>().ReverseMap();
    }
}

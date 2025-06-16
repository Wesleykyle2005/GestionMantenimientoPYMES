using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Equipo
{
    public class EquipoUpdateDTO
    {
        public int EquipoId { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        public string Tipo { get; set; }

        [Required, MaxLength(50)]
        public string Marca { get; set; }

        [Required, MaxLength(50)]
        public string Modelo { get; set; }

        [Required, MaxLength(50)]
        public string Estado { get; set; }

        [Required]
        public int EmpresaId { get; set; }
    }
}

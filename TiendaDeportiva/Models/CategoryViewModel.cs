using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TiendaDeportiva.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Descripcion { get; set; }
    }
}

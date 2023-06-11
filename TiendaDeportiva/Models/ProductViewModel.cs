using System.ComponentModel.DataAnnotations;

namespace TiendaDeportiva.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Categoria ")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CatId { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Nombre { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Precio { get; set; }
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Descripcion { get; set; }

    }
}

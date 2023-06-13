namespace TiendaDeportiva.Models
{
    public class OrderViewModel
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int perId { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}

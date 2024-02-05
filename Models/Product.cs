namespace Burger_API.Models
{
    public class Product
    {
        public int? IdProduct { get; set; }
        public string? Nombre { get; set; }
        public int? Stock { get; set; }
        public decimal Precio { get; set; }
        public string? Category { get; set; }
        public int? IdCategoria { get; set; }
    }
}

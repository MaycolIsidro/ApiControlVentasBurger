namespace Burger_API.Models
{
    public class DetailSale
    {
        public int IdProducto { get; set; }
        public string? Producto { get; set; }
        public decimal? Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Adicional { get; set; }
        public decimal Total { get; set; }
    }
}

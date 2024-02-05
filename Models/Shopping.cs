namespace Burger_API.Models
{
    public class Shopping
    {
        public int? IdCompra { get; set; }
        public DateTime FechaEmision { get; set; }
        public string? Descripcion { get; set; }
        public decimal Total { get; set; }
        public int IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public List<DetailsShop>? Detalles { get; set; }
    }
}

namespace Burger_API.Models
{
    public class Sale
    {
        public int? IdSale { get; set; }
        public DateTime FechaEmision { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }
        public int IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public List<DetailSale>? DetailsSale { get; set; }
    }
}

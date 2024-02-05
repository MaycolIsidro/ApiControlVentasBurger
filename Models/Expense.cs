namespace Burger_API.Models
{
    public class Expense
    {
        public int IdEgreso { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Descripcion { get; set; }
        public decimal Total { get; set; }
    }
}

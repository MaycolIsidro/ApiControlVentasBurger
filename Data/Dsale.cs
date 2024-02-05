using Burger_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace Burger_API.Data
{
    public class Dsale
    {
        readonly ConexionDB cn;
        public Dsale(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task<Sale> InsertVenta(Sale sale)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("insertVenta", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", sale.FechaEmision);
                    cmd.Parameters.AddWithValue("@metodoPago", sale.MetodoPago);
                    cmd.Parameters.AddWithValue("@total", sale.Total);
                    cmd.Parameters.AddWithValue("@idUser", sale.IdUsuario);
                    var idVenta = await cmd.ExecuteScalarAsync();
                    await sql.CloseAsync();
                    return new Sale()
                    {
                        IdSale = Convert.ToInt32(idVenta),
                        FechaEmision = sale.FechaEmision,
                        Total = sale.Total,
                        IdUsuario = sale.IdUsuario
                    };
                }
            }
        }
        public async Task InsertDetalleVenta(int idVenta, List<DetailSale> detailSale)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                foreach (var item in detailSale)
                {
                    using (var cmd = new SqlCommand("insertDetalleVenta", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idVenta", idVenta);
                        cmd.Parameters.AddWithValue("@idProduct", item.IdProducto);
                        cmd.Parameters.AddWithValue("@adicional", item.Adicional);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@total", item.Total);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await sql.CloseAsync();

            }
        }
        public async Task<List<Sale>> GetVentas()
        {
            var sales = new List<Sale>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getVentas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<Sale>();
                        while (await item.ReadAsync())
                        {
                            var sale = new Sale()
                            {
                                IdSale = item.GetInt32(0),
                                Total = item.GetDecimal(1),
                                FechaEmision = Convert.ToDateTime(item.GetString(2)),
                                MetodoPago = item.GetString(3),
                                Usuario = item.IsDBNull(4) ? null : item.GetString(4)
                            };
                            sales.Add(sale);
                        }
                    }
                }
            }
            return sales;
        }
        public async Task<List<DetailSale>> GetDetalleVentas(int idVenta)
        {
            var detailsSales = new List<DetailSale>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getDetalleVenta", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<DetailSale>();
                        while (await item.ReadAsync())
                        {
                            var detailSale = new DetailSale()
                            {
                                Producto = item.GetString(0),
                                Precio = item.GetDecimal(1),
                                Cantidad = item.GetInt32(2),
                                Adicional = item.GetDecimal(3),
                                Total = item.GetDecimal(4)
                            };
                            detailsSales.Add(detailSale);
                        }
                    }
                }
            }
            return detailsSales;
        }
        public async Task<bool> DeleteVenta(int idVenta)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("deleteVenta", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
    }
}

using Burger_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace Burger_API.Data
{
    public class Dshopping
    {
        readonly ConexionDB cn;
        public Dshopping(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task<List<Shopping>> GetCompras()
        {
            var compras = new List<Shopping>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getCompras", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<Shopping>();
                        while (await item.ReadAsync())
                        {
                            var compra = new Shopping()
                            {
                                IdCompra = item.GetInt32(0),
                                Usuario = item.IsDBNull(1) ? null : item.GetString(1),
                                FechaEmision = Convert.ToDateTime(item.GetString(2)),
                                Descripcion = item.GetString(3),
                                Total = item.GetDecimal(4)
                            };
                            compras.Add(compra);
                        }
                    }
                }
            }
            return compras;
        }
        public async Task<Shopping> InsertCompra(Shopping compra, int tipoCompra)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("insertCompra", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaEmision", compra.FechaEmision);
                    cmd.Parameters.AddWithValue("@descripcion", compra.Descripcion);
                    cmd.Parameters.AddWithValue("@total", compra.Total);
                    cmd.Parameters.AddWithValue("@idUser", compra.IdUsuario);
                    cmd.Parameters.AddWithValue("@tipo", tipoCompra);
                    var idCompra = await cmd.ExecuteScalarAsync();
                    await sql.CloseAsync();
                    compra.IdCompra = Convert.ToInt32(idCompra);
                    return compra;
                }
            }
        }
        public async Task InsertDetalleCompra(int idCompra, List<DetailsShop> detailShop)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                foreach (var item in detailShop)
                {
                    using (var cmd = new SqlCommand("insertDetalleCompra", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idCompra", idCompra);
                        cmd.Parameters.AddWithValue("@idProducto", item.IdProducto);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await sql.CloseAsync();

            }
        }
        public async Task<List<DetailsShop>> GetDetalleCompras(int idCompra)
        {
            var detailsShop = new List<DetailsShop>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getDetalleCompra", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCompra", idCompra);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<DetailsShop>();
                        while (await item.ReadAsync())
                        {
                            var detailShop = new DetailsShop()
                            {
                                Producto = item.GetString(0),
                                Cantidad = item.GetInt32(1)
                            };
                            detailsShop.Add(detailShop);
                        }
                    }
                }
            }
            return detailsShop;
        }
        public async Task<bool> DeleteCompra(int idCompra)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("deleteCompra", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCompra", idCompra);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
    }
}

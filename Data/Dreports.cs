using System.Data.SqlClient;
using System.Data;

namespace Burger_API.Data
{
    public class Dreports
    {
        readonly ConexionDB cn;
        public Dreports(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task<List<Dictionary<string, object>>> GetTotalVentas(string fechaInicio, string fechaFin)
        {
            var sales = new List<Dictionary<string, object>>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getTotalVentas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fFin", fechaFin);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        //if (!item.HasRows) return new List<Sale>();
                        while (await item.ReadAsync())
                        {
                            var sale = new Dictionary<string, object>()
                            {
                                { "fecha", item.GetDateTime(0) },
                                { "total", item.GetDecimal(1) }
                            };
                            sales.Add(sale);
                        }
                    }
                }
            }
            return sales;
        }
        public async Task<List<Dictionary<string, object>>> GetTotalCompras(string fechaInicio, string fechaFin)
        {
            var shops = new List<Dictionary<string, object>>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getTotalCompras", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fFin", fechaFin);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        //if (!item.HasRows) return new List<Sale>();
                        while (await item.ReadAsync())
                        {
                            var sale = new Dictionary<string, object>()
                            {
                                { "fecha", item.GetDateTime(0) },
                                { "total", item.GetDecimal(1) }
                            };
                            shops.Add(sale);
                        }
                    }
                }
            }
            return shops;
        }
        public async Task<List<Dictionary<string, object>>> GetComprasDia(string fecha)
        {
            var shops = new List<Dictionary<string, object>>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getComprasDia", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var shop = new Dictionary<string,object>()
                            {
                                { "descripcion", item.GetString(0)},
                                { "producto", item.GetString(1)},
                                { "total", item.GetString(2)}
                            };
                            shops.Add(shop);
                        }
                    }
                }
            }
            return shops;
        }
        public async Task<List<Dictionary<string, object>>> GetVentasDia(string fecha)
        {
            var sales = new List<Dictionary<string, object>>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getVentasDia", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var sale = new Dictionary<string, object>()
                            {
                                { "producto", item.GetString(0)},
                                { "total", item.GetInt32(1)}
                            };
                            sales.Add(sale);
                        }
                    }
                }
            }
            return sales;
        }
    }
}

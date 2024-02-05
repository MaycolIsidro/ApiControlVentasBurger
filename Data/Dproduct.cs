using Burger_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Burger_API.Data
{
    public class Dproduct
    {
        readonly ConexionDB cn;
        public Dproduct(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task InsertProduct(Product product)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("insertProduct", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", product.Nombre);
                    cmd.Parameters.AddWithValue("@stock", 0);
                    cmd.Parameters.AddWithValue("@price", product.Precio);
                    cmd.Parameters.AddWithValue("@idCategory", product.IdCategoria);
                    await cmd.ExecuteReaderAsync();
                    await sql.CloseAsync();
                }
            }
        }
        public async Task<List<Product>> GetProducts(string nombre)
        {
            var products = new List<Product>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getProducts", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<Product>();
                        while (await item.ReadAsync())
                        {
                            var product = new Product()
                            {
                                IdProduct = item.GetInt32(0),
                                Nombre = item.GetString(1),
                                Stock = item.GetInt32(2),
                                Precio = item.GetDecimal(3),
                                Category = item.IsDBNull(4) ? null : item.GetString(4)
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        public async Task<bool> DeleteProduct(int idProduct)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("deleteProduct", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", idProduct);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("updateProduct", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", product.Nombre);
                    cmd.Parameters.AddWithValue("@precio", product.Precio);
                    cmd.Parameters.AddWithValue("@idCategory", product.IdCategoria);
                    cmd.Parameters.AddWithValue("@idProduct", product.IdProduct);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
    }
}

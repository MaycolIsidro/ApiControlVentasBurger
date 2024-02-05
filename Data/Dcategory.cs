using Burger_API.Models;
using System.Data.SqlClient;

namespace Burger_API.Data
{
    public class Dcategory
    {
        readonly ConexionDB cn;
        public Dcategory(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task InsertCategory(Category product)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("insertCategory", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", product.Nombre);
                    await cmd.ExecuteReaderAsync();
                    await sql.CloseAsync();
                }
            }
        }
        public async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getCategories", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<Category>();
                        while (await item.ReadAsync())
                        {
                            var category = new Category()
                            {
                                IdCategory = item.GetInt32(0),
                                Nombre = item.GetString(1)
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            return categories;
        }
    }
}

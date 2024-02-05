using Burger_API.Models;
using System.Data.SqlClient;

namespace Burger_API.Data
{
    public class Duser
    {
        readonly ConexionDB cn;
        public Duser(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task<User?> LoginUser(UserRequest user)
        {
            var usuario = new User();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("loginUser", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user",user.Usuario);
                    cmd.Parameters.AddWithValue("@pass",user.Password);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return null;
                        while (await item.ReadAsync())
                        {
                            usuario.IdUser = item.GetInt32(0);
                            usuario.Nombres = item.GetString(1);
                        }
                    }
                }
            }
            return usuario;
        }
    }
}

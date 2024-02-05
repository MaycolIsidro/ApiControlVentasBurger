using Burger_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace Burger_API.Data
{
    public class Dexpense
    {
        readonly ConexionDB cn;
        public Dexpense(ConexionDB conexionDB)
        {
            cn = conexionDB;
        }
        public async Task<List<Expense>> GetEgresos(string? fechaInicio, string? fechaFin)
        {
            var egresos = new List<Expense>();
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("getEgresos", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter fInicio = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                    SqlParameter fFin = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                    if (fechaInicio != null || fechaFin != null)
                    {
                        fInicio.Value = fechaInicio;
                        fFin.Value = fechaFin;
                    }
                    else
                    {
                        fInicio.Value = DBNull.Value;
                        fFin.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(fInicio);
                    cmd.Parameters.Add(fFin);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        if (!item.HasRows) return new List<Expense>();
                        while (await item.ReadAsync())
                        {
                            var egreso = new Expense()
                            {
                                IdEgreso = item.GetInt32(0),
                                FechaEmision = item.GetDateTime(1),
                                Descripcion = item.GetString(2),
                                Total = item.GetDecimal(3)
                            };
                            egresos.Add(egreso);
                        }
                    }
                }
            }
            return egresos;
        }
        public async Task<Expense> InsertEgreso(Expense egreso)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("insertEgreso", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaEmision", egreso.FechaEmision);
                    cmd.Parameters.AddWithValue("@descripcion", egreso.Descripcion);
                    cmd.Parameters.AddWithValue("@total", egreso.Total);
                    var idEgreso = await cmd.ExecuteScalarAsync();
                    await sql.CloseAsync();
                    egreso.IdEgreso = Convert.ToInt32(idEgreso);
                    return egreso;
                }
            }
        }
        public async Task<bool> UpdateEgreso(Expense egreso)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("updateEgreso", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idEgreso", egreso.IdEgreso);
                    cmd.Parameters.AddWithValue("@descripcion", egreso.Descripcion);
                    cmd.Parameters.AddWithValue("@total", egreso.Total);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        public async Task<bool> DeleteEgreso(int idEgreso)
        {
            using (var sql = new SqlConnection(cn.ConnectiongDB))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("deleteEgreso", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idEgreso", idEgreso);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
    }
}

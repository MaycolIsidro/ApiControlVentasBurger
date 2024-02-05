using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/Egresos")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        readonly Dexpense data;
        public ExpensesController(ConexionDB conexionDB)
        {
            data = new Dexpense(conexionDB);
        }

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> Get(string? fechaInicio, string? fechaFin)
        {
            return await data.GetEgresos(fechaInicio,fechaFin);
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Post([FromBody] Expense expense)
        {
            return await data.InsertEgreso(expense);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Expense egreso, int id)
        {
            egreso.IdEgreso = id;
            return Ok(await data.UpdateEgreso(egreso));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await data.DeleteEgreso(id));
        }
    }
}

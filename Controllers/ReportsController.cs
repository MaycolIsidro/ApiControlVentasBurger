using Burger_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        readonly Dreports data;
        public ReportsController(ConexionDB conexionDB)
        {
            data = new Dreports(conexionDB);
        }

        [HttpGet("Ventas")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetTotalVentas(string fechaInicio, string fechaFin)
        {
            return await data.GetTotalVentas(fechaInicio, fechaFin);
        }

        [HttpGet("Compras")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetDetails(string fechaInicio, string fechaFin)
        {
            return await data.GetTotalCompras(fechaInicio, fechaFin);
        }

        [HttpGet("Compras/Dia")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetComprasDia(string dia)
        {
            return await data.GetComprasDia(dia);
        }

        [HttpGet("Ventas/Dia")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetVentasDia(string dia)
        {
            return await data.GetVentasDia(dia);
        }
    }
}

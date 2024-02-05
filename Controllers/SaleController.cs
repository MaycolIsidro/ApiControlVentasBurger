using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/Ventas")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        readonly Dsale data;
        public SaleController(ConexionDB conexionDB)
        {
            data = new Dsale(conexionDB);
        }

        [HttpGet]
        public async Task<ActionResult<List<Sale>>> Get()
        {
            return await data.GetVentas();
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> Post([FromBody] Sale sale)
        {
            if (sale.DetailsSale == null) return BadRequest("Es necesario insertar el detalle de la venta");
            var venta = await data.InsertVenta(sale);
            if (venta.IdSale > 0) await data.InsertDetalleVenta((int)venta.IdSale,sale.DetailsSale);
            return Ok(venta);
        }

        [HttpGet("Detalles/{id}")]
        public async Task<ActionResult<List<DetailSale>>> GetDetails(int id)
        {
            if (id == 0) return NoContent();
            return await data.GetDetalleVentas(id);
        }

        [HttpPut]
        public async Task<ActionResult> Delete(int idSale)
        {
            if (idSale == 0) return NoContent();
            return Ok(await data.DeleteVenta(idSale));
        }
    }
}

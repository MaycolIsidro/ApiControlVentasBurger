using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/Compras")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        readonly Dshopping data;
        public ShoppingController(ConexionDB conexionDB)
        {
            data = new Dshopping(conexionDB);
        }
        [HttpGet]
        public async Task<ActionResult<List<Shopping>>> Get()
        {
            return await data.GetCompras();
        }

        [HttpPost("{tipo}")]
        public async Task<ActionResult<Shopping>> Post([FromBody] Shopping compra, int tipo)
        {
            var result = await data.InsertCompra(compra, tipo);
            if (tipo == 1) return Ok(result);
            await data.InsertDetalleCompra(Convert.ToInt32(result.IdCompra), compra.Detalles);
            return Ok(result);
        }

        [HttpGet("Detalles/{id}")]
        public async Task<ActionResult<List<DetailsShop>>> Get(int id)
        {
            return await data.GetDetalleCompras(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<DetailsShop>>> Delete(int id)
        {
            await data.DeleteCompra(id);
            return Ok();
        }
    }
}

using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/Productos")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly Dproduct data;
        public ProductController(ConexionDB conexionDB)
        {
            data = new Dproduct(conexionDB);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get(string? nombre)
        {
            if (string.IsNullOrEmpty(nombre)) nombre = "%";
            return await data.GetProducts(nombre);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            await data.InsertProduct(product);
            return Ok();
        }

        [HttpDelete("{idProduct}")]
        public async Task<ActionResult> Post(int idProduct)
        {
            await data.DeleteProduct(idProduct);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Product product)
        {
            await data.UpdateProduct(product);
            return Ok();
        }
    }
}

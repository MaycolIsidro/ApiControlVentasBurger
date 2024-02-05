using Burger_API.Data;
using Burger_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Burger_API.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly Dcategory data;
        public CategoryController(ConexionDB conexionDB)
        {
            data = new Dcategory(conexionDB);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Category category)
        {
            await data.InsertCategory(category);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return await data.GetCategories();
        }
    }
}

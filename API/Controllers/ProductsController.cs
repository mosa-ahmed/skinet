using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

//we make use of ActionResult that we 've got available because of using the ControllerBase
//Returning ActionResult means it's gonna be some kind of http response status: OK200 | 400BadRequest 
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            //.ToList() command executes a select query on our database and return the results install them in this variable
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
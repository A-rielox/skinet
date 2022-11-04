using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetProductsAsync();

            return Ok(products);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            
            return product;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
    }
}

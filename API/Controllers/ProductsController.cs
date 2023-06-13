using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Inherrit from controller if you want to return HMTL page also for HTTP request
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        //Dependecy Injection inject a service via ctor like dbcontext to query db
        //When req comes in our framework routes our req to this controller and create an object of this controller 
        //then it looks at ctor and sees it needs a service , no need to worry about memoery management  becoz here framework
        //creates an object for our dependecy injection
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        //ActionResult is return for an HttpResponse
        //If we we make our below code asynchronous then main thread creates a taskand says that task
        // please go and get me some data, it goes away, meanwhile thread goes pick other request
        //upon completion task notifies main thread about completion -- makes our code more scalable as we can pick more req concurrently
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductbrands()
        {
            return Ok(await _repo.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}
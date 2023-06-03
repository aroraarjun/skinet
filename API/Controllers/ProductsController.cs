using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // Inherrit from controller if you want to return HMTL page also for HTTP request
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        //Dependecy Injection inject a service via ctor like dbcontext to query db
        //When req comes in our framework routes our req to this controller and create an object of this controller 
        //then it looks at ctor and sees it needs a service , no need to worry about memoery management  becoz here framework
        //creates an object for our dependecy injection
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        //ActionResult is return for an HttpResponse
        //If we we make our below code asynchronous then main thread creates a taskand says that task
        // please go and get me some data, it goes away, meanwhile thread goes pick other request
        //upon completion task notifies main thread about completion -- makes our code more scalable as we can pick more req concurrently
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {

        //Dependecy Injection inject a service via ctor like dbcontext to query db
        //When req comes in our framework routes our req to this controller and create an object of this controller 
        //then it looks at ctor and sees it needs a service , no need to worry about memoery management  becoz here framework
        //creates an object for our dependecy injection
        private readonly IGenericRepository<Product> _productRepo;
        public  readonly IGenericRepository<ProductBrand> _productBrandrepo;
        public  readonly IGenericRepository<ProductType> _productTyperepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandrepo, 
                                 IGenericRepository<ProductType> productTyperepo, IMapper mapper)
        {
            _productTyperepo = productTyperepo;
            _mapper = mapper;
            _productBrandrepo = productBrandrepo;
            _productRepo = productRepo;

        }

        //ActionResult is return for an HttpResponse
        //If we we make our below code asynchronous then main thread creates a taskand says that task
        // please go and get me some data, it goes away, meanwhile thread goes pick other request
        //upon completion task notifies main thread about completion -- makes our code more scalable as we can pick more req concurrently
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(spec);

            var data = _mapper
                    .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProducts(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product =  await _productRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductbrands()
        {
            return Ok(await _productBrandrepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTyperepo.ListAllAsync());
        }
    }
}
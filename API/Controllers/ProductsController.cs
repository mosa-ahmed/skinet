using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.Dtos;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
    //[ApiController] => this is reponsible for mapping the parameters that are passed into our methods. So it's doing some validation to make sure that the route parameter is like what's inside the method (int) | (string)
    
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> productTypeRepo, 
                                  IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            this.mapper = mapper;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }

        //we make use of ActionResult that we 've got available because of using the ControllerBase
        //Returning ActionResult means it's gonna be some kind of http response status: OK200 | 400BadRequest
        //because we are sending up our parameters as a query string but we 've told our API controller that these are an object : ProductSpecParams productParams then this is gonna start to look at the body of the request and of course we don't have a body when we are using [HttpGet] request and this is confusing our API controller
        //So what we need to do is to tell our API to go and look for these properties in the query string and we can do that by specifying this attribute : [FromQuery]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams); //and we are going to configure our specification (ProductsWithTypesAndBrandsSpecification) to accommodate this

            //Here we use our new spec for for the count as well
            var countSpec = new ProductWithFiltersForCountSpecfication(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            //.ToList() command executes a select query on our database and return the results install them in this variable
            var products = await _productsRepo.ListAsync(spec);

            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        //these attributes are somehowe useful because it tells us instantly just by looking at the attributes of a method what it returns from each request, but we don't have to do this with every single method, this is stupid
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //when we create a new instance of this below instead of calling the parameterless constructor, we are gonna be calling the constructor that takes a parameter and making use of the second constructor
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            //we do this to inform swagger or the client that this method can return 200ok or 404 reponse if we don't find the product that we are looking for
            if (product == null) return NotFound(new ApiResponse(404));
            //we have used our ApiResponse so that we can get our consistent errors

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}
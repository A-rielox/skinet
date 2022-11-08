using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(  IGenericRepository<Product> productsRepo,
                                    IGenericRepository<ProductBrand> productBrandRepo,
                                    IGenericRepository<ProductType> productTypeRepo,
                                    IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.ListAsync(spec);
            var productsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(productsToReturn);

            // p' ocupar Mapper pongo la config en MappingProfiles y lo agrego a Startup

            // p'verlo ocupo:
            // BaseSpecification, SpecificationEvaluator, GenericRepository, ProductsWithTypesAndBrandsSpecification
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/products/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //var product = await _productsRepo.GetByIdAsync(id);

            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        [HttpGet("brands")] // api/Products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandRepo.ListAllAsync();

            return Ok(brands);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        [HttpGet("types")] // api/Products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productTypeRepo.ListAllAsync();

            return Ok(types);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
    }
}

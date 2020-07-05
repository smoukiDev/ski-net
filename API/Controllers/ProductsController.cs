using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.DataTransferObjects;
using System.Linq;
using AutoMapper;
using API.Errors;

namespace API.Controllers
{
    public class ProductsController : BaseController
     {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IMapper mapper
        )
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepo.ListAsync(spec);
            return Ok(
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products)
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepo.GetEntityWithSpec(spec);

            if (product == null)
            {
                // TODO: Magic number
                return NotFound( new ApiResponse(404) ); 
            }

            return Ok(
                _mapper.Map<Product, ProductToReturnDTO>(product)
            );
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandRepo.ListAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productTypeRepo.ListAllAsync();
            return Ok(types);
        }
    }
}
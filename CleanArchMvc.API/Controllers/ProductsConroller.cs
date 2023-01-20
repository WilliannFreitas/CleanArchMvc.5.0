using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;
        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productsService.GetProducts();
            if (products == null)
            {
                return NotFound("Products not found");
            }
            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProducts")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productsService.GetById(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)

                return BadRequest("Invalid Data");

            await _productsService.Add(productDTO);

            return new CreatedAtRouteResult("GetProducts",
                new { id = productDTO.Id }, productDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
                return BadRequest();

            if (productDTO == null)
                return BadRequest();

            await _productsService.Update(productDTO);

            return Ok(productDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var productDto = await _productsService.GetById(id);
            if (productDto == null)
            {
                return NotFound("Product not found");
            }

            await _productsService.Remove(id);

            return Ok(productDto);

        }
    }
}

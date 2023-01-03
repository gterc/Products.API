using Microsoft.AspNetCore.Mvc;
using MediatR;
using Services;
using Models;
using DataAccess.Entities;
using Products.API;

namespace Controllers
{
    [Route("api/[controller]")]
    //TODO: produces response
    [ApiController]
    public class ProductsController : APIControllerBase
    {
        private readonly IMediator mediator;
        private readonly IKeyVaultManager secretManager;
        public ProductsController(IMediator mediator, IKeyVaultManager secretManager)
        {
            this.mediator = mediator;
            this.secretManager = secretManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await mediator.Send(new GetProducts());
            return HandleResult(result);
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetDataAsync()
        {
            try
            {
                string secretValue = await

                secretManager.GetSecret("database");

                if (!string.IsNullOrEmpty(secretValue))
                {
                    return Ok(secretValue);
                }
                else
                {
                    return NotFound("Secret key not found.");
                }
            }
            catch
            {
                return BadRequest("Error: Unable to read secret");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await mediator.Send(new GetProduct(id));
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Product product)
        {
            var result = await mediator.Send(new CreateProduct(product));
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Models.Product product)
        {
            var result = await mediator.Send(new UpdateProduct(product));
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteProduct(id));
            return HandleResult(result);
        }
    }
}

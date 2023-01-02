using Microsoft.AspNetCore.Mvc;
using MediatR;
using Services;
using Models;
using DataAccess.Entities;

namespace Controllers
{
    [Route("api/[controller]")]
    //TODO: produces response
    [ApiController]
    public class ProductsController : APIControllerBase
    {
        private readonly IMediator mediator;
        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await mediator.Send(new GetProducts());
            return HandleResult(result);
        }

        [HttpGet("{id}") ]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await mediator.Send(new GetProduct(id));
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Models.Product product)
        {
            var result = await mediator.Send(new CreateProduct(product));
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Models.Product product)
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

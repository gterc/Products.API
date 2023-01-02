using Models;
using MediatR;
using Services.Interfaces;
using FluentValidation;

namespace Services
{
    public record GetProducts() : IRequest<Result<IEnumerable<Product>>>;
    public record GetProduct(int id) : IRequest<Result<Product>>;
    public record CreateProduct(Product product) : IRequest<Result<int>>;
    public record UpdateProduct(Product product) : IRequest<Result<int>>;
    public record DeleteProduct(int id) : IRequest<Result<int>>;
    public class ProductsHandlers : IRequestHandler<GetProducts, Result<IEnumerable<Product>>>, 
        IRequestHandler<GetProduct, Result<Product>>,
         IRequestHandler<CreateProduct, Result<int>>,
         IRequestHandler<UpdateProduct, Result<int>>,
         IRequestHandler<DeleteProduct, Result<int>>
    {
        private readonly IProductsRepository productsRepository;

        public ProductsHandlers(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public async Task<Result<IEnumerable<Product>>> Handle(GetProducts request, CancellationToken cancellationToken) =>
            productsRepository.GetProductsAsync(cancellationToken).Result;

        public async Task<Result<Product>> Handle(GetProduct request, CancellationToken cancellationToken) => 
            productsRepository.GetProductAsync(request.id, cancellationToken).Result;

        public async Task<Result<int>> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            await new ProductValidator().ValidateAndThrowAsync(request.product, cancellationToken);
            return productsRepository.CreateProductAsync(request.product, cancellationToken).Result;
            
            /*make funtional:
            await (from _ in await new ProductValidator().ValidateAndThrowAsync(request.product, cancellationToken)
                   from r in await productsRepository.CreateProductAsync(request.product, cancellationToken).Result
                   select r);*/
        }

        public async Task<Result<int>> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            await new ProductValidator().ValidateAndThrowAsync(request.product, cancellationToken);
            return productsRepository.UpdateProductAsync(request.product, cancellationToken).Result;

            /*make funtional:
            await (from _ in await new ProductValidator().ValidateAndThrowAsync(request.product, cancellationToken)
                  from r in productsRepository.UpdateProductAsync(request.product, cancellationToken).Result
                  select r);
            */
        }
        public async Task<Result<int>> Handle(DeleteProduct request, CancellationToken cancellationToken) =>
            productsRepository.DeleteProductAsync(request.id, cancellationToken).Result;
    }
}

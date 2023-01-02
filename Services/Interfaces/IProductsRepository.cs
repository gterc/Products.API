using Models;

namespace Services.Interfaces
{
    public interface IProductsRepository
    {
        Task<Result<IEnumerable<Product>>> GetProductsAsync(CancellationToken cancellation = default);
        Task<Result<Product>> GetProductAsync(int id, CancellationToken cancellation = default);
        Task<Result<int>> CreateProductAsync(Product product, CancellationToken cancellation = default);
        Task<Result<int>> UpdateProductAsync(Product product, CancellationToken cancellation = default);
        Task<Result<int>> DeleteProductAsync(int id, CancellationToken cancellation = default);
    }
}

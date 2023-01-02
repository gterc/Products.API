using AutoMapper;
using Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services;
using Microsoft.Extensions.Logging;
using DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;
        public ProductsRepository(ProductsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new DataAccess.Entities.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                dbContext.Products.Add(new DataAccess.Entities.Product() { Id = 2, Name = "Monitor", Price = 150, Inventory = 200 });
                dbContext.Products.Add(new DataAccess.Entities.Product() { Id = 3, Name = "Mouse", Price = 5, Inventory = 100 });
                dbContext.Products.Add(new DataAccess.Entities.Product() { Id = 4, Name = "CPU", Price = 20, Inventory = 2000 });
                dbContext.SaveChanges();
            }
        }

        public async Task<Result<IEnumerable<Models.Product>>> GetProductsAsync(CancellationToken token = default)
        {
            var products = await dbContext.Products.ToListAsync();
            if (products != null && products.Any())
            {
                var result = mapper.Map<IEnumerable<DataAccess.Entities.Product>, IEnumerable<Models.Product>>(products);
                return new Result<IEnumerable<Models.Product>> { IsSuccess = true, Value = result };
            }
            return new Result<IEnumerable<Models.Product>> { IsSuccess = false, ErrorMessage = "Not found" };
        }

        public async Task<Result<Models.Product>> GetProductAsync(int id, CancellationToken token = default)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(i => i.Id == id);
            if (product != null)
            {
                var result = mapper.Map<DataAccess.Entities.Product, Models.Product>(product);
                return new Result<Models.Product> { IsSuccess = true, Value = result };
            }
            return new Result<Models.Product> { IsSuccess = false, ErrorMessage = "Not found" };
        }

        public async Task<Result<int>> CreateProductAsync(Models.Product product, CancellationToken token = default)
        {
            var model = mapper.Map<Models.Product, DataAccess.Entities.Product>(product);
            var result = await dbContext.Products.AddAsync(model);
            return new Result<int> { IsSuccess = true, Value = result.Entity.Id };
        }

        public async Task<Result<int>> UpdateProductAsync(Models.Product product, CancellationToken token = default)
        {
            var model = mapper.Map<Models.Product, DataAccess.Entities.Product>(product);
            var result = dbContext.Products.Update(model);
            return new Result<int> { IsSuccess = true, Value = result.Entity.Id };

        }

        public async Task<Result<int>> DeleteProductAsync(int id, CancellationToken token = default)
        {
            var product = await dbContext.Products.SingleAsync<DataAccess.Entities.Product>(cancellationToken: token);
            var result = dbContext.Products.Remove(product);
            return new Result<int> { IsSuccess = true, Value = result.Entity.Id };
        }
    }
}

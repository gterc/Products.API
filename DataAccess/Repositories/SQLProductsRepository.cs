using AutoMapper;
using Dapper;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.Repositories
{
    public class SQLProductsRepository : IProductsRepository
    {
        private readonly ProductsContext productsContext;
        private readonly IMapper mapper;

        public SQLProductsRepository(ProductsContext productsContext, IMapper mapper)
        {
            this.productsContext = productsContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> CreateProductAsync(Models.Product product, CancellationToken token = default)
        {
            var command = $"Insert [Products]([Name], Price, Inventory) " +
                $"OUTPUT INSERTED.Id Values(@Name, @Price, @Inventory)";

            using (var conn = productsContext.CreateConnection())
            {
                int id = await conn.QuerySingleAsync<int>(new CommandDefinition(command, product, cancellationToken: token));
                return new Result<int> { IsSuccess = true, Code = 201, Value = id };
            }
        }

        public async Task<Result<Models.Product>> GetProductAsync(int id, CancellationToken token = default)
        {
            var query = $"SELECT TOP 1 [Products].Id, Name, Price, Inventory FROM [Products] where [Products].Id = {id} ";

            using (var conn = productsContext.CreateConnection())
            {
                var product = await conn.QuerySingleAsync<DataAccess.Entities.Product>(new CommandDefinition(query, cancellationToken:token));

                if (product != null)
                {
                    var result = mapper.Map<DataAccess.Entities.Product, Models.Product>(product);
                    return new Result<Models.Product> { IsSuccess = true, Code = 200, Value = result };
                }
                return new Result<Models.Product> { IsSuccess = false, Code = 404, ErrorMessage = "Not found" };

            }
        }

        public async Task<Result<IEnumerable<Models.Product>>> GetProductsAsync(CancellationToken token = default)
        {
            var query = $"SELECT Id, Name, Price, Inventory FROM [Products] ";

            using (var conn = productsContext.CreateConnection())
            {
                var products = await conn.QueryAsync<DataAccess.Entities.Product>(new CommandDefinition(query, cancellationToken: token));

                if (products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<DataAccess.Entities.Product>, IEnumerable<Models.Product>>(products);
                    return new Result<IEnumerable<Models.Product>> { IsSuccess = true, Code = 200, Value = result };
                }
                return new Result<IEnumerable<Models.Product>> { IsSuccess = false, Code= 404, ErrorMessage = "Not found" };
            }
        }

        public async Task<Result<int>> UpdateProductAsync(Models.Product product, CancellationToken token = default)
        {
            var command = $"Update Products Set [Name] = @Name,Price=@Price, Inventory=@Inventory Where Id = @Id ";

            using (var conn = productsContext.CreateConnection())
            {
                var rowAffected = await conn.ExecuteAsync(new CommandDefinition( command, product, cancellationToken: token));
                if (rowAffected > 0)
                    return new Result<int> { IsSuccess = true, Code= 204, Value = rowAffected };
                return new Result<int> { IsSuccess = false, Code = 404, ErrorMessage = "Not found" };
            }
        }

        public async Task<Result<int>> DeleteProductAsync(int id, CancellationToken token = default)
        {
            var command = $"Delete Products Where Id = {id} ";

            using (var conn = productsContext.CreateConnection())
            {
                var rowAffected = await conn.ExecuteAsync(new CommandDefinition(command, cancellationToken: token));
                if (rowAffected > 0)
                    return new Result<int> { IsSuccess = true, Value = rowAffected };
                return new Result<int> { IsSuccess = false, Code = 404, ErrorMessage = "Not found" };
            }
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductValidator : AbstractValidator<Models.Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name).NotEmpty()
                .NotNull()
                .MaximumLength(10);
            RuleFor(product => product.Price).GreaterThan(0);
            RuleFor(product => product.Inventory).GreaterThan(0);
            
        }
    }
}

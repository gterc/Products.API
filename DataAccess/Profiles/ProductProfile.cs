namespace DataAccess.Profiles
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<DataAccess.Entities.Product, Models.Product>();
        }
    }
}

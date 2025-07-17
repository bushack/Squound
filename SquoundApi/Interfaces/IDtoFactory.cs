using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Interfaces
{
    public interface IDtoFactory
    {
        public ProductDto CreateProductDto(ProductModel model);
    }
}

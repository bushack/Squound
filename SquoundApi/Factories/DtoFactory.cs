using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Factories
{
    public class DtoFactory : IDtoFactory
    {
        public ProductDto CreateProductDto(ProductModel model)
        {
            var dto = new ProductDto();

            dto.Id              = model.Id;
            dto.Name            = model.Name;
            dto.Manufacturer    = model.Manufacturer;
            dto.Description     = model.Description;
            dto.Price           = model.Price;
            dto.Quantity        = model.Quantity;

            dto.Image0          = model.Image0;
            dto.Image1          = model.Image1;
            dto.Image2          = model.Image2;
            dto.Image3          = model.Image3;
            dto.Image4          = model.Image4;
            dto.Image5          = model.Image5;

            return dto;
        }
    }
}

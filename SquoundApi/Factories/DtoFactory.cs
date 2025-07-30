using System.Collections.ObjectModel;

using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Factories
{
    public class DtoFactory : IDtoFactory
    {
        /// <summary>
        /// Converts a ProductModel to a ProductDto.
        /// The ProductModel is an interal data model that directly maps to the
        /// Products table in the database.
        /// The ProductDto is a Data Transfer Object (DTO) that is used to transfer
        /// data between the server and the client application. As such, it contains
        /// only the necessary data for the client application.
        /// </summary>
        /// <param name="product">The ProductModel to convert.</param>
        /// <returns>New ProductDto object.</returns>
        public ProductDto CreateProductDto(ProductModel product)
        {
            return new ProductDto()
            {
                ProductId       = product.ProductId,
                Name            = product.Name,

                // Manufacturer is nullable in the database, so we default it to "Unknown" if it is null.
                Manufacturer    = product.Manufacturer ?? "Unknown",
                Description     = product.Description,
                Price           = product.Price,
                Quantity        = product.Quantity,

                Images          = new ObservableCollection<string>(product.Images.Select(image => image.ImageUrl))
            };
        }
    }
}

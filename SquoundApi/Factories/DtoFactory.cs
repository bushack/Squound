using System.Collections.ObjectModel;

using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Factories
{
    public class DtoFactory : IDtoFactory
    {
        /// <summary>
        /// Converts a ItemModel to a ItemDto.
        /// The ItemModel is an internal data model that directly maps to the
        /// Items table in the database.
        /// The ItemDto is a Data Transfer Object (DTO) that is used to transfer
        /// data between the server and the client application. As such, it contains
        /// only the necessary data for the client application.
        /// </summary>
        /// <param name="item">The ItemModel to convert.</param>
        /// <returns>New ItemDto object.</returns>
        public ItemDto CreateItemDto(ItemModel item)
        {
            return new ItemDto()
            {
                ItemId          = item.ItemId,
                Name            = item.Name,

                // Manufacturer is nullable in the database, so we default it to "Unknown" if it is null.
                Manufacturer    = item.Manufacturer ?? "Unknown",
                Description     = item.Description,
                Price           = item.Price,
                Quantity        = item.Quantity,

                Images          = new ObservableCollection<string>(item.Images.Select(image => image.ImageUrl))
            };
        }
    }
}

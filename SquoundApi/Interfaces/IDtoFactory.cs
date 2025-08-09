using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Interfaces
{
    public interface IDtoFactory
    {
        /// <summary>
        /// Converts an <see cref="ItemModel"/> to a <see cref="ItemSummaryDto"/>.
        /// The <see cref="ItemModel"/> is an internal API data model that directly maps to the Items table in the database.
        /// The <see cref="ItemSummaryDto"/> is a Data Transfer Object (DTO) that is used to transfer data between the server and
        /// the client application. As such, it contains only the necessary data for the client application.
        /// </summary>
        /// <param name="model">ItemModel instance to use as the source data.</param>
        /// <returns><see cref="ItemModel"/> Data Transfer Object (DTO) containing summary information about a single database item.</returns>
        public ItemSummaryDto CreateItemSummaryDto(ItemModel model);

        /// <summary>
        /// Converts an <see cref="ItemModel"/> to a <see cref="ItemDetailDto"/>.
        /// The <see cref="ItemModel"/> is an internal API data model that directly maps to the Items table in the database.
        /// The <see cref="ItemDetailDto"/> is a Data Transfer Object (DTO) that is used to transfer data between the server and
        /// the client application. As such, it contains only the necessary data for the client application.
        /// </summary>
        /// <param name="model">ItemModel instance to use as the source data.</param>
        /// <returns><see cref="ItemDetailDto"/> Data Transfer Object (DTO) containing detailed information about a single database item.</returns>
        public ItemDetailDto CreateItemDetailDto(ItemModel model);
    }
}

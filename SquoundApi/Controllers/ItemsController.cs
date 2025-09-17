using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SquoundApi.Data;
using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Controllers
{
    [ApiController]
    // Route to access the controller.
    // Token [controller] is replaced with the name of the class minus the "Controller" suffix.
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _Logger;
        private readonly DatabaseContext _DbContext;
        private readonly IDtoFactory _DtoFactory;

        public enum ErrorCode
        {
            Categories_Not_Found,

            Item_Data_Invalid,
            Item_Exists,
            Item_Does_Not_Exist,
            Item_Could_Not_Be_Created,
            Item_Could_Not_Be_Updated,
            Item_Could_Not_Be_Deleted,

            Undefined_Error
        }



        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger">Reference to a logging service instance.</param>
        /// <param name="dbContext">Reference to the database context instance.</param>
        /// <param name="dtoFactory">Reference to a DTO factory instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
        public ItemsController(ILogger<ItemsController> logger, DatabaseContext dbContext, IDtoFactory dtoFactory)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _DtoFactory = dtoFactory ?? throw new ArgumentNullException(nameof(dtoFactory));
        }


        /// <summary>
        /// Endpoint to retrieve all items from the database.
        /// </summary>
        /// <remarks>This method fetches all item records from the database and converts them into DTOs
        /// for client consumption. If no items are found, a 404 Not Found response is returned. If an error occurs
        /// during processing, a 400 Bad Request response is returned.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a list of item DTOs if items exist, a 404 Not Found
        /// response if no items are found, or a 400 Bad Request response in case of an error.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            _Logger.LogInformation("Processing GET request for all items.");

            try
            {
                var itemModels = await _DbContext.Items.ToListAsync();

                if (itemModels.Count == 0)
                {
                    return NotFound(ErrorCode.Item_Does_Not_Exist.ToString());
                }

                var itemDtos = new List<Shared.DataTransfer.ItemSummaryDto>();

                foreach (var model in itemModels)
                {
                    itemDtos.Add(_DtoFactory.CreateItemSummaryDto(model, "large"));
                }

                return Ok(itemDtos);
            }

            catch (Exception ex)
            {
                _Logger.LogDebug(ex, "Error processing GET request for all items.");
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }


        // GET : api/items/categories
        /// <summary>
        /// Endpoint to retrieve all categories and their subcategories.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            _Logger.LogInformation("Processing GET request for categories query.");

            try
            {
                var categoryDtos = await _DbContext.Categories      // Read from the Categories table in the database.
                    .Include(category => category.Subcategories)    // Include the Subcategory entity for each Category.
                    .Select(category => new CategoryDto             // Create a new CategoryDto for each Category.
                    {
                        Name = category.Name,                       // Map the Category Name property.
                        Subcategories = category.Subcategories      // Map the Subcategories property.
                        .Select(subcategory => new SubcategoryDto   // Create a new SubcategoryDto for each Subcategory.
                        {
                            Name = subcategory.Name                 // Map the Subcategory Name property.
                        })
                        .ToList()                                   // Aggregate the SubcategoryDtos into a list.
                    })
                    .ToListAsync();                                 // Execute the query and convert the results to a list of CategoryDtos.


                if (categoryDtos.IsNullOrEmpty())
                {
                    _Logger.LogCritical("No categories found.");
                    return NotFound(ErrorCode.Categories_Not_Found.ToString());
                }

                _Logger.LogInformation("{CategoryCount} categories found.", categoryDtos.Count);
                return Ok(categoryDtos);
            }

            catch (Exception ex)
            {
                _Logger.LogDebug(ex, "Error processing GET request for categories query.");
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }


        // GET : api/items/search?...
        /// <summary>
        /// Endpoint to search for items based on various criteria.
        /// </summary>
        /// <remarks>This method supports filtering, sorting, and pagination of item results. Filters
        /// can be applied for id, category, manufacturer, minimum price, and maximum price. Sorting can be performed by
        /// price, name, or ID in ascending or descending order. Pagination is controlled using the page number and page
        /// size parameters.</remarks>
        /// <param name="query">Query parameter object defining the objectives of the GET request.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of items matching the query parameters. Returns a 404
        /// status code if no items match the criteria. Returns a 400 status code if an error occurs during
        /// processing.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> GetSummaries([FromQuery]SearchQueryDto query)
        {
            _Logger.LogInformation("Processing GET request for search query: {@SearchQuery}", query);

            if (ModelState.IsValid == false)
            {
                _Logger.LogCritical("Aborting GET request. Invalid model state: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                // Build the Linq query to fetch items from the database.
                var linqQuery = _DbContext.Items                    // Read from the Items table in the database.
                    .Include(predicate => predicate.Category)       // Include the Category entity for each item.
                    .Include(predicate => predicate.Subcategory)    // Include the Subcategory entity for each item.
                    .Include(predicate => predicate.Images)         // Include the Images collection for each item.
                    .AsQueryable();                                 // Convert to IQueryable for further filtering and sorting (below).

                // Filter by subcategory.
                if (string.IsNullOrEmpty(query.Subcategory) == false)
                {
                    _Logger.LogInformation("Filtering by subcategory: {Subcategory}", query.Subcategory);
                    linqQuery = linqQuery.Where(predicate => (predicate.Subcategory.Name == query.Subcategory));
                }

                // Filter by category ONLY IF no subcategory is specified.
                else if (string.IsNullOrEmpty(query.Category) == false)
                {
                    _Logger.LogInformation("Filtering by category: {Category}", query.Category);
                    linqQuery = linqQuery.Where(predicate => (predicate.Category.Name == query.Category));
                }

                // Filter by manufacturer.
                if (string.IsNullOrEmpty(query.Manufacturer) == false)
                {
                    _Logger.LogInformation("Filtering by manufacturer: {Manufacturer}", query.Manufacturer);
                    linqQuery = linqQuery.Where(predicate => (predicate.Manufacturer == query.Manufacturer));
                }

                // Filter by material.
                if (string.IsNullOrEmpty(query.Material) == false)
                {
                    _Logger.LogInformation("Filtering by material: {Material}", query.Material);
                    linqQuery = linqQuery.Where(predicate => (predicate.Material == query.Material));
                }

                // Exclude items below minimum price.
                if ((query.MinPrice > 0) && (query.MinPrice <= (decimal)Shared.DataTransfer.Defaults.PracticalMaximumPrice))
                {
                    _Logger.LogInformation("Filtering by minimum price: {MinPrice}", query.MinPrice);
                    linqQuery = linqQuery.Where(predicate => (predicate.Price >= query.MinPrice));
                }

                // Exclude items above maximum price.
                if ((query.MaxPrice > 0) && (query.MaxPrice <= (decimal)Shared.DataTransfer.Defaults.PracticalMaximumPrice))
                {
                    _Logger.LogInformation("Filtering by maximum price: {MaxPrice}", query.MaxPrice);
                    linqQuery = linqQuery.Where(predicate => (predicate.Price <= query.MaxPrice));
                }

                // Sort by.
                {
                    _Logger.LogInformation("Sorting by: {SortBy}", query.SortBy.ToString());
                    linqQuery = query.SortBy switch
                    {
                        ItemSortOption.PriceAsc => linqQuery.OrderBy(predicate => predicate.Price),
                        ItemSortOption.PriceDesc => linqQuery.OrderByDescending(predicate => predicate.Price),

                        ItemSortOption.NameAsc => linqQuery.OrderBy(predicate => predicate.Name),
                        ItemSortOption.NameDesc => linqQuery.OrderByDescending(predicate => predicate.Name),

                        // Default sort by id.
                        _ => linqQuery.OrderBy(predicate => predicate.ItemId)
                    };
                }

                // Pagination.
                var totalItems = await linqQuery.CountAsync();

                // Execute the query and fetch the results.
                var skip = (query.PageNumber - 1) * query.PageSize;
                var take = query.PageSize;
                var modelList = await linqQuery.Skip(skip).Take(take).ToListAsync();
                _Logger.LogInformation("Paging: skipping {Skip}, taking {Take}", skip, take);

                // Check if any items were found.
                if (modelList.Count == 0)
                {
                    _Logger.LogInformation("No items found.");
                    return Ok(new SearchResponseDto<ItemSummaryDto>());
                }

                // Determine required image size based on dimensions requested by client.
                var requiredImageSize = GetRequiredImageSize(query.ImageWidth, query.ImageHeight);
                _Logger.LogInformation("Using image size: {ImageSize}", requiredImageSize.ToUpperInvariant());

                // Convert each ItemModel to an ItemDto.
                var dtoList = new List<ItemSummaryDto>();
                foreach (var model in modelList)
                {
                    dtoList.Add(_DtoFactory.CreateItemSummaryDto(model, requiredImageSize));
                }

                // Write metadata to response along with DTOs.
                var response = new SearchResponseDto<ItemSummaryDto>
                {
                    TotalItems  = totalItems,
                    PageSize    = query.PageSize,
                    CurrentPage = query.PageNumber,
                    Items       = dtoList
                };

                _Logger.LogInformation("{ItemCount} items found.", dtoList.Count);
                return Ok(response);
            }

            catch (Exception ex)
            {
                _Logger.LogDebug(ex, "Error processing GET request for search query: {@SearchQuery}", query);
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }

        // GET : api/items/detail?itemid=123456&...
        /// <summary>
        /// Endpoint to retrieve a specific item by its ID.
        /// </summary>
        /// <param name="query">Query parameter object defining the objectives of the GET request.</param>
        /// <returns>An <see cref="IActionResult"/> containing the item matching the query parameters. Returns a 404
        /// status code if no item matches the criteria. Returns a 400 status code if an error occurs during
        /// processing.</returns>
        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery]ItemDetailQueryDto query)
        {
            _Logger.LogInformation("Processing GET request for item {ItemId}.", query.ItemId);

            try
            {
                // Attempt to fetch the item from the database.
                var model = await _DbContext.Items              // Read from the Items table in the database.
                    .Include(i => i.Category)                   // Include the Category entity for each item.
                    .Include(i => i.Subcategory)                // Include the Subcategory entity for each item.
                    .Include(i => i.Images)                     // Include the Images collection for each item.
                    .FirstOrDefaultAsync(i => i.ItemId == query.ItemId);

                // Item not found.
                if (model is null)
                {
                    _Logger.LogDebug("Item {ItemId} not found.", query.ItemId);
                    return NotFound(ErrorCode.Item_Does_Not_Exist.ToString());
                }

                // Determine required image size based on dimensions requested by client.
                var requiredImageSize = GetRequiredImageSize(query.ImageWidth, query.ImageHeight);
                _Logger.LogInformation("Using image size: {ImageSize}", requiredImageSize.ToUpperInvariant());

                // Item found, convert to DTO and return.
                _Logger.LogInformation("Item {ItemId} found.", query.ItemId);
                return Ok(_DtoFactory.CreateItemDetailDto(model, requiredImageSize));
            }

            catch (Exception ex)
            {
                _Logger.LogDebug(ex, "Error processing GET request for item {ItemId}.", query.ItemId);
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ItemModel item)
        {
            _Logger.LogInformation("Processing POST request.");

            try
            {
                if ((item == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Item_Data_Invalid.ToString());
                }

                // TODO : Check if item already exists?

                _DbContext.Items.Add(item);

                var result = await _DbContext.SaveChangesAsync();

                if (result.Equals(false))
                {
                    return BadRequest(ErrorCode.Item_Could_Not_Be_Created.ToString());
                }

                return Ok(item);
            }

            catch(Exception ex)
            {
                _Logger.LogDebug(ex, "Error processing POST request.");
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }



        /// <summary>
        /// Determines the required image size category based on the provided width and height.
        /// </summary>
        /// <param name="width">Specified image width in pixels.</param>
        /// <param name="height">Specified image height in pixels.</param>
        /// <returns>String representing the required image size category: "small", "medium", "large", "xlarge" or "xxlarge".</returns>
        private static string GetRequiredImageSize(int width, int height)
        {
            if (width <= 400)
                return "small";

            if (width <= 800)
                return "medium";

            if (width <= 1200)
                return "large";

            if (width <= 1600)
                return "xlarge";

            return "xxlarge";
        }
    }
}

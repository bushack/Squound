using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SquoundApi.Data;
using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Controllers
{
    [ApiController]
    // Route to access the controller.
    // Token [controller] is replaced with the name of the class without the "Controller" suffix.
    [Route("api/[controller]")]
    public class ProductsController(DatabaseContext dbContext, IDtoFactory dtoFactory) : ControllerBase
    {
        private readonly DatabaseContext dbContext = dbContext;
        private readonly IDtoFactory dtoFactory = dtoFactory;

        public enum ErrorCode
        {
            Categories_Not_Found,

            Product_Data_Invalid,
            Product_Exists,
            Product_Does_Not_Exist,
            Product_Could_Not_Be_Created,
            Product_Could_Not_Be_Updated,
            Product_Could_Not_Be_Deleted,

            Undefined_Error
        }


        /// <summary>
        /// Endpoint to retrieve all products from the database.
        /// </summary>
        /// <remarks>This method fetches all product records from the database and converts them into DTOs
        /// for client consumption. If no products are found, a 404 Not Found response is returned. If an error occurs
        /// during processing, a 400 Bad Request response is returned.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a list of product DTOs if products exist, a 404 Not Found
        /// response if no products are found, or a 400 Bad Request response in case of an error.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            try
            {
                var productModels = await dbContext.Products.ToListAsync();

                if (productModels.Count == 0)
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                var productDtos = new List<Shared.DataTransfer.ProductDto>();

                foreach (var model in productModels)
                {
                    productDtos.Add(dtoFactory.CreateProductDto(model));
                }

                return Ok(productDtos);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching products : {ex.Message}");

                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }


        // GET : api/products/categories
        /// <summary>
        /// Endpoint to retrieve all categories and their subcategories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                Debug.WriteLine($"* * * Querying categories... * * *");

                var categoryDtos = await dbContext.Categories       // Read from the Categories table in the database.
                    .Include(c => c.Subcategories)                  // Include the Subcategory entity for each Category.
                    .Select(c => new CategoryDto                    // Create a new CategoryDto for each Category.
                    {
                        Name = c.Name,                              // Map the Category Name property.
                        Subcategories = c.Subcategories             // Map the Subcategories property.
                        .Select(s => new SubcategoyDto              // Create a new SubcategoyDto for each Subcategory.
                        {
                            Name = s.Name                           // Map the Subcategory Name property.
                        })
                        .ToList()                                   // Aggregate the SubcategoyDtos into a list.
                    })
                    .ToListAsync();                                 // Execute the query and convert the results to a list of CategoryDtos.


                if (categoryDtos.Count == 0)
                {
                    Debug.WriteLine("* * * No categories found * * *");
                    return NotFound(ErrorCode.Categories_Not_Found.ToString());
                }

                Debug.WriteLine($"* * * Returning {categoryDtos.Count} categories * * *");

                return Ok(categoryDtos);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"* * * Error fetching: {ex.Message} * * *");
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }

            finally
            {
                Debug.WriteLine($"* * * Database query complete * * *");
            }
        }


        // GET : api/products/search
        /// <summary>
        /// Endpoint to search for products based on various criteria.
        /// </summary>
        /// <remarks>This method supports filtering, sorting, and pagination of product results. Filters
        /// can be applied for id, category, manufacturer, minimum price, and maximum price. Sorting can be performed by
        /// price, name, or ID in ascending or descending order. Pagination is controlled using the page number and page
        /// size parameters.</remarks>
        /// <param name="query">The query parameters used to filter, sort, and paginate the product search results.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of products matching the query parameters. Returns a 404
        /// status code if no products match the criteria. Returns a 400 status code if an error occurs during
        /// processing.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]ProductQueryDto query)
        {
            if (ModelState.IsValid == false)
            {
                Debug.WriteLine($"* * * Invalid product model state: {ModelState} * * *");
                Debug.WriteLine($"* * * Aborting products query: {query} * * *");
                return BadRequest(ModelState);
            }

            try
            {
                Debug.WriteLine($"* * * Querying products... * * *");

                // Build the Linq query to fetch products from the database.
                var linqQuery = dbContext.Products                  // Read from the Products table in the database.
                    .Include(predicate => predicate.Category)       // Include the Category entity for each product.
                    .Include(predicate => predicate.Subcategory)    // Include the Subcategory entity for each product.
                    .Include(predicate => predicate.Images)         // Include the Images collection for each product.
                    .AsQueryable();                                 // Convert to IQueryable for further filtering and sorting (below).

                // Allows filtering by product id.
                if (query.ProductId is not null)
                {
                    Debug.WriteLine($"* * * Filtering by Id: {query.ProductId} * * *");
                    linqQuery = linqQuery.Where(predicate => (predicate.ProductId == query.ProductId));
                }

                // Filter by category.
                if (string.IsNullOrEmpty(query.Category) == false)
                {
                    Debug.WriteLine($"* * * Filtering by category: {query.Category} * * *");
                    linqQuery = linqQuery.Where(predicate => (predicate.Category.Name == query.Category));
                }

                // Filter by manufacturer.
                if (string.IsNullOrEmpty(query.Manufacturer) == false)
                {
                    Debug.WriteLine($"* * * Filtering by manufacturer: {query.Manufacturer} * * *");
                    linqQuery = linqQuery.Where(predicate => (predicate.Manufacturer == query.Manufacturer));
                }

                // Exclude products below minimum price.
                if ((query.MinPrice > 0) && (query.MinPrice <= (decimal)Shared.DataTransfer.ProductQueryDto.PracticalMaximumPrice))
                {
                    Debug.WriteLine($"* * * Applying minimum price filter: {query.MinPrice} * * *");
                    linqQuery = linqQuery.Where(predicate => (predicate.Price >= query.MinPrice));
                }

                // Exclude products above maximum price.
                if ((query.MaxPrice > 0) && (query.MaxPrice <= (decimal)Shared.DataTransfer.ProductQueryDto.PracticalMaximumPrice))
                {
                    Debug.WriteLine($"* * * Applying maximum price filter: {query.MaxPrice} * * *");
                    linqQuery = linqQuery.Where(predicate => (predicate.Price <= query.MaxPrice));
                }

                // Sort by.
                //if (query.SortBy is not null)
                {
                    Debug.WriteLine($"* * * Sorting by: {query.SortBy.ToString()} * * *");

                    linqQuery = query.SortBy switch
                    {
                        ProductSortOption.PriceAsc => linqQuery.OrderBy(predicate => predicate.Price),
                        ProductSortOption.PriceDesc => linqQuery.OrderByDescending(predicate => predicate.Price),

                        ProductSortOption.NameAsc => linqQuery.OrderBy(predicate => predicate.Name),
                        ProductSortOption.NameDesc => linqQuery.OrderByDescending(predicate => predicate.Name),

                        // Default sort by id.
                        _ => linqQuery.OrderBy(predicate => predicate.ProductId)
                    };
                }

                // Pagination.
                var skip = (query.PageNumber - 1) * query.PageSize;
                var take = query.PageSize;
                Debug.WriteLine($"* * * Paging: skipping {skip}, taking {take} * * *");

                // Execute the query and fetch the results.
                var pagedResult = await linqQuery.Skip(skip).Take(take).ToListAsync();

                // Check if any products were found.
                if (pagedResult.Count == 0)
                {
                    Debug.WriteLine("* * * No products found * * *");
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                // Convert to DTOs.
                var productDtos = new List<Shared.DataTransfer.ProductDto>();

                foreach (var model in pagedResult)
                {
                    productDtos.Add(dtoFactory.CreateProductDto(model));
                }

                Debug.WriteLine($"* * * Returning {productDtos.Count} products * * *");
                return Ok(productDtos);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"* * * Error fetching: {ex.Message} * * *");
                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }

            finally
            {
                Debug.WriteLine($"* * * Database query complete * * *");
            }
        }

        // GET : api/products/123456
        /// <summary>
        /// Endpoint to retrieve a specific product by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var query = new ProductQueryDto
            {
                ProductId = id
            };

            return await this.Search(query);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductModel product)
        {
            try
            {
                if ((product == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Product_Data_Invalid.ToString());
                }

                // TODO : Check if product already exists?

                dbContext.Products.Add(product);

                var result = await dbContext.SaveChangesAsync();

                if (result.Equals(false))
                {
                    return BadRequest(ErrorCode.Product_Could_Not_Be_Created.ToString());
                }

                return Ok(product);
            }

            catch(Exception ex)
            {
                Debug.WriteLine($"Error adding product : {ex.Message}");

                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }
    }
}

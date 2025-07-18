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
            Product_Data_Invalid,
            Product_Exists,
            Product_Does_Not_Exist,
            Product_Could_Not_Be_Created,
            Product_Could_Not_Be_Updated,
            Product_Could_Not_Be_Deleted,
            Undefined_Error
        }

        private void Sanitize(ProductQueryDto query)
        {
            if (query.MinPrice < 0)
                query.MinPrice = 0;

            if (query.MinPrice > query.MaxPrice)
                query.MinPrice = query.MaxPrice;

            if (query.MaxPrice < query.MinPrice)
                query.MaxPrice = query.MinPrice;

            if (query.PageNumber < 1)
                query.PageNumber = 1;

            if (query.PageSize < 10)
                query.PageSize = 10;

            if (query.PageSize > 100)
                query.PageSize = 100;
        }

        /// <summary>
        /// Retrieves all products from the database.
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

        /// <summary>
        /// Searches for products based on the specified query parameters.
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
            try
            {
                this.Sanitize(query);

                var products = dbContext.Products.AsQueryable();

                if (query.Id != null)
                {
                    products = products.Where(predicate => (predicate.Id == query.Id));
                }

                // Filter by category.
                if (string.IsNullOrEmpty(query.Category) == false)
                {
                    products = products.Where(predicate => (predicate.Category.Name == query.Category));
                }

                // Filter by manufacturer.
                if (string.IsNullOrEmpty(query.Manufacturer) == false)
                {
                    products = products.Where(predicate => (predicate.Manufacturer == query.Manufacturer));
                }

                // Exclude products below minimum price.
                products = products.Where(predicate => (predicate.Price >= query.MinPrice));

                // Exclude products above maximum price.
                products = products.Where(predicate => (predicate.Price <= query.MaxPrice));

                // Sort by.
                if (query.SortBy != null)
                {
                    products = query.SortBy switch
                    {
                        ProductSortOption.PriceAsc => products.OrderBy(predicate => predicate.Price),
                        ProductSortOption.PriceDesc => products.OrderByDescending(predicate => predicate.Price),

                        ProductSortOption.NameAsc => products.OrderBy(predicate => predicate.Name),
                        ProductSortOption.NameDesc => products.OrderByDescending(predicate => predicate.Name),

                        // Default sort by id.
                        _ => products.OrderBy(predicate => predicate.Id)
                    };
                }

                // Pagination.
                var pagedResult = await products.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

                if (pagedResult.Count == 0)
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                // Convert to DTOs.
                var productDtos = new List<Shared.DataTransfer.ProductDto>();

                foreach (var model in pagedResult)
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

        // GET : api/products/123456
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var query = new ProductQueryDto();

            query.Id = id;

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

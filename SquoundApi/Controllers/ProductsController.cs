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

        //
        [HttpGet("search")]
        public async Task<IActionResult> Search(ProductQueryDto query)
        {
            try
            {
                var products = dbContext.Products.AsQueryable();

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
                if (query.MinPrice.HasValue)
                {
                    products = products.Where(predicate => (predicate.Price >= query.MinPrice));
                }

                // Exclude products above maximum price.
                if (query.MaxPrice.HasValue)
                {
                    products = products.Where(predicate => (predicate.Price <= query.MaxPrice));
                }

                // Sort by.
                if (string.IsNullOrEmpty(query.SortBy) == false)
                {
                    products = query.SortBy.ToLower() switch
                    {
                        "price_asc" => products.OrderBy(predicate => predicate.Price),
                        "price_desc" => products.OrderByDescending(predicate => predicate.Price),

                        "name_asc" => products.OrderBy(predicate => predicate.Name),
                        "name_desc" => products.OrderByDescending(predicate => predicate.Name),

                        _ => products
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

        // GET : api/products/search/category?category=Lighting&sort=price_desc
        [HttpGet("search/category")]
        public async Task<IActionResult> Get(string category, string sort)
        {
            try
            {
                var query = dbContext.Products.AsQueryable();

                if (string.IsNullOrEmpty(category) == false)
                {
                    query = query.Where(predicate => (predicate.Category.Name == category));
                }

                if (sort == "price_asc")
                {
                    query = query.OrderBy(predicate => (predicate.Price));
                }

                else if (sort == "price_desc")
                {
                    query = query.OrderByDescending(predicate => (predicate.Price));
                }

                var productModels = await query.ToListAsync();

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

        // GET : api/products/123456
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            // TODO

            Debug.WriteLine($"***** ac : Not yet implemented *****");

            return NotFound(ErrorCode.Undefined_Error.ToString());
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

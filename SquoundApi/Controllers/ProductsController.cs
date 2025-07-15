using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SquoundApi.Data;
using SquoundApi.Models;
using System.Diagnostics;


namespace SquoundApi.Controllers
{
    [ApiController]
    // Route to access the controller.
    // Token [controller] is replaced with the name of the class without the "Controller" suffix.
    [Route("api/[controller]")]
    public class ProductsController(DatabaseContext dbContext) : ControllerBase
    {
        private readonly DatabaseContext dbContext = dbContext;

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

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();

                if (products.Count.Equals(0))
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                return Ok(products);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching products : {ex.Message}");

                return BadRequest(ErrorCode.Undefined_Error.ToString());
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            // TODO

            Debug.WriteLine($"ac : Not yet implemented");

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

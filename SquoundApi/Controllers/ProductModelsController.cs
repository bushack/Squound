using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;

using SquoundApi.Interfaces;
using SquoundApi.Models;


namespace SquoundApi.Controllers
{
    [ApiController]
    // Route to access the controller.
    // Token [controller] is replaced with the name of the class without the "Controller" suffix.
    [Route("api/[controller]")]
    public class ProductModelsController : ControllerBase
    {
        private readonly IProductRepository productRepository;

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

        public ProductModelsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(productRepository.All);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var product = productRepository.Find(id);

                if (product == null)
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                // Success.
                return Ok(product);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status303SeeOther, ErrorCode.Undefined_Error.ToString());
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]ProductModel product)
        {
            try
            {
                // Validate the product model.
                if ((product == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Product_Data_Invalid.ToString());
                }

                bool productExists = productRepository.DoesProductExist(product.Id);

                // Product already exists with the same ID.
                if (productExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.Product_Exists.ToString());
                }

                // Success.
                productRepository.Insert(product);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Product_Could_Not_Be_Created.ToString());
            }

            return Ok(product);
        }

        [HttpPut]
        public IActionResult Edit([FromBody]ProductModel product)
        {
            try
            {
                // Validate the product model.
                if ((product == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Product_Data_Invalid.ToString());
                }

                // No product exists with the given ID.
                if (productRepository.Find(product.Id) == null)
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                // Success.
                productRepository.Update(product);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Product_Could_Not_Be_Updated.ToString());
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (productRepository.Find(id) == null)
                {
                    return NotFound(ErrorCode.Product_Does_Not_Exist.ToString());
                }

                // Success.
                productRepository.Delete(id);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Product_Could_Not_Be_Deleted.ToString());
            }

            return NoContent();
        }
    }
}
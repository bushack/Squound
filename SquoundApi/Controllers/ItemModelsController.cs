using Microsoft.AspNetCore.Mvc;

using SquoundApi.Interfaces;
using SquoundApi.Models;


namespace SquoundApi.Controllers
{
    [ApiController]
    // Route to access the controller.
    // Token [controller] is replaced with the name of the class without the "Controller" suffix.
    [Route("api/[controller]")]
    public class ItemModelsController(IItemRepository itemRepository) : ControllerBase
    {
        private readonly IItemRepository itemRepository = itemRepository;

        public enum ErrorCode
        {
            Item_Data_Invalid,
            Item_Exists,
            Item_Does_Not_Exist,
            Item_Could_Not_Be_Created,
            Item_Could_Not_Be_Updated,
            Item_Could_Not_Be_Deleted,
            Undefined_Error
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(itemRepository.All);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var item = itemRepository.Get(id);

                if (item == null)
                {
                    return NotFound(ErrorCode.Item_Does_Not_Exist.ToString());
                }

                // Success.
                return Ok(item);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status303SeeOther, ErrorCode.Undefined_Error.ToString());
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]ItemModel item)
        {
            try
            {
                // Validate the item model.
                if ((item == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Item_Data_Invalid.ToString());
                }

                bool itemExists = itemRepository.DoesItemExist(item.ItemId);

                // Item already exists with the same ID.
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.Item_Exists.ToString());
                }

                // Success.
                itemRepository.Insert(item);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Item_Could_Not_Be_Created.ToString());
            }

            return Ok(item);
        }

        [HttpPut]
        public IActionResult Edit([FromBody]ItemModel item)
        {
            try
            {
                // Validate the item model.
                if ((item == null) || (ModelState.IsValid == false))
                {
                    return BadRequest(ErrorCode.Item_Data_Invalid.ToString());
                }

                // No item exists with the given ID.
                if (itemRepository.Find(item.ItemId) == null)
                {
                    return NotFound(ErrorCode.Item_Does_Not_Exist.ToString());
                }

                // Success.
                itemRepository.Update(item);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Item_Could_Not_Be_Updated.ToString());
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (itemRepository.Find(id) == null)
                {
                    return NotFound(ErrorCode.Item_Does_Not_Exist.ToString());
                }

                // Success.
                itemRepository.Delete(id);
            }

            catch (Exception)
            {
                return BadRequest(ErrorCode.Item_Could_Not_Be_Deleted.ToString());
            }

            return NoContent();
        }
    }
}
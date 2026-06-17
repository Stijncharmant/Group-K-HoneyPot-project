using API.Models.Part;
using DomainModels;
using LogicLayer.Management;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartsController : ControllerBase
    {
        // private fields
        private readonly PartManagement _partManagement;

        // constructor
        public PartsController(PartManagement partManagement)
        {
            _partManagement = partManagement;
        }

        // methods
        #region CREATE

        [HttpPost]
        public IActionResult AddPart([FromBody] PartCreateDto newPart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _partManagement.AddPart(newPart.Sku, newPart.Name, newPart.Description, newPart.Price, newPart.Quantity, newPart.Year, newPart.CategoryId, newPart.ManufacturerId);

            return Ok("Part added");
        }
                                         
        #endregion

        #region READ

        [HttpGet]
        public ActionResult<List<PartSummaryDto>> GetAllParts()
        {
            var parts = _partManagement.GetAllParts();

            var dtos = parts.Select(p => new PartSummaryDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<PartDetailsDto> GetPartById(int id)
        {
            var part = _partManagement.GetPartById(id);

            if (part == null)
                return NotFound($"Part with ID {id} not found.");

            var dto = new PartDetailsDto
            {
                Id = part.Id,
                Sku = part.Sku,
                Name = part.Name,
                Description = part.Description,
                Price = part.Price,
                Quantity = part.Quantity,
                Year = part.Year,
                CategoryId = part.CategoryId,
                ManufacturerId = part.ManufacturerId
            };

            return Ok(dto);
        }

        #endregion

        #region UPDATE

        [HttpPut("{id}")]
        public IActionResult UpdatePart(int id, [FromBody] PartUpdateDto updatedPart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPart = _partManagement.GetPartById(id);

            if (existingPart == null)
                return NotFound($"Part with ID {id} not found.");

            var part = new Part(
                updatedPart.Sku,
                updatedPart.Name,
                updatedPart.Description,
                updatedPart.Price,
                updatedPart.Quantity,
                updatedPart.Year,
                updatedPart.CategoryId,
                updatedPart.ManufacturerId
            );

            typeof(Part)
                .GetProperty("Id")!
                .SetValue(part, id);

            _partManagement.UpdatePart(part);

            return NoContent();
        }

        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult DeletePart(int id)
        {
            var part = _partManagement.GetPartById(id);

            if (part == null)
                return NotFound($"Part with ID {id} not found.");

            _partManagement.DeletePart(id);

            return NoContent();
        }

        #endregion
    }
}

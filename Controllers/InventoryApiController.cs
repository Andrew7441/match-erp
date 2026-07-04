using ERP.Data;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("/api/inventory")]
    [ApiController]
    public class InventoryApiController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/inventory
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.InventoryItems.ToList());
        }

        // GET /api/inventory/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.InventoryItems.Select(i => i.ID == id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST /api/inventory

        [HttpPost]
        public IActionResult Create([FromBody] InventoryItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.InventoryItems.Add(item);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = item.ID }, item);
        }

        // PUT /api/inventory/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] InventoryItem item)
        {

            if(id != item.ID)
            {
                return BadRequest("ID in URL does not match item ID in body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = _context.InventoryItems.Any(i => i.ID == id);

            if (!exists)
            {
                return NotFound();
            }

            _context.InventoryItems.Update(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.ID == id);

            if(item == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }

    }
}

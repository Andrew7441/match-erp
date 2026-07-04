using ERP.Data;
using Microsoft.AspNetCore.Mvc;
using ERP.Models;

namespace ERP.Controllers
{
    [Route("api/staff")] // sets the base url to allow mapping
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StaffApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/staff
        [HttpGet]
        public IActionResult Staff()
        {
            var staff = _context.Staff.ToList();
            return Ok(staff);
        }

        // GET /api/staff/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var staff = _context.Staff.FirstOrDefault(s => s.ID == id);

            if(staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

        // POST /api/staff
        // Creates a new staff member from JSON request Body
        public IActionResult Create([FromBody] Staff staff) // [FromBody] reads JSON from request Body
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Staff.Add(staff);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = staff.ID }, staff); // returns HTTP 201 Created
        }

        // PUT /api/staff
        // Updates staff member from JSON request Body
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Staff staff)
        {
           if(id != staff.ID)
            {
                return BadRequest("ID in URL does not match staff ID in body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = _context.Staff.Any(s => s.ID == id);

            if (!exists)
            {
                return NotFound();
            }

            // Mark staff as modified.
            _context.Staff.Update(staff);

            // Save UPDATE to Azure SQL.
            _context.SaveChanges();

            // 204 means success, no response body needed
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Find staff row by ID
            var staff = _context.Staff.FirstOrDefault(s => s.ID == id);

            if (staff == null)
            {
                return NotFound();
            }

            //Mark row for deletion
            _context.Staff.Remove(staff);
            
            //Save DELETE to Azure SQL
            _context.SaveChanges();

            //204 means deleted successfully
            return NoContent();
        }
    }
}

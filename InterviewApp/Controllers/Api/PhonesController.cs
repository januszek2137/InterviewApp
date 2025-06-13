using InterviewApp.Data;
using InterviewApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewApp.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public PhonesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: api/Phones
        [HttpGet]
        public IActionResult GetPhones() {
            var phones = _context.PhoneModel.ToList();
            return Ok(phones);
        }

        // GET: api/Phones/{id}
        [HttpGet("{id}")]
        public IActionResult GetPhone(Guid id) {
            var phone = _context.PhoneModel.Find(id);
            if(phone == null) {
                return NotFound();
            }
            return Ok(phone);
        }

        // POST: api/Phones
        [HttpPost]
        public IActionResult CreatePhone([FromBody] PhoneModel phone) {
            if(phone == null || !ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            _context.PhoneModel.Add(phone);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPhone), new { id = phone.Id }, phone);
        }

        // PUT: api/Phones/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePhone(Guid id, [FromBody] PhoneModel phone) {
            if(phone == null || id != phone.Id || !ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var existingPhone = _context.PhoneModel.Find(id);
            if(existingPhone == null) {
                return NotFound();
            }

            _context.Entry(existingPhone).CurrentValues.SetValues(phone);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Phones/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePhone(Guid id) {
            var phone = _context.PhoneModel.Find(id);
            if(phone == null) {
                return NotFound();
            }
            _context.PhoneModel.Remove(phone);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

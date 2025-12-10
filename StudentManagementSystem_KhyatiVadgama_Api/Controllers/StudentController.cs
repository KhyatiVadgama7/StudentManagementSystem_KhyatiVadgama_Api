using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem_KhyatiVadgama_Application.DTOs;
using StudentManagementSystem_KhyatiVadgama_Application.Services;

namespace StudentManagementSystem_KhyatiVadgama_Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool desc = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (data, total) = await _studentService.GetStudentsAsync(search, sortBy, desc, page, pageSize);

            return Ok(new
            {
                total,
                page,
                pageSize,
                data
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _studentService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequest req)
        {
            try
            {
                var dto = await _studentService.CreateAsync(req);

                return CreatedAtAction(nameof(GetById),
                    new { id = dto.Id }, dto);
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentRequest req)
        {
            try
            {
                var ok = await _studentService.UpdateAsync(id, req);
                if (!ok) return NotFound();

                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _studentService.DeleteAsync(id);
            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}


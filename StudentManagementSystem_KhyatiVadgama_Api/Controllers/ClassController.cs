using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem_KhyatiVadgama_Domain.Entities;
using StudentManagementSystem_KhyatiVadgama_Domain.Interfaces;
using System.Globalization;

namespace StudentManagementSystem_KhyatiVadgama_Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly long MaxFileBytes = 5 * 1024 * 1024;

        public ClassesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _unitOfWork.Classes.ListAsync());

        [HttpPost("import")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new
                {
                    message = "file required"
                });
            }

            if (file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "empty file"
                });
            }

            if (file.Length > MaxFileBytes)
            {
                return BadRequest(new
                {
                    message = "file exceeds 5 MB"
                });
            }

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "only csv allowed"
                });
            }

            var errors = new List<string>();
            var created = 0;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>();

            foreach (var recObj in records)
            {
                try
                {
                    var dict = (IDictionary<string, object>)recObj;
                    var name = dict.ContainsKey("Name") ? dict["Name"]?.ToString() : null;
                    var desc = dict.ContainsKey("Description") ? dict["Description"]?.ToString() : null;

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        errors.Add("Name required in one row");
                        continue;
                    }

                    if (desc != null && desc.Length > 100)
                    {
                        errors.Add($"Description too long for {name}");
                        continue;
                    }

                    if (await _unitOfWork.Classes.ExistsAsync(c => c.Name == name))
                    {
                        errors.Add($"Duplicate class {name}");
                        continue;
                    }

                    var cls = new Class { Name = name!, Description = desc };
                    await _unitOfWork.Classes.AddAsync(cls);

                    created++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Bad row: {ex.Message}");
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { created, errors });
        }
    }
}

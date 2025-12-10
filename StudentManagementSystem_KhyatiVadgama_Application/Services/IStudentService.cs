using StudentManagementSystem_KhyatiVadgama_Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Application.Services
{
    public interface IStudentService
    {
        Task<(IEnumerable<StudentDto> Data, int Total)> GetStudentsAsync(string? search, string? sortBy, bool desc, int page, int pageSize);
        Task<StudentDto?> GetByIdAsync(Guid id);
        Task<StudentDto> CreateAsync(StudentDto req);
        Task<bool> UpdateAsync(Guid id, StudentDto req);
        Task<bool> DeleteAsync(Guid id);
    }
}

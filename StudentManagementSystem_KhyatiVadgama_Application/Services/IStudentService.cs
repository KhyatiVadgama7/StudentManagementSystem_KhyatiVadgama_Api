using StudentManagementSystem_KhyatiVadgama_Application.DTOs;

namespace StudentManagementSystem_KhyatiVadgama_Application.Services
{
    public interface IStudentService
    {
        Task<(IEnumerable<StudentDto> Data, int Total)> GetStudentsAsync(string? search, string? sortBy, bool desc, int page, int pageSize);
        Task<StudentDto?> GetByIdAsync(Guid id);
        Task<StudentDto> CreateAsync(CreateStudentRequest req);
        Task<bool> UpdateAsync(Guid id, UpdateStudentRequest req);
        Task<bool> DeleteAsync(Guid id);
    }
}

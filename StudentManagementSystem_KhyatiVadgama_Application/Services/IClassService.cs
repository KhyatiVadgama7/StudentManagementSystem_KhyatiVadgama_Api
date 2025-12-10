using StudentManagementSystem_KhyatiVadgama_Application.DTOs;

namespace StudentManagementSystem_KhyatiVadgama_Application.Services
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetAllAsync();
        Task<ClassDto?> GetByIdAsync(Guid id);
        Task<ClassDto> CreateAsync(CreateClassRequest request);
        Task<ClassDto> UpdateAsync(Guid id, CreateClassRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}

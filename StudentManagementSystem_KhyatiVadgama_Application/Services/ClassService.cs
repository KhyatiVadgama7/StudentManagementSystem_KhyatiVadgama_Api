using AutoMapper;
using StudentManagementSystem_KhyatiVadgama_Application.DTOs;
using StudentManagementSystem_KhyatiVadgama_Domain.Entities;
using StudentManagementSystem_KhyatiVadgama_Domain.Interfaces;

namespace StudentManagementSystem_KhyatiVadgama_Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDto>> GetAllAsync()
        {
            var classes = await _unitOfWork.Classes.ListAsync();
            return _mapper.Map<IEnumerable<ClassDto>>(classes);
        }

        public async Task<ClassDto?> GetByIdAsync(Guid id)
        {
            var cls = await _unitOfWork.Classes.GetByIdAsync(id);
            return cls == null ? null : _mapper.Map<ClassDto>(cls);
        }

        public async Task<ClassDto> CreateAsync(CreateClassRequest request)
        {
            // Validate duplication
            bool exists = await _unitOfWork.Classes.ExistsAsync(c => c.Name == request.Name);
            if (exists)
                throw new Exception($"Class '{request.Name}' already exists.");

            var cls = new Class
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Classes.AddAsync(cls);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClassDto>(cls);
        }

        public async Task<ClassDto> UpdateAsync(Guid id, CreateClassRequest request)
        {
            var cls = await _unitOfWork.Classes.GetByIdAsync(id);
            if (cls == null)
                throw new Exception("Class not found.");

            // Check duplicate name with other classes
            bool exists = await _unitOfWork.Classes.ExistsAsync(c => c.Name == request.Name && c.Id != id);
            if (exists)
                throw new Exception($"Another class already exists with name '{request.Name}'.");

            cls.Name = request.Name;
            cls.Description = request.Description;

            _unitOfWork.Classes.Update(cls);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClassDto>(cls);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var cls = await _unitOfWork.Classes.GetByIdAsync(id);
            if (cls == null)
                return false;

            _unitOfWork.Classes.Delete(cls);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

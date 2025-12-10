using AutoMapper;
using StudentManagementSystem_KhyatiVadgama_Application.DTOs;
using StudentManagementSystem_KhyatiVadgama_Domain.Entities;
using StudentManagementSystem_KhyatiVadgama_Domain.Interfaces;
using System.Linq.Expressions;

namespace StudentManagementSystem_KhyatiVadgama_Application.Services
{
    public class StudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

        public async Task<(IEnumerable<StudentDto>, int)> GetStudentsAsync(string? search, string? sortBy, bool desc, int page, int pageSize)
        {
            Expression<Func<Student, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                filter = s == null ? null : (Expression<Func<Student, bool>>)(st =>
                    st.FirstName.ToLower().Contains(s) ||
                    st.LastName.ToLower().Contains(s) ||
                    st.Email.ToLower().Contains(s) ||
                    st.PhoneNumber.Contains(s));
            }

            Func<IQueryable<Student>, IOrderedQueryable<Student>>? orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("FirstName", StringComparison.OrdinalIgnoreCase))
                    orderBy = q => desc ? q.OrderByDescending(s => s.FirstName) : q.OrderBy(s => s.FirstName);
                else if (sortBy.Equals("LastName", StringComparison.OrdinalIgnoreCase))
                    orderBy = q => desc ? q.OrderByDescending(s => s.LastName) : q.OrderBy(s => s.LastName);
                else orderBy = q => q.OrderBy(s => s.FirstName);
            }

            var skip = (page - 1) * pageSize;
            var list = await _uow.Students.ListAsync(filter, orderBy, skip, pageSize);
            var total = (await _uow.Students.ListAsync(filter)).Count();
            var dtos = list.Select(s =>
            {
                return new StudentDto()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Id = s.Id,
                    PhoneNumber = s.PhoneNumber,
                    ClassIds = s.StudentClasses.Select(c => c.ClassId).ToList()
                };
            });
            return (dtos, total);
        }

        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var s = await _uow.Students.GetByIdAsync(id);
            if (s == null) return null;

            return new StudentDto()
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Id = s.Id,
                PhoneNumber = s.PhoneNumber,
                ClassIds = s.StudentClasses.Select(c => c.ClassId).ToList()
            };
        }

        public async Task<StudentDto> CreateAsync(CreateStudentRequest req)
        {
            if (await _uow.Students.ExistsAsync(s => s.Email == req.Email))
                throw new ApplicationException("Email already exists");
            if (await _uow.Students.ExistsAsync(s => s.PhoneNumber == req.PhoneNumber))
                throw new ApplicationException("Phone already exists");

            var student = new Student()
            { 
                FirstName = req.FirstName, 
                LastName = req.LastName, 
                Email = req.Email, 
                PhoneNumber = req.PhoneNumber 
            };

            var classes = new List<Class>();
            foreach (var cid in req.ClassIds.Distinct())
            {
                var c = await _uow.Classes.GetByIdAsync(cid);
                if (c == null) throw new ApplicationException($"Class {cid} not found");
                student.StudentClasses.Add(new StudentClass { ClassId = cid, Student = student });
            }

            await _uow.Students.AddAsync(student);
            await _uow.SaveChangesAsync();

            return new StudentDto()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Id = student.Id,
                PhoneNumber = student.PhoneNumber,
                ClassIds = student.StudentClasses.Select(c => c.ClassId).ToList()
            };
        }

        public async Task<bool> UpdateAsync(Guid id, CreateStudentRequest req)
        {
            var student = await _uow.Students.GetByIdAsync(id);
            if (student == null) return false;

            if (await _uow.Students.ExistsAsync(s => s.Email == req.Email && s.Id != id))
                throw new ApplicationException("Email already exists");
            if (await _uow.Students.ExistsAsync(s => s.PhoneNumber == req.PhoneNumber && s.Id != id))
                throw new ApplicationException("Phone already exists");

            student.FirstName = req.FirstName;
            student.LastName = req.LastName;
            student.Email = req.Email;
            student.PhoneNumber = req.PhoneNumber;

            var targetClassIds = req.ClassIds.Distinct().ToHashSet();
            var existing = student.StudentClasses.Select(sc => sc.ClassId).ToHashSet();

            foreach (var sc in student.StudentClasses.Where(sc => !targetClassIds.Contains(sc.ClassId)).ToList())
                student.StudentClasses.Remove(sc);

            foreach (var cid in targetClassIds.Except(existing))
            {
                var c = await _uow.Classes.GetByIdAsync(cid);
                if (c == null) throw new ApplicationException($"Class {cid} not found");
                student.StudentClasses.Add(new StudentClass { ClassId = cid, StudentId = student.Id });
            }

            _uow.Students.Update(student);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _uow.Students.GetByIdAsync(id);
            if (student == null) return false;
            _uow.Students.Delete(student);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}

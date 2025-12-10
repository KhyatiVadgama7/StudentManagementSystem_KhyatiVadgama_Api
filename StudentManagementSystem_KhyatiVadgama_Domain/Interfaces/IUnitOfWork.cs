using StudentManagementSystem_KhyatiVadgama_Domain.Entities;

namespace StudentManagementSystem_KhyatiVadgama_Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Student> Students { get; }
        IRepository<Class> Classes { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

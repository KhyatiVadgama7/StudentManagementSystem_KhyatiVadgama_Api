using StudentManagementSystem_KhyatiVadgama_Domain.Entities;
using StudentManagementSystem_KhyatiVadgama_Domain.Interfaces;
using StudentManagementSystem_KhyatiVadgama_Infrastructure.Context;

namespace StudentManagementSystem_KhyatiVadgama_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IRepository<Student> Students { get; }
        public IRepository<Class> Classes { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Students = new Repository<Student>(_db);
            Classes = new Repository<Class>(_db);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _db.SaveChangesAsync(ct);

        public ApplicationDbContext Context => _db;

        public void Dispose() => _db.Dispose();
    }
}

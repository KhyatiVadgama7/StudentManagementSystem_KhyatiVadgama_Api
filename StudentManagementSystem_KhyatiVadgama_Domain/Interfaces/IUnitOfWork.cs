using StudentManagementSystem_KhyatiVadgama_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Student> Students { get; }
        IRepository<Class> Classes { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

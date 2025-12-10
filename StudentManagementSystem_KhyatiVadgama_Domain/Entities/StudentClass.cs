using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Domain.Entities
{
    public class StudentClass
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!; // number only, max 10 digits
        public string Email { get; set; } = null!;

        // Navigation
        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    }
}

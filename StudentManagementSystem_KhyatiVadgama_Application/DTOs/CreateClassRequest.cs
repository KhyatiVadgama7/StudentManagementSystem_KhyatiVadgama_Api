using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Application.DTOs
{
    public class CreateClassRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}

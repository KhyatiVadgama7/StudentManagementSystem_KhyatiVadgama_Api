namespace StudentManagementSystem_KhyatiVadgama_Domain.Entities
{
    public class Class
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; } // max 100 chars

        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    }
}

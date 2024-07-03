using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public CategoryDTO(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public Category MapToEntity()
        {
            return new() { Name = Name };
        }
    }
}

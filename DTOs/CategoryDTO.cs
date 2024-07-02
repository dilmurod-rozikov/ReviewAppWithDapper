using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public CategoryDTO(string name)
        {
            Name = name;
        }

        public Category MapToEntity()
        {
            return new() { Name = Name };
        }
    }
}

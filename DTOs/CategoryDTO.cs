using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public CategoryDTO(Category category)
        {
            Name = category.Name;
        }

        public Category MapToEntity()
        {
            return new() { Name = Name };
        }
    }
}

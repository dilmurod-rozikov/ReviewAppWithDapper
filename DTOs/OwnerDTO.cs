using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class OwnerDTO
    {
        public string Name { get; set; }

        public string Gym { get; set; }

        public OwnerDTO(string name, string gym)
        {
            Name = name;
            Gym = gym;
        }

        public Owner MapToEntity()
        {
            return new() { Name = Name, Gym = Gym };
        }
    }
}

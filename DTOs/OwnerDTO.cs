using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class OwnerDTO
    {
        public string Name { get; set; }

        public string Gym { get; set; }

        public int Id { get; set; }

        public OwnerDTO(string name, string gym, int id)
        {
            Name = name;
            Gym = gym;
            Id = id;
        }

        public Owner MapToEntity()
        {
            return new() { Name = Name, Gym = Gym };
        }
    }
}

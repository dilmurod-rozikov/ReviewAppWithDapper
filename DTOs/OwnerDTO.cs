using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class OwnerDTO
    {
        public string Name { get; set; }

        public string Gym { get; set; }

        public OwnerDTO(Owner owner)
        {
            Name = owner.Name;
            Gym = owner.Gym;
        }

        public Owner MapToEntity()
        {
            return new() { Name = Name, Gym = Gym };
        }
    }
}

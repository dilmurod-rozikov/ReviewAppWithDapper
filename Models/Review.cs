﻿namespace ReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public Reviewer? Reviewer { get; set; }

        public Pokemon? Pokemon { get; set; }

        public int PokemonId { get; set; }
    }
}

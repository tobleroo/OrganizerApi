﻿using Newtonsoft.Json.Linq;

namespace OrganizerApi.Cookbook.CookModels
{
    public class Recipe
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public required string RecipeName { get; set; }
        public string? Description { get; set; } = "not set";
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Steps { get; set; } = new List<string>();

        public int CookTime { get; set; } = 0;

        public RecipeDifficulties Difficulty { get; set; } = RecipeDifficulties.Easy;
        public int TimesCooked { get; set; } = 0;
        public bool IsVegan { get; set; } = false;

        private int _spicyness = 0;
        public int Spicyness
        {
            get { return _spicyness; }
            set
            {
                if (value >= 1 && value <= 5) _spicyness = value;
                else throw new ArgumentOutOfRangeException("Spicyness must be between 1 and 5.");
            }
        }

    }
}

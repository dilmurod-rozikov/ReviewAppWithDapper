﻿using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<Category?> GetCategory(int id);

        Task<bool> CategoryExists(int categoryId);

        Task<bool> CreateCategory(Category category);

        Task<bool> UpdateCategory(string name, int categoryId);

        Task<bool> DeleteCategory(int id);
    }
}

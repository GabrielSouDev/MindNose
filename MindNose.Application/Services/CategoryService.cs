using MindNose.Domain.Interfaces.Commons;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Nodes;
using MindNose.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNose.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private List<CategoryResult> _categories = new();

        public void AddCategory(CategoryResult categories) =>
            _categories.Add(categories);

        public CategoryResult? GetCategory(string categoryId) =>
            _categories.FirstOrDefault(c => c.TitleId == categoryId);

        public List<CategoryResult> GetCategories() =>
            _categories;

        public void SetCategories(List<CategoryResult> categories)
        {
            _categories = categories;
            Console.WriteLine("- Lista de Categorias carregada!");
        }
    }
}

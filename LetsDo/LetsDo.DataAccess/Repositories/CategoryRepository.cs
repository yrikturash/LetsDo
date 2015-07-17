using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDo.DataAccess.Entities;

namespace LetsDo.DataAccess.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories;
        }

        public Category Get(int id)
        {
            return _db.Categories.Find(id);
        }

        public void Create(Category category)
        {
            _db.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _db.Entry(category).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Category category = _db.Categories.Find(id);
            if (category != null)
                _db.Categories.Remove(category);
        }
    }
}

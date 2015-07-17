using System.Collections.Generic;
using System.Data.Entity;
using LetsDo.DataAccess.Entities;

namespace LetsDo.DataAccess.Repositories
{
    public class IssueRepository : IRepository<Issue>
    {
        private readonly ApplicationDbContext _db;

        public IssueRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public IEnumerable<Issue> GetAll()
        {
            return _db.Issues;
        }

        public Issue Get(int id)
        {
            return _db.Issues.Find(id);
        }

        public void Create(Issue issue)
        {
            _db.Issues.Add(issue);
        }

        public void Update(Issue issue)
        {
            _db.Entry(issue).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Issue issue = _db.Issues.Find(id);
            if (issue != null)
                _db.Issues.Remove(issue);
        }

    }
}
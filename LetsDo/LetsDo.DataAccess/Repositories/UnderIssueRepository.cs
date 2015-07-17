using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDo.DataAccess.Entities;

namespace LetsDo.DataAccess.Repositories
{
    public class UnderIssueRepository : IRepository<UnderIssue>
    {
        private readonly ApplicationDbContext _db;

        public UnderIssueRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public IEnumerable<UnderIssue> GetAll()
        {
            return _db.UnderIssues;
        }

        public UnderIssue Get(int id)
        {
            return _db.UnderIssues.Find(id);
        }

        public void Create(UnderIssue underIssue)
        {
            _db.UnderIssues.Add(underIssue);
        }

        public void Update(UnderIssue underIssue)
        {
            _db.Entry(underIssue).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UnderIssue underIssue = _db.UnderIssues.Find(id);
            if (underIssue != null)
                _db.UnderIssues.Remove(underIssue);
        }
    }
}

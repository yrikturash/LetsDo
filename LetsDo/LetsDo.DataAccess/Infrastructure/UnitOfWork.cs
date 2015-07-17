using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDo.DataAccess.Entities;
using LetsDo.DataAccess.Repositories;

namespace LetsDo.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private IssueRepository _issueRepository;
        private UnderIssueRepository _underIssueRepository;
        private CategoryRepository _categoryRepository;

        public IssueRepository Issues
        {
            get
            {
                return _issueRepository ?? (_issueRepository = new IssueRepository(_db));
            }
        }

        public UnderIssueRepository UnderIssues
        {
            get
            {
                return _underIssueRepository ?? (_underIssueRepository = new UnderIssueRepository(_db));
            }
        }

        public CategoryRepository Categories
        {
            get { return _categoryRepository ?? (_categoryRepository = new CategoryRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

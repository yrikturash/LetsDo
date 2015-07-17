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
        private ApplicationDbContext db = new ApplicationDbContext();
        private IssueRepository _issueRepository;
        private UnderIssueRepository _underIssueRepository;

        public IssueRepository Issues
        {
            get
            {
                return _issueRepository ?? (_issueRepository = new IssueRepository(db));
            }
        }

        public UnderIssueRepository UnderIssues
        {
            get
            {
                return _underIssueRepository ?? (_underIssueRepository = new UnderIssueRepository(db));
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
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

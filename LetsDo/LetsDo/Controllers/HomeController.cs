using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LetsDo.DataAccess;
using LetsDo.DataAccess.Entities;
using LetsDo.DTO.ViewModels;
using Microsoft.Ajax.Utilities;

namespace LetsDo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
        }
        public ActionResult Index(int? categoryId = null)
        {
                var issues = (categoryId.HasValue) ?                     
                    _unitOfWork.Issues.GetAll().Where(n => n.CategoryId == categoryId).OrderByDescending(n => n.CreatedTime).ToList() :
                    _unitOfWork.Issues.GetAll().OrderByDescending(n => n.CreatedTime).ToList();

                IList<UnderIssue> uIssuesList = new List<UnderIssue>();
                if (issues.Count > 0)
                {
                    var id = _unitOfWork.Issues.Get(0).Id;
                    uIssuesList = _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id).ToList();
                }
            return View(new IndexViewModel()
            {
                Issues = issues,
                UnderIssues = uIssuesList,
                CategoriesList = _unitOfWork.Categories.GetAll().OrderByDescending(n => n.Id).ToList(),
                ActiveCategoryId = categoryId
            });
        }

        public JsonResult AddNewIssue(string Text, int? CategoryId)
        {
            var issue = new Issue()
            {
                Text = Text,
                CreatedTime = DateTime.Now,
                IsFinished = false,
                CategoryId = CategoryId
            };

            _unitOfWork.Issues.Create(issue);
            _unitOfWork.Save();



            var maxId = _unitOfWork.Issues.GetAll().Max(n => n.Id);
            var newId = 1;
            if (_unitOfWork.Issues.GetAll().Any(n => n.Id == maxId && n.CategoryId == CategoryId))
            {
                newId = _unitOfWork.Issues.GetAll().First(n => n.Id == maxId && n.CategoryId == CategoryId).Id;
            }
            return Json(new { Id = newId});

        }
        public JsonResult AddNewUIssue(int id, string text)
        {
                var uissue = new UnderIssue()
                {
                    Text = text,
                    IsFinished = false,
                    IssueId = id
                };

                _unitOfWork.UnderIssues.Create(uissue);

                return Json(_unitOfWork.UnderIssues.GetAll().First(n => n.IssueId == id && n.Id == _unitOfWork.UnderIssues.GetAll().Max(k => k.Id)));

        }

        public JsonResult GetUnderIssues(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var uissues = _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id).Select(n => new { Text = n.Text, Id = n.Id, IsFinished = n.IsFinished }).ToList();
                return Json(uissues, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SetDoneIssue(int id)
        {
            var issue = _unitOfWork.Issues.Get(id);
            issue.IsFinished = true;

            foreach (var item in _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id))
            {
                item.IsFinished = true;
            }

            _unitOfWork.Save();

            return Json("ok");

        }
        public JsonResult SetDoneUIssue(int id)
        {
                var issue = _unitOfWork.UnderIssues.Get(id);
                issue.IsFinished = true;
                _unitOfWork.UnderIssues.Update(issue);
                _unitOfWork.Save();

                return Json("ok");

        }
        public JsonResult UnfinishIssue(int id)
        {
                var issue = _unitOfWork.Issues.Get(id);
                issue.IsFinished = false;

                foreach (var item in _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id))
                {
                    item.IsFinished = false;
                }

                _unitOfWork.Save();
                return Json("ok");

        }
        public JsonResult UnfinishUIssue(int id)
        {
            var issue = _unitOfWork.UnderIssues.Get(id);
            issue.IsFinished = false;
                
            _unitOfWork.Save();

            return Json("ok");

        }
        public JsonResult RemoveCompletedIssues(string ids)
        {
            if (ids.ElementAt(ids.Length - 1) == ',')
                ids = ids.Remove(ids.Length - 1);

            var finishedIssues = ids.Split(',').Select(Int32.Parse).ToList();
            foreach (var id in finishedIssues)
            {
                _unitOfWork.Issues.Delete(id);
            }

            _unitOfWork.Save();
            return Json("ok");
        }
        public JsonResult RemoveCompletedUIssues(string ids)
        {
            if (ids.ElementAt(ids.Length - 1) == ',')
                ids = ids.Remove(ids.Length - 1);

            var finishedUIssues = ids.Split(',').Select(Int32.Parse).ToList();
            foreach (var id in finishedUIssues)
            {
                _unitOfWork.UnderIssues.Delete(id);
            }

            _unitOfWork.Save();
            return Json("ok");

        }
        public JsonResult AddCategory(string CategoryName)
        {
            _unitOfWork.Categories.Create(new Category(){Name = CategoryName});
            _unitOfWork.Save();

            var maxid = _unitOfWork.Categories.GetAll().Max(n => n.Id);
            return Json(new {Id = maxid});

        }
        public JsonResult RemoveCategory(int id)
        {
            var issues = _unitOfWork.Issues.GetAll().Where(n => n.CategoryId == id).ToList();
            foreach (var issue in issues)
            {
                _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == issue.Id).ForEach(n=>_unitOfWork.UnderIssues.Delete(n.Id));
            }
            issues.ForEach(n => _unitOfWork.Issues.Delete(n.Id));

            _unitOfWork.Categories.Delete(id);
            _unitOfWork.Save();

            return Json("ok");

        }

    }
}
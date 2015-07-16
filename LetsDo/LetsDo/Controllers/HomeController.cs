using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models;

namespace ASP.NET_MVC5_Bootstrap3_3_1_LESS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index(int? categoryId = null)
        {
            using (var db = new ApplicationDbContext())
            {
                var issues = (categoryId.HasValue) ?                     
                    db.Issues.Where(n => n.CategoryId == categoryId).OrderByDescending(n => n.CreatedTime).ToList() :
                    db.Issues.OrderByDescending(n => n.CreatedTime).ToList();

                List<UnderIssue> uIssuesList = new List<UnderIssue>();
                if (issues.Count > 0)
                {
                    var id = issues.ElementAt(0).Id;
                    uIssuesList = db.UnderIssues.Where(n => n.IssueId == id).ToList();
                }
                return View(new IndexViewModel() { Issues = issues, UnderIssues = uIssuesList,
                    CategoriesList = db.Categories.OrderByDescending(n => n.Id).ToList(),
                    ActiveCategoryId = categoryId
                });
            }
        }

        public async Task<JsonResult> AddNewIssue(string Text, int? CategoryId)
        {
            using (var db = new ApplicationDbContext())
            {
                var issue = new Issue()
                {
                    Text = Text,
                    CreatedTime = DateTime.Now,
                    IsFinished = false,
                    CategoryId = CategoryId
                };

                try
                {
                    db.Issues.Add(issue);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                var maxId = db.Issues.Max(n => n.Id);
                var newId = 1;
                if (db.Issues.Any(n => n.Id == maxId && n.CategoryId == CategoryId))
                {
                    newId = db.Issues.First(n => n.Id == maxId && n.CategoryId == CategoryId).Id;
                }
                return Json(new { Id = newId});
            }

        }
        public async Task<JsonResult> AddNewUIssue(int id, string text)
        {
            using (var db = new ApplicationDbContext())
            {
                var uissue = new UnderIssue()
                {
                    Text = text,
                    IsFinished = false,
                    IssueId = id
                };

                try
                {
                    db.UnderIssues.Add(uissue);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json(db.UnderIssues.First(n => n.IssueId == id && n.Id == db.UnderIssues.Max(k=>k.Id)));
            }

        }

        public JsonResult GetUnderIssues(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var uissues = db.UnderIssues.Where(n => n.IssueId == id).Select(n => new { Text = n.Text, Id = n.Id, IsFinished = n.IsFinished }).ToList();
                return Json(uissues, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> SetDoneIssue(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var issue = db.Issues.First(n => n.Id == id);
                issue.IsFinished = true;

                foreach (var item in db.UnderIssues.Where(n => n.IssueId == id))
                {
                    item.IsFinished = true;
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> SetDoneUIssue(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var issue = db.UnderIssues.First(n => n.Id == id);
                issue.IsFinished = true;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> UnfinishIssue(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var issue = db.Issues.First(n => n.Id == id);
                issue.IsFinished = false;

                foreach (var item in db.UnderIssues.Where(n => n.IssueId == id))
                {
                    item.IsFinished = false;
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> UnfinishUIssue(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var issue = db.UnderIssues.First(n => n.Id == id);
                issue.IsFinished = false;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> RemoveCompletedIssues(string ids)
        {
            using (var db = new ApplicationDbContext())
            {
                if (ids.ElementAt(ids.Length - 1) == ',')
                    ids = ids.Remove(ids.Length - 1);

                var finishedIssues = ids.Split(',').Select(Int32.Parse).ToList();
                foreach (var id in finishedIssues)
                {
                    db.Issues.Remove(db.Issues.First(n => n.Id == id));
                }


                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> RemoveCompletedUIssues(string ids)
        {
            using (var db = new ApplicationDbContext())
            {
                if (ids.ElementAt(ids.Length - 1) == ',')
                    ids = ids.Remove(ids.Length - 1);

                var finishedUIssues = ids.Split(',').Select(Int32.Parse).ToList();
                foreach (var id in finishedUIssues)
                {
                    db.UnderIssues.Remove(db.UnderIssues.First(n => n.Id == id));
                }


                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }
        public async Task<JsonResult> AddCategory(string CategoryName)
        {
            using (var db = new ApplicationDbContext())
            {
                var category = new Category()
                {
                    Name = CategoryName
                };

                try
                {
                    db.Categories.Add(category);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                var maxid = db.Categories.Max(n => n.Id);
                return Json(new{Id = maxid});
            }

        }
        public async Task<JsonResult> RemoveCategory(int id)
        {
            using (var db = new ApplicationDbContext())
            {

                try
                {

                    var issues = db.Issues.Where(n => n.CategoryId == id).ToList();
                    foreach (var item in issues)
                    {
                        db.UnderIssues.RemoveRange(db.UnderIssues.Where(n => n.IssueId == item.Id));
                    }
                    db.Issues.RemoveRange(issues);
                    db.Categories.Remove(db.Categories.First(n => n.Id == id));
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //return Json(ex.Message);
                    throw;
                }
                return Json("ok");
            }

        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LetsDo.DataAccess;
using LetsDo.DataAccess.Entities;
using LetsDo.DTO.ViewModels;
using WebGrease.Css.Extensions;

namespace LetsDo.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet]
        public ActionResult Index(int? categoryId = null)
        {
            var issues = (categoryId.HasValue) ?                     
                _unitOfWork.Issues.GetAll().Where(n => n.CategoryId == categoryId).OrderByDescending(n => n.CreatedTime).ToList() :
                _unitOfWork.Issues.GetAll().OrderByDescending(n => n.CreatedTime).ToList();

            IList<UnderIssue> uIssuesList = new List<UnderIssue>();
            if (issues.Count > 0)
            {
                var id = issues.OrderBy(n=>n.IsFinished).First().Id;
                uIssuesList = _unitOfWork.UnderIssues.GetAll().Where(n => n.IssueId == id).ToList();
            }


            IList<Category> categoriesList = _unitOfWork.Categories.GetAll().OrderByDescending(n => n.Id).ToList();
            //categoriesList.Remove(categoriesList.First(n=>n.Id == 1));

            return View(new IndexViewModel()
            {
                Issues = issues,
                UnderIssues = uIssuesList,
                CategoriesList = categoriesList,
                ActiveCategoryId = categoryId
            });
        }


        

    }
}
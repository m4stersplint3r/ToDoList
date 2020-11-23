using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoListDBContext _db;

        public ToDoController(ToDoListDBContext db)
        {
            _db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult ListTasks(string sortOrder, string searchString)
        {
            string currentUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // filtered list by task items that are owned by logged in user
            var toDoItems = _db.ToDoItem.ToList().Where(x => x.OwnerId == currentUser);

            // sets the values to the first option if null, otherwise sets to desc when clicking the title link to sort
            ViewData["DueDateSortParm"] = sortOrder == "duedate" ? "duedate_desc" : "duedate";
            ViewData["CompleteSortParm"] = sortOrder == "Complete" ? "complete_desc" : "Complete";
            ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "description_desc" : "Description";
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                toDoItems = toDoItems.Where(s => s.Description.ToLower().Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "Title":
                    toDoItems = toDoItems.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    toDoItems = toDoItems.OrderByDescending(s => s.Title);
                    break;
                case "Description":
                    toDoItems = toDoItems.OrderBy(s => s.Description);
                    break;
                case "description_desc":
                    toDoItems = toDoItems.OrderByDescending(s => s.Description);
                    break;
                case "duedate":
                    toDoItems = toDoItems.OrderBy(s => s.DueDate);
                    break;
                case "duedate_desc":
                    toDoItems = toDoItems.OrderByDescending(s => s.DueDate);
                    break;
                case "Complete":
                    toDoItems = toDoItems.OrderBy(s => s.Complete);
                    break;
                case "complete_desc":
                    toDoItems = toDoItems.OrderByDescending(s => s.Complete);
                    break;
                default:
                    toDoItems = toDoItems.OrderByDescending(s => s.Id);
                    break;
            }
            return View(toDoItems);
        }
        [HttpPost]
        public IActionResult ListTasks(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            return RedirectToAction("ListTasks");
        }


        public IActionResult Create()
        {
            ViewBag.CurrentUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ToDoItem td)
        {
            if (ModelState.IsValid)
            {
                _db.ToDoItem.Add(td);
                _db.SaveChanges();
            }
            return RedirectToAction("ListTasks");
        }
        public IActionResult Delete(int Id)
        {
            ToDoItem td = _db.ToDoItem.Find(Id);
            return View(td);
        }

        [HttpPost]
        public IActionResult Delete(ToDoItem td)
        {
            _db.ToDoItem.Remove(td);
            _db.SaveChanges();
            return RedirectToAction("ListTasks");
        }

        public IActionResult Edit(int Id)
        {
            ToDoItem td = _db.ToDoItem.Find(Id);
            return View(td);
        }

        [HttpPost]
        public IActionResult Edit(ToDoItem td)
        {
            _db.ToDoItem.Update(td);
            _db.SaveChanges();

            return RedirectToAction("ListTasks");
        }

        public IActionResult MarkComplete(int Id)
        {
            ToDoItem td = _db.ToDoItem.Find(Id);
            td.Complete = true;
            _db.ToDoItem.Update(td);
            _db.SaveChanges();

            return RedirectToAction("ListTasks");
        }
    }
}

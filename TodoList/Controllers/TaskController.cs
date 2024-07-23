using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using TodoList.Models;
using TodoList.Repositories;

namespace TodoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskRepository _taskRepository = new TaskRepository();


        // Get All Task
        public ActionResult Index()
        {
            var tasks = _taskRepository.GetAll();
            return View(tasks);
        }


        // Get Task Details
        public ActionResult Details(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        // Create Task View
        public ActionResult Create()
        {
            return View();
        }


        // Create Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TodoList.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedAt = DateTime.Now;
                _taskRepository.Add(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }


        // Edit Task
        public ActionResult Edit(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        // Edit Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TodoList.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _taskRepository.Update(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }


        // Delete Task By Id
        public ActionResult Delete(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        // Delete All Task
        public ActionResult DeleteAll()
        {
            _taskRepository.DeleteAll();
            return RedirectToAction("Index");
        }


        // Delete Confirm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _taskRepository.Delete(id);
            return RedirectToAction("Index");
        }


        // Export Task
        public ActionResult Export()
        {
            var tasks = _taskRepository.GetAll();
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Id,Title,CreatedAt,DateStarted,DateOfCompletion,Details,AssignedTo,Status");

            foreach (var task in tasks)
            {
                var line = string.Format(
                    "{0},{1},{2},{3},{4},{5},{6},{7}",
                    task.Id,
                    EscapeCsvValue(task.Title),
                    task.CreatedAt.ToString("yyyy-MM-dd HH:mm") ?? "",
                    task.DateStarted.ToString("yyyy-MM-dd") ?? "",
                    task.DateOfCompletion.ToString("yyyy-MM-dd") ?? "",
                    EscapeCsvValue(task.Details),
                    EscapeCsvValue(task.AssignedTo),
                    EscapeCsvValue(task.Status)
                );
                csv.AppendLine(line);
            }

            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "tasks.csv");
        }


        private string EscapeCsvValue(string value)
        {
            if (value.Contains(",") || value.Contains("\""))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }


        // Import Task
        [HttpPost]
        public ActionResult Import(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("Id"))
                        {
                            var values = line.Split(',');

                            var task = new TodoList.Models.Task
                            {
                                Id = int.Parse(values[0]),
                                Title = values[1],
                                CreatedAt = DateTime.Now,
                                DateStarted = DateTime.Parse(values[3]),
                                DateOfCompletion = DateTime.Parse(values[4]),
                                Details = values[5],
                                AssignedTo = values[6],
                                Status = values[7]
                            };

                            _taskRepository.Add(task);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}

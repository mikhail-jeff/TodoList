using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Models;

namespace TodoList.Repositories
{
    public class TaskRepository
    {
        private static List<TodoList.Models.Task> tasks = new List<TodoList.Models.Task>();

        public IEnumerable<TodoList.Models.Task> GetAll() => tasks;

        public TodoList.Models.Task GetById(int id) => tasks.FirstOrDefault(t => t.Id == id);

        public void Add(TodoList.Models.Task task)
        {
            task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
        }

        public void Update(TodoList.Models.Task task)
        {
            var existingTask = GetById(task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.DateStarted = task.DateStarted;
                existingTask.DateOfCompletion = task.DateOfCompletion;
                existingTask.Details = task.Details;
                existingTask.AssignedTo = task.AssignedTo;
                existingTask.Status = task.Status;
            }
        }

        public void Delete(int id)
        {
            var task = GetById(id);
            if (task != null)
            {
                tasks.Remove(task);
            }
        }


    }
}

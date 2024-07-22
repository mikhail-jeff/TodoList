using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfCompletion { get; set; }

        [StringLength(500, ErrorMessage = "Details cannot be longer than 500 characters.")]
        public string Details { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Assigned to cannot be longer than 50 characters.")]
        public string AssignedTo { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}

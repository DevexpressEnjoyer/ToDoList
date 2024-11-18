using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }    
        public string? Description { get; set; } 
        public string? Priority {  get; set; }
        public required string Date { get; set; }
        public bool IsCompleted {  get; set; }
    }
}

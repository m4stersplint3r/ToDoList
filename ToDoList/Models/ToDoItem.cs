using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public partial class ToDoItem
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Complete { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}

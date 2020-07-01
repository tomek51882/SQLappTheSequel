using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SQLappTheSequel.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
        public Subcategory Subcategory { get; set; }
        public AnswerModelName AnswerModelName { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime ModifiedAt { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
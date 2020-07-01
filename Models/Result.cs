using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SQLappTheSequel.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Exercise ExerciseId { get; set; }
        public DateTime CompletionDate { get; set; }
        public int Score { get; set; }
        public string UserAnswerSql { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required] //if leave an empty, will show message
        [MaxLength(50, ErrorMessage = "Title can not longger than 50 letters")]
        [MinLength(3, ErrorMessage = "Title can not less than 3 letters")]
        public string CommentDetails { get; set; }
        public int? AnswerId { get; set; }
        public int? QuestionId { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerDetail { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int UpVote { get; set; } = 0;
        public int DownVote { get; set; } = 0;


        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Answer()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionTitle { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int UpVote { get; set; } = 0;
        public int DownVote { get; set; } = 0;

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Question()
        {
            Comments = new HashSet<Comment>();
            Answers = new HashSet<Answer>();
        }

    }
}
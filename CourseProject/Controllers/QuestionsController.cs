using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace CourseProject.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*
        *  show each question page with answer
        */
        public ActionResult QuestionPage(string title)
        {
            var question = db.Questions
                .FirstOrDefault(qu => qu.QuestionTitle == title);
            return View(question);
        }

        /*
        *  show tag page with tag's name
        */
        public ActionResult Tagged(string tag, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var preQuestions = db.Questions;
            var questions = preQuestions.Where(q => q.Tag == tag);
            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(s => s.QuestionTitle.Contains(searchString)
                                       || s.Tag.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "date_desc":
                    questions = questions.OrderByDescending(s => s.DateTime);
                    break;
                default: 
                    questions = questions.OrderBy(s => s.DateTime);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }

        /*
         *  pagination index
         */
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.AnswerSortParm = String.IsNullOrEmpty(sortOrder) ? "answer_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var questions = from s in db.Questions
            //select s;
            IQueryable<Question> questions = db.Questions;
            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(s => s.QuestionTitle.Contains(searchString)
                                       || s.Tag.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "answer_desc":
                    questions = questions.OrderByDescending(s => s.Answers.Count());
                    break;
                case "Date":
                    questions = questions.OrderBy(s => s.DateTime);
                    break;
                case "date_desc":
                    questions = questions.OrderByDescending(s => s.DateTime);
                    break;
                default:
                    questions = questions.OrderByDescending(s => s.DateTime);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }


        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        [Authorize (Roles ="User, Admin")]
        public ActionResult Create()
        {
            //var user = db.Users.Where(u => u.UserName == User.Identity.Name);
            //ViewBag.UserId = new SelectList(user.ToList(), "Id", "Email");
            //ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");
            return View();
            
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,QuestionTitle,Description,Tag,DateTime")] Question question)
        {
            if (ModelState.IsValid)
            {
                //*add this to get log in user
                question.UserId = User.Identity.GetUserId();
                db.Questions.Add(question);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email", question.UserId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,QuestionTitle,Description,Tag,UserId,DateTime")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
         *  page to create a answer
         */
        [HttpPost]
        public ActionResult PostAnswer(int id, string content, Answer answer)
        {
            Question question = db.Questions.Find(id);

            answer.UserId = User.Identity.GetUserId();

            answer.QuestionId = id;
            answer.AnswerDetail = content;
            db.Answers.Add(answer);
            db.SaveChanges();

            //var routeValues = new RouteValueDictionary {
            //  { "title", answer.Question.QuestionTitle }
            //};

            //return RedirectToAction("QuestionPage", "Questions", routeValues);
            //return RedirectToAction("Index");

            return RedirectToAction("QuestionPage", "Questions", new { title = question.QuestionTitle });
        }


        public ActionResult VoteValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            if(question.UserId != User.Identity.GetUserId())
            {
                question.User.Reputation += 5;
                question.UpVote++;
                db.SaveChanges();
            }

            return RedirectToAction("QuestionPage", "Questions", new {title=question.QuestionTitle });
        }

        public ActionResult DownVoteValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            if (question.UserId != User.Identity.GetUserId())
            {
                question.User.Reputation -= 5;
                question.DownVote++;
                db.SaveChanges();
            }

            return RedirectToAction("QuestionPage", "Questions", new { title = question.QuestionTitle });
        }


        /*
         *  comment for a question
         */
        public ActionResult QuestionComment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionComment(int id, string commentDetails, [Bind(Include = "Id,CommentDetails")] Comment comment)
        {                                                   
            if (ModelState.IsValid)
            {
                Question question = db.Questions.Find(id);
                comment.QuestionId = id;
                comment.CommentDetails = commentDetails;

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("QuestionPage", "Questions", new { title = question.QuestionTitle });
            }

            return RedirectToAction("Index");
        }


        /*
         *  comment for a answer
         */
        public ActionResult AnswerComment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerComment(int id, string commentDetails, [Bind(Include = "Id,CommentDetails")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                Answer answer = db.Answers.Find(id);
                comment.AnswerId = id;
                comment.CommentDetails = commentDetails;

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("QuestionPage", "Questions", new { title = answer.Question.QuestionTitle });
            }

            return RedirectToAction("Index");
        }

    }
}

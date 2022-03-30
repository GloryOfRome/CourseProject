using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject.Models;
using Microsoft.AspNet.Identity;

namespace CourseProject.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Answer).Include(c => c.Question);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "AnswerDetail");
            //ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionTitle");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CommentDetails")] Comment comment)
        {                                                        //,AnswerId,QuestionId
            if (ModelState.IsValid)
            {
                //*add this to get QuestionId, answer Id
                //comment.AnswerId = Comment.Identity.GetUserId();

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "AnswerDetail", comment.AnswerId);
            //ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionTitle", comment.QuestionId);
            return View(comment);
        }

        [HttpPost]
        public ActionResult CommentAnswer(int? id, string content, Comment comment)
        {   //*add this to get log in user
            //question.UserId = User.Identity.GetUserId();

            comment.AnswerId = id;
            comment.CommentDetails = content;
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Index", "Questions");

        }


        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "AnswerDetail", comment.AnswerId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionTitle", comment.QuestionId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CommentDetails,AnswerId,QuestionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "AnswerDetail", comment.AnswerId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionTitle", comment.QuestionId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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


    }
}

using AutoMapper;
using CSharpCollective.Services.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Services;
using Services.Interfaces;

namespace CSharpCollective.Controllers
{
    public class CommentController : Controller
    {

        private ICommentService commentService;



        public CommentController(ICommentService commentService)
        {


            this.commentService = commentService;


        }


        [HttpGet]
        public IActionResult Comment()
        {
            var comments = commentService.GetAll();

            if (comments.Count().Equals(0))
            {return RedirectToAction("Create"); }
                

            return View(comments);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateComment");
        }
        
        [HttpPost]
        public IActionResult Create(CommentDto comment)
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            ;
            comment.AuthorId = Guid.Parse(userIdString);

            var commentCheck = commentService.CommentCheck(comment);
            if (commentCheck == null)
            {
                TempData["ErrorMessage"] = "Content exceeds maximum length of 2000 or is empty. Please try again.";
                return RedirectToAction("Create");
            }
            commentService.Create(comment);

            return RedirectToAction("Comment");
        }
        //Ninject ,Casstle Windsor

        public IActionResult Delete(Guid id)
        {
           commentService.Delete(id);


            return RedirectToAction("Comment"); // Back to list
        }
    }
}

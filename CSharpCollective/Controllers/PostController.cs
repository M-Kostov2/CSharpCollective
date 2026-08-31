using AutoMapper;
using CSharpCollective.Services;
using CSharpCollective.Services.DtoModels;
using DataBase.DataContext;
using DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Interfaces;
using System;

namespace CSharpCollective.Controllers
{
    public class PostController : Controller
    {
        private IPostService postService;



        public PostController(IPostService postservice)
        {
            this.postService = postservice;
        }


        [HttpGet]
        public IActionResult Post()
        {
            var posts = postService.GetAll();

            if (posts.Count().Equals(0))
                return RedirectToAction("Create");

            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CreatePost");
        }

        [HttpPost]
        public IActionResult Create(PostDto post)
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            post.AuthorId = Guid.Parse(userIdString);

            var postCheck = postService.PostCheck(post);
            if (postCheck == null)
            {
                TempData["ErrorMessage"] = "Title or content exceeds maximum length of 100 and 2000 or one of them is empty. Please try again.";
                return RedirectToAction("Create");
            }
            postService.Create(post);

            return RedirectToAction("Post");
        }
        //Ninject ,Casstle Windsor


        [HttpGet]
        public IActionResult Edit(Guid id)
        {

            PostDto post = postService.GetById(id);
            if (post == null)
            {
                return NotFound();

            }
            return View("Edit", post);
        }

        public IActionResult Edit(PostDto post)
        {



            var postCheck = postService.PostCheck(post);
            if (postCheck == null)
            {
                TempData["EditError"] = "Title or content exceeds maximum length of 100 and 2000 or one of them is empty. Please try again.";
                return RedirectToAction("Edit");
            }
            postService.Edit(post);
            return RedirectToAction("Post");
        }



        [HttpGet]
        public IActionResult AddCategory(Guid id)
        {
            var post = postService.GetById(id); 
            if (post == null) return NotFound();

            return View(post);
        }

 
        [HttpPost]
        public IActionResult AddCategory(PostDto post, string Category)
        {
            var postCheck = postService.PostCheck(post);
            var categoryCheck = postService.PostCategoryCheck(post);

            if (postCheck == null)
            {
                TempData["EditError"] = "Validation failed. Please try again.";
                return View(post); 
            }

            if (categoryCheck == null)
            {
                TempData["EditError"] = "Post Already Has A Category";
                return View(post);
            }

            postService.AddCategoryToPost(post.Id, Category);

            return RedirectToAction("Post");
        }


        [HttpGet]
        public IActionResult AddTags(Guid id)
        {
            var post = postService.GetById(id); 
            if (post == null) return NotFound();

            return View(post);
        }

 
        [HttpPost]
        public IActionResult AddTags(PostDto post, string Tags)
        {
            var postCheck = postService.PostCheck(post);
            if (postCheck == null)
            {
                TempData["EditError"] = "Validation failed. Please try again.";
                return View(post); 
            }

  
            postService.AddTagsToPost(post.Id, Tags);

            return RedirectToAction("Post");
        }



        public IActionResult Delete(Guid id)
        {
            postService.Delete(id);


            return RedirectToAction("Post"); // Back to list
        }



    }
}

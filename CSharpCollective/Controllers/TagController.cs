using CSharpCollective.Services.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CSharpCollective.Controllers
{
    public class TagController : Controller
    {
        private IPostService postService;

        public TagController(IPostService postService)
        {
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult Tag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return View(new List<PostDto>());
            }

            var posts = postService.GetAllByTag(tag);

            return View(posts);
        }
    }
}

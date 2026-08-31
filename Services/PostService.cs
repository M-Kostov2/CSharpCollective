using AutoMapper;
using CSharpCollective.Services.DtoModels;
using DataBase.DataContext;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Linq;


namespace Services
{
    public class PostService : IPostService
    {
        private CollectiveContext context;
        private readonly IMapper mapper;


        public PostService(IMapper mapper, CollectiveContext context)
        {
            this.context = context;
            this.mapper = mapper;

        }



        public PostDto Create(PostDto Datarecieved)
        {


            Post postInfo = new Post(Datarecieved.Title, Datarecieved.Content);


            mapper.Map(Datarecieved, postInfo);

            var postExists = context.Posts.Any(p => p.Id == postInfo.Id);
            if (postExists != null)
            {
                string authorName = context.Users.Where(u => u.Id == postInfo.AuthorId).Select(u => u.UserName).FirstOrDefault();
                context.Posts.AddAsync(postInfo);
                context.SaveChanges();
            }
            PostDto postDtoInfo = new PostDto(postInfo.Title, postInfo.Content, postInfo.Id);




            return postDtoInfo;

        }
        public void Edit(PostDto Datarecieved)
        {

            string title = Datarecieved.Title;
            string content = Datarecieved.Content;
            if (title == null & content == null || title.Length > 100 & content.Length > 2000 & title.Length <= 0)
            {
                Datarecieved = null;
            }


            Post postInfo = new Post();
            mapper.Map(Datarecieved, postInfo);
            postInfo = context.Posts.Where(p => p.Id == Datarecieved.Id).Single();
            Datarecieved.AuthorId = postInfo.AuthorId;
            Datarecieved.UpdatedAt = DateTime.Now;
            mapper.Map(Datarecieved, postInfo);
            context.SaveChangesAsync();

        }
        public void Delete(Guid Id)
        {
            Post postInfo = new Post();
            postInfo = context.Posts.SingleOrDefault(p => p.Id == Id);

            context.Posts.Remove(postInfo);
            context.SaveChanges();
        }

        public IEnumerable<PostDto> GetAll()
        {
            var posts = context.Posts.Select(n => new Post
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                AuthorId = n.AuthorId
            }
                ).ToList();
            var postDtos = mapper.Map<List<Post>, List<PostDto>>(posts);
            return postDtos;
        }

        public IEnumerable<PostDto> GetAllByCategory(string tag)
        {
            var posts = context.Posts
            .Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Name.ToLower() == tag.ToLower())).
            Select(n => new Post
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                AuthorId = n.AuthorId
            }
                )
            .ToList();
            var postDtos = mapper.Map<List<Post>, List<PostDto>>(posts);
            return postDtos;
        }


        public IEnumerable<PostDto> GetAllByTag(string tag)
        {
            var posts = context.Posts
            .Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Name.ToLower() == tag.ToLower())).
            Select(n => new Post
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                AuthorId = n.AuthorId
            }
                )
            .ToList();
            var postDtos = mapper.Map<List<Post>, List<PostDto>>(posts);
            return postDtos;
        }


        public PostDto GetById(Guid id)
        {

            Post post = context.Posts.SingleOrDefault(p => p.Id == id);
            PostDto postDto = mapper.Map<Post, PostDto>(post);

            return postDto;


        }

      
        public void AddCategoryToPost(Guid Id, string Category)
        {

            var name = Category.Trim();

            var category = context.Categories
                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (category == null)
            {
                category = new Category { Name = name };
                context.Categories.Add(category);
            }

            var post = context.Posts
                .Include(p => p.Category)
                .SingleOrDefault(p => p.Id == Id);

            if (post != null && post.Category == null)
            {
                post.Category = category;
                post.CategoryId = category.Id;
            }

            context.SaveChangesAsync();
        }

        public void AddTagsToPost(Guid Id, string Tag)
        {

            var tag = context.Tags
           .FirstOrDefault(c => c.Name.ToLower() == Tag.ToLower().Trim());

            if (tag == null)
            {
                tag = new Tag { Name = Tag };
            }

            var post = context.Posts
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == Id);

            if (post != null)
            {

                if (!post.Tags.Any(t => t.Name.ToLower() == Tag.ToLower().Trim()))
                {
                    post.Tags.Add(tag);
                }

                context.SaveChanges();



            }
        }

        public PostDto PostCheck(PostDto Datarecieved)
        {
            PostDto postDto = new PostDto();
            postDto = Datarecieved;
            string title = Datarecieved.Title;
            string content = Datarecieved.Content;
            

            if (title.IsNullOrEmpty() || content.IsNullOrEmpty()|| title.Length > 100 & content.Length > 2000 & title.Length <= 0)
            {
                return null;
            }

            return postDto;

        }


        public PostDto PostCategoryCheck(PostDto Datarecieved)
        {
            var category = context.Posts.Where(p => p.Id == Datarecieved.Id).Select(p => new { p.Category.Name }).ToString();

        

            if (!category.IsNullOrEmpty())
            {
                return null;
            }

            return Datarecieved;

        }
    }
}

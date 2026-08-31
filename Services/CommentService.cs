using AutoMapper;
using CSharpCollective.Services.DtoModels;
using DataBase.DataContext;
using DataBase.Models;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CommentService : ICommentService
    {


        private CollectiveContext context;
        private readonly IMapper mapper;


        public CommentService(IMapper mapper, CollectiveContext context)
        {
            this.context = context;
            this.mapper = mapper;

        }



        public CommentDto Create(CommentDto Datarecieved)
        {


            Comment commentInfo = new Comment(Datarecieved.Content);


            mapper.Map(Datarecieved, commentInfo);


            string authorName = context.Users.Where(u => u.Id == commentInfo.AuthorId).Select(u => u.UserName).FirstOrDefault();
            context.Comments.AddAsync(commentInfo);
            context.SaveChanges();

            CommentDto commentDtoInfo = new CommentDto(commentInfo.Content);
            mapper.Map(commentInfo, commentDtoInfo);



            return commentDtoInfo;

        }

        public void Delete(Guid Id)
        {
            Comment commentInfo = new Comment();
            commentInfo = context.Comments.SingleOrDefault(p => p.Id == Id);

            context.Comments.Remove(commentInfo);
            context.SaveChanges();
        }

        public IEnumerable<CommentDto> GetAll()
        {
            var comments = context.Comments.Select(n => new Comment
            {
                Id = n.Id,
                Content = n.Content,
                AuthorId = n.AuthorId
            }
                ).ToList();
            var commentDtos = mapper.Map<List<Comment>, List<CommentDto>>(comments);
            return commentDtos;
        }

        public CommentDto CommentCheck(CommentDto Datarecieved)
        {
            CommentDto commentDto = new CommentDto();
            commentDto = Datarecieved;
            string content = Datarecieved.Content;

            if (content.IsNullOrEmpty() || content.Length > 2000)
            {
                return null;
            }

            return commentDto;

        }



    }
}

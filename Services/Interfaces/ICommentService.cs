using CSharpCollective.Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        public CommentDto Create(CommentDto Datarecieved);
        public void Delete(Guid Id);
        public IEnumerable<CommentDto> GetAll();
        public CommentDto CommentCheck(CommentDto Datarecieved);
    }
}

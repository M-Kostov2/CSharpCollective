using CSharpCollective.Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPostService
    {
        public PostDto Create(PostDto Datarecieved);
        public void Edit(PostDto Datarecieved);
        public void Delete(Guid Id);
        public IEnumerable<PostDto> GetAll();
        public IEnumerable<PostDto> GetAllByCategory(string category);
        public IEnumerable<PostDto> GetAllByTag(string tag);
        public PostDto GetById(Guid id);
        public void AddCategoryToPost(Guid Id, string Category);
        public void AddTagsToPost(Guid Id, string Tag);
        public PostDto PostCheck(PostDto Datarecieved);

        public PostDto PostCategoryCheck(PostDto Datarecieved);

    }
}

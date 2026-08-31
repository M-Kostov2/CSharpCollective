using DataBase.ModelConstrains;
using DataBase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpCollective.Services.DtoModels;
public class PostDto
{

    public PostDto()
    {

    }

    public PostDto(string title, string content,Guid Id)
    {
        this.Id = Id;
        this.Title = title;
        this.Content = content;
        this.Tags = new HashSet<Tag>();
       ;
    }
    public Guid Id { get; set; }

    [ForeignKey("AuthorDto")]
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; }
    public string AuthorName { get; set; }

    [MinLength(5), MaxLength(Constrains.MaxPostTitleLength)]
    public string Title { get;  set; }
    [MaxLength(Constrains.MaxPostContentLength)]
    public string Content { get; set; }

    public virtual Category? Category { get; set; }
    public virtual ICollection<Tag>? Tags { get; set; }
    public DateTime UpdatedAt { get; set; }

}

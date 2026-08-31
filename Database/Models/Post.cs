
using DataBase.ModelConstrains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Post
    {
        public Post()
        {
            
        }
        public Post(string title,string content)
        {
            this.Id = Guid.NewGuid();       
            this.Title = title;
            this.Content = content;
            this.CreatedAt = DateTime.UtcNow;
            this.Tags = new HashSet<Tag>();
            
        }
        [Key]
        public Guid Id { get;  set; }

        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;
        [MinLength(5), MaxLength(Constrains.MaxPostTitleLength)]
        public string Title { get;  set; }
        [MaxLength(Constrains.MaxPostContentLength)]
        public string Content { get;  set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }

    }
}

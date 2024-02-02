using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Model;

[Table("Article")]
public class Article
{
    [Key]
    public int Id { set; get; }
    
    [Required]
    [DataType(DataType.Text)]
    public string Title { set; get; }
    
    [Required]
    [DataType(DataType.Text)]
    public string Content { set; get; }
    
    [Required]
    [DataType(DataType.Text)]
    public string Author { set; get; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { set; get; }
    public DateTime UpdatedAt { set; get; }
}
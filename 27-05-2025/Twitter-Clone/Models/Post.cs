using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get;  set; } = string.Empty;

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public ICollection<Like>? Likes { get; set; }
    public ICollection<PostHashtags>? PostHashtags { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}

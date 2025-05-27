using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("PostId")]
    public Post? Post { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}

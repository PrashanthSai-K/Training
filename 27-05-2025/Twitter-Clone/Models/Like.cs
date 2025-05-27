using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class Like
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    public User? User { get; set; }
    [ForeignKey("PostId")]
    public Post? Post { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class FollowList
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("FollowerId")]
    public User? Follower { get; set; }
    [ForeignKey("FollowingId")]
    public User? Following { get; set; }
}

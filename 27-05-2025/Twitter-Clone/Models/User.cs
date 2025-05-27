using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter_Clone.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DoB { get; set; }
    public string Status { get; set; } = string.Empty;

    public ICollection<Like>? Likes { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<FollowList>? Followers { get; set; }
    public ICollection<FollowList>? Followings { get; set; }
}

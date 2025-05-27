using System;

namespace Twitter_Clone.Models;

public class Hashtag
{
    public int Id { get; set; }
    public string Tag { get; set; } = string.Empty;

    public ICollection<PostHashtags>? PostHashtags { get; set; }
}

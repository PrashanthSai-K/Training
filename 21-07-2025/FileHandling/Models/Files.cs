using System;

namespace FileHandling.Models;

public class UploadedFile
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string BlobUrl { get; set; } = string.Empty;
}

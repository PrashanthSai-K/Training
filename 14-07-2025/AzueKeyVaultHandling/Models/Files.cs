using System;

namespace FileHandling.Models;

public class UploadedFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = new byte[0];
    
}

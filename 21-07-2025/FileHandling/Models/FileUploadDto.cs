namespace FileHandling.Models;

public class FileUploadDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile file { get; set; }
}
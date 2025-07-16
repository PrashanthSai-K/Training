namespace FileHandling.Models;

public class SasResponse
{
    public string sasUrl { get; set; }
    public DateTimeOffset expiresOn { get; set; }
}

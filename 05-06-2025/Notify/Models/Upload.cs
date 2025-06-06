using System;

namespace Notify.Models;

public class Upload
{
    public int Id { get; set; }
    public string Filename { get; set; } = string.Empty;
    public byte[] Content { get; set; } = new byte[0];
}

using System.Text.Json.Serialization;

namespace ChienVHShopOnline.Models.Dto;

public class CaptchaResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; }
}

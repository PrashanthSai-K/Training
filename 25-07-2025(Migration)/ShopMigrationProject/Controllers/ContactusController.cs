using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
// using ChienVHShopOnline.Data; // Replace with your actual namespace
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
// using ChienVHShopOnline.Models.Dto; // for ContactUsDto

[Route("api/[controller]")]
[ApiController]
public class ContactUsController : ControllerBase
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string SecretKey = "6LdD75IrAAAAAP3f7wEswmmjn4wMDXipT82zz-BL";

    public ContactUsController(ChienVHShopDBEntities context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContactForm([FromBody] ContactUsDto dto)
    {
        if (!await ValidateCaptchaAsync(dto.RecaptchaToken))
        {
            return BadRequest(new { message = "Captcha validation failed" });
        }

        var contact = new ContactUs
        {
            name = dto.Name,
            email = dto.Email,
            phone = dto.Phone,
            content = dto.Content
        };

        _context.ContactUs.Add(contact);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Your query has been submitted successfully." });
    }

    private async Task<bool> ValidateCaptchaAsync(string token)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={SecretKey}&response={token}"
        );

        var captchaResult = JsonConvert.DeserializeObject<ChienVHShopOnline.Models.Dto.CaptchaResponse>(response);
        return captchaResult != null && captchaResult.Success;
    }
}

using CustomerSupport.Models;

namespace CustomerSupport.Models.Dto;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public User? User { get; set; }

}
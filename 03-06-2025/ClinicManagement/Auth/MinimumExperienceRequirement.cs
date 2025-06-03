using System;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Auth;

public class MinimumExperienceRequirement : IAuthorizationRequirement
{
    public int MinimumYears { get; set; }

    public MinimumExperienceRequirement(int minimumYears)
    {
        MinimumYears = minimumYears;
    }
}

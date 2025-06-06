using System;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Auth;

public class MinimumExperienceHandler : AuthorizationHandler<MinimumExperienceRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumExperienceRequirement requirement)
    {
        var experienced = context.User.FindFirst("YearsOfExperience");

        if (experienced != null && int.TryParse(experienced.Value, out int YearsOfExperience))
        {
            if (YearsOfExperience >= requirement.MinimumYears)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}

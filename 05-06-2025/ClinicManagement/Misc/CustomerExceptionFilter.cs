using System;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClinicManagement.Misc;

public class CustomerExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new BadRequestObjectResult(new ErrotObjectDto()
        {
            ErrorNumber = 500,
            ErrorMessage = context.Exception.Message
        });
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class BaseController : ControllerBase
{
    protected string Login => HttpContext.User.FindFirstValue(ClaimTypes.Name)!;
}
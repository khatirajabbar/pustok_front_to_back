using Microsoft.AspNetCore.Mvc;

namespace pustok_front_to_back.Controllers;

public class ErrorController : Controller
{
    [Route("error/{code}")]
    public IActionResult HttpStatusCodeHandler(int code)
    {
        return code switch
        {
            404 => View("404"),
            403 => View("403"),
            500 => View("500"),
            _ => View("Error")
        };
    }

    [Route("/error")]
    public IActionResult Error()
    {
        return View("500");
    }

    [HttpGet("test403")]
    public IActionResult Test403()
    {
        return View("403");
    }

    [HttpGet("test500")]
    public IActionResult Test500()
    {
        return View("500");
    }
}
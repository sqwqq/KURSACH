using Microsoft.AspNetCore.Mvc;

namespace Kursach.Controllers;

public class ErrorController : Controller
{
    [Route("/error/{statusCode:int}")]
    public IActionResult StatusCodeError(int statusCode)
    {
        ViewBag.StatusCode = statusCode;
        ViewBag.Message = statusCode switch
        {
            404 => "Страница не найдена",
            403 => "Доступ запрещён",
            500 => "Внутренняя ошибка сервера",
            _ => "Произошла ошибка"
        };
        ViewBag.Icon = statusCode switch
        {
            404 => "bi-emoji-frown",
            403 => "bi-shield-lock",
            500 => "bi-gear",
            _ => "bi-exclamation-triangle"
        };
        return View("Error");
    }

    [Route("/error")]
    public IActionResult GeneralError()
    {
        ViewBag.StatusCode = 500;
        ViewBag.Message = "Произошла непредвиденная ошибка";
        ViewBag.Icon = "bi-gear";
        return View("Error");
    }
}

using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Infrastructure;
using RaceDay.Web.Models;

namespace RaceDay.Web.Controllers;
public class HomeController : Controller
{
    private readonly ApiClient _api;
    public HomeController(ApiClient api) => _api = api;
    public async Task<IActionResult> Index()
    {
        var result = await _api.GetAsync<List<EventVm>>("api/events");
        ViewBag.ApiError = result.Success ? null : result.Error;
        return View(result.Data ?? new List<EventVm>());
    }
    public IActionResult Error() => View();
}

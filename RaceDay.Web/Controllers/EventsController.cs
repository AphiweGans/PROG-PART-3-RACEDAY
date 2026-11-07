using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Infrastructure;
using RaceDay.Web.Models;

namespace RaceDay.Web.Controllers;
public class EventsController : Controller
{
    private readonly ApiClient _api;
    public EventsController(ApiClient api) => _api = api;
    public async Task<IActionResult> Index() { var r = await _api.GetAsync<List<EventVm>>("api/events"); ViewBag.ApiError = r.Success ? null : r.Error; return View(r.Data ?? new()); }
    public async Task<IActionResult> Details(int id)
    {
        var r = await _api.GetAsync<EventVm>($"api/events/{id}");
        if (!r.Success || r.Data == null) { TempData["Error"] = r.Error.Length > 0 ? r.Error : "Event not found."; return RedirectToAction(nameof(Index)); }
        var cats = await _api.GetAsync<List<CategoryVm>>($"api/events/{id}/categories");
        ViewBag.Categories = cats.Data ?? new();
        return View(r.Data);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Enter(int categoryId, int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Account");
        var r = await _api.PostAsync<System.Text.Json.JsonElement>($"api/categories/{categoryId}/enrol");
        TempData[r.Success ? "Success" : "Error"] = r.Success ? "You have entered this event category." : r.Error;
        return RedirectToAction(nameof(Details), new { id = eventId });
    }
}

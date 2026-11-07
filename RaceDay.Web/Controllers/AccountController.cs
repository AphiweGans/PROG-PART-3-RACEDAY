using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Web.Infrastructure;
using RaceDay.Web.Models;

namespace RaceDay.Web.Controllers;
public class AccountController : Controller
{
    private readonly ApiClient _api;
    public AccountController(ApiClient api) => _api = api;
    [HttpGet] public IActionResult Login() => View(new LoginVm());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _api.PostAsync<System.Text.Json.JsonElement>("api/auth/login", new { model.Email, model.Password });
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Error); return View(model); }
        var data = result.Data;
        var role = data.TryGetProperty("role", out var roleValue) ? roleValue.GetString() ?? "" : "";
        var name = data.TryGetProperty("fullName", out var nameValue) ? nameValue.GetString() ?? "RaceDay user" : "RaceDay user";
        var id = data.TryGetProperty("organiserID", out var oid) ? oid.GetInt32() : data.TryGetProperty("participantID", out var pid) ? pid.GetInt32() : 0;
        HttpContext.Session.SetString("UserRole", role);
        HttpContext.Session.SetString("UserName", name);
        HttpContext.Session.SetInt32("UserId", id);
        TempData["Success"] = $"Welcome back, {name}!";
        return RedirectToAction("Index", role == "Organiser" ? "Organiser" : "Participant");
    }
    [HttpGet] public IActionResult Register() => View(new RegisterVm());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid) return View(model);
        var role = model.Role == "Organiser" ? UserRole.Organiser : UserRole.Participant;
        var result = await _api.PostAsync<System.Text.Json.JsonElement>("api/auth/register", new { model.FullName, model.Email, model.Password, role = (int)role, model.Phone, model.DateOfBirth, model.Gender });
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.Error); return View(model); }
        TempData["Success"] = "Account created. Please log in with your new details.";
        return RedirectToAction(nameof(Login));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _api.PostAsync<System.Text.Json.JsonElement>("api/auth/logout", new { });
        HttpContext.Session.Clear();
        TempData["Success"] = "You have been logged out.";
        return RedirectToAction("Index", "Home");
    }
}

using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Infrastructure;
using RaceDay.Web.Models;
namespace RaceDay.Web.Controllers;
public class ParticipantController : Controller
{
    private readonly ApiClient _api;
    public ParticipantController(ApiClient api) => _api = api;
    private bool Allowed() => HttpContext.Session.GetString("UserRole") == "Participant";
    public async Task<IActionResult> Index() { if (!Allowed()) return RedirectToAction("Login","Account"); var r=await _api.GetAsync<List<EventVm>>("api/events"); ViewBag.ApiError=r.Success?null:r.Error; return View(r.Data??new()); }
    public async Task<IActionResult> MyEvents() { if(!Allowed()) return RedirectToAction("Login","Account"); var r=await _api.GetAsync<List<EnrolmentVm>>("api/enrolments/me"); ViewBag.ApiError=r.Success?null:r.Error; return View(r.Data??new()); }
    public async Task<IActionResult> MyResults() { if(!Allowed()) return RedirectToAction("Login","Account"); var r=await _api.GetAsync<List<ResultVm>>("api/participants/me/results"); ViewBag.ApiError=r.Success?null:r.Error; return View(r.Data??new()); }
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Cancel(int id) { if(!Allowed()) return RedirectToAction("Login","Account"); var r=await _api.DeleteAsync<System.Text.Json.JsonElement>($"api/enrolments/{id}"); TempData[r.Success?"Success":"Error"]=r.Success?"Your entry was cancelled.":r.Error; return RedirectToAction(nameof(MyEvents)); }
}

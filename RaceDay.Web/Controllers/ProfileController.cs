using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Infrastructure;
using RaceDay.Web.Models;
namespace RaceDay.Web.Controllers;
public class ProfileController : Controller
{
    private readonly ApiClient _api;
    public ProfileController(ApiClient api) => _api=api;
    [HttpGet] public async Task<IActionResult> Index() { if(string.IsNullOrEmpty(HttpContext.Session.GetString("UserRole"))) return RedirectToAction("Login","Account"); var r=await _api.GetAsync<System.Text.Json.JsonElement>("api/users/me"); if(!r.Success){TempData["Error"]=r.Error;return RedirectToAction("Login","Account");} var d=r.Data; var id=d.TryGetProperty("organiserID",out var oi)?oi.GetInt32():d.TryGetProperty("participantID",out var pi)?pi.GetInt32():0; var m=new ProfileVm {UserId=id,Role=HttpContext.Session.GetString("UserRole")??"",FullName=d.TryGetProperty("fullName",out var n)?n.GetString()??"":"",Email=d.TryGetProperty("email",out var e)?e.GetString()??"":"",Phone=d.TryGetProperty("phone",out var p)&&p.ValueKind!=System.Text.Json.JsonValueKind.Null?p.GetString():null,Gender=d.TryGetProperty("gender",out var g)&&g.ValueKind!=System.Text.Json.JsonValueKind.Null?g.GetString():null,DateOfBirth=d.TryGetProperty("dateOfBirth",out var dob)&&dob.ValueKind==System.Text.Json.JsonValueKind.String?dob.GetDateTime():null}; return View(m); }
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Index(ProfileVm m) { if(!ModelState.IsValid)return View(m); var r=await _api.PutAsync<System.Text.Json.JsonElement>("api/users/me",new{m.FullName,m.Phone,m.Gender}); TempData[r.Success?"Success":"Error"]=r.Success?"Profile updated.":r.Error; return RedirectToAction(nameof(Index)); }
}

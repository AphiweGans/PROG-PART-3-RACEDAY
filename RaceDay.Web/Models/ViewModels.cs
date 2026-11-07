using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RaceDay.Web.Models;

public sealed class EventVm
{
    [JsonPropertyName("eventID")] public int EventId { get; set; }
    [JsonPropertyName("organiserID")] public int OrganiserId { get; set; }
    [JsonPropertyName("eventName")] public string EventName { get; set; } = "";
    [JsonPropertyName("eventDate")] public DateTime EventDate { get; set; }
    [JsonPropertyName("location")] public string Location { get; set; } = "";
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("distance")] public decimal Distance { get; set; }
    [JsonPropertyName("eventType")] public int EventType { get; set; }
    [JsonPropertyName("imageUrl")] public string? ImageUrl { get; set; }
    [JsonPropertyName("categories")] public List<CategoryVm> Categories { get; set; } = new();
}
public sealed class CategoryVm
{
    [JsonPropertyName("categoryID")] public int CategoryId { get; set; }
    [JsonPropertyName("eventID")] public int EventId { get; set; }
    [JsonPropertyName("event")] public EventSummaryVm? Event { get; set; }
    [JsonPropertyName("categoryName")] public string CategoryName { get; set; } = "";
    [JsonPropertyName("distanceKm")] public decimal DistanceKm { get; set; }
    [JsonPropertyName("maxParticipants")] public int MaxParticipants { get; set; }
    [JsonPropertyName("entryFee")] public decimal EntryFee { get; set; }
}
public sealed class EventSummaryVm
{
    [JsonPropertyName("eventID")] public int EventId { get; set; }
    [JsonPropertyName("eventName")] public string EventName { get; set; } = "";
    [JsonPropertyName("eventDate")] public DateTime EventDate { get; set; }
    [JsonPropertyName("location")] public string Location { get; set; } = "";
}
public sealed class EnrolmentVm
{
    [JsonPropertyName("enrolmentID")] public int EnrolmentId { get; set; }
    [JsonPropertyName("participantID")] public int ParticipantId { get; set; }
    [JsonPropertyName("categoryID")] public int CategoryId { get; set; }
    [JsonPropertyName("enrolmentDate")] public DateTime EnrolmentDate { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("participant")] public PersonVm? Participant { get; set; }
    [JsonPropertyName("category")] public CategoryVm? Category { get; set; }
    [JsonPropertyName("result")] public ResultVm? Result { get; set; }
}
public sealed class PersonVm
{
    [JsonPropertyName("fullName")] public string FullName { get; set; } = "";
}
public sealed class ResultVm
{
    [JsonPropertyName("resultID")] public int ResultId { get; set; }
    [JsonPropertyName("enrolmentID")] public int EnrolmentId { get; set; }
    [JsonPropertyName("finishTime")] public TimeSpan? FinishTime { get; set; }
    [JsonPropertyName("position")] public int? Position { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("enrolment")] public EnrolmentVm? Enrolment { get; set; }
    [JsonPropertyName("categoryName")] public string? CategoryName { get; set; }
    [JsonPropertyName("participantName")] public string? ParticipantName { get; set; }
}
public sealed class RegisterVm
{
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required, MinLength(8)] public string Password { get; set; } = "";
    [Required] public string Role { get; set; } = "Participant";
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
}
public sealed class LoginVm
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}
public sealed class EventFormVm
{
    public int EventId { get; set; }
    [Required, StringLength(150)] public string EventName { get; set; } = "";
    [Required] public DateTime EventDate { get; set; } = DateTime.Today.AddDays(14);
    [Required, StringLength(150)] public string Location { get; set; } = "";
    [Required, StringLength(500)] public string? Description { get; set; }
    [Range(0.1, 1000)] public decimal Distance { get; set; } = 5;
    [Range(0, 2)] public int EventType { get; set; }
}
public sealed class CategoryFormVm
{
    public int CategoryId { get; set; }
    public int EventId { get; set; }
    [Required, StringLength(50)] public string CategoryName { get; set; } = "";
    [Range(0.1, 1000)] public decimal DistanceKm { get; set; } = 5;
    [Range(1, 100000)] public int MaxParticipants { get; set; } = 100;
    [Range(0, 100000)] public decimal EntryFee { get; set; }
}
public sealed class ProfileVm
{
    public int UserId { get; set; }
    public string Role { get; set; } = "";
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
public sealed class ResultFormVm
{
    public int EnrolmentId { get; set; }
    public string FinishTime { get; set; } = "00:45:00";
    public int? Position { get; set; }
    public string Status { get; set; } = "Finished";
}

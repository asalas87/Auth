namespace Application.Security.Abstractions;

public class FailureTrackingEntry
{
    public int Attempts { get; set; }
    public DateTime? LastAttempt { get; set; }
    public DateTime? BlockedUntil { get; set; }
}

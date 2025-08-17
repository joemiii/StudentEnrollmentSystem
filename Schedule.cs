namespace StudentEnrollmentSystem;

public class Schedule
{
    // Basic schedule details
    public string Subject;
    public string Day;
    public TimeSpan StartTime;
    public TimeSpan EndTime;
    public int Units;
    public int Capacity;
    public int Enrolled;

    // Create a schedule instance
    public Schedule(string subject, string day, TimeSpan startTime, TimeSpan endTime, int units, int capacity)
    {
        Subject = subject;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        Units = units;
        Capacity = capacity;
        Enrolled = 0;
    }

    // True if there is still room
    public bool HasSlot() => Enrolled < Capacity;

    // Add one enrolled student if there is space
    public void EnrollOne()
    {
        if (HasSlot()) Enrolled++;
    }

    // Display string for this schedule
    public string Info() => $"{Subject} - {Day} {StartTime:hh\\:mm} to {EndTime:hh\\:mm} | Units: {Units} | Capacity: {Capacity} | Enrolled: {Enrolled}";

    // Label the time of day by start time
    public string TimeLabel()
    {
        int hour = StartTime.Hours;
        if (hour >= 6 && hour < 12) return "Morning";
        if (hour >= 12 && hour < 17) return "Afternoon";
        return "Evening";
    }

    // Check if this schedule overlaps with another on the same day
    public bool ConflictsWith(Schedule other)
    {
        if (other == null) return false;
        if (this.Day != other.Day) return false;
        return this.StartTime < other.EndTime && other.StartTime < this.EndTime;
    }
}

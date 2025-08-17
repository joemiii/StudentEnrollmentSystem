using System;
using System.Collections.Generic;

namespace StudentEnrollmentSystem
{
    class Program
    {
        // Main application state
        static Schedule[] schedules = new Schedule[21];
        static Student[] students = new Student[2];

        static void Main(string[] args)
        {
            StoredData();

            Console.Clear();
            Console.WriteLine("Welcome to the Student Enrollment System!\n");

            Student stud = null;
            while (stud == null)
            {
                stud = StudentLogin();
            }

            StudentMenu(stud);

            Console.WriteLine("Thank you for using the system. Goodbye!");
        }

        // Load sample users and schedules; each schedule has capacity 10 with 5-7 already enrolled
        static void StoredData()
        {
            students[0] = new Student("joem", "1234");

            schedules[0] = new Schedule("Web Development", "Monday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[1] = new Schedule("Web Development", "Tuesday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[2] = new Schedule("Web Development", "Tuesday", new TimeSpan(14, 40, 0), new TimeSpan(17, 40, 0), 3, 10);

            schedules[3] = new Schedule("IT Infrastructure and Network Technologies", "Friday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[4] = new Schedule("IT Infrastructure and Network Technologies", "Saturday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[5] = new Schedule("IT Infrastructure and Network Technologies", "Wednesday", new TimeSpan(18, 0, 0), new TimeSpan(21, 0, 0), 3, 10);

            schedules[6] = new Schedule("Professional Issues in Information Systems", "Saturday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[7] = new Schedule("Professional Issues in Information Systems", "Saturday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);

            schedules[8] = new Schedule("Data Structures and Algorithm", "Tuesday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[9] = new Schedule("Data Structures and Algorithm", "Wednesday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[10] = new Schedule("Data Structures and Algorithm", "Thursday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);

            schedules[11] = new Schedule("Enterprise Programming", "Monday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[12] = new Schedule("Enterprise Programming", "Tuesday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[13] = new Schedule("Enterprise Programming", "Thursday", new TimeSpan(14, 40, 0), new TimeSpan(17, 40, 0), 3, 10);

            schedules[14] = new Schedule("Business Process Management", "Wednesday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);
            schedules[15] = new Schedule("Business Process Management", "Wednesday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[16] = new Schedule("Business Process Management", "Thursday", new TimeSpan(18, 0, 0), new TimeSpan(21, 0, 0), 2, 10);

            schedules[17] = new Schedule("Physical Education", "Monday", new TimeSpan(8, 0, 0), new TimeSpan(10, 0, 0), 2, 10);
            schedules[18] = new Schedule("ASEAN", "Tuesday", new TimeSpan(11, 20, 0), new TimeSpan(14, 20, 0), 3, 10);
            schedules[19] = new Schedule("Evaluation of Business Performance", "Wednesday", new TimeSpan(14, 0, 0), new TimeSpan(17, 0, 0), 3, 10);
            schedules[20] = new Schedule("Art Appreciation", "Thursday", new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0), 3, 10);

            var rnd = new Random();
            for (int i = 0; i < schedules.Length; i++)
            {
                if (schedules[i] != null)
                {
                    schedules[i].Enrolled = rnd.Next(5, 8);
                    if (schedules[i].Enrolled > schedules[i].Capacity)
                        schedules[i].Enrolled = schedules[i].Capacity;
                }
            }
        }

        // Ask for username and password; keep asking until success or the user chooses not to retry
        static Student StudentLogin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== STUDENT LOGIN =====");
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                for (int i = 0; i < students.Length; i++)
                {
                    if (students[i] != null &&
                        students[i].Username == username &&
                        students[i].Password == password)
                    {
                        Console.WriteLine("Login successful!");
                        Pause();
                        return students[i];
                    }
                }

                Console.WriteLine("Login failed.");
                Console.Write("Retry login? (Y to retry, any other key to return to Main Menu): ");
                string retry = Console.ReadLine();
                if (string.IsNullOrEmpty(retry) || !retry.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }
        }

        // Main menu for student actions
        static void StudentMenu(Student s)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== STUDENT MENU =====");
                Console.WriteLine("1. Enlist Subjects");
                Console.WriteLine("2. Pick Schedule (Register via Preferences)");
                Console.WriteLine("3. View Registered Subjects/Schedules");
                Console.WriteLine("4. Enroll Registered Subjects");
                Console.WriteLine("5. View Enrolled Subjects");
                Console.WriteLine("6. Logout");
                Console.Write("\nEnter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": s.EnlistSubjects(schedules); break;
                    case "2": PickScheduleFlow(s); break;
                    case "3": Console.Clear(); s.ViewRegistered(schedules); Pause(); break;
                    case "4": Console.Clear(); s.EnrollRegistered(schedules); Pause(); break;
                    case "5": Console.Clear(); s.ViewEnrolled(schedules); Pause(); break;
                    case "6": return;
                    default: Console.WriteLine("Invalid choice."); Pause(); break;
                }
            }
        }

        // Guide the user to pick schedules using preferred days and time
        static void PickScheduleFlow(Student student)
        {
            if (student.EnlistedSubjects.Count() == 0)
            {
                Console.WriteLine("You have not enlisted any subjects.");
                Pause();
                return;
            }

            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            Console.Clear();
            Console.WriteLine("How many days do you want classes per week? (1-6): ");
            int numDays = ReadIntInRange(1, 6);

            Console.Clear();
            Console.WriteLine("Pick your preferred days (no duplicates):");
            int[] selectedDayIdx = ReadMultipleDays(days, numDays);
            HashSet<string> preferredDays = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var i in selectedDayIdx) preferredDays.Add(days[i]);

            Console.Clear();
            Console.WriteLine("Select preferred time slot:");
            Console.WriteLine("1. Morning\n2. Afternoon\n3. Evening");
            string slotInput = Console.ReadLine();
            string preferredSlot = slotInput switch { "1" => "Morning", "2" => "Afternoon", _ => "Evening" };
            Console.Clear();

            List<int> exactMatches = new List<int>();
            List<int> outsideDayMatches = new List<int>();

            for (int i = 0; i < schedules.Length; i++)
            {
                if (schedules[i] == null) continue;

                bool isEnlistedSubject = false;
                for (int j = 0; j < student.EnlistedSubjects.Count(); j++)
                {
                    int idx = student.EnlistedSubjects.Get(j);
                    if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == schedules[i].Subject)
                    { isEnlistedSubject = true; break; }
                }
                if (!isEnlistedSubject) continue;

                if (!schedules[i].HasSlot()) continue;
                if (HasTimeConflict(student, schedules[i], schedules)) continue;
                if (schedules[i].TimeLabel() != preferredSlot) continue;

                bool inPreferred = preferredDays.Contains(schedules[i].Day);
                if (inPreferred) exactMatches.Add(i); else outsideDayMatches.Add(i);
            }

            if (exactMatches.Count > 0)
            {
                Console.WriteLine("Exact matches found (preferred day and slot):\n");
                if (exactMatches.Count == 1)
                {
                    AskRegister(student, exactMatches[0], "Exact match found");
                    return;
                }
                else
                {
                    if (ShowAndRegisterFromList(student, exactMatches.ToArray(), "Exact matches")) return;
                }
            }

            Console.WriteLine("No exact matches with your preferred days.");
            if (outsideDayMatches.Count > 0)
            {
                Console.Write("Some schedules are outside your preferred days. Do you want to see them anyway? (Y/N): ");
                string ans = Console.ReadLine();
                if (!string.IsNullOrEmpty(ans) && ans.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    if (ShowAndRegisterFromList(student, outsideDayMatches.ToArray(), "Outside preferred days")) return;
                }
            }

            // Offer to drop enlisted subjects that have no schedules matching preferred days and slot
            List<string> dropCandidates = SubjectsWithNoPreferredMatches(student, preferredDays, preferredSlot);
            if (dropCandidates.Count > 0)
            {
                Console.WriteLine("\nNo schedules match your preferences for some enlisted subjects.");
                Console.Write("Do you want to drop any of them? (Y/N): ");
                string drop = Console.ReadLine();
                if (!string.IsNullOrEmpty(drop) && drop.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 0; i < dropCandidates.Count; i++)
                        Console.WriteLine($"{i + 1}. {dropCandidates[i]}");
                    Console.Write("Pick number to drop (or 0 to skip): ");
                    if (int.TryParse(Console.ReadLine(), out int pick) && pick > 0 && pick <= dropCandidates.Count)
                    {
                        RemoveEnlistedSubjectByName(student, dropCandidates[pick - 1], schedules);
                        Console.WriteLine($"Dropped: {dropCandidates[pick - 1]}");
                        Pause();
                    }
                }
            }

            Console.Write("\nNo schedules registered via preferences. Do you want to pick manually from all available? (Y/N): ");
            string manual = Console.ReadLine();
            if (!string.IsNullOrEmpty(manual) && manual.Equals("Y", StringComparison.OrdinalIgnoreCase))
                ChooseFromScheduleMenuForStudent(student);
            else
                Console.WriteLine("No schedule registered.\n");
            Pause();
        }

        // Read N unique days by showing the menu repeatedly
        static int[] ReadMultipleDays(string[] days, int count)
        {
            List<int> selected = new List<int>();
            while (selected.Count < count)
            {
                for (int i = 0; i < days.Length; i++)
                {
                    bool taken = selected.Contains(i);
                    Console.WriteLine($"{i + 1}. {days[i]}{(taken ? " (chosen)" : "")}");
                }
                Console.Write($"Pick day {selected.Count + 1} of {count} (1-6): ");
                if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= 6)
                {
                    int idx = num - 1;
                    if (!selected.Contains(idx)) selected.Add(idx);
                    else Console.WriteLine("You already picked that day.");
                }
                else Console.WriteLine("Invalid input.");
                Console.WriteLine();
            }
            return selected.ToArray();
        }

        // Read an integer in [min, max]
        static int ReadIntInRange(int min, int max)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (int.TryParse(s, out int n) && n >= min && n <= max) return n;
                Console.Write($"Enter a number between {min} and {max}: ");
            }
        }

        // Show a list of schedule indices and let the user pick one to register
        static bool ShowAndRegisterFromList(Student student, int[] indices, string label)
        {
            Console.WriteLine($"===== {label.ToUpper()} =====");
            for (int i = 0; i < indices.Length; i++)
            {
                var s = schedules[indices[i]];
                Console.WriteLine($"{i + 1}. {s.Info()}");
            }
            Console.Write("\nEnter number to register (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out int pick) && pick > 0 && pick <= indices.Length)
            {
                AskRegister(student, indices[pick - 1], label);
                return true;
            }
            Console.WriteLine("Cancelled.");
            Pause();
            return false;
        }

        // Compute subjects that have no schedules matching preferred days and slot
        static List<string> SubjectsWithNoPreferredMatches(Student student, HashSet<string> preferredDays, string preferredSlot)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int j = 0; j < student.EnlistedSubjects.Count(); j++)
            {
                int idx = student.EnlistedSubjects.Get(j);
                if (idx < 0 || schedules[idx] == null) continue;
                string subject = schedules[idx].Subject;
                if (seen.Contains(subject)) continue;
                seen.Add(subject);

                bool anyMatch = false;
                for (int i = 0; i < schedules.Length; i++)
                {
                    if (schedules[i] == null) continue;
                    if (schedules[i].Subject != subject) continue;
                    if (!schedules[i].HasSlot()) continue;
                    if (HasTimeConflict(student, schedules[i], schedules)) continue;
                    if (!preferredDays.Contains(schedules[i].Day)) continue;
                    if (schedules[i].TimeLabel() != preferredSlot) continue;
                    anyMatch = true; break;
                }
                if (!anyMatch) result.Add(subject);
            }
            return new List<string>(result);
        }

        // Remove all enlisted entries that belong to a subject name
        static void RemoveEnlistedSubjectByName(Student student, string subjectName, Schedule[] schedules)
        {
            int i = 0;
            while (i < student.EnlistedSubjects.Count())
            {
                int idx = student.EnlistedSubjects.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == subjectName)
                    student.EnlistedSubjects.RemoveAt(i);
                else i++;
            }
        }

        // Read a valid day selection (1-6) and return its index
        static int ReadDayFromMenu(string[] days)
        {
            for (int i = 0; i < days.Length; i++) Console.WriteLine($"{i + 1}. {days[i]}");
            Console.Write("Enter choice (1-6): ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= 6)
                    return num - 1;
                Console.Write("Invalid. Enter again (1-6): ");
            }
        }

        // Find the first schedule that matches day/slot, belongs to enlisted subjects, has space, and has no conflict
        static int FindScheduleBySlotForStudent(string day, string slot, Student student)
        {
            for (int i = 0; i < schedules.Length; i++)
            {
                if (schedules[i] == null) continue;

                bool isEnlisted = false;
                for (int j = 0; j < student.EnlistedSubjects.Count(); j++)
                {
                    int idx = student.EnlistedSubjects.Get(j);
                    if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == schedules[i].Subject)
                    {
                        isEnlisted = true;
                        break;
                    }
                }
                if (!isEnlisted) continue;

                if (schedules[i].Day.Equals(day, StringComparison.OrdinalIgnoreCase) &&
                    schedules[i].TimeLabel() == slot &&
                    schedules[i].HasSlot() &&
                    !HasTimeConflict(student, schedules[i], schedules))
                    return i;
            }
            return -1;
        }

        // Check time overlap against already registered and enrolled schedules
        static bool HasTimeConflict(Student student, Schedule newSchedule, Schedule[] schedules)
        {
            for (int i = 0; i < student.RegisteredIdx.Count(); i++)
            {
                int idx = student.RegisteredIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].ConflictsWith(newSchedule))
                    return true;
            }

            for (int i = 0; i < student.EnrolledIdx.Count(); i++)
            {
                int idx = student.EnrolledIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].ConflictsWith(newSchedule))
                    return true;
            }

            return false;
        }

        // Show the found schedule and ask for confirmation to register
        static void AskRegister(Student student, int scheduleIndex, string label)
        {
            Schedule s = schedules[scheduleIndex];
            Console.WriteLine($"{label}: {s.Subject} on {s.Day} {s.StartTime:hh\\:mm} to {s.EndTime:hh\\:mm}");
            Console.Write("Register this subject? (Y/N): ");
            string ans = Console.ReadLine();
            if (!string.IsNullOrEmpty(ans) && ans.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                student.RegisterSchedule(scheduleIndex, schedules);
                Console.WriteLine("Registered! Confirm via 'Enroll Registered Subjects'.\n");
            }
            else Console.WriteLine("Registration cancelled.\n");
            Pause();
        }

        // List all schedules for enlisted subjects and let the user pick by number
        static void ChooseFromScheduleMenuForStudent(Student student)
        {
            Console.Clear();
            Console.WriteLine("===== AVAILABLE SCHEDULES =====");
            int count = 0;
            int[] map = new int[schedules.Length];

            for (int i = 0; i < schedules.Length; i++)
            {
                if (schedules[i] == null) continue;

                bool isEnlisted = false;
                for (int j = 0; j < student.EnlistedSubjects.Count(); j++)
                {
                    int idx = student.EnlistedSubjects.Get(j);
                    if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == schedules[i].Subject)
                    { isEnlisted = true; break; }
                }

                if (!isEnlisted) continue;

                string fullTag = schedules[i].HasSlot() ? "" : " [FULL]";
                string conflictTag = HasTimeConflict(student, schedules[i], schedules) ? " [CONFLICT]" : "";
                count++;
                map[count - 1] = i;
                Console.WriteLine($"{count}. {schedules[i].Info()}{fullTag}{conflictTag}");
            }

            if (count == 0)
            {
                Console.WriteLine("No schedules found for your enlisted subjects.");
                Pause();
                return;
            }

            Console.Write("\nEnter number to register (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out int pick) && pick > 0 && pick <= count)
            {
                int idx = map[pick - 1];
                if (schedules[idx].HasSlot() && !HasTimeConflict(student, schedules[idx], schedules))
                    student.RegisterSchedule(idx, schedules);
                else Console.WriteLine("Cannot register this schedule (full or conflict).");
            }
            else Console.WriteLine("Cancelled.");
            Pause();
        }

        // Pause the screen until the user presses a key
        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}

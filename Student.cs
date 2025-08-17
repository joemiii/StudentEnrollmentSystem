namespace StudentEnrollmentSystem
{
    public class Student
    {
        // Login credentials
        public string Username;
        public string Password;

        // Lists of schedule indices
        public MyList RegisteredIdx = new MyList(20); // pending
        public MyList EnrolledIdx = new MyList(20);   // confirmed
        public MyList EnlistedSubjects = new MyList(20); // chosen subjects (by any schedule index of that subject)

        public Student(string username, string password)
        {
            Username = username;
            Password = password;
        }

        // True if the exact schedule index is already registered or enrolled
        public bool IsAlreadyRegistered(int scheduleIndex)
        {
            return RegisteredIdx.Contains(scheduleIndex) || EnrolledIdx.Contains(scheduleIndex);
        }

        // Flow to pick which subjects the student wants to take
        public void EnlistSubjects(Schedule[] schedules)
        {
            const int MaxUnits = 18;
            MyList enlistedSubjectsTemp = new MyList(20); // first found index per subject (temporary)

            while (true)
            {
                MyList shownSubjects = new MyList(50); // track which subjects were shown on screen
                Console.Clear();
                Console.WriteLine("=== SUBJECT ENLISTMENT ===");

                int menuNum = 1;
                MyList menuMap = new MyList(20); // map menu number -> schedule index

                for (int i = 0; i < schedules.Length; i++)
                {
                    if (schedules[i] == null) continue;

                    bool alreadyShown = false;
                    for (int j = 0; j < shownSubjects.Count(); j++)
                    {
                        int shownIdx = shownSubjects.Get(j);
                        if (shownIdx >= 0 && schedules[shownIdx] != null && schedules[i].Subject == schedules[shownIdx].Subject)
                        {
                            alreadyShown = true;
                            break;
                        }
                    }
                    if (alreadyShown) continue;

                    Console.WriteLine($"{menuNum}. {schedules[i].Subject} ({schedules[i].Units} units)");
                    menuMap.Add(i);
                    menuNum++;
                    shownSubjects.Add(i);
                }

                Console.WriteLine("0. Done enlisting");
                Console.Write("\nChoose a subject to enlist: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice)) continue;

                if (choice == 0)
                {
                    if (enlistedSubjectsTemp.Count() == 0)
                    {
                        Console.WriteLine("You haven't enlisted any subjects yet.");
                        Pause();
                        continue;
                    }

                    Console.WriteLine("\nYou have chosen:");
                    int totalUnits = 0;
                    for (int i = 0; i < enlistedSubjectsTemp.Count(); i++)
                    {
                        int idx = enlistedSubjectsTemp.Get(i);
                        Console.WriteLine($"{i + 1}. {schedules[idx].Subject} ({schedules[idx].Units} units)");
                        totalUnits += schedules[idx].Units;
                    }
                    Console.WriteLine($"Total units: {totalUnits}");
                    Console.Write("Confirm enlistment? (Y/N): ");
                    string confirm = Console.ReadLine();
                    if (!string.IsNullOrEmpty(confirm) && confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        for (int i = 0; i < enlistedSubjectsTemp.Count(); i++)
                            EnlistedSubjects.Add(enlistedSubjectsTemp.Get(i));

                        Console.WriteLine("Enlistment complete.");
                        Pause();
                        return;
                    }
                    else continue;
                }

                choice -= 1; // to 0-based
                if (choice >= 0 && choice < menuMap.Count())
                {
                    int selectedIdx = menuMap.Get(choice);
                    if (selectedIdx < 0 || schedules[selectedIdx] == null)
                    {
                        Console.WriteLine("Invalid selection.");
                        Pause();
                        continue;
                    }

                    // Avoid duplicate subject enlistment
                    bool duplicate = false;
                    string selectedSubject = schedules[selectedIdx].Subject;

                    for (int i = 0; i < enlistedSubjectsTemp.Count(); i++)
                    {
                        int idx = enlistedSubjectsTemp.Get(i);
                        if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == selectedSubject)
                        { duplicate = true; break; }
                    }
                    if (!duplicate)
                    {
                        for (int i = 0; i < EnlistedSubjects.Count(); i++)
                        {
                            int idx = EnlistedSubjects.Get(i);
                            if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == selectedSubject)
                            { duplicate = true; break; }
                        }
                    }
                    if (duplicate)
                    {
                        Console.WriteLine($"Already enlisted: {selectedSubject}");
                        Pause();
                        continue;
                    }

                    // Check max units against already selected + temp
                    int currentUnits = 0;
                    for (int i = 0; i < EnlistedSubjects.Count(); i++)
                    {
                        int idx = EnlistedSubjects.Get(i);
                        if (idx >= 0 && schedules[idx] != null) currentUnits += schedules[idx].Units;
                    }
                    for (int i = 0; i < enlistedSubjectsTemp.Count(); i++)
                    {
                        int idx = enlistedSubjectsTemp.Get(i);
                        if (idx >= 0 && schedules[idx] != null) currentUnits += schedules[idx].Units;
                    }

                    if (currentUnits + schedules[selectedIdx].Units > MaxUnits)
                    {
                        Console.WriteLine($"Cannot enlist {selectedSubject}. Max units reached.");
                        Pause();
                        continue;
                    }

                    enlistedSubjectsTemp.Add(selectedIdx);
                    Console.WriteLine($"Enlisted: {selectedSubject}");
                    Pause();
                }
            }
        }

        // Register a specific schedule index for an already enlisted subject
        public void RegisterSchedule(int scheduleIndex, Schedule[] schedules)
        {
            Schedule s = schedules[scheduleIndex];
            if (s == null)
            {
                Console.WriteLine("Invalid schedule.");
                return;
            }

            // Only allow registration if the subject is enlisted
            bool isEnlisted = false;
            for (int i = 0; i < EnlistedSubjects.Count(); i++)
            {
                int idx = EnlistedSubjects.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == s.Subject)
                {
                    isEnlisted = true;
                    break;
                }
            }
            if (!isEnlisted)
            {
                Console.WriteLine($"Cannot register {s.Subject}. You have not enlisted this subject.");
                return;
            }

            if (!s.HasSlot())
            {
                Console.WriteLine($"Cannot register {s.Subject}. Schedule is full.");
                return;
            }

            // Do not allow more than one schedule for the same subject
            for (int i = 0; i < RegisteredIdx.Count(); i++)
            {
                int idx = RegisteredIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == s.Subject)
                {
                    Console.WriteLine($"Already have a registered schedule for {s.Subject}.");
                    return;
                }
            }
            for (int i = 0; i < EnrolledIdx.Count(); i++)
            {
                int idx = EnrolledIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && schedules[idx].Subject == s.Subject)
                {
                    Console.WriteLine($"Already have an enrolled schedule for {s.Subject}.");
                    return;
                }
            }

            // Ensure no time conflict
            for (int i = 0; i < RegisteredIdx.Count(); i++)
            {
                int idx = RegisteredIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && s.ConflictsWith(schedules[idx]))
                {
                    Console.WriteLine($"Cannot register {s.Subject}. It conflicts with {schedules[idx].Subject}.");
                    return;
                }
            }

            for (int i = 0; i < EnrolledIdx.Count(); i++)
            {
                int idx = EnrolledIdx.Get(i);
                if (idx >= 0 && schedules[idx] != null && s.ConflictsWith(schedules[idx]))
                {
                    Console.WriteLine($"Cannot register {s.Subject}. It conflicts with {schedules[idx].Subject}.");
                    return;
                }
            }

            if (!IsAlreadyRegistered(scheduleIndex))
            {
                RegisteredIdx.Add(scheduleIndex);
                Console.WriteLine($"Registered: {s.Subject} on {s.Day} {s.StartTime:hh\\:mm} to {s.EndTime:hh\\:mm}");
            }
            else
            {
                Console.WriteLine($"Already registered: {s.Subject}");
            }
        }

        // Show all pending registrations
        public void ViewRegistered(Schedule[] schedules)
        {
            if (RegisteredIdx.Count() == 0)
            {
                Console.WriteLine("No registered subjects.\n");
                return;
            }

            Console.WriteLine("--- Registered (pending) ---");
            for (int i = 0; i < RegisteredIdx.Count(); i++)
                Console.WriteLine($"{i + 1}. {schedules[RegisteredIdx.Get(i)].Info()}");
            Console.WriteLine();
        }

        // Show all confirmed enrollments
        public void ViewEnrolled(Schedule[] schedules)
        {
            if (EnrolledIdx.Count() == 0)
            {
                Console.WriteLine("No enrolled subjects yet.\n");
                return;
            }

            Console.WriteLine("--- Enrolled (confirmed) ---");
            for (int i = 0; i < EnrolledIdx.Count(); i++)
                Console.WriteLine($"{i + 1}. {schedules[EnrolledIdx.Get(i)].Info()}");
            Console.WriteLine();
        }

        // Confirm all pending registrations (if slots still available)
        public void EnrollRegistered(Schedule[] schedules)
        {
            if (RegisteredIdx.Count() == 0)
            {
                Console.WriteLine("No registered subjects to enroll.\n");
                return;
            }

            Console.Write("Are you sure you want to enroll your registered subjects? (Y/N): ");
            string confirm = Console.ReadLine();
            if (string.IsNullOrEmpty(confirm) || !confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enrollment cancelled.\n");
                return;
            }

            int i = 0;
            while (i < RegisteredIdx.Count())
            {
                int idx = RegisteredIdx.Get(i);
                Schedule s = schedules[idx];

                if (s.HasSlot())
                {
                    s.EnrollOne();
                    EnrolledIdx.Add(idx);
                    RegisteredIdx.RemoveAt(i);
                    Console.WriteLine($"✓ Enrolled: {s.Subject} - {s.Day} {s.StartTime:hh\\:mm} to {s.EndTime:hh\\:mm}");
                }
                else i++;
            }

            Console.WriteLine($"\nEnrollment complete. Remaining pending: {RegisteredIdx.Count()}\n");
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}

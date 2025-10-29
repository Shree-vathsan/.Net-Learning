using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCabApp
{
    // ------------------------------
    // Abstract Account base (abstraction + encapsulation)
    // ------------------------------
    abstract class Account
    {
        public string Username { get; protected set; }
        private string _password; // encapsulated

        protected Account(string username, string password)
        {
            Username = username;
            _password = password;
        }

        // Password check (protected so derived classes can't expose the field directly)
        public bool CheckPassword(string password) => _password == password;

        // Virtual for potential customization
        public virtual void DisplayMenu()
        {
            Console.WriteLine("Base account - no menu.");
        }
    }

    // ------------------------------
    // Person class to share common fields (inheritance)
    // ------------------------------
    class Person
    {
        public string Name { get; set; }
        public string Contact { get; set; }

        public Person(string name, string contact)
        {
            Name = name;
            Contact = contact;
        }
    }

    // ------------------------------
    // Customer (User) class
    // ------------------------------
    class Customer : Account
    {
        public Person Profile { get; private set; }
        // Trip history belongs to the customer
        public List<Trip> TripHistory { get; } = new List<Trip>();

        public Customer(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        public override void DisplayMenu()
        {
            Console.WriteLine($"Customer menu for {Profile.Name} ({Username})");
        }
    }

    // ------------------------------
    // Driver class
    // ------------------------------
    class Driver : Account
    {
        public Person Profile { get; private set; }
        public List<Trip> Trips { get; } = new List<Trip>();
        public Car AssignedCar { get; set; }

        public Driver(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        // Hiding base behaviour with new to demo 'new' keyword (method hiding)
        public new void DisplayMenu()
        {
            Console.WriteLine($"Driver menu for {Profile.Name} ({Username}) - Car: {(AssignedCar != null ? AssignedCar.Name : "None")}");
        }
    }

    // ------------------------------
    // Owner class - sealed so you cannot derive further
    // ------------------------------
    sealed class Owner : Account
    {
        public Person Profile { get; private set; }
        public Owner(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        public override void DisplayMenu()
        {
            Console.WriteLine($"Owner menu for {Profile.Name} ({Username})");
        }
    }

    // ------------------------------
    // Car class - demonstrates encapsulation
    // ------------------------------
    class Car
    {
        public string Id { get; private set; } // unique
        public string Name { get; set; }       // e.g., "Swift-AB12"
        public int Position { get; set; }      // 1..10
        public bool IsAvailable { get; private set; } = true;
        public Driver Driver { get; set; }     // assigned driver (can be null)

        public Car(string id, string name, int position)
        {
            Id = id;
            Name = name;
            Position = position;
            IsAvailable = true;
        }

        public void StartTrip()
        {
            IsAvailable = false;
        }

        public void EndTrip(int newPosition)
        {
            IsAvailable = true;
            Position = newPosition;
        }

        public override string ToString()
        {
            return $"{Name} (Id:{Id}) Pos:{Position} Available:{IsAvailable} Driver:{(Driver != null ? Driver.Profile.Name : "Unassigned")}";
        }
    }

    // ------------------------------
    // Trip class (abstraction of a ride)
    // ------------------------------
    class Trip
    {
        public string TripId { get; private set; }
        public Customer Customer { get; private set; }
        public Driver Driver { get; private set; }
        public Car Car { get; private set; }
        public int Pickup { get; private set; }
        public int Drop { get; private set; }
        public int Price { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }

        public Trip(string tripId, Customer customer, Driver driver, Car car, int pickup, int drop, int price)
        {
            TripId = tripId;
            Customer = customer;
            Driver = driver;
            Car = car;
            Pickup = pickup;
            Drop = drop;
            Price = price;
            StartedAt = DateTime.Now;
            EndedAt = null;
        }

        public void CompleteTrip()
        {
            EndedAt = DateTime.Now;
        }

        public override string ToString()
        {
            string ended = EndedAt.HasValue ? EndedAt.Value.ToString("g") : "In-Progress";
            return $"Trip:{TripId} Cust:{Customer.Profile.Name} Driver:{Driver.Profile.Name} Car:{Car.Name} P:{Pickup} D:{Drop} Price:{Price} Start:{StartedAt:g} End:{ended}";
        }
    }

    // ------------------------------
    // Simple data store (in-memory)
    // ------------------------------
    static class DataStore
    {
        public static List<Customer> Customers { get; } = new List<Customer>();
        public static List<Driver> Drivers { get; } = new List<Driver>();
        public static List<Car> Cars { get; } = new List<Car>();
        public static List<Owner> Owners { get; } = new List<Owner>();
        public static List<Trip> Trips { get; } = new List<Trip>();

        // Helper to find accounts by username quickly
        public static Account FindAccount(string username)
        {
            var c = Customers.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (c != null) return c;
            var d = Drivers.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (d != null) return d;
            var o = Owners.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (o != null) return o;
            return null;
        }
    }

    // ------------------------------
    // Main app
    // ------------------------------
    class Program
    {
        static void Main()
        {
            SeedInitialData();
            Console.WriteLine("=== Welcome to ConsoleCab (Demo) ===");
            MainLoop();
        }

        static void MainLoop()
        {
            while (true)
            {
                Console.WriteLine("\nSelect:");
                Console.WriteLine("1) Register (Customer)");
                Console.WriteLine("2) Login");
                Console.WriteLine("3) Exit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        RegisterCustomer();
                        break;
                    case "2":
                        LoginFlow();
                        break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // ----------------------------
        // Registration
        // ----------------------------
        static void RegisterCustomer()
        {
            Console.WriteLine("\n--- Customer Registration ---");
            string username = ReadNonEmpty("Choose username: ");
            if (DataStore.FindAccount(username) != null)
            {
                Console.WriteLine("Username already exists. Choose another.");
                return;
            }
            string password = ReadPassword("Choose password: ");
            string name = ReadNonEmpty("Your full name: ");
            string contact = ReadNonEmpty("Contact (phone/email): ");

            var customer = new Customer(username, password, name, contact);
            DataStore.Customers.Add(customer);
            Console.WriteLine($"Customer '{username}' created. You can login now.");
        }

        // ----------------------------
        // Login
        // ----------------------------
        static void LoginFlow()
        {
            Console.WriteLine("\n--- Login ---");
            string username = ReadNonEmpty("Username: ");
            Account acc = DataStore.FindAccount(username);
            if (acc == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            string password = ReadPassword("Password: ");
            if (!acc.CheckPassword(password))
            {
                Console.WriteLine("Invalid password.");
                return;
            }

            Console.WriteLine($"Welcome, {username}!");
            if (acc is Customer c)
            {
                CustomerMenu(c);
            }
            else if (acc is Driver d)
            {
                DriverMenu(d);
            }
            else if (acc is Owner o)
            {
                OwnerMenu(o);
            }
        }

        // ----------------------------
        // Customer menu
        // ----------------------------
        static void CustomerMenu(Customer customer)
        {
            while (true)
            {
                Console.WriteLine($"\n--- Customer: {customer.Profile.Name} ---");
                Console.WriteLine("1) Book a Cab");
                Console.WriteLine("2) View My Trips");
                Console.WriteLine("3) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();
                switch (ch)
                {
                    case "1":
                        BookCabFlow(customer);
                        break;
                    case "2":
                        ViewCustomerTrips(customer);
                        break;
                    case "3":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ----------------------------
        // Driver menu
        // ----------------------------
        static void DriverMenu(Driver driver)
        {
            while (true)
            {
                driver.DisplayMenu(); // demonstrates 'new' hiding earlier
                Console.WriteLine("1) View My Trips");
                Console.WriteLine("2) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();
                switch (ch)
                {
                    case "1":
                        ViewDriverTrips(driver);
                        break;
                    case "2":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ----------------------------
        // Owner menu (owner can add cars and view all trips)
        // ----------------------------
        static void OwnerMenu(Owner owner)
        {
            while (true)
            {
                owner.DisplayMenu();
                Console.WriteLine("1) View All Trips");
                Console.WriteLine("2) View Car Status");
                Console.WriteLine("3) Add Car");
                Console.WriteLine("4) Add Driver & Assign to Car");
                Console.WriteLine("5) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();
                switch (ch)
                {
                    case "1":
                        ViewAllTrips();
                        break;
                    case "2":
                        ViewCarStatus();
                        break;
                    case "3":
                        AddCarFlow();
                        break;
                    case "4":
                        AddDriverAndAssignFlow();
                        break;
                    case "5":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ----------------------------
        // Booking flow
        // ----------------------------
        static void BookCabFlow(Customer customer)
        {
            Console.WriteLine("\n--- Book a Cab ---");
            int pickup = ReadIntInRange("Enter your pickup point (1-10): ", 1, 10);
            int drop = ReadIntInRange("Enter your drop point (1-10): ", 1, 10);
            if (pickup == drop)
            {
                Console.WriteLine("Pickup and drop cannot be the same.");
                return;
            }

            // Gather available cars + compute price = distance between pickup and car pos + distance from pickup to drop?
            // As per requirement: cost to travel is diff between points value (so price = |pickup - drop|).
            // But user asked "show cab near them in ascending order with the price in side" — we'll show cars sorted by distance from pickup
            var availableCars = DataStore.Cars.Where(c => c.IsAvailable).ToList();
            if (!availableCars.Any())
            {
                Console.WriteLine("No cars available right now.");
                return;
            }

            var carsWithDistance = availableCars
                .Select(car => new
                {
                    Car = car,
                    DistanceToPickup = Math.Abs(car.Position - pickup),
                    TripPrice = Math.Abs(pickup - drop) // price determined by difference between pickup and drop as specified
                })
                .OrderBy(x => x.DistanceToPickup)
                .ThenBy(x => x.TripPrice)
                .ToList();

            Console.WriteLine("\nAvailable cars near you (sorted by distance):");
            for (int i = 0; i < carsWithDistance.Count; i++)
            {
                var item = carsWithDistance[i];
                Console.WriteLine($"{i + 1}) {item.Car.Name} (Id:{item.Car.Id}) Pos:{item.Car.Position} Driver:{(item.Car.Driver != null ? item.Car.Driver.Profile.Name : "Unassigned")} Distance:{item.DistanceToPickup} Price:{item.TripPrice}");
            }

            int sel = ReadIntInRange($"Select car (1-{carsWithDistance.Count}) or 0 to cancel: ", 0, carsWithDistance.Count);
            if (sel == 0)
            {
                Console.WriteLine("Cancelled booking.");
                return;
            }

            var chosen = carsWithDistance[sel - 1];
            // Validate driver assigned
            if (chosen.Car.Driver == null)
            {
                Console.WriteLine("Selected car has no assigned driver. Owner must assign a driver first.");
                return;
            }

            // Create trip
            chosen.Car.StartTrip();
            var tripId = $"T{DataStore.Trips.Count + 1:000}";
            var trip = new Trip(tripId, customer, chosen.Car.Driver, chosen.Car, pickup, drop, chosen.TripPrice);
            DataStore.Trips.Add(trip);

            // Add to customer's and driver's histories
            customer.TripHistory.Add(trip);
            chosen.Car.Driver.Trips.Add(trip);

            Console.WriteLine($"Booked {chosen.Car.Name} with Driver {chosen.Car.Driver.Profile.Name}. Trip id: {trip.TripId}. Price: {trip.Price}");
            Console.WriteLine("Simulating trip... (press Enter to complete trip)");
            Console.ReadLine();

            // Complete trip simulation
            trip.CompleteTrip();
            chosen.Car.EndTrip(drop);
            Console.WriteLine($"Trip {trip.TripId} completed. Car now at position {chosen.Car.Position} and available.");
        }

        // ----------------------------
        // Viewing functions
        // ----------------------------
        static void ViewCustomerTrips(Customer customer)
        {
            Console.WriteLine($"\n--- Trip History for {customer.Profile.Name} ---");
            if (!customer.TripHistory.Any())
            {
                Console.WriteLine("No trips yet.");
                return;
            }
            foreach (var t in customer.TripHistory)
                Console.WriteLine(t.ToString());
        }

        static void ViewDriverTrips(Driver driver)
        {
            Console.WriteLine($"\n--- Trips for Driver {driver.Profile.Name} ---");
            if (!driver.Trips.Any())
            {
                Console.WriteLine("No trips yet.");
                return;
            }
            foreach (var t in driver.Trips)
                Console.WriteLine(t.ToString());
        }

        static void ViewAllTrips()
        {
            Console.WriteLine("\n--- All Trips (Owner view) ---");
            if (!DataStore.Trips.Any())
            {
                Console.WriteLine("No trips recorded yet.");
                return;
            }
            foreach (var t in DataStore.Trips)
                Console.WriteLine(t.ToString());
        }

        static void ViewCarStatus()
        {
            Console.WriteLine("\n--- Cars Status ---");
            foreach (var c in DataStore.Cars)
                Console.WriteLine(c.ToString());
        }

        // ----------------------------
        // Owner actions: Add car
        // ----------------------------
        static void AddCarFlow()
        {
            Console.WriteLine("\n--- Add New Car ---");
            string id;
            while (true)
            {
                id = ReadNonEmpty("Car unique Id: ");
                if (DataStore.Cars.Any(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Id already exists. Provide another.");
                }
                else break;
            }
            string name = ReadNonEmpty("Car name/model: ");
            int pos = ReadIntInRange("Initial position (1-10): ", 1, 10);
            var car = new Car(id, name, pos);
            DataStore.Cars.Add(car);
            Console.WriteLine($"Added car {car.Name} at position {pos}.");
        }

        static void AddDriverAndAssignFlow()
        {
            Console.WriteLine("\n--- Add Driver and Assign to Car ---");
            string username = ReadNonEmpty("Driver username: ");
            if (DataStore.FindAccount(username) != null)
            {
                Console.WriteLine("Username already exists.");
                return;
            }
            string password = ReadPassword("Driver password: ");
            string name = ReadNonEmpty("Driver full name: ");
            string contact = ReadNonEmpty("Driver contact: ");
            var driver = new Driver(username, password, name, contact);

            // Choose a car to assign
            Console.WriteLine("Available cars to assign:");
            var unassignedOrAny = DataStore.Cars.ToList();
            for (int i = 0; i < unassignedOrAny.Count; i++)
                Console.WriteLine($"{i + 1}) {unassignedOrAny[i].Name} Id:{unassignedOrAny[i].Id} Driver:{(unassignedOrAny[i].Driver != null ? unassignedOrAny[i].Driver.Profile.Name : "None")} Pos:{unassignedOrAny[i].Position}");

            int sel = ReadIntInRange($"Select car to assign (1-{unassignedOrAny.Count}) or 0 to cancel: ", 0, unassignedOrAny.Count);
            if (sel == 0)
            {
                Console.WriteLine("Cancelled.");
                return;
            }
            var chosenCar = unassignedOrAny[sel - 1];
            // Assign both ways
            driver.AssignedCar = chosenCar;
            chosenCar.Driver = driver;
            DataStore.Drivers.Add(driver);
            Console.WriteLine($"Driver {driver.Profile.Name} added and assigned to {chosenCar.Name}.");
        }

        // ----------------------------
        // Seed initial data (5 cars at different positions + owner)
        // ----------------------------
        static void SeedInitialData()
        {
            // Owner (username: owner / password: owner)
            var owner = new Owner("owner", "owner", "Super Owner", "owner@example.com");
            DataStore.Owners.Add(owner);

            // Create 5 initial cars
            var c1 = new Car("C1", "Swift-101", 2);
            var c2 = new Car("C2", "Dzire-202", 5);
            var c3 = new Car("C3", "Alto-303", 1);
            var c4 = new Car("C4", "Innova-404", 8);
            var c5 = new Car("C5", "Baleno-505", 10);
            DataStore.Cars.AddRange(new[] { c1, c2, c3, c4, c5 });

            // Create 3 drivers and assign to some cars
            var d1 = new Driver("driver1", "pass1", "Ramesh", "9990001");
            var d2 = new Driver("driver2", "pass2", "Suresh", "9990002");
            var d3 = new Driver("driver3", "pass3", "Mahesh", "9990003");

            d1.AssignedCar = c1; c1.Driver = d1;
            d2.AssignedCar = c2; c2.Driver = d2;
            d3.AssignedCar = c4; c4.Driver = d3;

            DataStore.Drivers.AddRange(new[] { d1, d2, d3 });

            // Create one sample customer
            var cust = new Customer("alice", "alice123", "Alice", "alice@example.com");
            DataStore.Customers.Add(cust);

            Console.WriteLine("Seeded initial data: owner, 5 cars, 3 drivers, 1 sample customer (alice/alice123).");
        }

        // ----------------------------
        // Helpers: Input validation
        // ----------------------------
        static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(s)) return s;
                Console.WriteLine("Cannot be empty.");
            }
        }

        static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine()?.Trim();
                if (int.TryParse(s, out int v))
                {
                    if (v >= min && v <= max) return v;
                }
                Console.WriteLine($"Enter a valid number between {min} and {max}.");
            }
        }

        static string ReadPassword(string prompt)
        {
            // Simple password entry (no masking for simplicity)
            return ReadNonEmpty(prompt);
        }
    }

    // ------------------------------
    // Small extension method to add range in older runtimes
    // ------------------------------
    static class Extensions
    {
        public static void AddRange<T>(this List<T> list, IEnumerable<T> items)
        {
            foreach (var it in items) list.Add(it);
        }
    }
}
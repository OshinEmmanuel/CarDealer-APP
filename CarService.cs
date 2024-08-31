public class CarService
{
    InvoiceService invoiceService = new InvoiceService();
    private const string CarFile = "cars.txt";
    public CarService()
    {
        // Ensure the user file exists
        if (!File.Exists(CarFile))
        {
            File.Create(CarFile).Close();
        }
    }

    public void GetAllCars()
    {
        var cars = File.ReadAllText(CarFile);
        if(cars == null)
        {
            Console.WriteLine("No car currently in the Lot");
        }
        else
        {
                 Console.WriteLine(cars);
        }

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void SearchCars()
    {
        Console.Write("Enter car make or model: ");
        var search = Console.ReadLine().ToLower();

        var cars = new List<Car>();

        foreach (var line in File.ReadAllLines(CarFile))
        {
            var parts = line.Split('\t');
            cars.Add(new Car
            {
                Make = parts[0],
                Model = parts[1],
                LotNumber = parts[2],
                Year = int.Parse(parts[3]),
                Price = decimal.Parse(parts[4]),
                IsAvailable = bool.Parse(parts[5])
            });
        }

        var searchCars = cars
        .Where(car => car.Make.ToLower().Contains(search) || car.Make.ToLower().Contains(search) || car.Model.ToLower().Contains(search))
        .ToList();

        
        Console.Clear();
        Console.WriteLine("Search Results");
        foreach (var car in cars)
        {
            Console.WriteLine($"LotNumber:{car.LotNumber} Maker:{car.Make} Model:{car.Model} ({car.Year}) - ${car.Price} - {(car.IsAvailable ? "Available" : "Sold")}");
        }
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void AddCar()
    {
        Console.Write("Enter car make: ");
        var make = Console.ReadLine();

        Console.Write("Enter car model: ");
        var model = Console.ReadLine();

         Console.Write("Enter car lot number: ");
        var lotNumber = Console.ReadLine();

        Console.Write("Enter car year: ");
        if (!int.TryParse(Console.ReadLine(), out int year) || year < 1886 || year > DateTime.Now.Year)
        {
            Console.WriteLine("Invalid year. Press any key to try again.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter car price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Invalid price. Press any key to try again.");
            Console.ReadKey();
            return;
        }

        var car = new Car
        {
            Make = make,
            Model = model,
            LotNumber = lotNumber,
            Year = year,
            Price = price
        };
        File.AppendAllText(CarFile, $"{car.Make}\t{car.Model}\t{car.LotNumber}\t{car.Year}\t{car.Price}\t{car.IsAvailable}\n");
        Console.WriteLine("Car added successfully! Press any key to continue.");
        Console.ReadKey();
    }

    public void UpdateCars(List<Car> cars)
    {
        File.WriteAllLines(CarFile, cars.Select(car => $"{car.Make}\t{car.Model}\t{car.LotNumber}\t{car.Year}\t{car.Price}\t{car.IsAvailable}"));
    }

    public void PurchaseCar(User user)
    {
        var cars = new List<Car>();

        foreach (var line in File.ReadAllLines(CarFile))
        {
            var parts = line.Split('\t');
            cars.Add(new Car
            {
                Make = parts[0],
                Model = parts[1],
                LotNumber = parts[2],
                Year = int.Parse(parts[3]),
                Price = decimal.Parse(parts[4]),
                IsAvailable = bool.Parse(parts[5])
            });
        }
        Car car = null;
        // Step 1: Find the Car
        while (car == null)
        {
            Console.Write("Enter car lot number to purchase: ");
            var search = Console.ReadLine()?.ToLower();

            car = cars.FirstOrDefault(c => c.LotNumber.ToLower() == search);

            if (car == null)
            {
                Console.WriteLine("Car not found. Press any key to try again or 'Q' to quit.");
                if (Console.ReadKey().Key == ConsoleKey.Q) return;
                Console.Clear();
            }
            else if (!car.IsAvailable)
            {
                Console.WriteLine("Car has been sold. Press any key to try again or 'Q' to quit.");
                car = null;
                if (Console.ReadKey().Key == ConsoleKey.Q) return;
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Car is available. Press any key to continue the purchase.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        // Step 2: Collect Buyer Information
        string firstName, lastName, address, phone, email;
        while (true)
        {
            Console.WriteLine("BUYER INFORMATION");
            Console.WriteLine("=========================");

            Console.Write("Enter your FirstName: ");
            firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName)) continue;

            Console.Write("Enter your LastName: ");
            lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName)) continue;

            Console.Write("Enter your Address: ");
            address = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(address)) continue;

            Console.Write("Enter your Phone number: ");
            phone = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phone)) continue;

            Console.Write("Enter your Email: ");
            email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) continue;

            break; // All fields were provided
        }

        // Step 3: Payment Details
        string cardNumber;
        decimal amount;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("PAYMENT DETAILS");
            Console.WriteLine("=========================");

            Console.Write("Enter your Payment Card Number: ");
            cardNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cardNumber)) continue;

            Console.Write("Enter your Payment amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out amount) || amount != car.Price)
            {
                Console.WriteLine("Invalid payment amount! Payment must match the car price.");
                Console.WriteLine("Press any key to try again or 'Q' to quit.");
                if (Console.ReadKey().Key == ConsoleKey.Q) return;
                continue;
            }

            break; // Valid payment amount
        }

        Console.WriteLine("Payment successful! Press any key to continue.");
        Console.ReadKey();

        // Step 4: Update the car's availability and generate the invoice
        car.IsAvailable = false;
        UpdateCars(cars);
        invoiceService.GenerateInvoice(user, car, firstName, lastName, email, phone, address, cardNumber);

        Console.WriteLine("Purchase successful! Press any key to continue.");
        Console.ReadKey();
    }
}


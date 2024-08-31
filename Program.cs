UserService userService = new UserService();
CarService carService = new CarService();

while (true)
{
    Console.Clear();
    Console.WriteLine("Car Management System");
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.WriteLine("3. Exit");
    Console.Write("Select an option: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            userService.Register();
            Console.ReadKey();
            break;
        case "2":
            bool login = userService.Login();
            if(login)
            {
                if (userService.LoggedInUser.IsAdmin)
                {
                    AdminMenu();
                }
                else
                {
                    UserMenu();
                }
            }
            else
            {
                Console.WriteLine("Login failed. Press any key to try again.");
                Console.ReadKey();
            }
            break;
        case "3":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid choice. Press any key to try again.");
            Console.ReadKey();
            break;
    }
}       

void AdminMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Admin Menu");
        Console.WriteLine("1. Add Car");
        Console.WriteLine("2. View All Cars");
        Console.WriteLine("3. Search Cars");
        Console.WriteLine("4. Create Admin");
        Console.WriteLine("5. Logout");
        Console.Write("Select an option: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                carService.AddCar();
                break;
            case "2":
                carService.GetAllCars();
                break;
            case "3":
                carService.SearchCars();
                break;
            case "4":
                userService.CreateAdmin();
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Invalid choice. Press any key to try again.");
                Console.ReadKey();
                break;
        }
    }
}

void UserMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("User Menu");
        Console.WriteLine("1. View All Cars");
        Console.WriteLine("2. Search Cars");
        Console.WriteLine("3. Purchase Car");
        Console.WriteLine("4. Logout");
        Console.Write("Select an option: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                carService.GetAllCars();
                break;
            case "2":
                carService.SearchCars();
                break;
            case "3":
                carService.PurchaseCar(userService.LoggedInUser);
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Invalid choice. Press any key to try again.");
                Console.ReadKey();
                break;
        }
    }
}

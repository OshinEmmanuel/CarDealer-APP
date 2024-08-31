using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class UserService
{
    private const string userFile = "users.txt";
    public User LoggedInUser { get; private set; }

    public UserService()
    {
        // Ensure the user file exists
        if (!File.Exists(userFile))
        {
            File.Create(userFile).Close();
        }

        // Seed initial admin if no users exist
        if (!File.ReadAllLines(userFile).Any())
        {
            SeedInitialAdmin();
        }
    }

    private void SeedInitialAdmin()
    {
        var user = new User
        {
            Username = "admin",
            Password = "admin", 
            IsAdmin = true
        };
        File.AppendAllText(userFile, $"{user.Username}\t{user.Password}\t{user.IsAdmin}\n");
        Console.WriteLine("Admin user seeded successful!.");
    }

    public void Register()
    {
         Console.Write("Enter username: ");
        var username = Console.ReadLine();

        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var user = new User
        {
            Username = username,
            Password = password,
            IsAdmin = false
        };

        if (File.ReadAllLines(userFile).Any(line => line.Split('\t')[0] == user.Username))
        {
            Console.WriteLine("Username already exists.");
            Console.WriteLine("Registration failed. Press any key to try again.");
            return;
        }
        File.AppendAllText(userFile, $"{user.Username}\t{user.Password}\t{user.IsAdmin}\n");
        Console.WriteLine("Registration successful! Press any key to continue.");
    }

    public bool Login()
    {
         Console.Write("Enter username: ");
        var username = Console.ReadLine();

        Console.Write("Enter password: ");
        var password = Console.ReadLine();
        var userRecord = File.ReadAllLines(userFile)
            .FirstOrDefault(line => line.Split('\t')[0] == username && line.Split('\t')[1] == password);

        if (userRecord != null)
        {
            var parts = userRecord.Split('\t');
            LoggedInUser = new User
            {
                Username = parts[0],
                Password = parts[1],
                IsAdmin = bool.Parse(parts[2])
            };
            return true;
        }
        return false;
    }

public void CreateAdmin()
{
    Console.Write("Enter username for new admin: ");
    var username = Console.ReadLine();

    Console.Write("Enter password for new admin: ");
    var password = Console.ReadLine();

    var user = new User
    {
        Username = username,
        Password = password,
    };

     if (File.ReadAllLines(userFile).Any(line => line.Split('\t')[0] == user.Username))
    {
        Console.WriteLine("Admin Username already exists.");
        Console.WriteLine("Registration failed. Press any key to try again.");
        return;
    }
    File.AppendAllText(userFile, $"{user.Username}\t{user.Password}\t{user.IsAdmin}\n");
    Console.WriteLine("Admin created successfully! Press any key to continue.");
    Console.ReadKey();
}
}


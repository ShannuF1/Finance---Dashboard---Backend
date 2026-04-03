using System;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = "Viewer"; // Viewer, Analyst, or Admin
    public bool IsActive { get; set; } = true;
}

public class RoleConstants
{
    public const string Admin = "Admin";
    public const string Analyst = "Analyst";
    public const string Viewer = "Viewer";
}

class Program
{
    static void Main(string[] args)
    {
        // Instantiate a new user using the constants
        User newUser = new User 
        { 
            Id = 1, 
            Username = "ShannuF1_Zorvyn Fintech", 
            Role = RoleConstants.Admin 
        };

        Console.WriteLine($"User: {newUser.Username}");
        Console.WriteLine($"Role: {newUser.Role}");
        Console.WriteLine($"Status: {(newUser.IsActive ? "Active" : "Inactive")}");

        // Example of Role Restriction Logic
        if (newUser.Role == RoleConstants.Admin)
        {
            Console.WriteLine("Access Level: Full Administrative Privileges.");
        }
        else
        {
            Console.WriteLine("Access Level: Restricted.");
        }

        // Keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

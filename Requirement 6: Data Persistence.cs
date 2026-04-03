using System;
using System.Collections.Generic;
using System.Linq;

// 1. Data Models
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}

public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
}

// 2. Simulated Database Context (Requirement 6 simulation)
// In a console app, we use Lists to simulate DbSet tables.
public class FinanceDbContext
{
    public List<User> Users { get; set; }
    public List<FinancialRecord> FinancialRecords { get; set; }

    public FinanceDbContext()
    {
        Users = new List<User>();
        FinancialRecords = new List<FinancialRecord>();
    }

    public void SaveChanges()
    {
        // Requirement 6: Ensure Data Integrity (Precision Simulation)
        foreach (var record in FinancialRecords)
        {
            // Simulating "HasPrecision(18, 2)" by rounding to 2 decimal places
            record.Amount = Math.Round(record.Amount, 2);
        }
        Console.WriteLine("Changes saved with data integrity (18,2 precision).");
    }
}

// 3. Entry Point
class Program
{
    static void Main(string[] args)
    {
        // Initialize Context
        FinanceDbContext context = new FinanceDbContext();

        // Add a record with high precision to test the logic
        context.FinancialRecords.Add(new FinancialRecord 
        { 
            Id = 1, 
            Amount = 150.12345m, // Extra decimals
            Category = "Testing Precision" 
        });

        Console.WriteLine("Original Amount: 150.12345");

        // Simulate the OnModelCreating/Save behavior
        context.SaveChanges();

        // Output result
        var savedRecord = context.FinancialRecords.First();
        Console.WriteLine("Saved Amount: " + savedRecord.Amount);

        Console.WriteLine("\nExecution complete. Press any key to exit.");
        if (!Console.IsInputRedirected) Console.ReadKey();
    }
}

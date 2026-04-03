using System;
using System.Collections.Generic;
using System.Linq;

// 1. Data Model
public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = "Expense";
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

// 2. Role Constants
public class RoleConstants
{
    public const string Admin = "Admin";
    public const string Analyst = "Analyst";
    public const string Viewer = "Viewer";
}

// 3. Service Logic (Replaces the Controller)
public class FinancialService
{
    private List<FinancialRecord> _database = new List<FinancialRecord>();

    // Simulated "PostRecord" with Role Restriction (Requirement 1 & 4)
    public string PostRecord(FinancialRecord record, string userRole)
    {
        // Manual Authorization Check
        if (userRole != RoleConstants.Admin)
        {
            return "403 Forbidden: Only Admins can create records.";
        }

        record.Id = _database.Count + 1;
        _database.Add(record);
        
        return "201 Created: Record added successfully with ID " + record.Id;
    }

    public List<FinancialRecord> GetRecords()
    {
        return _database;
    }
}

// 4. Entry Point
class Program
{
    static void Main(string[] args)
    {
        FinancialService service = new FinancialService();

        // Scenario A: A Viewer tries to create a record
        Console.WriteLine("--- Testing Viewer Access ---");
        FinancialRecord lunch = new FinancialRecord { Amount = 15.50m, Category = "Food", Date = DateTime.Now };
        string result1 = service.PostRecord(lunch, RoleConstants.Viewer);
        Console.WriteLine(result1);

        Console.WriteLine("\n--- Testing Admin Access ---");
        // Scenario B: An Admin tries to create a record
        FinancialRecord salary = new FinancialRecord { Amount = 5000m, Category = "Salary", Type = "Income", Date = DateTime.Now };
        string result2 = service.PostRecord(salary, RoleConstants.Admin);
        Console.WriteLine(result2);

        // Display results
        Console.WriteLine("\nCurrent Records in Database: " + service.GetRecords().Count);

        Console.WriteLine("\nPress any key to exit...");
        if (!Console.IsInputRedirected) Console.ReadKey();
    }
}

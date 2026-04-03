using System;
using System.Collections.Generic;
using System.Linq;

// 1. Data Model
public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // Income or Expense
    public string Category { get; set; }
    public DateTime Date { get; set; }
}

// 2. Dashboard Logic Class
public class FinancialDashboard
{
    private List<FinancialRecord> _records = new List<FinancialRecord>();

    public void AddRecord(FinancialRecord record) => _records.Add(record);

    // Requirement: Get Dashboard Summary
    public void DisplayDashboardSummary()
    {
        var totalIncome = _records.Where(r => r.Type == "Income").Sum(r => r.Amount);
        var totalExpenses = _records.Where(r => r.Type == "Expense").Sum(r => r.Amount);
        var netBalance = totalIncome - totalExpenses;

        var categoryTotals = _records.GroupBy(r => r.Category)
                                     .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) });

        var recentActivity = _records.OrderByDescending(r => r.Date).Take(5);

        // Output results to Console
        Console.WriteLine("--- DASHBOARD SUMMARY ---");
        Console.WriteLine("Total Income:   {0:C}", totalIncome);
        Console.WriteLine("Total Expenses: {0:C}", totalExpenses);
        Console.WriteLine("Net Balance:    {0:C}", netBalance);
        
        Console.WriteLine("\n--- BY CATEGORY ---");
        foreach (var cat in categoryTotals)
        {
            Console.WriteLine("{0}: {1:C}", cat.Category, cat.Total);
        }

        Console.WriteLine("\n--- RECENT ACTIVITY ---");
        foreach (var act in recentActivity)
        {
            Console.WriteLine("{0:yyyy-MM-dd}: {1} ({2:C})", act.Date, act.Category, act.Amount);
        }
    }
}

// 3. Entry Point
class Program
{
    static void Main(string[] args)
    {
        FinancialDashboard dashboard = new FinancialDashboard();

        // Seed data for testing
        dashboard.AddRecord(new FinancialRecord { Id = 1, Amount = 3000, Type = "Income", Category = "Salary", Date = DateTime.Now.AddDays(-2) });
        dashboard.AddRecord(new FinancialRecord { Id = 2, Amount = 50, Type = "Expense", Category = "Food", Date = DateTime.Now.AddDays(-1) });
        dashboard.AddRecord(new FinancialRecord { Id = 3, Amount = 100, Type = "Expense", Category = "Utilities", Date = DateTime.Now });
        dashboard.AddRecord(new FinancialRecord { Id = 4, Amount = 200, Type = "Income", Category = "Freelance", Date = DateTime.Now });

        // Execute Summary Logic
        dashboard.DisplayDashboardSummary();

        Console.WriteLine("\nPress any key to exit...");
        if (!Console.IsInputRedirected) Console.ReadKey();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

// 1. Data Model
public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }

    public FinancialRecord()
    {
        Type = "Expense";
        Category = string.Empty;
        Notes = string.Empty;
    }
}

// 2. Logic Class
public class FinancialManager
{
    private List<FinancialRecord> _records = new List<FinancialRecord>();

    public void AddRecord(FinancialRecord record) => _records.Add(record);

    // FIX: Removed '?' from string parameters because strings are nullable by default
    public List<FinancialRecord> GetRecords(DateTime? date, string category, string type)
    {
        IEnumerable<FinancialRecord> query = _records;

        if (date.HasValue) 
            query = query.Where(r => r.Date.Date == date.Value.Date);
        
        if (!string.IsNullOrEmpty(category)) 
            query = query.Where(r => r.Category == category);
        
        if (!string.IsNullOrEmpty(type)) 
            query = query.Where(r => r.Type == type);

        return query.ToList();
    }
}

// 3. Entry Point
class Program
{
    static void Main(string[] args)
    {
        FinancialManager manager = new FinancialManager();

        // Adding sample data
        manager.AddRecord(new FinancialRecord { 
            Id = 1, 
            Amount = 50.00m, 
            Category = "Food", 
            Type = "Expense", 
            Date = DateTime.Now 
        });

        manager.AddRecord(new FinancialRecord { 
            Id = 2, 
            Amount = 1200.00m, 
            Category = "Salary", 
            Type = "Income", 
            Date = DateTime.Now 
        });

        Console.WriteLine("--- Filtered Records (Income) ---");

        // Passing 'null' works fine for standard strings
        var results = manager.GetRecords(null, null, "Income");

        foreach (var record in results)
        {
            Console.WriteLine("ID: {0} | Category: {1} | Amount: {2:C}", 
                record.Id, record.Category, record.Amount);
        }

        Console.WriteLine("\nPress any key to exit.");
        if (Console.IsInputRedirected == false) Console.ReadKey();
    }
}

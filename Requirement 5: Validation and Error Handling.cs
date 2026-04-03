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

// 2. Service Logic Class
public class FinancialService
{
    private List<FinancialRecord> _database = new List<FinancialRecord>();

    public void SeedData(FinancialRecord record) => _database.Add(record);

    // Requirement: Update Record with Validation
    public string UpdateRecord(int id, FinancialRecord record)
    {
        // 400 Status: ID Mismatch
        if (id != record.Id) 
            return "400 Bad Request: ID mismatch";

        // 422 Status: Validation
        if (record.Amount <= 0) 
            return "422 Unprocessable Entity: Amount must be greater than zero";

        // Find existing record
        var existingRecord = _database.FirstOrDefault(r => r.Id == id);

        // 404 Status: Not Found
        if (existingRecord == null)
            return "404 Not Found: Record does not exist";

        // Simulate Update
        existingRecord.Amount = record.Amount;
        existingRecord.Category = record.Category;
        existingRecord.Type = record.Type;
        existingRecord.Date = record.Date;

        return "204 No Content: Update successful";
    }
}

// 3. Entry Point
class Program
{
    static void Main(string[] args)
    {
        FinancialService service = new FinancialService();

        // Seed a record for testing (ID: 10)
        service.SeedData(new FinancialRecord { Id = 10, Amount = 100, Category = "Utilities" });

        Console.WriteLine("--- Testing Update Logic ---");

        // Test 1: ID Mismatch
        var update1 = new FinancialRecord { Id = 99, Amount = 50 };
        Console.WriteLine("Test ID Mismatch: " + service.UpdateRecord(10, update1));

        // Test 2: Invalid Amount
        var update2 = new FinancialRecord { Id = 10, Amount = -5 };
        Console.WriteLine("Test Invalid Amount: " + service.UpdateRecord(10, update2));

        // Test 3: Successful Update
        var update3 = new FinancialRecord { Id = 10, Amount = 150, Category = "Electricity" };
        Console.WriteLine("Test Success: " + service.UpdateRecord(10, update3));

        Console.WriteLine("\nExecution finished. Press any key to exit.");
        if (!Console.IsInputRedirected) Console.ReadKey();
    }
}

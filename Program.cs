using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// 1. Data Persistence (Requirement 6: In-Memory Store for simplicity)
var db = new FinanceDb();

var app = builder.Build();

// --- 1. User & Role Management & 4. Access Control Logic ---
// Simple Middleware-like logic for Role Validation
bool IsAuthorized(string? role, string requiredRole) {
    if (role == "Admin") return true; // Admin has full access
    if (role == "Analyst" && (requiredRole == "Analyst" || requiredRole == "Viewer")) return true;
    if (role == "Viewer" && requiredRole == "Viewer") return true;
    return false;
}

// --- 2. Financial Records Management (CRUD) ---

// Create Record (Admin only)
app.MapPost("/records", ([FromHeader(Name = "X-Role")] string role, [FromBody] FinancialRecord record) => {
    if (!IsAuthorized(role, "Admin")) return Results.Forbid();
    
    // 5. Validation and Error Handling
    if (record.Amount <= 0) return Results.BadRequest("Amount must be positive.");
    
    record.Id = Guid.NewGuid();
    db.Records.TryAdd(record.Id, record);
    return Results.Created($"/records/{record.Id}", record);
});

// View Records (Filtered) - Requirement 2 & Checklist "Record Filtering"
app.MapGet("/records", ([FromHeader(Name = "X-Role")] string role, string? category, string? type) => {
    if (!IsAuthorized(role, "Viewer")) return Results.Forbid();

    var query = db.Records.Values.AsEnumerable();
    if (!string.IsNullOrEmpty(category)) query = query.Where(r => r.Category == category);
    if (!string.IsNullOrEmpty(type)) query = query.Where(r => r.Type == type);

    return Results.Ok(query.ToList());
});

// --- 3. Dashboard Summary APIs ---
app.MapGet("/dashboard/summary", ([FromHeader(Name = "X-Role")] string role) => {
    if (!IsAuthorized(role, "Analyst")) return Results.Forbid();

    var records = db.Records.Values;
    var totalIncome = records.Where(r => r.Type == "Income").Sum(r => r.Amount);
    var totalExpenses = records.Where(r => r.Type == "Expense").Sum(r => r.Amount);

    return Results.Ok(new {
        TotalIncome = totalIncome,
        TotalExpenses = totalExpenses,
        NetBalance = totalIncome - totalExpenses,
        RecentActivity = records.OrderByDescending(r => r.Date).Take(5),
        CategoryWise = records.GroupBy(r => r.Category)
                             .Select(g => new { Category = g.Key, Total = g.Sum(r => r.Amount) })
    });
});

// Optional Enhancement: API Documentation (Simple Info Endpoint)
app.MapGet("/", () => "Finance Dashboard API is running. Use X-Role header (Admin/Analyst/Viewer) to test.");

app.Run();

// --- Models and Mock DB ---

public class FinanceDb {
    public ConcurrentDictionary<Guid, FinancialRecord> Records { get; set; } = new();
    public FinanceDb() {
        // Seed data for immediate testing
        var id = Guid.NewGuid();
        Records.TryAdd(id, new FinancialRecord { Id = id, Amount = 5000, Type = "Income", Category = "Salary", Date = DateTime.UtcNow });
    }
}

public class FinancialRecord {
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = "Expense"; // Income or Expense
    public string Category { get; set; } = "General";
    public DateTime Date { get; set; }
    public string Notes { get; set; } = "";
}

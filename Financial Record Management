public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = "Expense"; // Income or Expense
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
}

// Controller Logic for CRUD and Filtering (Requirement 2)
[HttpGet]
public async Task<ActionResult<IEnumerable<FinancialRecord>>> GetRecords(
    [FromQuery] DateTime? date, [FromQuery] string? category, [FromQuery] string? type)
{
    var query = _context.FinancialRecords.AsQueryable();

    if (date.HasValue) query = query.Where(r => r.Date.Date == date.Value.Date);
    if (!string.IsNullOrEmpty(category)) query = query.Where(r => r.Category == category);
    if (!string.IsNullOrEmpty(type)) query = query.Where(r => r.Type == type);

    return await query.ToListAsync();
}

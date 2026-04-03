[HttpGet("summary")]
public async Task<IActionResult> GetDashboardSummary()
{
    var records = await _context.FinancialRecords.ToListAsync();

    var summary = new
    {
        TotalIncome = records.Where(r => r.Type == "Income").Sum(r => r.Amount),
        TotalExpenses = records.Where(r => r.Type == "Expense").Sum(r => r.Amount),
        NetBalance = records.Where(r => r.Type == "Income").Sum(r => r.Amount) - 
                     records.Where(r => r.Type == "Expense").Sum(r => r.Amount),
        CategoryTotals = records.GroupBy(r => r.Category)
                                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) }),
        RecentActivity = records.OrderByDescending(r => r.Date).Take(5)
    };

    return Ok(summary);
}

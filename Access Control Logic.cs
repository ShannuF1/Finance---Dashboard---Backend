// Example of restricting a 'Create' action to Admin Only
[HttpPost]
[Authorize(Roles = RoleConstants.Admin)] 
public async Task<ActionResult<FinancialRecord>> PostRecord(FinancialRecord record)
{
    _context.FinancialRecords.Add(record);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetRecords), new { id = record.Id }, record);
}

// Viewers can ONLY access GetRecords, not summaries or modifications.

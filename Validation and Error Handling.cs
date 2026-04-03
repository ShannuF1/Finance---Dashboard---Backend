[HttpPut("{id}")]
public async Task<IActionResult> UpdateRecord(int id, FinancialRecord record)
{
    if (id != record.Id) return BadRequest("ID mismatch"); // 400 Status

    if (record.Amount <= 0) 
        return UnprocessableEntity("Amount must be greater than zero"); // 422 Status

    _context.Entry(record).State = EntityState.Modified;
    
    try {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) {
        if (!RecordExists(id)) return NotFound(); // 404 Status
        throw;
    }

    return NoContent();
}

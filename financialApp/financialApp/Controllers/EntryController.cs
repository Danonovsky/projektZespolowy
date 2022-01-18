#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using financialApp.DAO;
using financialApp.DAO.Models;
using Microsoft.AspNetCore.Authorization;
using financialApp.Helpers;

namespace financialApp.Controllers;

public class EntryIn
{
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal Amount { get; set; } = 0;
    public string Description { get; set; } = "";
    public Guid CategoryId { get; set; }
    public Guid WalletId { get; set; }
    public string EntryType { get; set; }
    public string Priority { get; set; }
}

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EntryController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid UserId { get; set; }

    public EntryController(MyDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        UserId = _httpContextAccessor.GetUserId();
    }

    // GET: api/Entry
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
    {
        return await _context.Entries
            .Where(_ => _.Id == UserId)
            .ToListAsync();
    }
    
    [HttpGet("Wallet/{walletId}")]
    public async Task<ActionResult<IEnumerable<EntryOut>>> GetEntriesFromWallet(Guid WalletId)
    {
        return await _context.Entries
            .Where(_ => _.WalletId == WalletId)
            .Select(_ => new EntryOut
            {
                Amount = _.Amount,
                Category = new CategoryOut
                {
                    Id = _.Category.Id,
                    Name = _.Category.Name,
                },
                CategoryId = _.CategoryId,
                Date = _.Date,
                Description = _.Description,
                EntryType = _.EntryType.ToString(),
                Id = _.Id,
                Priority = _.Priority.ToString(),
                WalletId = _.WalletId
            })
            .OrderByDescending(_ => _.Date)
            .ToListAsync();
    }

    // GET: api/Entry/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Entry>> GetEntry(Guid id)
    {
        var entry = await _context.Entries.FindAsync(id);

        if (entry == null)
        {
            return NotFound();
        }

        return entry;
    }

    // PUT: api/Entry/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntry(Guid id, Entry entry)
    {
        if (id != entry.Id)
        {
            return BadRequest();
        }

        _context.Entry(entry).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Entry
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Entry>> PostEntry(EntryIn request)
    {
        Entry entry = new Entry
        {
            Amount = request.Amount,
            CategoryId = request.CategoryId,
            WalletId = request.WalletId,
            Date = request.Date,
            Description = request.Description
        };
        entry.EntryType = request.EntryType switch
        {
            "income" => EntryType.Income,
            _ => EntryType.Expense,
        };
        entry.Priority = request.Priority switch
        {
            "low" => Priority.Low,
            "high" => Priority.High,
            _ => Priority.Medium,
        };
        _context.Entries.Add(entry);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEntry", new { id = entry.Id }, entry);
    }

    // DELETE: api/Entry/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEntry(Guid id)
    {
        var entry = await _context.Entries.FindAsync(id);
        if (entry == null)
        {
            return NotFound();
        }

        _context.Entries.Remove(entry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EntryExists(Guid id)
    {
        return _context.Entries.Any(e => e.Id == id);
    }
}

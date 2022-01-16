#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using financialApp.DAO;
using financialApp.DAO.Models;
using Microsoft.AspNetCore.Authorization;
using financialApp.Helpers;

namespace financialApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid UserId { get; set; }

    public WalletController(
        MyDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        UserId = _httpContextAccessor.GetUserId();
    }

    // GET: api/Wallet
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
    {
        return await _context.Wallets
            .Where(_ => _.Id == UserId)
            .Include(_ => _.Entries)
            .ThenInclude(_ => _.Category)
            .ToListAsync();
    }

    // GET: api/Wallet/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Wallet>> GetWallet(Guid id)
    {
        var wallet = await _context.Wallets.
            Include(_ => _.Entries)
            .FirstAsync(_ => _.Id == id);

        if (wallet == null)
        {
            return NotFound();
        }

        return wallet;
    }

    // PUT: api/Wallet/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWallet(Guid id, Wallet wallet)
    {
        if (id != wallet.Id)
        {
            return BadRequest();
        }

        _context.Entry(wallet).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WalletExists(id))
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

    // POST: api/Wallet
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Wallet>> PostWallet(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetWallet", new { id = wallet.Id }, wallet);
    }

    // DELETE: api/Wallet/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet == null)
        {
            return NotFound();
        }

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WalletExists(Guid id)
    {
        return _context.Wallets.Any(e => e.Id == id);
    }
}

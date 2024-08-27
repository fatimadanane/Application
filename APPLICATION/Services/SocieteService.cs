using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPLICATION.Models.ARCHIVE;
using APPLICATION.Data;
namespace APPLICATION
{
public class SocieteService
{
    private readonly ARCHIVEContext _context;

    public SocieteService(ARCHIVEContext context)
    {
        _context = context;
    }

    public async Task<List<TSociete>> GetSocietes()
    {
        return await _context.TSocietes.ToListAsync();
    }

    public async Task<TSociete> GetSocieteById(int id)
    {
        return await _context.TSocietes.FindAsync(id);
    }

    public async Task AddSociete(TSociete societe)
    {
        _context.TSocietes.Add(societe);
        await _context.SaveChangesAsync();
       
    }

    public async Task UpdateSociete(TSociete societe)
    {
        _context.Entry(societe).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSociete(int id)
    {
        var societe = await _context.TSocietes.FindAsync(id);
        _context.TSocietes.Remove(societe);
        await _context.SaveChangesAsync();
    }
}
}
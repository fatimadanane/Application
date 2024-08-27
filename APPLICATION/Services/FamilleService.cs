using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPLICATION.Models.ARCHIVE;
using APPLICATION.Data;
namespace APPLICATION
{
public class FamilleService
{
    private readonly ARCHIVEContext _context; 

    public FamilleService(ARCHIVEContext context)
    {
        _context = context;
    }



 public async Task<List<TFamille>> GetGrandParentsAsync()
{
    return await _context.TFamilles
        .AsNoTracking().Where(f => f.niveau == 0)
        .ToListAsync();
}

    public async Task<List<TFamille>> GetChildrenByParentIdAsync(int parentId)
    {
        return await _context.TFamilles
            .Where(f => f.parent == parentId)
            .ToListAsync();
    }

    public async Task<List<TFamille>> GetSubChildrenByParentIdAsync(int parentId)
    {
        return await _context.TFamilles
            .Where(f => f.parent == parentId)
            .ToListAsync();
    }

//delete 

    public async Task DeleteGrandParentAsync(TFamille grandParent)
    {
        _context.TFamilles.Remove(grandParent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteChildAsync(TFamille child)
    {
        _context.TFamilles.Remove(child);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubChildAsync(TFamille subChild)
    {
        _context.TFamilles.Remove(subChild);
        await _context.SaveChangesAsync();
    }
 public async Task AddFamille(TFamille famille)
    {
        _context.TFamilles.Add(famille);
        await _context.SaveChangesAsync();
       
    }
    public async Task UpdateFamille(TFamille famille)
{
    _context.TFamilles.Update(famille);
    await _context.SaveChangesAsync();
}
}}

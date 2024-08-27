using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPLICATION.Models.ARCHIVE;
using APPLICATION.Data;
namespace APPLICATION
{
 public class DocumentService
    {
        private readonly ARCHIVEContext _context;

        public DocumentService(ARCHIVEContext context)
        {
            _context = context;
        }


public async Task<List<TDocument>> GetDocumentsWithCloseExpirationAsync()
{
    // Fetch all documents
    var allDocuments = await GetAllDocumentsAsync();

    // Get documents with alert date equal to today
    var today = DateTime.Today;
    var closeExpirationDocuments = allDocuments
        .Where(document => document.Date_Alerte == today)
        .ToList();

    return closeExpirationDocuments;
}



        public async Task<List<TDocument>> GetAllDocumentsAsync()
        {
            return await _context.TDocuments.ToListAsync();
        }

        public async Task<TDocument> GetDocumentByIdAsync(int id)
        {
            return await _context.TDocuments.FirstOrDefaultAsync(d => d.id == id);
        }

        public async Task AddDocumentAsync(TDocument document)
        {
            _context.TDocuments.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDocumentAsync(TDocument document)
        {
            _context.Entry(document).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _context.TDocuments.FindAsync(id);
            if (document != null)
            {
                _context.TDocuments.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}



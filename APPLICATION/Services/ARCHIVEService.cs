using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using APPLICATION.Data;

namespace APPLICATION
{
    public partial class ARCHIVEService
    {
        ARCHIVEContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ARCHIVEContext context;
        private readonly NavigationManager navigationManager;

        public ARCHIVEService(ARCHIVEContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportTDocumentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tdocuments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tdocuments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTDocumentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tdocuments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tdocuments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTDocumentsRead(ref IQueryable<APPLICATION.Models.ARCHIVE.TDocument> items);

        public async Task<IQueryable<APPLICATION.Models.ARCHIVE.TDocument>> GetTDocuments(Query query = null)
        {
            var items = Context.TDocuments.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTDocumentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTDocumentGet(APPLICATION.Models.ARCHIVE.TDocument item);
        partial void OnGetTDocumentById(ref IQueryable<APPLICATION.Models.ARCHIVE.TDocument> items);


        public async Task<APPLICATION.Models.ARCHIVE.TDocument> GetTDocumentById(int id)
        {
            var items = Context.TDocuments
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetTDocumentById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTDocumentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTDocumentCreated(APPLICATION.Models.ARCHIVE.TDocument item);
        partial void OnAfterTDocumentCreated(APPLICATION.Models.ARCHIVE.TDocument item);

        public async Task<APPLICATION.Models.ARCHIVE.TDocument> CreateTDocument(APPLICATION.Models.ARCHIVE.TDocument tdocument)
        {
            OnTDocumentCreated(tdocument);

            var existingItem = Context.TDocuments
                              .Where(i => i.id == tdocument.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TDocuments.Add(tdocument);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tdocument).State = EntityState.Detached;
                throw;
            }

            OnAfterTDocumentCreated(tdocument);

            return tdocument;
        }

        public async Task<APPLICATION.Models.ARCHIVE.TDocument> CancelTDocumentChanges(APPLICATION.Models.ARCHIVE.TDocument item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTDocumentUpdated(APPLICATION.Models.ARCHIVE.TDocument item);
        partial void OnAfterTDocumentUpdated(APPLICATION.Models.ARCHIVE.TDocument item);

        public async Task<APPLICATION.Models.ARCHIVE.TDocument> UpdateTDocument(int id, APPLICATION.Models.ARCHIVE.TDocument tdocument)
        {
            OnTDocumentUpdated(tdocument);

            var itemToUpdate = Context.TDocuments
                              .Where(i => i.id == tdocument.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tdocument);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTDocumentUpdated(tdocument);

            return tdocument;
        }

        partial void OnTDocumentDeleted(APPLICATION.Models.ARCHIVE.TDocument item);
        partial void OnAfterTDocumentDeleted(APPLICATION.Models.ARCHIVE.TDocument item);

        public async Task<APPLICATION.Models.ARCHIVE.TDocument> DeleteTDocument(int id)
        {
            var itemToDelete = Context.TDocuments
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTDocumentDeleted(itemToDelete);


            Context.TDocuments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTDocumentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTFamillesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tfamilles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tfamilles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTFamillesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tfamilles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tfamilles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTFamillesRead(ref IQueryable<APPLICATION.Models.ARCHIVE.TFamille> items);

        public async Task<IQueryable<APPLICATION.Models.ARCHIVE.TFamille>> GetTFamilles(Query query = null)
        {
            var items = Context.TFamilles.AsQueryable();

            items = items.Include(i => i.TFamille1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTFamillesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTFamilleGet(APPLICATION.Models.ARCHIVE.TFamille item);
        partial void OnGetTFamilleById(ref IQueryable<APPLICATION.Models.ARCHIVE.TFamille> items);


        public async Task<APPLICATION.Models.ARCHIVE.TFamille> GetTFamilleById(int id)
        {
            var items = Context.TFamilles
                              .AsNoTracking()
                              .Where(i => i.id == id);

            items = items.Include(i => i.TFamille1);
 
            OnGetTFamilleById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTFamilleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTFamilleCreated(APPLICATION.Models.ARCHIVE.TFamille item);
        partial void OnAfterTFamilleCreated(APPLICATION.Models.ARCHIVE.TFamille item);

        public async Task<APPLICATION.Models.ARCHIVE.TFamille> CreateTFamille(APPLICATION.Models.ARCHIVE.TFamille tfamille)
        {
            OnTFamilleCreated(tfamille);

            var existingItem = Context.TFamilles
                              .Where(i => i.id == tfamille.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TFamilles.Add(tfamille);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tfamille).State = EntityState.Detached;
                throw;
            }

            OnAfterTFamilleCreated(tfamille);

            return tfamille;
        }

        public async Task<APPLICATION.Models.ARCHIVE.TFamille> CancelTFamilleChanges(APPLICATION.Models.ARCHIVE.TFamille item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTFamilleUpdated(APPLICATION.Models.ARCHIVE.TFamille item);
        partial void OnAfterTFamilleUpdated(APPLICATION.Models.ARCHIVE.TFamille item);

        public async Task<APPLICATION.Models.ARCHIVE.TFamille> UpdateTFamille(int id, APPLICATION.Models.ARCHIVE.TFamille tfamille)
        {
            OnTFamilleUpdated(tfamille);

            var itemToUpdate = Context.TFamilles
                              .Where(i => i.id == tfamille.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tfamille);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTFamilleUpdated(tfamille);

            return tfamille;
        }

        partial void OnTFamilleDeleted(APPLICATION.Models.ARCHIVE.TFamille item);
        partial void OnAfterTFamilleDeleted(APPLICATION.Models.ARCHIVE.TFamille item);

        public async Task<APPLICATION.Models.ARCHIVE.TFamille> DeleteTFamille(int id)
        {
            var itemToDelete = Context.TFamilles
                              .Where(i => i.id == id)
                              .Include(i => i.TFamilles1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTFamilleDeleted(itemToDelete);


            Context.TFamilles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTFamilleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTSocietesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tsocietes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tsocietes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTSocietesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/archive/tsocietes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/archive/tsocietes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTSocietesRead(ref IQueryable<APPLICATION.Models.ARCHIVE.TSociete> items);

        public async Task<IQueryable<APPLICATION.Models.ARCHIVE.TSociete>> GetTSocietes(Query query = null)
        {
            var items = Context.TSocietes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTSocietesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTSocieteGet(APPLICATION.Models.ARCHIVE.TSociete item);
        partial void OnGetTSocieteById(ref IQueryable<APPLICATION.Models.ARCHIVE.TSociete> items);


        public async Task<APPLICATION.Models.ARCHIVE.TSociete> GetTSocieteById(int id)
        {
            var items = Context.TSocietes
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetTSocieteById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTSocieteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTSocieteCreated(APPLICATION.Models.ARCHIVE.TSociete item);
        partial void OnAfterTSocieteCreated(APPLICATION.Models.ARCHIVE.TSociete item);

        public async Task<APPLICATION.Models.ARCHIVE.TSociete> CreateTSociete(APPLICATION.Models.ARCHIVE.TSociete tsociete)
        {
            OnTSocieteCreated(tsociete);

            var existingItem = Context.TSocietes
                              .Where(i => i.id == tsociete.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TSocietes.Add(tsociete);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tsociete).State = EntityState.Detached;
                throw;
            }

            OnAfterTSocieteCreated(tsociete);

            return tsociete;
        }

        public async Task<APPLICATION.Models.ARCHIVE.TSociete> CancelTSocieteChanges(APPLICATION.Models.ARCHIVE.TSociete item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTSocieteUpdated(APPLICATION.Models.ARCHIVE.TSociete item);
        partial void OnAfterTSocieteUpdated(APPLICATION.Models.ARCHIVE.TSociete item);

        public async Task<APPLICATION.Models.ARCHIVE.TSociete> UpdateTSociete(int id, APPLICATION.Models.ARCHIVE.TSociete tsociete)
        {
            OnTSocieteUpdated(tsociete);

            var itemToUpdate = Context.TSocietes
                              .Where(i => i.id == tsociete.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tsociete);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTSocieteUpdated(tsociete);

            return tsociete;
        }

        partial void OnTSocieteDeleted(APPLICATION.Models.ARCHIVE.TSociete item);
        partial void OnAfterTSocieteDeleted(APPLICATION.Models.ARCHIVE.TSociete item);

        public async Task<APPLICATION.Models.ARCHIVE.TSociete> DeleteTSociete(int id)
        {
            var itemToDelete = Context.TSocietes
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTSocieteDeleted(itemToDelete);


            Context.TSocietes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTSocieteDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}
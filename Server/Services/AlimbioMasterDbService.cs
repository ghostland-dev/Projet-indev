using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Alimbio.Server.Data;

namespace Alimbio.Server
{
    public partial class alimbio_master_dbService
    {
        alimbio_master_dbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly alimbio_master_dbContext context;
        private readonly NavigationManager navigationManager;

        public alimbio_master_dbService(alimbio_master_dbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportSalariesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSalariesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSalariesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Salary> items);

        public async Task<IQueryable<Alimbio.Server.Models.alimbio_master_db.Salary>> GetSalaries(Query query = null)
        {
            var items = Context.Salaries.AsQueryable();


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

            OnSalariesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSalaryGet(Alimbio.Server.Models.alimbio_master_db.Salary item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> GetSalaryById(ulong id)
        {
            var items = Context.Salaries
                              .AsNoTracking()
                              .Where(i => i.id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnSalaryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSalaryCreated(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryCreated(Alimbio.Server.Models.alimbio_master_db.Salary item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> CreateSalary(Alimbio.Server.Models.alimbio_master_db.Salary salary)
        {
            OnSalaryCreated(salary);

            var existingItem = Context.Salaries
                              .Where(i => i.id == salary.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Salaries.Add(salary);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(salary).State = EntityState.Detached;
                throw;
            }

            OnAfterSalaryCreated(salary);

            return salary;
        }

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> CancelSalaryChanges(Alimbio.Server.Models.alimbio_master_db.Salary item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSalaryUpdated(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryUpdated(Alimbio.Server.Models.alimbio_master_db.Salary item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> UpdateSalary(ulong id, Alimbio.Server.Models.alimbio_master_db.Salary salary)
        {
            OnSalaryUpdated(salary);

            var itemToUpdate = Context.Salaries
                              .Where(i => i.id == salary.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(salary);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSalaryUpdated(salary);

            return salary;
        }

        partial void OnSalaryDeleted(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryDeleted(Alimbio.Server.Models.alimbio_master_db.Salary item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> DeleteSalary(ulong id)
        {
            var itemToDelete = Context.Salaries
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSalaryDeleted(itemToDelete);


            Context.Salaries.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSalaryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEntreprisesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/entreprises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/entreprises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEntreprisesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/entreprises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/entreprises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEntreprisesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Entreprise> items);

        public async Task<IQueryable<Alimbio.Server.Models.alimbio_master_db.Entreprise>> GetEntreprises(Query query = null)
        {
            var items = Context.Entreprises.AsQueryable();


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

            OnEntreprisesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEntrepriseGet(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> GetEntrepriseById(ulong id)
        {
            var items = Context.Entreprises
                              .AsNoTracking()
                              .Where(i => i.id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnEntrepriseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEntrepriseCreated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseCreated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> CreateEntreprise(Alimbio.Server.Models.alimbio_master_db.Entreprise entreprise)
        {
            OnEntrepriseCreated(entreprise);

            var existingItem = Context.Entreprises
                              .Where(i => i.id == entreprise.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Entreprises.Add(entreprise);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(entreprise).State = EntityState.Detached;
                throw;
            }

            OnAfterEntrepriseCreated(entreprise);

            return entreprise;
        }

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> CancelEntrepriseChanges(Alimbio.Server.Models.alimbio_master_db.Entreprise item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEntrepriseUpdated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseUpdated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> UpdateEntreprise(ulong id, Alimbio.Server.Models.alimbio_master_db.Entreprise entreprise)
        {
            OnEntrepriseUpdated(entreprise);

            var itemToUpdate = Context.Entreprises
                              .Where(i => i.id == entreprise.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(entreprise);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEntrepriseUpdated(entreprise);

            return entreprise;
        }

        partial void OnEntrepriseDeleted(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseDeleted(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> DeleteEntreprise(ulong id)
        {
            var itemToDelete = Context.Entreprises
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEntrepriseDeleted(itemToDelete);


            Context.Entreprises.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEntrepriseDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}
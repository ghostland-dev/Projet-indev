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
                              .Include(i => i.Sites)
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

            items = items.Include(i => i.Service);
            items = items.Include(i => i.Site);

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

            items = items.Include(i => i.Service);
            items = items.Include(i => i.Site);
  
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
    
        public async Task ExportServicesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/services/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/services/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportServicesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/services/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/services/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnServicesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Service> items);

        public async Task<IQueryable<Alimbio.Server.Models.alimbio_master_db.Service>> GetServices(Query query = null)
        {
            var items = Context.Services.AsQueryable();


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

            OnServicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnServiceGet(Alimbio.Server.Models.alimbio_master_db.Service item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Service> GetServiceById(ulong id)
        {
            var items = Context.Services
                              .AsNoTracking()
                              .Where(i => i.id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnServiceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnServiceCreated(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceCreated(Alimbio.Server.Models.alimbio_master_db.Service item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Service> CreateService(Alimbio.Server.Models.alimbio_master_db.Service service)
        {
            OnServiceCreated(service);

            var existingItem = Context.Services
                              .Where(i => i.id == service.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Services.Add(service);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(service).State = EntityState.Detached;
                throw;
            }

            OnAfterServiceCreated(service);

            return service;
        }

        public async Task<Alimbio.Server.Models.alimbio_master_db.Service> CancelServiceChanges(Alimbio.Server.Models.alimbio_master_db.Service item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnServiceUpdated(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceUpdated(Alimbio.Server.Models.alimbio_master_db.Service item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Service> UpdateService(ulong id, Alimbio.Server.Models.alimbio_master_db.Service service)
        {
            OnServiceUpdated(service);

            var itemToUpdate = Context.Services
                              .Where(i => i.id == service.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(service);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterServiceUpdated(service);

            return service;
        }

        partial void OnServiceDeleted(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceDeleted(Alimbio.Server.Models.alimbio_master_db.Service item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Service> DeleteService(ulong id)
        {
            var itemToDelete = Context.Services
                              .Where(i => i.id == id)
                              .Include(i => i.Salaries)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnServiceDeleted(itemToDelete);


            Context.Services.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterServiceDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSitesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/sites/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/sites/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSitesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/sites/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/sites/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSitesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Site> items);

        public async Task<IQueryable<Alimbio.Server.Models.alimbio_master_db.Site>> GetSites(Query query = null)
        {
            var items = Context.Sites.AsQueryable();

            items = items.Include(i => i.Entreprise);
            items = items.Include(i => i.Ville);

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

            OnSitesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSiteGet(Alimbio.Server.Models.alimbio_master_db.Site item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Site> GetSiteById(ulong id)
        {
            var items = Context.Sites
                              .AsNoTracking()
                              .Where(i => i.id == id);

            items = items.Include(i => i.Entreprise);
            items = items.Include(i => i.Ville);
  
            var itemToReturn = items.FirstOrDefault();

            OnSiteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSiteCreated(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteCreated(Alimbio.Server.Models.alimbio_master_db.Site item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Site> CreateSite(Alimbio.Server.Models.alimbio_master_db.Site site)
        {
            OnSiteCreated(site);

            var existingItem = Context.Sites
                              .Where(i => i.id == site.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Sites.Add(site);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(site).State = EntityState.Detached;
                throw;
            }

            OnAfterSiteCreated(site);

            return site;
        }

        public async Task<Alimbio.Server.Models.alimbio_master_db.Site> CancelSiteChanges(Alimbio.Server.Models.alimbio_master_db.Site item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSiteUpdated(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteUpdated(Alimbio.Server.Models.alimbio_master_db.Site item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Site> UpdateSite(ulong id, Alimbio.Server.Models.alimbio_master_db.Site site)
        {
            OnSiteUpdated(site);

            var itemToUpdate = Context.Sites
                              .Where(i => i.id == site.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(site);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSiteUpdated(site);

            return site;
        }

        partial void OnSiteDeleted(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteDeleted(Alimbio.Server.Models.alimbio_master_db.Site item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Site> DeleteSite(ulong id)
        {
            var itemToDelete = Context.Sites
                              .Where(i => i.id == id)
                              .Include(i => i.Salaries)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSiteDeleted(itemToDelete);


            Context.Sites.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSiteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVillesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/villes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/villes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVillesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/villes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/villes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVillesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Ville> items);

        public async Task<IQueryable<Alimbio.Server.Models.alimbio_master_db.Ville>> GetVilles(Query query = null)
        {
            var items = Context.Villes.AsQueryable();


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

            OnVillesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVilleGet(Alimbio.Server.Models.alimbio_master_db.Ville item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Ville> GetVilleById(ulong id)
        {
            var items = Context.Villes
                              .AsNoTracking()
                              .Where(i => i.id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnVilleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVilleCreated(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleCreated(Alimbio.Server.Models.alimbio_master_db.Ville item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Ville> CreateVille(Alimbio.Server.Models.alimbio_master_db.Ville ville)
        {
            OnVilleCreated(ville);

            var existingItem = Context.Villes
                              .Where(i => i.id == ville.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Villes.Add(ville);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ville).State = EntityState.Detached;
                throw;
            }

            OnAfterVilleCreated(ville);

            return ville;
        }

        public async Task<Alimbio.Server.Models.alimbio_master_db.Ville> CancelVilleChanges(Alimbio.Server.Models.alimbio_master_db.Ville item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVilleUpdated(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleUpdated(Alimbio.Server.Models.alimbio_master_db.Ville item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Ville> UpdateVille(ulong id, Alimbio.Server.Models.alimbio_master_db.Ville ville)
        {
            OnVilleUpdated(ville);

            var itemToUpdate = Context.Villes
                              .Where(i => i.id == ville.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(ville);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVilleUpdated(ville);

            return ville;
        }

        partial void OnVilleDeleted(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleDeleted(Alimbio.Server.Models.alimbio_master_db.Ville item);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Ville> DeleteVille(ulong id)
        {
            var itemToDelete = Context.Villes
                              .Where(i => i.id == id)
                              .Include(i => i.Sites)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVilleDeleted(itemToDelete);


            Context.Villes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVilleDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}
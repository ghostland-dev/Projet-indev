using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Alimbio.Server.Controllers.alimbio_master_db
{
    [Route("odata/alimbio_master_db/Sites")]
    public partial class SitesController : ODataController
    {
        private Alimbio.Server.Data.alimbio_master_dbContext context;

        public SitesController(Alimbio.Server.Data.alimbio_master_dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Alimbio.Server.Models.alimbio_master_db.Site> GetSites()
        {
            var items = this.context.Sites.AsQueryable<Alimbio.Server.Models.alimbio_master_db.Site>();
            this.OnSitesRead(ref items);

            return items;
        }

        partial void OnSitesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Site> items);

        partial void OnSiteGet(ref SingleResult<Alimbio.Server.Models.alimbio_master_db.Site> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/alimbio_master_db/Sites(id={id})")]
        public SingleResult<Alimbio.Server.Models.alimbio_master_db.Site> GetSite(ulong key)
        {
            var items = this.context.Sites.Where(i => i.id == key);
            var result = SingleResult.Create(items);

            OnSiteGet(ref result);

            return result;
        }
        partial void OnSiteDeleted(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteDeleted(Alimbio.Server.Models.alimbio_master_db.Site item);

        [HttpDelete("/odata/alimbio_master_db/Sites(id={id})")]
        public IActionResult DeleteSite(ulong key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Sites
                    .Where(i => i.id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnSiteDeleted(item);
                this.context.Sites.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSiteDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSiteUpdated(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteUpdated(Alimbio.Server.Models.alimbio_master_db.Site item);

        [HttpPut("/odata/alimbio_master_db/Sites(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSite(ulong key, [FromBody]Alimbio.Server.Models.alimbio_master_db.Site item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.id != key))
                {
                    return BadRequest();
                }
                this.OnSiteUpdated(item);
                this.context.Sites.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sites.Where(i => i.id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Entreprise,Ville");
                this.OnAfterSiteUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/alimbio_master_db/Sites(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSite(ulong key, [FromBody]Delta<Alimbio.Server.Models.alimbio_master_db.Site> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Sites.Where(i => i.id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnSiteUpdated(item);
                this.context.Sites.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sites.Where(i => i.id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Entreprise,Ville");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSiteCreated(Alimbio.Server.Models.alimbio_master_db.Site item);
        partial void OnAfterSiteCreated(Alimbio.Server.Models.alimbio_master_db.Site item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Alimbio.Server.Models.alimbio_master_db.Site item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnSiteCreated(item);
                this.context.Sites.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sites.Where(i => i.id == item.id);

                Request.QueryString = Request.QueryString.Add("$expand", "Entreprise,Ville");

                this.OnAfterSiteCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}

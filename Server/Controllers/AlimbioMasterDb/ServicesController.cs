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
    [Route("odata/alimbio_master_db/Services")]
    public partial class ServicesController : ODataController
    {
        private Alimbio.Server.Data.alimbio_master_dbContext context;

        public ServicesController(Alimbio.Server.Data.alimbio_master_dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Alimbio.Server.Models.alimbio_master_db.Service> GetServices()
        {
            var items = this.context.Services.AsQueryable<Alimbio.Server.Models.alimbio_master_db.Service>();
            this.OnServicesRead(ref items);

            return items;
        }

        partial void OnServicesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Service> items);

        partial void OnServiceGet(ref SingleResult<Alimbio.Server.Models.alimbio_master_db.Service> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/alimbio_master_db/Services(id={id})")]
        public SingleResult<Alimbio.Server.Models.alimbio_master_db.Service> GetService(ulong key)
        {
            var items = this.context.Services.Where(i => i.id == key);
            var result = SingleResult.Create(items);

            OnServiceGet(ref result);

            return result;
        }
        partial void OnServiceDeleted(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceDeleted(Alimbio.Server.Models.alimbio_master_db.Service item);

        [HttpDelete("/odata/alimbio_master_db/Services(id={id})")]
        public IActionResult DeleteService(ulong key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Services
                    .Where(i => i.id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnServiceDeleted(item);
                this.context.Services.Remove(item);
                this.context.SaveChanges();
                this.OnAfterServiceDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnServiceUpdated(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceUpdated(Alimbio.Server.Models.alimbio_master_db.Service item);

        [HttpPut("/odata/alimbio_master_db/Services(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutService(ulong key, [FromBody]Alimbio.Server.Models.alimbio_master_db.Service item)
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
                this.OnServiceUpdated(item);
                this.context.Services.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Services.Where(i => i.id == key);
                ;
                this.OnAfterServiceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/alimbio_master_db/Services(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchService(ulong key, [FromBody]Delta<Alimbio.Server.Models.alimbio_master_db.Service> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Services.Where(i => i.id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnServiceUpdated(item);
                this.context.Services.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Services.Where(i => i.id == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnServiceCreated(Alimbio.Server.Models.alimbio_master_db.Service item);
        partial void OnAfterServiceCreated(Alimbio.Server.Models.alimbio_master_db.Service item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Alimbio.Server.Models.alimbio_master_db.Service item)
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

                this.OnServiceCreated(item);
                this.context.Services.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Services.Where(i => i.id == item.id);

                ;

                this.OnAfterServiceCreated(item);

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

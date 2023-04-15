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
    [Route("odata/alimbio_master_db/Villes")]
    public partial class VillesController : ODataController
    {
        private Alimbio.Server.Data.alimbio_master_dbContext context;

        public VillesController(Alimbio.Server.Data.alimbio_master_dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Alimbio.Server.Models.alimbio_master_db.Ville> GetVilles()
        {
            var items = this.context.Villes.AsQueryable<Alimbio.Server.Models.alimbio_master_db.Ville>();
            this.OnVillesRead(ref items);

            return items;
        }

        partial void OnVillesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Ville> items);

        partial void OnVilleGet(ref SingleResult<Alimbio.Server.Models.alimbio_master_db.Ville> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/alimbio_master_db/Villes(id={id})")]
        public SingleResult<Alimbio.Server.Models.alimbio_master_db.Ville> GetVille(ulong key)
        {
            var items = this.context.Villes.Where(i => i.id == key);
            var result = SingleResult.Create(items);

            OnVilleGet(ref result);

            return result;
        }
        partial void OnVilleDeleted(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleDeleted(Alimbio.Server.Models.alimbio_master_db.Ville item);

        [HttpDelete("/odata/alimbio_master_db/Villes(id={id})")]
        public IActionResult DeleteVille(ulong key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Villes
                    .Where(i => i.id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnVilleDeleted(item);
                this.context.Villes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterVilleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVilleUpdated(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleUpdated(Alimbio.Server.Models.alimbio_master_db.Ville item);

        [HttpPut("/odata/alimbio_master_db/Villes(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutVille(ulong key, [FromBody]Alimbio.Server.Models.alimbio_master_db.Ville item)
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
                this.OnVilleUpdated(item);
                this.context.Villes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Villes.Where(i => i.id == key);
                ;
                this.OnAfterVilleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/alimbio_master_db/Villes(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchVille(ulong key, [FromBody]Delta<Alimbio.Server.Models.alimbio_master_db.Ville> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Villes.Where(i => i.id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnVilleUpdated(item);
                this.context.Villes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Villes.Where(i => i.id == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVilleCreated(Alimbio.Server.Models.alimbio_master_db.Ville item);
        partial void OnAfterVilleCreated(Alimbio.Server.Models.alimbio_master_db.Ville item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Alimbio.Server.Models.alimbio_master_db.Ville item)
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

                this.OnVilleCreated(item);
                this.context.Villes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Villes.Where(i => i.id == item.id);

                ;

                this.OnAfterVilleCreated(item);

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

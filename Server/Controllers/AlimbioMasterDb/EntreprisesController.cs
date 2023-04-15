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
    [Route("odata/alimbio_master_db/Entreprises")]
    public partial class EntreprisesController : ODataController
    {
        private Alimbio.Server.Data.alimbio_master_dbContext context;

        public EntreprisesController(Alimbio.Server.Data.alimbio_master_dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Alimbio.Server.Models.alimbio_master_db.Entreprise> GetEntreprises()
        {
            var items = this.context.Entreprises.AsQueryable<Alimbio.Server.Models.alimbio_master_db.Entreprise>();
            this.OnEntreprisesRead(ref items);

            return items;
        }

        partial void OnEntreprisesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Entreprise> items);

        partial void OnEntrepriseGet(ref SingleResult<Alimbio.Server.Models.alimbio_master_db.Entreprise> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/alimbio_master_db/Entreprises(id={id})")]
        public SingleResult<Alimbio.Server.Models.alimbio_master_db.Entreprise> GetEntreprise(ulong key)
        {
            var items = this.context.Entreprises.Where(i => i.id == key);
            var result = SingleResult.Create(items);

            OnEntrepriseGet(ref result);

            return result;
        }
        partial void OnEntrepriseDeleted(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseDeleted(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        [HttpDelete("/odata/alimbio_master_db/Entreprises(id={id})")]
        public IActionResult DeleteEntreprise(ulong key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Entreprises
                    .Where(i => i.id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnEntrepriseDeleted(item);
                this.context.Entreprises.Remove(item);
                this.context.SaveChanges();
                this.OnAfterEntrepriseDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEntrepriseUpdated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseUpdated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        [HttpPut("/odata/alimbio_master_db/Entreprises(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutEntreprise(ulong key, [FromBody]Alimbio.Server.Models.alimbio_master_db.Entreprise item)
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
                this.OnEntrepriseUpdated(item);
                this.context.Entreprises.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Entreprises.Where(i => i.id == key);
                ;
                this.OnAfterEntrepriseUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/alimbio_master_db/Entreprises(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchEntreprise(ulong key, [FromBody]Delta<Alimbio.Server.Models.alimbio_master_db.Entreprise> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Entreprises.Where(i => i.id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnEntrepriseUpdated(item);
                this.context.Entreprises.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Entreprises.Where(i => i.id == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEntrepriseCreated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);
        partial void OnAfterEntrepriseCreated(Alimbio.Server.Models.alimbio_master_db.Entreprise item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Alimbio.Server.Models.alimbio_master_db.Entreprise item)
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

                this.OnEntrepriseCreated(item);
                this.context.Entreprises.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Entreprises.Where(i => i.id == item.id);

                ;

                this.OnAfterEntrepriseCreated(item);

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

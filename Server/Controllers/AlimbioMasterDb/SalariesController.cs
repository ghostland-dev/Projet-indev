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
    [Route("odata/alimbio_master_db/Salaries")]
    public partial class SalariesController : ODataController
    {
        private Alimbio.Server.Data.alimbio_master_dbContext context;

        public SalariesController(Alimbio.Server.Data.alimbio_master_dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Alimbio.Server.Models.alimbio_master_db.Salary> GetSalaries()
        {
            var items = this.context.Salaries.AsQueryable<Alimbio.Server.Models.alimbio_master_db.Salary>();
            this.OnSalariesRead(ref items);

            return items;
        }

        partial void OnSalariesRead(ref IQueryable<Alimbio.Server.Models.alimbio_master_db.Salary> items);

        partial void OnSalaryGet(ref SingleResult<Alimbio.Server.Models.alimbio_master_db.Salary> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/alimbio_master_db/Salaries(id={id})")]
        public SingleResult<Alimbio.Server.Models.alimbio_master_db.Salary> GetSalary(ulong key)
        {
            var items = this.context.Salaries.Where(i => i.id == key);
            var result = SingleResult.Create(items);

            OnSalaryGet(ref result);

            return result;
        }
        partial void OnSalaryDeleted(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryDeleted(Alimbio.Server.Models.alimbio_master_db.Salary item);

        [HttpDelete("/odata/alimbio_master_db/Salaries(id={id})")]
        public IActionResult DeleteSalary(ulong key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Salaries
                    .Where(i => i.id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnSalaryDeleted(item);
                this.context.Salaries.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSalaryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSalaryUpdated(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryUpdated(Alimbio.Server.Models.alimbio_master_db.Salary item);

        [HttpPut("/odata/alimbio_master_db/Salaries(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSalary(ulong key, [FromBody]Alimbio.Server.Models.alimbio_master_db.Salary item)
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
                this.OnSalaryUpdated(item);
                this.context.Salaries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Salaries.Where(i => i.id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Service,Site");
                this.OnAfterSalaryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/alimbio_master_db/Salaries(id={id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSalary(ulong key, [FromBody]Delta<Alimbio.Server.Models.alimbio_master_db.Salary> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Salaries.Where(i => i.id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnSalaryUpdated(item);
                this.context.Salaries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Salaries.Where(i => i.id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Service,Site");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSalaryCreated(Alimbio.Server.Models.alimbio_master_db.Salary item);
        partial void OnAfterSalaryCreated(Alimbio.Server.Models.alimbio_master_db.Salary item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Alimbio.Server.Models.alimbio_master_db.Salary item)
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

                this.OnSalaryCreated(item);
                this.context.Salaries.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Salaries.Where(i => i.id == item.id);

                Request.QueryString = Request.QueryString.Add("$expand", "Service,Site");

                this.OnAfterSalaryCreated(item);

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

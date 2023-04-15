using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Alimbio.Server.Data;

namespace Alimbio.Server.Controllers
{
    public partial class Exportalimbio_master_dbController : ExportController
    {
        private readonly alimbio_master_dbContext context;
        private readonly alimbio_master_dbService service;

        public Exportalimbio_master_dbController(alimbio_master_dbContext context, alimbio_master_dbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/alimbio_master_db/salaries/csv")]
        [HttpGet("/export/alimbio_master_db/salaries/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSalariesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSalaries(), Request.Query), fileName);
        }

        [HttpGet("/export/alimbio_master_db/salaries/excel")]
        [HttpGet("/export/alimbio_master_db/salaries/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSalariesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSalaries(), Request.Query), fileName);
        }

        [HttpGet("/export/alimbio_master_db/entreprises/csv")]
        [HttpGet("/export/alimbio_master_db/entreprises/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEntreprisesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEntreprises(), Request.Query), fileName);
        }

        [HttpGet("/export/alimbio_master_db/entreprises/excel")]
        [HttpGet("/export/alimbio_master_db/entreprises/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEntreprisesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEntreprises(), Request.Query), fileName);
        }
    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using APPLICATION.Data;

namespace APPLICATION.Controllers
{
    public partial class ExportARCHIVEController : ExportController
    {
        private readonly ARCHIVEContext context;
        private readonly ARCHIVEService service;

        public ExportARCHIVEController(ARCHIVEContext context, ARCHIVEService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ARCHIVE/tdocuments/csv")]
        [HttpGet("/export/ARCHIVE/tdocuments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTDocumentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTDocuments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ARCHIVE/tdocuments/excel")]
        [HttpGet("/export/ARCHIVE/tdocuments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTDocumentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTDocuments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ARCHIVE/tfamilles/csv")]
        [HttpGet("/export/ARCHIVE/tfamilles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTFamillesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTFamilles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ARCHIVE/tfamilles/excel")]
        [HttpGet("/export/ARCHIVE/tfamilles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTFamillesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTFamilles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ARCHIVE/tsocietes/csv")]
        [HttpGet("/export/ARCHIVE/tsocietes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTSocietesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTSocietes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ARCHIVE/tsocietes/excel")]
        [HttpGet("/export/ARCHIVE/tsocietes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTSocietesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTSocietes(), Request.Query, false), fileName);
        }
    }
}

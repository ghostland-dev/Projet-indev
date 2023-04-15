
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace Alimbio.Client
{
    public partial class alimbio_master_dbService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public alimbio_master_dbService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/alimbio_master_db/");
        }


        public async System.Threading.Tasks.Task ExportSalariesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSalariesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSalaries(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Salary>> GetSalaries(Query query)
        {
            return await GetSalaries(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Salary>> GetSalaries(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Salaries");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSalaries(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Salary>>(response);
        }

        partial void OnCreateSalary(HttpRequestMessage requestMessage);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> CreateSalary(Alimbio.Server.Models.alimbio_master_db.Salary salary = default(Alimbio.Server.Models.alimbio_master_db.Salary))
        {
            var uri = new Uri(baseUri, $"Salaries");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(salary), Encoding.UTF8, "application/json");

            OnCreateSalary(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Alimbio.Server.Models.alimbio_master_db.Salary>(response);
        }

        partial void OnDeleteSalary(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSalary(ulong id = default(ulong))
        {
            var uri = new Uri(baseUri, $"Salaries({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSalary(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSalaryById(HttpRequestMessage requestMessage);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Salary> GetSalaryById(string expand = default(string), ulong id = default(ulong))
        {
            var uri = new Uri(baseUri, $"Salaries({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSalaryById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Alimbio.Server.Models.alimbio_master_db.Salary>(response);
        }

        partial void OnUpdateSalary(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSalary(ulong id = default(ulong), Alimbio.Server.Models.alimbio_master_db.Salary salary = default(Alimbio.Server.Models.alimbio_master_db.Salary))
        {
            var uri = new Uri(baseUri, $"Salaries({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(salary), Encoding.UTF8, "application/json");

            OnUpdateSalary(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportEntreprisesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/entreprises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/entreprises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportEntreprisesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/alimbio_master_db/entreprises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/alimbio_master_db/entreprises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetEntreprises(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Entreprise>> GetEntreprises(Query query)
        {
            return await GetEntreprises(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Entreprise>> GetEntreprises(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Entreprises");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEntreprises(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Alimbio.Server.Models.alimbio_master_db.Entreprise>>(response);
        }

        partial void OnCreateEntreprise(HttpRequestMessage requestMessage);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> CreateEntreprise(Alimbio.Server.Models.alimbio_master_db.Entreprise entreprise = default(Alimbio.Server.Models.alimbio_master_db.Entreprise))
        {
            var uri = new Uri(baseUri, $"Entreprises");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(entreprise), Encoding.UTF8, "application/json");

            OnCreateEntreprise(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Alimbio.Server.Models.alimbio_master_db.Entreprise>(response);
        }

        partial void OnDeleteEntreprise(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteEntreprise(ulong id = default(ulong))
        {
            var uri = new Uri(baseUri, $"Entreprises({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteEntreprise(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetEntrepriseById(HttpRequestMessage requestMessage);

        public async Task<Alimbio.Server.Models.alimbio_master_db.Entreprise> GetEntrepriseById(string expand = default(string), ulong id = default(ulong))
        {
            var uri = new Uri(baseUri, $"Entreprises({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEntrepriseById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Alimbio.Server.Models.alimbio_master_db.Entreprise>(response);
        }

        partial void OnUpdateEntreprise(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateEntreprise(ulong id = default(ulong), Alimbio.Server.Models.alimbio_master_db.Entreprise entreprise = default(Alimbio.Server.Models.alimbio_master_db.Entreprise))
        {
            var uri = new Uri(baseUri, $"Entreprises({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(entreprise), Encoding.UTF8, "application/json");

            OnUpdateEntreprise(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
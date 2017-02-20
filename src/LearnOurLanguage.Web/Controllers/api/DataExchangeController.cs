using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Const;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    /// <summary>
    /// kontroler odpowiedzialny za wymianę danych słowników
    /// </summary>
    public class DataExchangeController : BaseController
    {
        private IHostingEnvironment HostingEnvironment { get; }
        private IDictionariesRepository DictionariesRepository { get; }
        private IDataExchangeService DataExchangeService { get; }

        /// <summary>
        /// konstruktor wstrzykujący zależności
        /// </summary>
        /// <param name="dataExchangeService"></param>
        /// <param name="dictionariesRepository"></param>
        /// <param name="hostingEnvironment"></param>
        public DataExchangeController(IDataExchangeService dataExchangeService, IDictionariesRepository dictionariesRepository, IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            DataExchangeService = dataExchangeService;
            DictionariesRepository = dictionariesRepository;
        }

        /// <summary>
        /// eksportowanie słownika do pliku csv
        /// </summary>
        /// <returns></returns>
        [HttpGet("Export/{dictionaryId}")]
        public FileResult Export(int dictionaryId)
        {
            var dictionary = DictionariesRepository.GetById(dictionaryId);
            if (dictionary.Public)
            {
                AccessGuardian(Roles.AccessUser);
            }
            else
            {
                AccessGuardian(Roles.AccessUser, dictionary.UserId);
            }

            var stream = DataExchangeService.ExportDictionary(dictionaryId);

            /*
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"slownik_{dictionaryId}_{DateTime.Today}.csv"
                };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;*/

            Context.Response.ContentType = "application/octet-stream";
            var result = new FileContentResult(stream.ToArray(), "application/octet-stream")
            {
                FileDownloadName = $"eksport_slownik_{dictionaryId}_{DateTime.Now:dd-MM-yyyy}.csv"
            };

            return result;

        }

        /// <summary>
        /// import słownika z pliku csv
        /// </summary>
        /// <returns></returns>
        [HttpPost("Import/{dictionaryId}")]
        public void Import(int dictionaryId)
        {
            if (Context.Request.Form != null && Context.Request.Form.Files.Count > 0)
            {
                var file = Context.Request.Form.Files.First();
                var stream = new MemoryStream();
                file.CopyTo(stream);
                DataExchangeService.ImportDictionary(stream, dictionaryId);
            }

            Context.Response.Redirect($"/user/dictionaries/{dictionaryId}");
        }
    }
}

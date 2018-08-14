using Hihapanesu.Web.Models.RequestModels;
using Hihapanesu.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Hihapanesu.Web.Controllers
{
    [RoutePrefix("api/translate")]
    public class TranslateController : ApiController
    {
        //[HttpPost]
        //public HttpResponseMessage Transcribe([FromBody]string text)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPost]
        //public HttpResponseMessage Generate([FromBody]string text)
        //{
        //    throw new NotImplementedException();
        //}

        [Route("TranscribeAndGenerate")]
        [HttpPost]
        public HttpResponseMessage TranscribeAndGenerate(TranscribeAndGenerateModel model)
        {
            var service = new ElvishTranslationService("elvishSymbols.svg");
            var result = service.TranscribeAndGenerate(model.Text, model.IsTest);
            //var 
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content.
            using (var device = new Kean.IO.Chara
            var resultString = result.ToString();
            return 
        }
    }
}

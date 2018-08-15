using Hihapanesu.Web.Models.RequestModels;
using Hihapanesu.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> TranscribeAndGenerate(TranscribeAndGenerateModel model)
        {
            var service = new ElvishTranslationService("elvishSymbols.svg");
            var result = service.TranscribeAndGenerate(model.Text, model.IsTest);
            var bigData = new List<byte>();
            var data = new byte[1024 * 1024];
            //byte[] data;
            var stringContent = "";
            var cancellationToken = new System.Threading.CancellationToken();
            using (var stream = new System.IO.MemoryStream(data))
            {
                //using (var device = Kean.IO.ByteDevice.Open(stream))
                using (var device = Kean.IO.CharacterDevice.Open(stream))
                {
                    //var locatorDevice = 
                    using (var writer = Kean.IO.CharacterWriter.Open(device))
                    {

                        result.Save(writer);
                        //stringContent = result.ToString();

                        //var reader = new StreamReader(stream);
                        //stringContent = reader.ReadToEnd();
                        //    var buffer = new byte[stream.Length];
                        //stream.Read(buffer, 0, (int)stream.Length);

                        var lastChars = "";
                        var endOfFile = "</xml>";
                        var nullCount = 0;
                        foreach(var c in data)
                        {
                            if (lastChars.Length == 4)
                            {
                                lastChars.Remove(lastChars.Length - 1, 1);
                                lastChars = c + lastChars;
                            }
                            if (c == '\0')
                                break;
                            if (lastChars == endOfFile)
                                break;
                            stringContent += (char)c;
                        }
                        stringContent += endOfFile;
                    }
                }
            }
            //HttpContent bytesContent = new ByteArrayContent(byteData);
            //using (var formData = new MultipartFormDataContent())
            return Request.CreateResponse(HttpStatusCode.OK, stringContent);
            //    var response = this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK));
            //response.Response.
            //    using (var device = new Kean.IO.Chara
            //var resultString = result.ToString();
            //return 
        }
    }
}

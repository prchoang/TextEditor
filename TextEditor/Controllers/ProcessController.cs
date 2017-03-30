﻿using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TextEditor.Extends.Library;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    [RoutePrefix("api/process")]
    public class ProcessController : ApiController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<HttpResponseMessage> UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsByteArrayAsync();
                    // use the data stream to persist the data to the server (file system etc)
                    Stream stream = new MemoryStream(dataStream);

                    using (var doc = DocX.Load(stream))
                    {
                        var convert = new Extends.Library.Convert();
                        var ml = convert.PaperClipsToCentimeters(doc.MarginLeft);
                        var view = new DocxView();

                        view.PageLayout.Margin.Top = doc.MarginTop;
                        view.PageLayout.Margin.Bottom = doc.MarginBottom;
                        view.PageLayout.Margin.Left = doc.MarginLeft;
                        view.PageLayout.Margin.Right = doc.MarginRight;

                        view.PageLayout.Size.Width = doc.PageWidth;
                        view.PageLayout.Size.Height = doc.PageHeight;
                        

                        var pps = new Models.PaperSize();
                        view.PageLayout.Size.PaperType = pps.PaperType(view.PageLayout.Size);

                        MemoryStream ms = new MemoryStream();

                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DocxView));

                        ser.WriteObject(ms, view);

                        string json = Encoding.Default.GetString(ms.ToArray());

                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(json, Encoding.UTF8, "text/plain");
                        response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            return null;
        }
    }
}

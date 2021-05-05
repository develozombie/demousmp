using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Company.Function
{
    public static class RegistrarFunction
    {
        [FunctionName("RegistrarFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string url = "";
            log.LogInformation("C# HTTP trigger function processed a request.");

            string imagen = req.Query["imagen"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            imagen = imagen ?? data?.imagen;

            using (var client = new HttpClient())
            {
                Img img = new Img
                {
                    imagen = imagen
                };
                var uri = new Uri(Environment.GetEnvironmentVariable("logic_apps_uri"));
                var imgJSON = JsonConvert.SerializeObject(img);
                var buffer = System.Text.Encoding.UTF8.GetBytes(imgJSON);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(uri, byteContent);
                url = await response.Content.ReadAsStringAsync();
                log.LogInformation(url);
            }
            return new OkObjectResult(url);
        }
    }

    public class Img
    {
        public string imagen { get; set; }
        public string descripcion { get; set; }
    }
}

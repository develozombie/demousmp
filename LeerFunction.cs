using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace Company.Function
{
    public static class LeerFunction
    {

        [FunctionName("LeerFunction")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [CosmosDB(databaseName: "demo", collectionName: "posts", SqlQuery = "SELECT * FROM posts", ConnectionStringSetting = "cosmos_conexion")] IEnumerable<Img> img,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(img);
        }
    }
}

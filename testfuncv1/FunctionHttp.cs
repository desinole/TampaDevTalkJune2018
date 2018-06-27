using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace testfuncv1
{
    public static class FunctionHttp
    {
        [FunctionName("FunctionHttp")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
             [DocumentDB(
            databaseName:"salesdb",
            collectionName:"salescollection",
            CreateIfNotExists = true,
            CollectionThroughput = 1000,
            PartitionKey = "/sku",
            ConnectionStringSetting = "cosmosConnectionString")] IAsyncCollector<dynamic> outputDocument,
           TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            await outputDocument.AddAsync(data);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

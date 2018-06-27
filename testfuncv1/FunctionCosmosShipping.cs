using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace testfuncv1
{
    public static class FunctionCosmosShipping
    {
        [FunctionName("FunctionCosmosShipping")]
        public static void Run([CosmosDBTrigger(
            databaseName: "salesdb",
            collectionName: "salescollection",
            ConnectionStringSetting = "cosmosConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "salesleases")]IReadOnlyList<Document> documents, 
            TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                foreach (var document in documents)
                {
                    if (((dynamic)document).requires_shipping)
                        log.Warning("Invoked shipping function");
                    else
                        log.Warning("No shipping!!!!");
                }
            }
        }
    }
}

using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace testfuncv1
{
    public static class FunctionCosmosTax
    {
        [FunctionName("FunctionCosmosTax")]
        public static void Run([CosmosDBTrigger(
            databaseName: "salesdb",
            collectionName: "salescollection",
            ConnectionStringSetting = "cosmosConnectionString",
             CreateLeaseCollectionIfNotExists = true,
           LeaseCollectionName = "taxleases")]IReadOnlyList<Document> documents, TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                foreach (var document in documents)
                {
                    if (((dynamic)document).taxable)
                    {
                        var tax = ((dynamic)document).price * 0.07;
                        log.Warning($"Calculated tax {tax}");
                    }
                    else
                        log.Warning($"No tax!!!");

                }
            }
        }
    }
}

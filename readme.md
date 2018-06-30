# Local coding instructions

- Make sure Azure Storage emulator is running

- Make sure CosmosDB emulator is running

- Create function with v1 and empty

- Add Nuget Package Microsoft.Azure.WebJobs.Extensions.DocumentDB v1.2.0

- Copy-paste in local settings file (where do you get Cosmos Connection string)

'''csharP
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "cosmosConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
  }
}
'''

- Add HTTP trigger

- Run -> Copy paste url in POSTMAN

- Walk through code and add message with "name"

- Show incoming data - one for CD and one for downloadable version

- Delete all code and add DocumentDB binding

'''csharp
[DocumentDB(databaseName:"salesdb",collectionName:"salescollection",CreateIfNotExists = true,CollectionThroughput = 1000,PartitionKey = "/sku",ConnectionStringSetting = "cosmosConnectionString")] IAsyncCollector<dynamic> outputDocument,
'''
- Add this code to body

'''csharp
            dynamic data = await req.Content.ReadAsAsync<object>();
            await outputDocument.AddAsync(data);
            return new HttpResponseMessage(HttpStatusCode.OK);
'''

- Run with both examples of different schemes and show saved data

- Add Cosmos trigger function with salesdb, salescollection and cosmosConnectionString

- Add code
'''csharp
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
'''

- Run, will fail, set leasecollection to autocreate and run again

- Post both orders, one will require shipping, other won't

- Add another function with cosmos trigger salesdb, salescollection and cosmosConnectionString

- Add code

'''csharp
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
'''

- Run, will fail, set leasecollection to autocreate and run again

- Only one will fire. Why? Show lease diagram where hosts and lease database interact

- create separate lease database for each and autocreatE

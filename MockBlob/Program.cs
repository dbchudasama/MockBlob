using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Adding directives
using Microsoft.Azure; //Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; //Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; //Namespace for Blob storage types	
using Newtonsoft.Json;
using System.IO;
using Microsoft.Azure.EventHubs;
//using System.Threading;
using System.Timers;
using System.Threading;

namespace MockBlob
{
	class Program
	{
		private static EventHubClient eventHubClient;
		private const string EhConnectionString = "Endpoint=sb://mockprojecteventhub2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uud9P5fYfkSvqhV0+Y/zEjBNDmjjhUqHQdQKK1lsEKk=";
		private const string EhEntityPath = "mockprojecteventhubdemo2";


		static void Main(string[] args)
		{
			//Parse the connection string and return a reference to the storage account
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
			//Creating a CloudBlobClient to retrieve the container and blob storage
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			//Retrieve reference to the created container 'input'
			CloudBlobContainer container = blobClient.GetContainerReference("input");
			//Retrieve reference to the file arilines[2].json inside the container
			CloudBlockBlob blockBob = container.GetBlockBlobReference("airlines.json");
			//Save blob contents to a file (Download)
			using (var fileStream = System.IO.File.OpenWrite(@"./blobDownload.json"))
			{
				blockBob.DownloadToStream(fileStream);
			}

			//Calling method to do the message processing to Event Hub
			MainAsync(args).GetAwaiter().GetResult();
		}


		private static async Task MainAsync(string[] args)
		{
			// Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
			// Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
			// we are using the connection string from the namespace.
			var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
			{
				EntityPath = EhEntityPath
			};

			eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
			//Using external package Newtonsoft.json to read in the downloaded blob file and then deserialize the JSON as a list of type x
			//Read File into List and Deserialise JSON to type 'Airline'
			var airline = JsonConvert.DeserializeObject<List<Airline>>(File.ReadAllText(@"./blobDownload.json"));
			 
			//Calling Method to send messages to Event Hub
			await SendMessagesToEventHub(airline);

			//Closing connection
			await eventHubClient.CloseAsync();

			Console.WriteLine("Press ENTER to exit");
			//Keep Console open till user input
			Console.ReadLine();

		}


		//Creates an Event Hub client and sends messages to the event hub every 0.5 seconds.
		private static async Task SendMessagesToEventHub(List<Airline> air)
		{
			//While loop for continous action execution
			while (true)
			{
				air.ForEach(async (entry) =>
				{
					try
					{
						var message = JsonConvert.SerializeObject(entry);
						Console.WriteLine($"Sending message: {message}");

						await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
					//Time interval - 0.5 secs = 500 milliseconds
					System.Threading.Thread.Sleep(500);
					}
					catch (Exception exception)
					{
						Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
					}
				});
			}
		}
	}
}
 
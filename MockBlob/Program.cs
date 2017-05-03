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

namespace MockBlob
{
	class Program
	{
		private static EventHubClient eventHubClient;

		static void Main(string[] args)
		{
			//Parse the connection string and return a reference to the storage account
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
			//Creating a CloudBlobClient to retrieve the container and blob storage
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			//Retrieve reference to the created container 'input'
			CloudBlobContainer container = blobClient.GetContainerReference("input");
			//Retrieve reference to the file arilines[2].json inside the container
			CloudBlockBlob blockBob = container.GetBlockBlobReference("airlines[2].json");
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
			//Looping through JSON Deserialised Objects with an increment of +1
			foreach (var a in air)
			{
				//Try Catch statement in order to be able to catch any exceptions
				try
				{
					var message = $"Message {a}"; //Message is incremented by 1
					Console.WriteLine($"Sending message: {message}");
					await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
				}
				catch (Exception exception)
				{
					Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
				}

				//Time interval - 0.5 secs
				await Task.Delay(TimeSpan.FromSeconds(0.5));
			}
		}
	}
}

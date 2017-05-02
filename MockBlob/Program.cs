using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Adding directives
using Microsoft.Azure; //Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; //Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; //Namespace for Blob storage types	


namespace MockBlob
{
	class Program
	{
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
		}
	}
}

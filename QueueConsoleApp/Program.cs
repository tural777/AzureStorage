using AzureStorageLibrary.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace QueueConsoleApp
{
    class Program
    {
        private async static Task Main(string[] args)
        {
            Console.WriteLine("Start");

            AzureStorageLibrary.ConnectionStrings.AzureStorageConnectionString =
                "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVE" +
                "rCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=" +
                "http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";



            // kicik herfler istifade etmek lazimdi
            AzQueue queue = new AzQueue("mytestqueue");


            // Queue-ya yazmaq
            //base64 encoding - ona gore edirik ki, bashqa symbollari da istifade ede bilek ve server uchun anlamli ola bilen char-lar ola biler.
            //string base64message = Convert.ToBase64String(Encoding.UTF8.GetBytes("Tural Novruzov"));
            //queue.SendMessageAsync(base64message).Wait();


            // Queue-dan oxumaq
            //var message = queue.RetrieveNextMessageAsync().Result;
            //string text = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
            //Console.WriteLine("Message: " + text);


            // Queue-dan silmek
            //var message = queue.RetrieveNextMessageAsync().Result;
            //await queue.DeleteMessage(message.MessageId, message.PopReceipt);


        }
    }
}

using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class AzQueue
    {
        private readonly QueueClient _queueClient;

        public AzQueue(string queueName)
        {
            _queueClient = new QueueClient(ConnectionStrings.AzureStorageConnectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        // Mesajlarmizi string olaraq save edirik, class lazim olsa stringe convert etmek olar
        public async Task SendMessageAsync(string message)
        {
            // TIMEOUT - Quyruqdan mesaji goturende ne qeder vaxt gorsenmesin(visibility).
            // bu ona goredirki, hansi program onu handle etmeye chalishanda, digerleri onu gormesin, 
            // artiq hemin ishi handle eden program var menasina gelir.Eger hemin mesaj gorsense, diger programlar
            // da o mesaji handle elemekle meshgul olacaqlar.
            // Gelen mesaji, programimiz ne qeder vaxta handle ede bilecekse ondan daha chox vaxt 
            // (timeout >= mesajHandleTime) vermek lazimdi. 2 defe artiq vaxt versek daha yaxshi olar.
            // Default 30s


            // TimeToLive - Mesaj quyrugda ne qeder vaxt qalsin, default 7 gundu. artira da bilerik.
            // ve ya TimeSpan.FromSeconds ile -1 versek, omur boyu quyruqda qalar, biz silene kimi.
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();


            //_queueClient.PeekMessagesAsync().Wait(); - bununla mesaj quyruqdan mesajlari oxuyanda mesajlar gorunmez olmur.
            // hal-hazirda yazdigmizda mesajlari handle edende gorsenmez olur.
            if (properties.ApproximateMessagesCount > 0)
            {
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));

                if (queueMessages.Any())
                {
                    return queueMessages[0];
                }
            }

            return null;
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketNetCore.WebSocketManager;

namespace WebSocketNetCore.StatusSender
{
    public class StatusSenderConnection : WebSocketConnection
    {
        public StatusSenderConnection(WebSocketHandler handler) : base(handler)
        {
        }

        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            //aca se pueden definir los receptores, por ahora todos
            var receivers = Handler.Connections.ToList();

            if (receivers.Count > 0)
            {
                receivers.ForEach(async (websocketConnection) =>
                {
                    var sendMessage = JsonConvert.SerializeObject(new SendMessage
                    {
                        Sender = "Server",
                        Message = receiveMessage.Message
                    });

                    await websocketConnection.SendMessageAsync(sendMessage);

                });
            }
        }

        private class ReceiveMessage
        {
            public string Receiver { get; set; }

            public string Message { get; set; }
        }

        private class SendMessage
        {
            public string Sender { get; set; }

            public string Message { get; set; }
        }

    }
}

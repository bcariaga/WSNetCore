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

            //aca se pueden definir los receptores, por ahora todos
            var receivers = Handler.Connections.ToList();

            receivers.ForEach(async (websocketConnection) =>
            {
                var sendMessage = new SendMessage
                {
                    Sender = "Server",
                    Message = message
                };

                await websocketConnection.SendMessageAsync(sendMessage.ToString());

            });
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

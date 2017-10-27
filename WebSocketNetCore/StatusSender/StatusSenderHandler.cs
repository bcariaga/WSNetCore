using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketNetCore.WebSocketManager;

namespace WebSocketNetCore.StatusSender
{
    public class StatusSenderHandler : WebSocketHandler
    {
        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<WebSocketConnection> OnConnected(HttpContext context)
        {
            //Todo Usar algo asi com un token para validar la conexión
            //var name = context.Request.Query["Token"];

            //para filtar las conexiones
            //var connections = Connections.ToList();

            //connections.ForEach(async conn => 
            //{
            //    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            //    var senderClient = new StatusSenderConnection(this)
            //    {
            //        WebSocket = webSocket
            //    };

            //    Connections.Add(senderClient);
            //});

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var senderClient = new StatusSenderConnection(this)
            {
                WebSocket = webSocket
            };

            Connections.Add(senderClient);

            //TODO: esto esta mal ya lo se
            return null;
        }
    }
}

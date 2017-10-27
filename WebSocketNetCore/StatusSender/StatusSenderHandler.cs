using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebSocketNetCore.WebSocketManager;

namespace WebSocketNetCore.StatusSender
{
    public class StatusSenderHandler : WebSocketHandler
    {
        protected override int BufferSize { get => 1024 * 4; }

        protected bool WatcherIsRuning = false;

        public override async Task<WebSocketConnection> OnConnected(HttpContext context)
        {
            
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var senderClient = new StatusSenderConnection(this)
            {
                WebSocket = webSocket
            };

            Connections.Add(senderClient);

            if (!WatcherIsRuning)
                StartFileSystemWatcher();

            return senderClient;
        }

        private void StartFileSystemWatcher()
        {
            var watcher = new FileSystemWatcher();

            watcher.Path = @"C:\Users\bcariaga\Desktop\WebSocketTest\";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += async (object sender, FileSystemEventArgs e) => {

                foreach (var con in Connections)
                    await con.SendMessageAsync(e.Name + " cambio (" + e.ChangeType + ")");
                
            };

            WatcherIsRuning = true;
        }


    }
}

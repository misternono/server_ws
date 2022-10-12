using System.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace server_ws
{
    class Program
    {
        

        static async Task Main(string[] args)
        {
            var ip = ConfigurationManager.AppSettings["server"];
            var port = ConfigurationManager.AppSettings["port"];
            TcpListener server = new TcpListener(IPAddress.Parse(ip), Int32.Parse(port));
            server.Stop();
            server.Start();
            Console.WriteLine("Server has started on {0}:{1}\nWaiting for a connection…", ip, port);

            for(; ; )
            {
                var client = server.AcceptSocket();
                Console.WriteLine("A client connected.");
                
                Handler handler = new Handler();
                handler.Handle(client);
            }
            server.Stop();

        }
    }
}

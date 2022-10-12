using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace server_ws
{
    public class Handler
    {
        public async void Handle(Socket client)
        {
            byte[] option = new byte[4];
            client.Receive(option);

            var opt = Encoding.ASCII.GetString(option);

            if (Encoding.ASCII.GetString(option) == "uplo")
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] rec = new byte[256];
                    while (client.Receive(rec) != 0)
                    {
                        ms.Write(rec, 0, rec.Length);
                    }
                    var guid = Guid.NewGuid();
                    File.WriteAllBytes(ConfigurationManager.AppSettings["download_dir"] + guid.ToString() + ".pdf", ms.ToArray());
                }
            }
            else if (Encoding.ASCII.GetString(option) == "down")
            {

                byte[] file = null;
                byte[] id = new byte[8];
                client.Receive(id);
                try
                {
                    file = System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings["upload_dir"] + Encoding.ASCII.GetString(id, 0, id.TakeWhile(b => b != 0).Count()) + ".pdf");
                }
                catch (Exception e){
                    file = Encoding.ASCII.GetBytes("ERROR: " + e.Message);
                }

                client.Send(file);
                client.Disconnect(true);
            }
        }
    }
}

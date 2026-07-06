using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class NetworkIPDiscoverer
{
    // Retorna o IP IPv4 local do dispositivo ativo na rede
    public static string GetLocalIPv4()
    {   
        
        string localIP = "0.0.0.0";
        try
        {
            // Busca o nome do host do dispositivo
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            // Varre a lista de endereços do dispositivo procurando um IPv4 válido
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    // Evita capturar o endereço de loopback local (127.0.0.1)
                    if (!ip.ToString().StartsWith("127."))
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao recuperar IP Local: {e.Message}");
        }

        return localIP;
    }
}
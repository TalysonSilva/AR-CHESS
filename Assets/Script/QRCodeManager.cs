using UnityEngine;
using UnityEngine.UI;
using Mirror;
using ZXing; // Biblioteca de leitura/geração
using ZXing.QrCode;
using System.Net;
using System.Net.Sockets;

public class QRCodeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage qrCodeDisplay;
    public RawImage cameraFeedDisplay;

    private WebCamTexture camTexture;
    private Rect screenRect;
    private bool isScanning = false;

    // --- FUNÇÕES DO HOST (GERAR QR CODE) ---

    // Chame esta função a partir do seu botão "Criar Partida"
    public void GenerateQRCodeForHost()
    {
        string localIP = GetLocalIPAddress();
        string port = "7777"; // Porta padrão do Mirror
        string sessionHash = "Sessao_" + Random.Range(1000, 9999); 
        
        // A string final que o Client vai ler
        string qrString = localIP + ":" + port + ":" + sessionHash;

        Texture2D qrTexture = GenerateQR(qrString);
        qrCodeDisplay.texture = qrTexture;
        qrCodeDisplay.gameObject.SetActive(true);
        
        Debug.Log("QR Code gerado com os dados: " + qrString);
    }

    private Texture2D GenerateQR(string text)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 256,
                Width = 256
            }
        };
        var color32 = writer.Write(text);
        Texture2D texture = new Texture2D(256, 256);
        texture.SetPixels32(color32);
        texture.Apply();
        return texture;
    }

    // Pega o IP do celular na rede Wi-Fi local
    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

    // --- FUNÇÕES DO CLIENT (LER QR CODE) ---

    // Chame esta função a partir do seu botão "Entrar na Partida"
    public void StartScanner()
    {
        cameraFeedDisplay.gameObject.SetActive(true);
        camTexture = new WebCamTexture();
        cameraFeedDisplay.texture = camTexture;
        camTexture.Play();
        isScanning = true;
    }

    void Update()
    {
        if (isScanning && camTexture != null && camTexture.isPlaying)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // Tenta decodificar o que a câmera está vendo
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                
                if (result != null)
                {
                    isScanning = false;
                    camTexture.Stop();
                    cameraFeedDisplay.gameObject.SetActive(false);
                    
                    Debug.Log("QR Code Lido! Dados: " + result.Text);
                    ConnectToServer(result.Text);
                }
            }
            catch
            {
                // Continua tentando se não achar nada
            }
        }
    }

    private void ConnectToServer(string qrData)
    {
        // Separa os dados lidos (IP, Porta, Hash)
        string[] dataParts = qrData.Split(':');
        string serverIP = dataParts[0];

        // Atualiza o Mirror com o IP escaneado
        NetworkManager.singleton.networkAddress = serverIP;
        
        // Inicia a conexão do Client e trava a sessão 
        NetworkManager.singleton.StartClient();
    }
}
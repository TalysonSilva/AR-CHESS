using UnityEngine;
using Mirror;

public class MainMenuController : MonoBehaviour
{
    [Header("Arraste o objeto que tem o QRCodeManager aqui")]
    public QRCodeManager qrCodeManager;

    [Header("Arraste os seus Botoes (ou o Painel deles) aqui")]
    public GameObject painelMenu;

    public void CriarPartida()
    {
        // Inicia o servidor
        NetworkManager.singleton.StartHost();
        
        // Esconde SÓ os botões, mantendo o Canvas e o QR Code vivos!
        if (painelMenu != null)
        {
            painelMenu.SetActive(false); 
        }
        
        // Gera o QR Code
        if (qrCodeManager != null)
        {
            qrCodeManager.GenerateQRCodeForHost();
        }
    }

    public void EntrarNaPartida()
    {
        if (painelMenu != null)
        {
            painelMenu.SetActive(false);
        }
        
        if (qrCodeManager != null)
        {
            qrCodeManager.StartScanner();
        }
    }
}
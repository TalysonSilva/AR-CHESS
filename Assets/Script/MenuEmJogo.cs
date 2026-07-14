using UnityEngine;
using Mirror;

public class MenuEmJogo : MonoBehaviour
{
    [Header("Arraste o Botão Start aqui")]
    public GameObject botaoStart;

    void Update()
    {
        // Garante que o botão Start só apareça para o Host (dono da sala)
        if (botaoStart != null)
        {
            if (NetworkServer.active && NetworkClient.isConnected) 
            {
                // Se o Host já clicou e iniciou, esconde o botão
                TurnManager tm = FindObjectOfType<TurnManager>();
                if (tm != null && tm.partidaIniciada)
                {
                    botaoStart.SetActive(false);
                }
            }
            else
            {
                // Client (desafiante) não vê o botão Start
                botaoStart.SetActive(false);
            }
        }
    }

    // Coloque esta função no OnClick do Botão START
    public void PressionouStart()
    {
        TurnManager tm = FindObjectOfType<TurnManager>();
        if (tm != null && NetworkServer.active)
        {
            tm.BotaoIniciarPartida();
        }
    }

    // Coloque esta função no OnClick do Botão SAIR
    public void PressionouSair()
    {
        // Se for o dono da sala, desliga o servidor inteiro
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // Se for o convidado, apenas se desconecta
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}

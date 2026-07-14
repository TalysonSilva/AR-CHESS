using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuLocal : MonoBehaviour
{
    [Header("Interface")]
    public GameObject botaoStart; // Arraste o seu botão Start aqui

    [Header("Cérebro do Jogo")]
    public InteracaoJogador scriptJogador; // Arraste o objeto que tem o script InteracaoJogador

    void Start()
    {
        // Garante que o jogador NÃO pode mexer nas peças assim que o jogo abre
        if (scriptJogador != null)
        {
            scriptJogador.enabled = false;
        }
    }

    // Coloque esta função no OnClick do seu Botão START
    public void PressionouStart()
    {
        // 1. Esconde o botão Start da tela
        if (botaoStart != null)
        {
            botaoStart.SetActive(false);
        }

        // 2. Acorda o script da mão do jogador para ele poder tocar no tabuleiro!
        if (scriptJogador != null)
        {
            scriptJogador.enabled = true;
        }

        Debug.Log("Partida Local Iniciada!");
    }

    // Coloque esta função no OnClick do seu Botão SAIR
    public void PressionouSair()
    {
        Debug.Log("A fechar o jogo...");
        
        // Fecha o aplicativo no telemóvel (Game Over / Sair)
        Application.Quit();
        
    }
}

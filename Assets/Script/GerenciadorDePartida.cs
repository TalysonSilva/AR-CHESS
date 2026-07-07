using UnityEngine;
using Mirror;

public class GerenciadorDePartida : NetworkBehaviour
{
    // O [SyncVar] garante que se o Host mudar essa variável, o Client também saberá instantaneamente.
    [SyncVar]
    public bool faseDePreparacao = true; 

    // Variável para o seu futuro relógio/tempo
    public float tempoDeJogo = 0f;

    void Update()
    {
        // Se ainda for a fase de preparação, cancela a contagem do tempo!
        if (faseDePreparacao)
        {
            return; // O código para aqui. Nada daqui para baixo acontece.
        }

        // Se passou daqui, é porque a partida começou de verdade!
        // O tempo começa a correr:
        tempoDeJogo += Time.deltaTime;
        // Debug.Log("Tempo rolando: " + tempoDeJogo);
    }

    // Você vai chamar essa função depois, através de um botão "Estou Pronto!"
    [Server]
    public void IniciarPartidaDeVerdade()
    {
        faseDePreparacao = false;
        Debug.Log("Fase de preparação concluída! A partida começou!");
    }
}
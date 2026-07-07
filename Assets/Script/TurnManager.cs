using UnityEngine;
using Mirror;
using TMPro;

public class TurnManager : NetworkBehaviour
{
    [Header("Elementos de UI")]
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoCronometro;

    [Header("Conexão com o Gerenciador")]
    public GerenciadorDePartida gerenciador; // <-- Criamos a ponte para o novo script aqui!

    [SyncVar(hook = nameof(AtualizarUI_Turno))]
    public bool isHostTurn = true; 

    [SyncVar(hook = nameof(AtualizarUI_Tempo))]
    public int tempoRestante = 60;

    private float timerInterno = 0f;

    void Update()
    {
        // O tempo SÓ DEVE passar no Servidor
        if (isServer)
        {
            // SE AINDA ESTIVER NA FASE DE PREPARAÇÃO, O RELÓGIO DE XADREZ NÃO RODA!
            if (gerenciador != null && gerenciador.faseDePreparacao)
            {
                return; // Para o código aqui, impedindo o tempo de cair.
            }

            timerInterno += Time.deltaTime;
            
            if (timerInterno >= 1f)
            {
                tempoRestante--;
                timerInterno = 0f;

                if (tempoRestante <= 0)
                {
                    PassarTurno(); 
                }
            }
        }
    }

    [Server]
    public void PassarTurno()
    {
        isHostTurn = !isHostTurn;
        tempoRestante = 60;
        
        Debug.Log("Turno passado! Agora é a vez do Host? " + isHostTurn);
    }
    
    void AtualizarUI_Turno(bool turnoAntigo, bool turnoNovo)
    {
        if ((isServer && turnoNovo) || (!isServer && !turnoNovo))
        {
            textoTurno.text = "SUA VEZ!";
            textoTurno.color = Color.green;
        }
        else
        {
            textoTurno.text = "VEZ DO OPONENTE";
            textoTurno.color = Color.red;
        }
    }

    void AtualizarUI_Tempo(int tempoAntigo, int tempoNovo)
    {
        if (textoCronometro != null)
        {
            textoCronometro.text = tempoNovo.ToString() + "s";
        }
    }
}
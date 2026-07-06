using UnityEngine;
using Mirror;
using TMPro; // Usado para manipular o TextMeshPro

public class TurnManager : NetworkBehaviour
{
    [Header("Elementos de UI")]
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoCronometro;

    // [SyncVar] significa que o Servidor controla essa variável.
    // Quando o servidor muda o valor, todos os clientes são atualizados automaticamente.
    // O "hook" chama uma função sempre que o valor muda.
    [SyncVar(hook = nameof(AtualizarUI_Turno))]
    public bool isHostTurn = true; 

    [SyncVar(hook = nameof(AtualizarUI_Tempo))]
    public int tempoRestante = 60; // 60 segundos por jogada

    private float timerInterno = 0f;

    void Update()
    {
        // O tempo SÓ DEVE passar no Servidor para evitar dessincronização
        if (isServer)
        {
            timerInterno += Time.deltaTime;
            
            // A cada 1 segundo real, diminui 1 segundo do jogo
            if (timerInterno >= 1f)
            {
                tempoRestante--;
                timerInterno = 0f;

                // Passa a vez automaticamente se o tempo acabar
                if (tempoRestante <= 0)
                {
                    PassarTurno(); 
                }
            }
        }
    }

    // A tag [Server] impede que um Client mal-intencionado (ou bugado) chame essa função.
    // Apenas o Host executa isso.
    [Server]
    public void PassarTurno()
    {
        isHostTurn = !isHostTurn; // Inverte o turno (Se era true, vira false e vice-versa)
        tempoRestante = 60; // Reseta o cronômetro para o próximo jogador
        
        Debug.Log("Turno passado! Agora é a vez do Host? " + isHostTurn);
        
        // NOTA: É exatamente AQUI que, no futuro, chamaremos a função de 
        // salvar o estado da partida em JSON!
    }

    // --- FUNÇÕES DE HOOK (Atualizam a interface de ambos os jogadores) ---
    
    void AtualizarUI_Turno(bool turnoAntigo, bool turnoNovo)
    {
        // Se eu for o Host e for a vez do Host, ou se eu for o Client e NÃO for a vez do Host...
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
        textoCronometro.text = tempoNovo.ToString() + "s";
    }
}
using UnityEngine;
using TMPro;
using Mirror;

public class TurnManager : NetworkBehaviour
{
    [Header("Elementos de UI")]
    public TMP_Text textoTurno;
    public TMP_Text textoCronometro;

    // A mágica acontece toda aqui agora, sem precisar de outro script!
    [SyncVar] public bool partidaIniciada = false;
    [SyncVar] public bool isHostTurn = true; 
    [SyncVar] public float tempoRestante = 60f; 

    void Update()
    {
        // 1. Atualiza os textos na tela para os jogadores
        AtualizarUI();

        // 2. O relógio só corre no Servidor se a partida tiver começado
        if (!isServer || !partidaIniciada) return;

        tempoRestante -= Time.deltaTime;

        // Passa a vez automaticamente quando o tempo zera
        if (tempoRestante <= 0)
        {
            PassarTurno();
        }
    }

    void AtualizarUI()
    {
        if (textoCronometro != null)
        {
            textoCronometro.text = Mathf.CeilToInt(tempoRestante).ToString() + "s";
        }

        if (textoTurno != null)
        {
            bool minhaVez = (isServer && isHostTurn) || (!isServer && !isHostTurn);
            
            if (minhaVez)
            {
                textoTurno.text = "SUA VEZ";
                textoTurno.color = Color.green;
            }
            else
            {
                textoTurno.text = "VEZ DO OPONENTE";
                textoTurno.color = Color.red;
            }
        }
    }

    [Server]
    public void BotaoIniciarPartida()
    {
        partidaIniciada = true;
        isHostTurn = true;
        tempoRestante = 60f; 
    }

    [Server]
    public void PassarTurno()
    {
        isHostTurn = !isHostTurn;
        tempoRestante = 60f; 
        Debug.Log("Turno Passado! Vez do Host? " + isHostTurn);
    }
}
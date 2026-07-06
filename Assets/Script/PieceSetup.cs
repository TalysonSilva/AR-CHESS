using UnityEngine;
using Mirror;

public class PieceSetup : NetworkBehaviour
{
    [Header("Materiais")]
    public Material materialJogador1; // Ex: Azul Holográfico
    public Material materialJogador2; // Ex: Laranja Holográfico

    // Variável sincronizada para definir o time (1 ou 2)
    [SyncVar(hook = nameof(AplicarCor))]
    public int timeDaPeca = 1;

    void Start()
    {
        // Garante que a cor seja aplicada assim que o jogo começar
        AplicarCor(timeDaPeca, timeDaPeca);
    }

    void AplicarCor(int timeAntigo, int timeNovo)
    {
        MeshRenderer renderizador = GetComponentInChildren<MeshRenderer>();
        if (renderizador != null)
        {
            renderizador.material = (timeNovo == 1) ? materialJogador1 : materialJogador2;
        }
    }
}

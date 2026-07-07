using UnityEngine;


public class PecaDeXadrez : MonoBehaviour // <-- Mudou aqui!
{
    [Header("Qual é o time dessa peça?")]
    public bool ehDoTimeBranco;

    [Header("Qual é o tipo dessa peça?")]
    public TipoPeca tipoDePeca;

    public enum TipoPeca
    {
        Peao, Torre, Cavalo, Bispo, Rainha, Rei
    }
}

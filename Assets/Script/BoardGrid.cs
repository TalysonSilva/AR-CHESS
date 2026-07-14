using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    [Header("Configurações do Tabuleiro")]
    [Tooltip("Tamanho exato de UMA casa em metros (Ex: 0.032 = 3.2cm)")]
    public float tamanhoDaCasa = 0.032f; 
    
    [Tooltip("Objeto posicionado no centro da casa A1 (canto)")]
    public Transform pontoDeOrigemA1; 

    public Vector3 PegarCentroDaCasaMaisProxima(Vector3 posicaoDoToque)
    {
        if (pontoDeOrigemA1 == null) 
        {
            Debug.LogError("Faltou arrastar o Ponto de Origem A1 no BoardGrid!");
            return posicaoDoToque;
        }

        // Descobre a distância real entre o toque do seu dedo e a Origem
        Vector3 direcao = posicaoDoToque - pontoDeOrigemA1.position;

        // Projeta essa distância considerando a Rotação do tabuleiro no mundo AR
        float distanciaX = Vector3.Dot(direcao, pontoDeOrigemA1.right);
        float distanciaZ = Vector3.Dot(direcao, pontoDeOrigemA1.forward);

        // Acha qual é o número da casa (Coluna e Linha de 0 a 7)
        int indexX = Mathf.RoundToInt(distanciaX / tamanhoDaCasa);
        int indexZ = Mathf.RoundToInt(distanciaZ / tamanhoDaCasa);

        // Trava para a peça nunca tentar voar para fora da mesa de xadrez
        indexX = Mathf.Clamp(indexX, 0, 7);
        indexZ = Mathf.Clamp(indexZ, 0, 7);

        // Recalcula a posição 3D exata do centro daquela casa escolhida
        Vector3 centroDaCasa = pontoDeOrigemA1.position 
                             + (pontoDeOrigemA1.right * (indexX * tamanhoDaCasa))
                             + (pontoDeOrigemA1.forward * (indexZ * tamanhoDaCasa));

        // Mantém a altura original do toque para não afundar na madeira
        centroDaCasa.y = posicaoDoToque.y;

        return centroDaCasa;
    }
}
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    [Header("Configurações do Tabuleiro")]
    // O tamanho físico de uma única casa do seu modelo 3D (ex: 0.1 metros)
    public float tamanhoDaCasa = 0.1f; 
    
    // O ponto de origem (canto inferior esquerdo do tabuleiro, a casa A1)
    public Transform pontoDeOrigemA1; 

    // Função que recebe a posição onde o jogador soltou a peça e devolve o centro da casa mais próxima
    public Vector3 PegarCentroDaCasaMaisProxima(Vector3 posicaoSolta)
    {
        // Calcula a diferença entre onde a peça foi solta e a origem do tabuleiro
        Vector3 diferenca = posicaoSolta - pontoDeOrigemA1.position;

        // Descobre em qual "coluna" (X) e "linha" (Z) a peça caiu, arredondando para o número inteiro mais próximo
        int indexX = Mathf.RoundToInt(diferenca.x / tamanhoDaCasa);
        int indexZ = Mathf.RoundToInt(diferenca.z / tamanhoDaCasa);

        // Trava os valores entre 0 e 7 para garantir que a peça não caia fora das 8 casas do tabuleiro
        indexX = Mathf.Clamp(indexX, 0, 7);
        indexZ = Mathf.Clamp(indexZ, 0, 7);

        // Calcula a posição final perfeita no centro da casa
        float novaPosicaoX = indexX * tamanhoDaCasa;
        float novaPosicaoZ = indexZ * tamanhoDaCasa;

        // Retorna a posição exata no mundo 3D
        return pontoDeOrigemA1.position + new Vector3(novaPosicaoX, 0, novaPosicaoZ);
    }
}

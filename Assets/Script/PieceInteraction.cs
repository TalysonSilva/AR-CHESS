using UnityEngine;

public class PieceInteraction : MonoBehaviour
{
    [Header("Configurações")]
    public Camera arCamera;
    
    private GameObject pecaSegurada;
    private Transform paiOriginalDaPeca;

    // Função chamada quando o jogador PRESSIONA e SEGURA o botão "Segurar Peça"
    public void PegarPeca()
    {
        // Cria um raio partindo exatamente do centro da tela (onde está o retículo)
        Ray ray = arCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Dispara o raio. Se bater em algo a até 3 metros de distância...
        if (Physics.Raycast(ray, out hit, 3f))
        {
            // Verifica se o objeto atingido tem a Tag "Peca"
            if (hit.collider.CompareTag("Peca"))
            {
                pecaSegurada = hit.collider.gameObject;
                
                // Salva quem era o "pai" original da peça para devolver depois
                paiOriginalDaPeca = pecaSegurada.transform.parent;
                
                // Transforma a câmera no "pai" da peça. 
                // Agora, mover o celular fisicamente moverá a peça no mundo!
                pecaSegurada.transform.SetParent(arCamera.transform);
                
                Debug.Log("Peça capturada: " + pecaSegurada.name);
            }
        }
    }

    // Função chamada quando o jogador SOLTA o botão "Segurar Peça"
  public void SoltarPeca()
    {
        if (pecaSegurada != null)
        {
            // Devolve a peça para a mesa (remove a câmera como pai)
            pecaSegurada.transform.SetParent(paiOriginalDaPeca);
            
            // PROCURA O TABULEIRO NA CENA
            BoardGrid grid = FindObjectOfType<BoardGrid>();
            if (grid != null)
            {
                // Faz o Snap da peça para o centro da casa
                Vector3 posicaoCorrigida = grid.PegarCentroDaCasaMaisProxima(pecaSegurada.transform.position);
                pecaSegurada.transform.position = posicaoCorrigida;
            }

            pecaSegurada = null;
            Debug.Log("Peça solta e alinhada no tabuleiro.");
        }
    }

    public void CmdPedirParaPassarTurno()
    {
    // O Client "pede" ao servidor, e o servidor executa
    FindObjectOfType<TurnManager>().PassarTurno();
    }
}
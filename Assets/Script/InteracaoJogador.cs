using UnityEngine;
using TMPro; 

public class InteracaoJogador : MonoBehaviour 
{
    [Header("Configuração do Tabuleiro")]
    [Tooltip("Tamanho exato de UMA casa em metros (O seu é 0.0625)")]
    public float tamanhoDaCasa = 0.0625f;

    [Header("Interface (UI)")]
    public TMP_Text textoMensagem;

    private GameObject pecaSelecionada;
    private Color corOriginal;
    
    public bool vezDasBrancas = true; 
    private bool jogoAcabou = false; 

    void Start()
    {
        if (textoMensagem != null) textoMensagem.text = "";
    }

    void Update()
    {
        if (jogoAcabou) return;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 toquePos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            Ray raio = Camera.main.ScreenPointToRay(toquePos);
            RaycastHit hit;

            if (Physics.Raycast(raio, out hit))
            {
                PecaDeXadrez pecaClicada = hit.collider.GetComponent<PecaDeXadrez>();

                // 1. SELECIONAR A PEÇA
                if (pecaSelecionada == null && pecaClicada != null)
                {
                    if (pecaClicada.ehDoTimeBranco == vezDasBrancas)
                    {
                        pecaSelecionada = pecaClicada.gameObject;
                        
                        MeshRenderer renderer = pecaSelecionada.GetComponent<MeshRenderer>();
                        if (renderer != null)
                        {
                            corOriginal = renderer.material.color;
                            renderer.material.color = Color.yellow; 
                        }
                    }
                }
                // 2. MOVER OU CAPTURAR
                else if (pecaSelecionada != null)
                {
                    Vector3 destino = Vector3.zero;
                    bool ehCaptura = false; 

                    if (pecaClicada != null)
                    {
                        if (pecaClicada.ehDoTimeBranco == vezDasBrancas)
                        {
                            TirarCorAmarela();
                            pecaSelecionada = pecaClicada.gameObject;
                            corOriginal = pecaSelecionada.GetComponent<MeshRenderer>().material.color;
                            pecaSelecionada.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            return;
                        }
                        else
                        {
                            destino = pecaClicada.transform.position;
                            ehCaptura = true; 
                        }
                    }
                    else if (hit.collider.CompareTag("Tabuleiro"))
                    {
                        destino = hit.point;
                    }
                    else return;

                    PecaDeXadrez infoPeca = pecaSelecionada.GetComponent<PecaDeXadrez>();
                    
                    if (MovimentoValido(infoPeca, pecaSelecionada.transform.position, destino, ehCaptura))
                    {
                        destino.y = pecaSelecionada.transform.position.y; 

                        TirarCorAmarela();
                        pecaSelecionada.transform.position = destino;
                        
                        if (ehCaptura)
                        {
                            PecaDeXadrez infoInimigo = pecaClicada.GetComponent<PecaDeXadrez>();
                            if (infoInimigo != null && infoInimigo.tipoDePeca == PecaDeXadrez.TipoPeca.Rei)
                            {
                                string vencedor = vezDasBrancas ? "BRANCAS" : "PRETAS";
                                DeclararVitoria(vencedor);
                            }
                            Destroy(pecaClicada.gameObject); 
                        }

                        pecaSelecionada = null;
                        if (!jogoAcabou) vezDasBrancas = !vezDasBrancas; 
                    }
                    else
                    {
                        Debug.Log("Movimento Inválido para esta peça!");
                    }
                }
            }
        }
    }

    void DeclararVitoria(string vencedor)
    {
        jogoAcabou = true; 
        if (textoMensagem != null)
        {
            textoMensagem.text = "VITÓRIA DAS " + vencedor + "!";
            textoMensagem.color = Color.yellow;
            textoMensagem.fontSize = 60; 
        }
    }

    // --- REGRAS DO XADREZ ---
    bool MovimentoValido(PecaDeXadrez peca, Vector3 origem, Vector3 destino, bool ehCaptura)
    {
        GameObject tabuleiro = GameObject.FindGameObjectWithTag("Tabuleiro");
        if (tabuleiro == null) return true; 

        Vector3 direcao = destino - origem;
        
        float distX = Mathf.Abs(Vector3.Dot(direcao, tabuleiro.transform.right));
        float distZ = Mathf.Abs(Vector3.Dot(direcao, tabuleiro.transform.forward));

        int deltaX = Mathf.RoundToInt(distX / tamanhoDaCasa);
        int deltaZ = Mathf.RoundToInt(distZ / tamanhoDaCasa);

        bool padraoValido = false;

        // 1. Verifica se o desenho do movimento é válido
        switch (peca.tipoDePeca)
        {
            case PecaDeXadrez.TipoPeca.Peao:
                if (ehCaptura)
                    padraoValido = (deltaX == 1 && deltaZ == 1); 
                else
                    padraoValido = (deltaX == 0 && (deltaZ == 1 || deltaZ == 2)); 
                break;
            case PecaDeXadrez.TipoPeca.Torre:
                padraoValido = (deltaX == 0 && deltaZ > 0) || (deltaX > 0 && deltaZ == 0);
                break;
            case PecaDeXadrez.TipoPeca.Cavalo:
                padraoValido = (deltaX == 1 && deltaZ == 2) || (deltaX == 2 && deltaZ == 1);
                break;
            case PecaDeXadrez.TipoPeca.Bispo:
                padraoValido = (deltaX == deltaZ && deltaX > 0);
                break;
            case PecaDeXadrez.TipoPeca.Rainha:
                padraoValido = (deltaX == 0 && deltaZ > 0) || (deltaX > 0 && deltaZ == 0) || (deltaX == deltaZ && deltaX > 0);
                break;
            case PecaDeXadrez.TipoPeca.Rei:
                padraoValido = (deltaX <= 1 && deltaZ <= 1);
                break;
        }

        if (!padraoValido) return false;

        // 2. SISTEMA ANTI-PULO: Verifica se há peças no meio do caminho!
        if (peca.tipoDePeca != PecaDeXadrez.TipoPeca.Cavalo)
        {
            if (!CaminhoLivre(origem, destino))
            {
                Debug.Log("Movimento bloqueado! Uma peça está no caminho.");
                return false;
            }
        }

        return true;
    }

    // --- RADAR DE OBSTÁCULOS ---
    bool CaminhoLivre(Vector3 origem, Vector3 destino)
    {
        int passos = Mathf.RoundToInt(Vector3.Distance(origem, destino) / tamanhoDaCasa);
        
        if (passos <= 1) return true; 

        Vector3 avancoPorCasa = (destino - origem) / passos;

        for (int i = 1; i < passos; i++)
        {
            Vector3 pontoDeVerificacao = origem + (avancoPorCasa * i);
            pontoDeVerificacao.y += 0.02f; 

            Collider[] objetosNaCasa = Physics.OverlapSphere(pontoDeVerificacao, tamanhoDaCasa * 0.3f);
            
            // Aqui estava o erro! Troquei o "em" por "in"
            foreach (Collider col in objetosNaCasa)
            {
                if (col.GetComponent<PecaDeXadrez>() != null)
                {
                    return false; 
                }
            }
        }

        return true; 
    }

    void TirarCorAmarela()
    {
        if (pecaSelecionada != null)
        {
            MeshRenderer renderer = pecaSelecionada.GetComponent<MeshRenderer>();
            if (renderer != null) renderer.material.color = corOriginal;
        }
    }
}
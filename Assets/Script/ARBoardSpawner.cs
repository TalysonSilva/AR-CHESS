using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Mirror;

public class ARBoardSpawner : MonoBehaviour
{
    [Header("Arraste o Prefab do Tabuleiro aqui")]
    public GameObject tabuleiroPrefab;
    
    [Header("Arraste a Tela do QR Code aqui")]
    public GameObject telaQRCode;

    [Header("Arraste o AR Plane Manager (XR Origin) aqui")]
    public ARPlaneManager planeManager; // <-- Criamos a ponte para desligar a malha!

    private GameObject tabuleiroSpawnado;
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        
        // Se você esquecer de arrastar na Unity, o código pega sozinho por segurança
        if (planeManager == null) 
            planeManager = GetComponent<ARPlaneManager>(); 
    }

    void Update()
    {
        if (!NetworkServer.active) return;
        if (tabuleiroSpawnado != null) return;

        bool tocouNaTela = false;
        Vector2 posicaoDoToque = Vector2.zero;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tocouNaTela = true;
            posicaoDoToque = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            tocouNaTela = true;
            posicaoDoToque = Input.mousePosition;
        }

        if (tocouNaTela)
        {
            if (telaQRCode != null && telaQRCode.activeSelf)
            {
                telaQRCode.SetActive(false);
                return; 
            }

            if (raycastManager.Raycast(posicaoDoToque, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                
                // Cria o tabuleiro
                tabuleiroSpawnado = Instantiate(tabuleiroPrefab, hitPose.position, hitPose.rotation);
                
                // Manda para a rede
                NetworkServer.Spawn(tabuleiroSpawnado);

                // --- MÁGICA: Esconde os pontinhos brancos do chão! ---
                EsconderMalhaAR();
            }
        }
    }

    void EsconderMalhaAR()
    {
        if (planeManager != null)
        {
            // Apaga todos os pontos que já foram desenhados na tela
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            // Desliga o "radar" para ele parar de procurar chão novo
            planeManager.enabled = false;
        }
    }
}
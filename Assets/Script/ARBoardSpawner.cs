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

    private GameObject tabuleiroSpawnado;
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!NetworkServer.active) return;
        if (tabuleiroSpawnado != null) return;

        bool tocouNaTela = false;
        Vector2 posicaoDoToque = Vector2.zero;

        // Captura o toque no celular
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tocouNaTela = true;
            posicaoDoToque = Input.GetTouch(0).position;
        }
        // Captura extra de segurança (para o novo sistema de input)
        else if (Input.GetMouseButtonDown(0))
        {
            tocouNaTela = true;
            posicaoDoToque = Input.mousePosition;
        }

        if (tocouNaTela)
        {
            // 1. O PRIMEIRO TOQUE: Se o QR Code estiver visível, apaga ele!
            if (telaQRCode != null && telaQRCode.activeSelf)
            {
                telaQRCode.SetActive(false);
                return; // Para aqui e espera você tocar novamente depois
            }

            // 2. O SEGUNDO TOQUE: Tenta achar o chão do ARCore
            if (raycastManager.Raycast(posicaoDoToque, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                
                // Cria o tabuleiro (fininho) na mesa
                tabuleiroSpawnado = Instantiate(tabuleiroPrefab, hitPose.position, hitPose.rotation);
                
                // Manda o tabuleiro pra rede!
                NetworkServer.Spawn(tabuleiroSpawnado);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem; // Adicionamos a biblioteca do sistema novo!

public class ARBoardSpawner : MonoBehaviour
{
    [Header("Arraste o Prefab do Tabuleiro aqui")]
    public GameObject tabuleiroPrefab;
    
    private GameObject tabuleiroSpawnado;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Verifica se o celular tem uma tela de toque E se a pessoa tocou nela neste exato frame
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            // Pega as coordenadas X e Y de onde o dedo encostou na tela
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Dispara o raio a partir dessa posição do toque
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose; 

                // Se o tabuleiro ainda não existe, cria um novo
                if (tabuleiroSpawnado == null)
                {
                    tabuleiroSpawnado = Instantiate(tabuleiroPrefab, hitPose.position, hitPose.rotation);
                    Debug.Log("Tabuleiro fixado na mesa!");
                }
                else
                {
                    // Se já existe, apenas move
                    tabuleiroSpawnado.transform.position = hitPose.position;
                }
            }
        }
    }
}
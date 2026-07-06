using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacementInteraction : MonoBehaviour
{
    [Header("Objetos de Referência")]
    public GameObject testCubePrefab; // Aqui vai o seu Peão

    private GameObject spawnedCube;
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // 1. LÓGICA DE INTERAÇÃO: Clique do mouse (ou toque na tela)
        if (Input.GetMouseButtonDown(1))
        {
            // Atira um raio a partir da posição do mouse/toque
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                // Se não tem peça, cria uma. Se já tem, move ela para o novo local.
                if (spawnedCube == null)
                {
                    spawnedCube = Instantiate(testCubePrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnedCube.transform.position = hitPose.position;
                }
            }
        }
    }
}
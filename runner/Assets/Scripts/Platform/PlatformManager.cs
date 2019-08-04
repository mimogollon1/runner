
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject plane;

    public GameObject[] planes;
    private Transform player;
    private string[,] mat;

    private Vector3 posFinal;
    private float sizePlatform = 14.5f;

    public Transform lastSpawnPlane;

    // Start is called before the first frame update
    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        planes = GameObject.FindGameObjectsWithTag("Platform");
        
        // Indicar ultimo plano instanciado
        lastSpawnPlane = planes[4].transform;
        posFinal = new Vector3(0, 0, -25.50f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            float dist = Vector3.Distance(planes[i].transform.localPosition, posFinal);

            // Valida si la distancia entre el plano actual y su posición final es muy pequeña
            if (dist <= 0.25f)
            {                
                RemovePlane(planes[i].gameObject);
                SpawnPlane(i);

                break;
            }
        }
    }

    /// <summary>
    /// Genera un nuevo plano en la misma posición donde se elimino el ultimo
    /// </summary>
    /// <param name="index">posición del plano dentro del array.</param>
    void SpawnPlane(int index)
    {                
        GameObject newPlane = Instantiate(plane) as GameObject;
        
        newPlane.transform.SetParent(gameObject.transform);
        newPlane.transform.localPosition = lastSpawnPlane.localPosition + (Vector3.forward * sizePlatform);
        planes[index] = newPlane;
        lastSpawnPlane = planes[index].transform;
    }

    /// <summary>
    /// Elimina el plano indicado
    /// </summary>
    /// <param name="plane">GameObject a destruir.</param>
    private void RemovePlane(GameObject plane)
    {
        Destroy(plane);
    }
}

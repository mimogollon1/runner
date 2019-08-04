using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager00 : MonoBehaviour
{
    public GameObject obstacle;

    public GameObject[] obstacles;
    public Transform lastSpawnObstacle;
    private Vector3 posFinal;
    private bool isStarting = true;
    // Start is called before the first frame update
    void Start()
    {
        posFinal = new Vector3(0, 0, -38.00f);
        lastSpawnObstacle = obstacles[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            // Valida si el conjunto de obstaculos ya no es visible en pantalla
            if (obstacles[i].transform.localPosition.z <= -50f)
            {
                RemoveObstacle(obstacles[i].gameObject);
                SpawnObstacle(i);

                break;
            }
        }
    }

    /// <summary>
    /// Genera un obstaculo validando si es la primera vez que se ejecuta
    /// </summary>
    /// <param name="index">posición del Obstacuo dentro del array.</param>
    void SpawnObstacle(int index)
    {
        GameObject newPlane = Instantiate(obstacle) as GameObject;
        newPlane.GetComponent<Obstacle00>().Cargar();
        newPlane.transform.SetParent(gameObject.transform);

        // valida si es primera ejecucion
        if (!isStarting)
        {
            newPlane.transform.localPosition = lastSpawnObstacle.localPosition + (Vector3.forward * 30f);
        }

        else
        {
            isStarting = false;
            newPlane.transform.localPosition = lastSpawnObstacle.localPosition + (Vector3.forward * 62f);
        }
       

        obstacles[index] = newPlane;
        lastSpawnObstacle = obstacles[index].transform;
    }

    /// <summary>
    /// Elimina el plano indicado
    /// </summary>
    /// <param name="plane">GameObject a destruir.</param>
    private void RemoveObstacle(GameObject Obstacle)
    {
        Destroy(Obstacle);
    }
}

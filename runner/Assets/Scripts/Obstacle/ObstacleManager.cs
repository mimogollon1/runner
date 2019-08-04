using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private int positionsFreeA, positionsFreeB, positionsFreeC;
    private int pendientA, pendientB, pendientC;

    public string[,] obstaclesArray;
    private GameObject[] obstacles;
    public GameObject[] obstaclesModel;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle Enabled");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle Enabled");
        bool generateA = true, generateB = true, generatec = true;
        if (obstacles.Length != 0)
        {
            if (positionsFreeA != 0)
            {
                generateA = false;
            }
            if (positionsFreeB != 0)
            {
                generateB = false;
            }
            if (positionsFreeC != 0)
            {
                generatec = false;
            }
            Cargar(generateA, generateB, generatec);
        }
    }



    /// <summary>
    /// Elimina el plano indicado
    /// </summary>
    /// <param name="obstacle">GameObject a destruir.</param>
    private void RemoveObstacle(GameObject Obstacle)
    {
        Destroy(Obstacle);
    }

    /// <summary>
    /// valida si el espacio debe quedar libre antes de generar un obstaculo en esa posicion.
    /// </summary>
    /// <param name="A">Carril izquierdo.</param>
    /// <param name="B">Carril central.</param>
    /// <param name="C">Carril derecho.</param>
    public void Cargar(bool a, bool b, bool c)
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            switch (obstacles[i].name)
            {
                case "A":
                    if (a == true)
                    {
                        positionsFreeA = generar(obstacles[i], "A", 0);
                    }
                    else if (positionsFreeA == 1)
                    {
                        generar(obstacles[i], "A", pendientA);
                        positionsFreeA--;
                        pendientA = 0;
                    }
                    else if (positionsFreeA > 1)
                    {
                        generar(obstacles[i], "A", 1);
                        positionsFreeA--;
                    }
                    break;
                case "B":
                    if (b == true)
                    {
                        positionsFreeB = generar(obstacles[i], "B", 0);
                    }
                    else if (positionsFreeB == 1)
                    {
                        generar(obstacles[i], "B", pendientB);
                        positionsFreeB--;
                        pendientB = 0;
                    }
                    else if (positionsFreeB > 1)
                    {
                        generar(obstacles[i], "B", 1);
                        positionsFreeB--;
                    }
                    break;
                case "C":
                    if (c == true)
                    {
                        positionsFreeC = generar(obstacles[i], "C", 0);
                    }
                    else if (positionsFreeC == 1)
                    {
                        generar(obstacles[i], "C", pendientC);
                        positionsFreeC--;
                        pendientC = 0;
                    }
                    else if (positionsFreeC > 1)
                    {
                        generar(obstacles[i], "C", 1);
                        positionsFreeC--;
                    }
                    break;
            }
            obstacles[i].tag = "Obstacle Disabled";
        }
    }

    /// <summary>
    /// Genera obstaculo de forma aleatoria.
    /// </summary>
    /// <param name="obstacle">Gameobject donde se va a generar el obstaculo.</param>
    /// <param name="line">linea donde se genera el obsraculo(A,B,C).</param>
    /// <param name="pending">Carril derecho.</param>
    public int generar(GameObject obstacle, string line, int pending)
    {
        int aleatorio = Random.Range(1, 6);
        int positionsFreee = 0;
        GameObject f;
        if (pending == 0)
        {
            switch (aleatorio)
            {
                case 1:
                    Destroy(obstacle);
                    break;
                case 2:
                    f = instanciar(1,obstacle);
                    f.transform.position += (Vector3.up * 2f);
                    
                    break;
                case 3:
                    f = instanciar(1, obstacle);
                    break;
                case 4:
                    f = instanciar(2, obstacle);
                    f.transform.position += (Vector3.left * 1.5f);
                    if (line.Equals("A"))
                    {
                        f.transform.position -= (Vector3.left * 1.5f);
                        positionsFreeA++;
                    }else if (line.Equals("C"))
                    {
                        positionsFreeB++;
                    }
                    else if (line.Equals("B"))
                    {
                        positionsFreeA++;
                    }
                    break;

                case 5:
                    obstacle.SetActive(false);
                    positionsFreee = 3;
                    switch (line)
                    {
                        case "A":
                            pendientA = 2;
                            break;
                        case "B":
                            pendientB = 2;
                            break;
                        case "C":
                            pendientC = 2;
                            break;
                    }
                    break;
                case 6:
                    Destroy(obstacle);
                    break;
            }
        }
        else
        {
            switch (pending)
            {
                case 1:
                    obstacle.SetActive(false);
                    break;
                case 2:
                    f = instanciar(0, obstacle);
                    break;
            }
        }
        return positionsFreee;
    }

    /// <summary>
    /// Intacncia el obstaculo en base a al point
    /// </summary>
    /// <param name="prefab">Prefab que se va a poner en reemplazo del point.</param>
    /// <param name="point">Point que va a ser eliminado y reemplazado.</param>
    private GameObject instanciar(int prefab, GameObject point)
    {
        GameObject obstacle;

        obstacle = Instantiate(obstaclesModel[prefab]);
        obstacle.transform.SetParent(point.transform.parent);
        obstacle.transform.position = point.transform.position;
        Destroy(point);
        return obstacle;
    }

}

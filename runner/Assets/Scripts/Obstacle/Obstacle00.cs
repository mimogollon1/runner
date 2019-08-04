using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle00 : MonoBehaviour
{
    public GameObject[,] obstacles;

    // temporal
    public Material[] material;

    private int speed = 8;

    // Start is called before the first frame update
    void Start()
    {
        Cargar();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));
    }

    /// <summary>
    /// Genera obstaculos de manera aleatoria (Rojo = alto; Azul = medio; Magenta = Bajo; cyan = carro; Verde = Inamobible)
    /// </summary>
    public void Cargar()
    {
        obstacles = new GameObject[5, 3];
        int j = 0;

        for (int i = 0; i < gameObject.transform.childCount; i = i + 3)
        {
            obstacles[j, 0] = gameObject.transform.GetChild(i).gameObject;
            obstacles[j, 1] = gameObject.transform.GetChild(i + 1).gameObject;
            obstacles[j, 2] = gameObject.transform.GetChild(i + 2).gameObject;
            j++;
        }

        for (int f = 0; f < obstacles.GetLength(0); f++)
        {
            for (int c = 0; c < obstacles.GetLength(1); c++)
            {
                int aleatorio = Random.Range(1, 6);

                if (obstacles[f, c].activeSelf == false)
                {
                    switch (aleatorio)
                    {
                        case 1:
                            obstacles[f, c].SetActive(true);
                            obstacles[f, c].GetComponent<Renderer>().material = material[0];
                            break;
                        case 2:
                            obstacles[f, c].SetActive(true);
                            obstacles[f, c].GetComponent<Renderer>().material = material[1];
                            break;
                        case 3:
                            obstacles[f, c].SetActive(true);
                            obstacles[f, c].GetComponent<Renderer>().material = material[2];
                            break;
                        case 4:
                            if (f <= 1)
                            {
                                obstacles[f, c].SetActive(true);
                                obstacles[f, c].GetComponent<Renderer>().material = material[3];
                                obstacles[f + 1, c].SetActive(true);
                                obstacles[f + 1, c].GetComponent<Renderer>().material = material[3];
                                obstacles[f + 2, c].SetActive(true);
                                obstacles[f + 2, c].GetComponent<Renderer>().material = material[3];
                                obstacles[f + 3, c].SetActive(true);
                                obstacles[f + 3, c].GetComponent<Renderer>().material = material[3];
                            }
                            else
                            {
                                obstacles[f, c].SetActive(false);
                            }
                            break;
                        case 5:
                            if (c < 2)
                            {
                                if (obstacles[f, c].activeSelf == false && obstacles[f, c + 1].activeSelf == false)
                                {
                                    obstacles[f, c].SetActive(true);
                                    obstacles[f, c].GetComponent<Renderer>().material = material[4];
                                    obstacles[f, c + 1].SetActive(true);
                                    obstacles[f, c + 1].GetComponent<Renderer>().material = material[4];
                                }
                                else
                                {
                                    obstacles[f, c].SetActive(false);
                                }
                            }
                            else
                            {
                                obstacles[f, c].SetActive(false);
                            }
                            break;
                        case 6:
                            Destroy(obstacles[f, c]);
                            break;
                    }
                }
            }
        }
    }
}

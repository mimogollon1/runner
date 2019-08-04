using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform enemy;
    float ramdomX, ramdomZ;
    public GameObject destiny;

    // Start is called before the first frame update
    void Start()
    {
        ramdomX = Random.Range(-4.1f, 4.06f);
        ramdomZ = Random.Range(1.6f, 34f);
        destiny.transform.position = new Vector3(ramdomX, -4.84f, ramdomZ);
        Instantiate(destiny);
    }

    // Update is called once per frame
    void Update()
    {

        float distance;


        distance = Vector3.Distance(enemy.transform.position, new Vector3(ramdomX, -4.34f, ramdomZ));

        if (distance > ramdomZ / 2)
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(ramdomX, -1.34f, ramdomZ), 15 * Time.deltaTime);
        else
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(ramdomX, -4.34f, ramdomZ), 15 * Time.deltaTime);

    }
}

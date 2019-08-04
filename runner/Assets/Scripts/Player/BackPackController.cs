using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPackController : MonoBehaviour
{    
    public GameObject[] garbage;

    public List<GameObject> garbageActive;

    private float rangeRecolect = 10f;

    // Start is called before the first frame update
    void Start()
    {
        garbageActive = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        ClassifyGarbage();

        RecolectGarbage();
    }

    /// <summary>
    /// Clasifica todos los objectos existentes que son de tipo Garbage y los almacena
    /// </summary>    
    private void ClassifyGarbage()
    {
        garbage = GameObject.FindGameObjectsWithTag("Garbage");

        float distance;

        foreach (GameObject item in garbage)
        {
            distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance < rangeRecolect)
            {
                ResetGarbage(item);

                garbageActive.Add(item);
            }
        }
    }

    /// <summary>
    /// Despues de haber clasificado los objectos Garbage, se procede a desplazarlos hasta la Quantum Backpack
    /// </summary>    
    private void RecolectGarbage()
    {
        float distance;

        for (int i = 0; i < garbageActive.Count; i++)
        {
            distance = Vector3.Distance(garbageActive[i].transform.position, transform.position);
           
            if (distance > 2f)
                garbageActive[i].transform.position = Vector3.MoveTowards(garbageActive[i].transform.position, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), 30 * Time.deltaTime);
            else if (distance > 1.2f && distance < 2f)
                garbageActive[i].transform.position = Vector3.MoveTowards(garbageActive[i].transform.position, transform.position, 15 * Time.deltaTime);
            else
            {
                Destroy(garbageActive[i]);
                garbageActive.Remove(garbageActive[i]);
            }
        }
    }

    /// <summary>
    /// Reinicia los objectos tipo Garbage para que no sean detectados nuevamente y sean procesados una única vez
    /// </summary>
    /// <param name="garbage">objecto que sera reiniciado.</param>
    private void ResetGarbage(GameObject garbage)
    {
        garbage.transform.parent = null;
        garbage.tag = "Untagged";
    }
}
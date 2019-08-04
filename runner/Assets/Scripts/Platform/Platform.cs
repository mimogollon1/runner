using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int speed = 12;
        
    public GameObject garbage;

    public GameObject[] building;

    public Transform buildingParent;

    // Start is called before the first frame update
    void Start()
    {
        generateGarbage();
        generateGarbage();

        buildingParent = transform.Find("Buildings");

        generateBuildings();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));
    }

    /// <summary>
    /// Genera un objecto tipo Garbage dentro de la plataforma
    /// </summary>    
    public void generateGarbage()
    {
        GameObject newGarbage = Instantiate(garbage, transform.position, Quaternion.identity);
        newGarbage.transform.parent = transform;
        newGarbage.transform.localPosition = new Vector3(Random.Range(-4, 4), 0.5f, Random.Range(-4, 4));
    }

    /// <summary>
    /// Genera los edificios correspondientes a los dos costados
    /// </summary>    
    public void generateBuildings()
    {
        GameObject newBuildingLeft = Instantiate(building[Random.Range(0, building.Length)], transform.position, Quaternion.identity);

        newBuildingLeft.transform.parent = buildingParent;
        newBuildingLeft.transform.localPosition = new Vector3(-5.4f, 0, 0);

        Vector3 rot = newBuildingLeft.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);

        GameObject newBuildingRight = Instantiate(building[Random.Range(0, building.Length)], transform.position, Quaternion.Euler(rot));

        newBuildingRight.transform.parent = buildingParent;
        newBuildingRight.transform.localPosition = new Vector3(5.4f, 0, 0);
    }
}

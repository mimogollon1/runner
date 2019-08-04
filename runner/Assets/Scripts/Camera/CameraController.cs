using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Propiedades y Atributos

    public Transform playerTarget;

    #endregion
    
    void Start()
    {
        
    }
    
    void Update()
    {
        transform.position = new Vector3(playerTarget.position.x, transform.position.y, transform.position.z);
    }
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stop"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
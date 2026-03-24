using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    public float verticality = 0.6f;
    public float speed = 2f;
    private Vector3 startPos;


    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * verticality;
        transform.position = startPos + new Vector3 (0, yOffset, 0);
    }
}

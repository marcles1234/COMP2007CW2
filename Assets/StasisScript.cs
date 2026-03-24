using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StasisScript : MonoBehaviour
{
    public Transform InteractorSource;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask detectableLayer;
    public GameObject hitMarker;
    public MeshRenderer mesh;
    public Text crosshair;
    public Text shootText;

    void Start()
    {
        mesh.enabled = true;
        crosshair.enabled = true;
    }

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SingleRaycast(r);
        }
        Debug.DrawRay(InteractorSource.position, InteractorSource.forward * 100, Color.red, 1f);
    }

    void SingleRaycast(Ray ray)
    {
        if (Physics.Raycast(ray, out hit, 100f, detectableLayer))
        {
            shootText.enabled = false;
            //Debug.Log("Ray hit: " + hit.collider.name);
            GameObject targetObject = hit.collider.gameObject;
            ObjectMove script = targetObject.GetComponent<ObjectMove>();
            script.StartCoroutine(script.stasis());
            Vector3 impactPoint = hit.point;
            Quaternion facingRotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
            Destroy(Instantiate(hitMarker, impactPoint, facingRotation), 2f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StasisScript : MonoBehaviour
{
    public Transform InteractorSource;
    public Transform PitchforkTip;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask detectableLayer;
    public MeshRenderer mesh;
    public Text crosshair;
    public Text shootText;
    public LineRenderer line;

    void Start()
    {
        crosshair.enabled = true;
        line.enabled = false;
    }

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SingleRaycast(r);
        }
        line.SetPosition(0, PitchforkTip.position);
    }

    void SingleRaycast(Ray ray)
    {
        if (Physics.Raycast(ray, out hit, 100f, detectableLayer))
        {
            shootText.enabled = false;
            GameObject targetObject = hit.collider.gameObject;
            ObjectMove script = targetObject.GetComponent<ObjectMove>();
            script.StartCoroutine(script.stasis());
            Vector3 impactPoint = hit.point;

            line.enabled = true;
            line.SetPosition(1, impactPoint);
            StartCoroutine(HideLine());
        }
    }

    IEnumerator HideLine()
    {
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;
    }
}

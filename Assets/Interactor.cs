using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public Transform holdPoint;
    private GameObject heldObject = null;
    private bool inZone = false;
    public Trigger triggerZone;
    public Text interactText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
                {
                    GameObject targetObject = hitInfo.collider.gameObject;

                    interactText.enabled = false;

                    // Interact if possible
                    if (targetObject.TryGetComponent<IInteractable>(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }

                    // Pickup if possible
                    if (targetObject.TryGetComponent<Pickupable>(out Pickupable pu))
                    {
                        targetObject.transform.SetParent(holdPoint);
                        targetObject.transform.localPosition = Vector3.zero;
                        targetObject.transform.localRotation = Quaternion.identity;

                        // Stop movement if it has ObjectMove
                        if (targetObject.TryGetComponent<ObjectMove>(out ObjectMove mover))
                        {
                            mover.canMove = false;
                        }
                        if (inZone)
                        {
                            triggerZone.ObjectDeposit(-1);
                        }
                        heldObject = targetObject;
                    }
                }
            }
            else
            {
                heldObject.transform.SetParent(null);
                Vector3 dropPosition = transform.position + transform.forward * 10f;
                heldObject.transform.position = new Vector3(dropPosition.x, 0.2f, dropPosition.z);

                if (heldObject.TryGetComponent<Pickupable>(out Pickupable pickup))
                {
                    pickup.isHeld = false;
                }

                // Re-enable movement
                if (heldObject.TryGetComponent<ObjectMove>(out ObjectMove mover))
                {
                    mover.canMove = !inZone;  // Only allow movement if not in drop zone
                }

                // Notify trigger if in zone
                if (inZone)
                {
                    triggerZone.ObjectDeposit(1);
                } else
                {
                    mover.speed = mover.runSpeed;
                    mover.StartCoroutine(mover.teleport());
                }

                heldObject = null;
            }
        }
        //Debug.DrawRay(InteractorSource.position, InteractorSource.forward * InteractRange, Color.red);
    }

    public void InDropoffZone(bool trigger)
    {
        inZone = trigger;
    }
}


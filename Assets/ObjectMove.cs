using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Transform checkPoints;
    private int currentPointIndex = 0;
    private Vector3 currentPoint = Vector3.zero;
    float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float speed;
    private Vector3 target;
    public bool canMove = true;
    public bool pointIncrementPositive = true;
    public PlayerSphere playerSphere;
    float stasisTime = 10f;
    public bool inStasis = false;
    public Animator anim;
    int damping = 2;

    void Start()
    {
        speed = walkSpeed;
        anim = GetComponentInChildren<UnityEngine.Animator>();
        transform.position = checkPoints.GetChild(0).position;
        StartCoroutine(WaitToGetY());
        NextPoint();
    }

    void Update()
    {
        anim.SetBool("Run", canMove);
        anim.speed = speed;
        if (!canMove)
        {
            speed = 1f;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        Vector2 posXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetXZ = new Vector2(currentPoint.x, currentPoint.z);
        var lookPos = currentPoint - transform.position;
        lookPos.y = 0;
        var roatation = Quaternion.LookRotation(lookPos) *Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, roatation, Time.deltaTime * damping);
        if (Vector2.Distance(posXZ, targetXZ) < 0.05f)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        if (pointIncrementPositive) {
            currentPointIndex++;
        } else
        {
            if (currentPointIndex-1 < 0) {
                currentPointIndex = checkPoints.childCount-1;
            } else
            {
                currentPointIndex--;
            }
        }

        if (currentPointIndex >= checkPoints.childCount && pointIncrementPositive)
        {
            currentPointIndex = 0;
        }

        currentPoint = checkPoints.GetChild(currentPointIndex).position;
        target = new Vector3 (currentPoint.x, transform.position.y, currentPoint.z);
    }

    public GameObject findCheckpoint()
    {
        return checkPoints.GetChild(currentPointIndex).gameObject;
    }

    public void inDanger(bool isInDanger, bool swapDirection)
    {
        if (!inStasis)
        {
            if (isInDanger)
            {
                speed = runSpeed;
                if (swapDirection)
                {
                    if (pointIncrementPositive)
                    {
                        pointIncrementPositive = false;
                    }
                    else
                    {
                        pointIncrementPositive = true;
                    }
                    NextPoint();
                }
            }
            else
            {
                speed = walkSpeed;
            }
        }
    }

    public IEnumerator stasis()
    {
        inStasis = true;
        speed = 0f;
        yield return new WaitForSeconds(stasisTime);
        inStasis = false;
        speed = walkSpeed;
    }

    public IEnumerator teleport()
    {
        SkinnedMeshRenderer[] skinned = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 6; i > 0; i--)
        {
            float blinkTime = i / 10f;
            foreach (SkinnedMeshRenderer s in skinned)
            {
                s.enabled = false;
            }
            yield return new WaitForSeconds(blinkTime);
            foreach (SkinnedMeshRenderer s in skinned)
            {
                s.enabled = true;
            }
            yield return new WaitForSeconds(blinkTime);
        }
        transform.position = target;
    }

    IEnumerator WaitToGetY()
    {
        yield return new WaitForSeconds(0.5f);
        target = new Vector3(currentPoint.x, transform.position.y, currentPoint.z);
    }

}

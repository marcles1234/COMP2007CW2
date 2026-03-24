using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Transform checkPoints;
    private int currentPointIndex = 0;
    private Vector3 currentPoint = Vector3.zero;
    public float speed = 2;
    private Vector3 target;
    public bool canMove = true;
    public bool pointIncrementPositive = true;
    public PlayerSphere playerSphere;
    float stasisTime = 10f;
    bool inStasis = false;

    void Start()
    {
        transform.position = checkPoints.GetChild(0).position;
        StartCoroutine(WaitToGetY());
        NextPoint();
    }

    void Update()
    {

        if (!canMove) return;

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        Vector2 posXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetXZ = new Vector2(currentPoint.x, currentPoint.z);
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
                speed = 5f;
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
                speed = 2f;

            }
        }
    }

    public IEnumerator stasis()
    {
        inStasis = true;
        speed = 0f;
        yield return new WaitForSeconds(stasisTime);
        inStasis = false;
        speed = 2f;
    }

    IEnumerator WaitToGetY()
    {
        yield return new WaitForSeconds(0.5f);
        target = new Vector3(currentPoint.x, transform.position.y, currentPoint.z);
    }

}

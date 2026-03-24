using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    //Chicken detection variables
    public float detectionRadius = 5f;
    private Collider[] hits;
    public LayerMask detectableLayer;
    private List<ObjectMove> currentlyDetected = new List<ObjectMove> ();

    //Checkpoint detection variables
    public GameObject[] allCheckpoints;
    public GameObject nearestCheck;
    float distance;
    float nearsetDistance = 10000;


    void Update()
    {
        //----Detect Chicken's checkpoints----
        nearsetDistance = Mathf.Infinity;
        allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoints");
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, allCheckpoints[i].transform.position);

            if (distance < nearsetDistance)
            {
                nearestCheck = allCheckpoints[i];
                nearsetDistance = distance;
            }
            
        }
        //Debug.Log("Nearest point: " + nearestCheck.name);


        //----Detect Chickens----
        hits = Physics.OverlapSphere(transform.position, detectionRadius, detectableLayer); //Find chickens
        List<ObjectMove> newDetected = new List<ObjectMove>(); //Find list of objects with ObjectMove script
        foreach (var hit in hits) //Loop through hits
        {
            if (hit.gameObject == gameObject) continue; // Skip the player itself

            ObjectMove script = hit.GetComponent<ObjectMove>(); //Get the script from the hit object

            if (!currentlyDetected.Contains(script))
            {
                GameObject travellingTo = script.findCheckpoint();
                //Debug.Log("Object travelling to: " + travellingTo.name);
                if (nearestCheck == travellingTo)
                {
                    script.inDanger(true, true);
                }
                else
                {
                    script.inDanger(true, false);
                }
            }
            newDetected.Add(script); //Add object to list of objects
        }
        foreach (var obj in currentlyDetected) //Checks for objects that are no longer detected
        {
            if (!newDetected.Contains(obj))
            {
                obj.inDanger(false, false);
            }
        }
        currentlyDetected = newDetected; //Add newly detected components to total list
    }

}

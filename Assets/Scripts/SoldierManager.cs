using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class SoldierManager : MonoBehaviour
{
    [SerializeField] private SoldierController[] soldierPrefabs;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
	private List<SoldierController> soldiers = new List<SoldierController>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Only attempt to spawn when the user touches the screen
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        // Only attempt to spawn at the start of the touch
        if (touch.phase == TouchPhase.Began)
        {
            // Determine the position of the touch on the plane
			if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
			{
                // Use the first hit's Pose
				Pose pose = hits[0].pose;

                // Instantiate a new soldier
                int soldierIndex = Random.Range(0, soldierPrefabs.Length);
				SoldierController newSoldier = Instantiate(soldierPrefabs[soldierIndex], pose.position, pose.rotation);
                newSoldier.transform.Rotate(new Vector3(0, 180, 0));
                soldiers.Add(newSoldier);

                // If this soldier completes a pair, start the attack sequence
                if (soldiers.Count % 2 == 0)
                    BeginAttack();
			}
        }
    }

    private void BeginAttack()
    {
        // Grab the two most recently created soldiers
        SoldierController first = soldiers[soldiers.Count - 1];
        SoldierController second = soldiers[soldiers.Count - 2];

        // Tell them to attack each other
        first.AttackSoldier(second);
        second.AttackSoldier(first); 
    }
}

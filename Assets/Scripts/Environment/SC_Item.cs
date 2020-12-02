using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static EasingFunction;
using static SC_ItemTypes;

public class SC_Item : MonoBehaviour {
    
    public int ID;
    public string itemName;

    public Sprite icon;
    public ItemType type;

    public bool isPickedUp;

    private SC_Inventory inventory;

    private bool isOnTargetPos = true;
    public Vector3 targetPosition;


    void Awake() {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
        targetPosition = transform.position;
    }

    void Start() {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
        targetPosition = transform.position;
    }

    private void Update() {
        if (isOnTargetPos) {
            if (!isOnGround()) {
                transform.position += new Vector3(0, -0.25f, 0); 
            }
        }
    }

    void AnimateMovementToTarget() {
        StartCoroutine(
            Vector3Ease(
                (result => transform.position = new Vector3(result.x, transform.position.y, result.z)),
                transform.position, 
                targetPosition, 
                Ease.EaseInSine,
                .025f
            )
        );

        StartCoroutine(
            Vector3Ease(
                (result => {
                    if (result.y > 0.9f) {
                        isOnTargetPos = true;
                    } else {
                        transform.position += result;
                    }
                }
                ),
                new Vector3(0, 0.0f, 0), 
                new Vector3(0, 1.0f, 0), 
                Ease.Spring,
                .03f
            )
        );

        // StartCoroutine(
        //     Vector3Ease(
        //         (result => transform.localScale = result),
        //         new Vector3(0, 0, 0), 
        //         new Vector3(1, 1, 1), 
        //         Ease.Spring,
        //         .03f
        //     )
        // );
    }

    public void PickItem() {
        gameObject.SetActive(false);
        isPickedUp = true;
        inventory.pointsForSorting++;
        Debug.Log("Item picked up, points: " + inventory.pointsForSorting);
    }

    public void DropItem() {
        isPickedUp = false;
        gameObject.SetActive(true);

        isOnTargetPos = false;
        AnimateMovementToTarget();

        // TODO: переделать по-умному, чтобы злой подросток очки не отнимал
        // inventory.pointsForSorting--;
        // Debug.Log("Item dropped, points: " + inventory.pointsForSorting);
    }

    private bool isOnGround() {
        Vector3 startPos = transform.position + new Vector3(0, 1.0f, 0);
        Vector3 direction = Vector3.down;

        return getRayCollision(startPos, direction, 1.5f, 8);
	}

    private bool getRayCollision(Vector3 startPos, Vector3 direction, float rayLength, int layerNumber) {
        int layerMask = 1 << layerNumber;

        if (!Physics.Raycast(startPos, direction, out RaycastHit hit, rayLength, layerMask)) {
			return false;
		}

        return true;
    }

    IEnumerator Vector3Ease(Action<Vector3> valueToChange, Vector3 from, Vector3 to, Ease easingType, float interpolationIncrement) {
        Vector3 interpolationRatio;
        float interpolationProgress = 0.0f;

        Function easingFunc = GetEasingFunction(easingType);


        while (interpolationProgress < 1.0f) {
            var x = easingFunc(from.x, to.x, interpolationProgress);
            var y = easingFunc(from.y, to.y, interpolationProgress);
            var z = easingFunc(from.z, to.z, interpolationProgress);

            interpolationRatio = new Vector3(x, y, z);
            interpolationProgress += interpolationIncrement;
            
            valueToChange(interpolationRatio);

            yield return new WaitForEndOfFrame();
        }
    }
}

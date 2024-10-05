using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private GameObject hook;
    [SerializeField] private LayerMask fishLayer;
    [SerializeField] private float hookSpeed = 5f;

    private Vector3 hookStartPosition;
    private Vector3 targetPosition;
    private bool isHookMoving = false;
    private GameObject hookedFish = null;

    void Start()
    {
        hookStartPosition = hook.transform.position;
    }

    void Update()
    {
        if (hookedFish != null)
        {
            hookedFish.transform.position = hook.transform.position;
        }

        if (hook.transform.position == hookStartPosition)
        {
            ResetHook();
        }

        CheckForFishCollision();

        if (Input.GetMouseButtonDown(0) && !isHookMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isHookMoving = true;
            }
        }

        if (isHookMoving)
        {
            MoveHook();
        }
    }

    void MoveHook()
    {
        hook.transform.position = Vector3.MoveTowards(hook.transform.position, targetPosition, hookSpeed * Time.deltaTime);

        if (hook.transform.position == targetPosition)
        {
            if (hookedFish == null)
            {
                targetPosition = hookStartPosition;
            }
            else
            {
                hookedFish.transform.position = hook.transform.position;
                targetPosition = hookStartPosition;
            }

            if (hook.transform.position == hookStartPosition)
            {
                ResetHook();
            }
        }

        CheckForFishCollision();
    }

    void CheckForFishCollision()
    {
        Collider[] fishColliders = Physics.OverlapSphere(hook.transform.position, 0.5f, fishLayer);

        if (fishColliders.Length > 0 && hookedFish == null)
        {
            hookedFish = fishColliders[0].gameObject;
            Debug.Log("Fish Caught!");
        }
    }

    void ResetHook()
    {
        isHookMoving = false;
        hookedFish = null;
        hook.transform.position = hookStartPosition;
    }

}

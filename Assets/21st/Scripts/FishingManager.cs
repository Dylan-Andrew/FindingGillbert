using System.Collections;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject topGun;
    [SerializeField]
    private GameObject hook;
    [SerializeField]
    private LayerMask fishLayer;
    [SerializeField]
    private float hookSpeed = 5f;
    [SerializeField]
    private AudioSource ShootingAudio;

    private Vector3 targetPosition;
    private bool isHookMoving = false;
    private GameObject hookedFish = null;
    private bool canCastHook = true;
    private Quaternion originalHookRotation;
    private Vector3 hookStartPosition;
    void Start()
    {
        hookStartPosition = hook.transform.position;
        originalHookRotation = hook.transform.rotation;
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
        if (Input.GetMouseButtonDown(0) && !isHookMoving && canCastHook)
        {
            if (ShootingAudio != null)
            {
                ShootingAudio.Play();
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isHookMoving = true;
                StartCoroutine(HookFishingProcess());
            }
        }

        if (isHookMoving)
        {
            MoveHook();
        }
    }


    private IEnumerator HookFishingProcess()
    {
        yield return new WaitUntil(() => hook.transform.position == targetPosition);
        if (hookedFish == null)
        {
            gameManager.DecreaseHookCount();
        }
        else
        {
            HandleCaughtFish();
        }
    }

    private void MoveHook()
    {
        if (gameManager.IsGameOver()) return;

        Vector3 directionToTarget = targetPosition - hook.transform.position;

        hook.transform.position = Vector3.MoveTowards(hook.transform.position, targetPosition, hookSpeed * Time.deltaTime);

        if (hook.transform.position != hookStartPosition && hook.transform.position != targetPosition)
        {
            if (targetPosition != hookStartPosition)
            {
                RotateHookTowardsTarget(directionToTarget);
            }
        }

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

    private void RotateHookTowardsTarget(Vector3 directionToTarget)
    {
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation *= Quaternion.Euler(0, 90, 0);

        hook.transform.rotation = Quaternion.Slerp(hook.transform.rotation, targetRotation, Time.deltaTime * hookSpeed);
    }

    private void HandleCaughtFish()
    {
        canCastHook = false;

        gameManager.DecreaseHookCount();

        RandomMovement randomMovement = hookedFish.GetComponent<RandomMovement>();
        if (randomMovement != null)
        {
            randomMovement.enabled = false;
        }

        Fish fishComponent = hookedFish.GetComponent<Fish>();
        if (fishComponent != null && fishComponent.fishData != null)
        {
            if (fishComponent.fishData.fishName == "Gilbert")
            {
                gameManager.Win();
            }
            gameManager.DisplayFishCaught(fishComponent.fishData.fishName);
        }

        Destroy(hookedFish, 3f);
        Invoke(nameof(ResetCatch), 3f);
    }

    private void ResetCatch()
    {
        canCastHook = true;
    }

    private void CheckForFishCollision()
    {
        Collider[] fishColliders = Physics.OverlapSphere(hook.transform.position, 0.5f, fishLayer);

        if (fishColliders.Length > 0 && hookedFish == null)
        {
            hookedFish = fishColliders[0].gameObject;
        }
    }

    private void ResetHook()
    {
        isHookMoving = false;
        hookedFish = null;
        hook.transform.position = hookStartPosition;
        hook.transform.rotation = originalHookRotation;
    }
}

using System.Collections;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private GameObject hook;
    [SerializeField] private LayerMask fishLayer;
    [SerializeField] private float hookSpeed = 5f;

    private Vector3 hookStartPosition;
    private Vector3 targetPosition;
    private bool isHookMoving = false;
    private GameObject hookedFish = null;
    private bool canCastHook = true; // Flag to track if the hook can be cast

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

        // Check if the hook can be cast and the player clicks
        if (Input.GetMouseButtonDown(0) && !isHookMoving && canCastHook)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isHookMoving = true;
                StartCoroutine(HookFishingProcess()); // Start the hooking process
            }
        }

        if (isHookMoving)
        {
            MoveHook();
        }
    }

    private IEnumerator HookFishingProcess()
    {
        yield return new WaitUntil(() => hook.transform.position == targetPosition); // Wait until the hook reaches the target

        // Check if a fish was caught
        if (hookedFish == null)
        {
            // Decrease hook count if no fish was caught
            gameManager.DecreaseHookCount();
        }
        else
        {
            HandleCaughtFish(); // Handle the case of catching fish
        }
    }

    private void MoveHook()
    {
        if (gameManager.IsGameOver()) return;
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

    private void HandleCaughtFish()
    {
        // Prevent casting while fish is caught
        canCastHook = false;

        // Decrease the hook count since a fish is caught
        gameManager.DecreaseHookCount();

        RandomMovement randomMovement = hookedFish.GetComponent<RandomMovement>();
        if (randomMovement != null)
        {
            randomMovement.enabled = false;
        }

        Fish fishComponent = hookedFish.GetComponent<Fish>();
        if (fishComponent != null && fishComponent.fishData != null)
        {
            Debug.Log($"Caught: {fishComponent.fishData.fishName}");
            gameManager.DisplayFishCaught(fishComponent.fishData.fishName);
            if (fishComponent.fishData.fishName == "Gillbert")
            {
                gameManager.Win();
            }
        }

        // Destroy the fish after a delay
        Destroy(hookedFish, 5f);
        Invoke(nameof(ResetCatch), 5f); // Reset the ability to cast after the fish is destroyed
    }

    private void ResetCatch()
    {
        canCastHook = true; // Allow casting again after the fish is destroyed
    }

    private void CheckForFishCollision()
    {
        Collider[] fishColliders = Physics.OverlapSphere(hook.transform.position, 0.5f, fishLayer);

        if (fishColliders.Length > 0 && hookedFish == null)
        {
            hookedFish = fishColliders[0].gameObject;
            Debug.Log("Fish Caught!");
        }
    }

    private void ResetHook()
    {
        isHookMoving = false;
        hookedFish = null;
        hook.transform.position = hookStartPosition;
    }
}

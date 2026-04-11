using TMPro;
using UnityEngine;

public class RadioTowerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI QuestUI;
    [SerializeField] private float detectionRadius = 3f;

    private Transform player;
    private PlayerMovement playerMovement;
    private bool isWorking = false;
    private float workTimer = 0f;
    private float workDuration = 5f;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool playerIsNear = distance <= detectionRadius;


        if (playerIsNear && !isWorking && Input.GetKeyDown(KeyCode.E))
        {
            isWorking = true;
            workTimer = workDuration;
            playerMovement?.SetWorking(true);
        }

        if (isWorking)
        {
            workTimer -= Time.deltaTime;
            if (workTimer <= 0f)
            {
                isWorking = false;
                playerMovement?.SetWorking(false);
                QuestUI.text = "Radio Tower repaired! Quest complete.";
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

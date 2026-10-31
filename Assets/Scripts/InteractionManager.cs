using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    // public enum InteractionType{
    //     Plant,
    // }
    [HideInInspector]
    public Movement playerMovement;

    [HideInInspector]
    public Watering watering;

    [HideInInspector]
    public GameManager gameManager;

    public GameObject gameManagerGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerGO = GameObject.Find("GameManager");
        gameManager = gameManagerGO.GetComponent<GameManager>();
        watering = this.gameObject.GetComponent<Watering>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction zone.");
            playerMovement = collision.GetComponent<Movement>();
            if (playerMovement != null)
            {
                watering.wateringUI.SetActive(true);
                watering.wateringValueUI = this.gameObject.GetComponent<Watering>().wateringValueUI;
                canInteract();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited interaction zone.");
            if (playerMovement != null)
            {
                watering.wateringUI.SetActive(false);
                stopInteract();
            }
        }
    }

    public void canInteract()
    {
        if (playerMovement == null)
        {
            return;
        }
        playerMovement.canInteract = true;
        playerMovement.watering = this.gameObject.GetComponent<Watering>();
    }

    public void stopInteract()
    {
        if (playerMovement == null)
        {
            return;
        }
        playerMovement.canInteract = false;
        playerMovement.watering = null;
    }
}

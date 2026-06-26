using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    //Manager for game
    public OpenAI_NPC_Behaviour gameManager;
    public PlayerController playerController;

    public GameObject interactionPrompt;

    NPC_Behaviour currentNPCToInteract;


    void Update()
    {
        // Input System Action is mapped in Player controller class on start of the game
        if (playerController.p_Interact.WasPressedThisFrame() && gameManager.IsPlayerSpeaking == false)
        {
            gameManager.StartNPCDialougue(currentNPCToInteract);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         Debug.Log("Its going");
        if (collision.GetComponent<NPC_Behaviour>() != null)
        {

            NPC_Behaviour tempNPC = collision.GetComponent<NPC_Behaviour>();
            currentNPCToInteract = tempNPC;
            interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentNPCToInteract.gameObject)
        {
            currentNPCToInteract = null;
            interactionPrompt.SetActive(false);

        }
    }
}

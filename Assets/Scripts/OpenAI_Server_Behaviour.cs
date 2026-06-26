using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEditor.MPE;
using UnityEditor;
//needed for Unity UI classes
using UnityEngine.UI;
// used for text or button UI variables
using TMPro;


public class OpenAI_NPC_Behaviour : MonoBehaviour
{
    [SerializeField] NPC_Behaviour activeNPC;
    // Sets overall rules of what to do and what not to do like this sprite is in a game and dont mention your an ai or outside world references
    [SerializeField][TextArea(minLines: 5, maxLines: 10)] string aiDialogueRules;

    [Header("Ai Components")]
    [SerializeField] GameObject dialougueUI;
    [SerializeField] GameObject playerUI;
    [SerializeField] TMP_Text npcDialogue;
    [SerializeField] TMP_InputField playerReply;
    [SerializeField] Image npcPortrait;

// used for testing only
     [SerializeField] NPC_Behaviour dummy;
    // Open Ai agent to talk back to OpenAi Servers
    OpenAIApi aiAgent = new OpenAIApi();
    // Storage for all player and Ai messages since Open Ai server does not save them
    List<ChatMessage> chatMessages = new List<ChatMessage>();

    public bool IsPlayerSpeaking;

    void Start()
    {
        IsPlayerSpeaking = false;
        // sends request to Ai servers 
        // StartNPCDialougue(dummy);
    }

    async void PlayerRequest(string _request)
    {
        //Update UI to show the player the Ai is thinking after their reply
        npcDialogue.text = "Hrrm.....";
        //empties player input field
        playerReply.text = string.Empty;
        //sets ui to inactive so player can't fire multiple messages
        playerUI.SetActive(false);

        // makes a request of the player to send to Open Ai server
        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = _request
        };

        if (chatMessages.Count == 0)
        {
            // As the first message sets the rules for the NPC
            newMessage.Content = $"Act as a {activeNPC.npcLook} in a fantasy RPG" +
            $"As a Character, you are {activeNPC.npcPersonality}" +
            $"Your location is {activeNPC.npcLocation}" +
            $"You have an answer for the player only when they interact with you: " +
            $"{activeNPC.npcAnswer}.\n{aiDialogueRules}";
        }

        chatMessages.Add(newMessage);

        // This sets the model and gives the storage of messages to send to the Ai server
        var newRequest = await aiAgent.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-5",
            Messages = chatMessages

        });
        // this adds the response to the List of messages to 
        if (newRequest.Choices != null)
        {
            ChatMessage aiResponse = newRequest.Choices[0].Message;
            chatMessages.Add(aiResponse);


            npcDialogue.text = aiResponse.Content;

            /* var debugText = aiResponse;
             // prints out message in debug console
             Debug.Log(debugText.Content.ToString());*/
        }
         playerUI.SetActive(true);
    }

    public void StartNPCDialougue(NPC_Behaviour _npc)
    {
        IsPlayerSpeaking = true;
        activeNPC = _npc;
        dialougueUI.SetActive(true);
        npcPortrait.sprite = _npc.npcSprite;
        Time.timeScale = 0f;
        chatMessages.Clear();
        PlayerRequest("");

    }
    // Function for Reply Button
    public void ReplyToNPC()
    {
        PlayerRequest(playerReply.text);
    }
    //Function for leaving NPC conversation
    public void LeaveConversation()
    {
        activeNPC = null;
        dialougueUI.SetActive(false);
        npcPortrait.sprite = null;
        Time.timeScale = 1f;
        chatMessages.Clear();
        IsPlayerSpeaking = false;
    }
}

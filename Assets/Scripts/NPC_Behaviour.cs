using JetBrains.Annotations;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour
{
    // For NPC sprite reference
    public Sprite npcSprite;
    // Field to give npc their description of how sprtie looks
   [TextArea] public string npcLook;
   // sets Personality on how the npc starts an interaction with the player(s)
    [TextArea] public string npcPersonality;
    // Gives npc an idea where they are in the world
   [TextArea] public string npcLocation;
   // sets what type of interaction with player will be done
   [TextArea] public string npcAnswer;
}

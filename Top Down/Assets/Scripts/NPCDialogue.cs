using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialouge", menuName = "Scriptable Objects/NPCDialouge")]
public class NPCDialouge : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    
}

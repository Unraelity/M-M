using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{

    [SerializeField] private string text;
    [SerializeField] private string speakerName;
    [SerializeField] private string voiceAudioKey;
    [SerializeField] private DialogueTextColor textColor;
    [SerializeField] private DialogueNode nextDialogueNode;
    public string Text => text;
    public string SpeakerName => speakerName;
    public string VoiceAudioKey => voiceAudioKey;
    public DialogueTextColor TextColor => textColor;
    public virtual DialogueNode NextDialogueNode => nextDialogueNode;

    public void SetNextDialogueNode(DialogueNode nextDialogueNode)
    {
        this.nextDialogueNode = nextDialogueNode;
    }
}

public enum DialogueTextColor
{
    Black,
    Red,
    Green,
    Blue,
    Yellow   
}

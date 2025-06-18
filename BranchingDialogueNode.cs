using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Branching Dialogue Node")]
public class BranchingDialogueNode : DialogueNode
{
    [SerializeField] private List<DialogueNode> nextDialogueOptions;
    public override DialogueNode NextDialogueNode => GetNextDialogueNode();

    public DialogueNode GetNextDialogueNode()
    {
        throw new System.NotImplementedException();
    }
}

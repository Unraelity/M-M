using System;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Bindings")]
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI tmpText;
    [Header("Text Speed")]
    [SerializeField] private float delayBetweenLetters = 0.025f;
    private bool inConversation;
    private DialogueNode currDialogueNode;
    private string currDialogueString;
    private int currStringPos;
    private float currTime;
    private string currVoiceAudioKey;
    public static DialogueManager Instance { get; private set; }
    // when conversation begins and ends
    public event Action OnConversationStart;
    public event Action OnConversationEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (inConversation)
        {
            currTime += Time.deltaTime;
            if (currTime > delayBetweenLetters)
            {
                currTime = 0;

                currStringPos++;
                if (currStringPos < currDialogueString.Length)
                {
                    tmpText.text += currDialogueString[currStringPos].ToString();
                }
                else
                {
                    //GameManager.Instance.AudioManager.StopSound(4);
                    inConversation = false;
                }

            }
        }
    }

    private void GetNextDialogue()
    {
        if (currDialogueNode != null)
        {
            textBox.SetActive(true);
            //OnConversationStart.Invoke();
            currDialogueString = currDialogueNode.Text;
            inConversation = true;
            currTime = 0;
            currStringPos = 0;
            tmpText.text = currDialogueString[currStringPos].ToString();
            currDialogueNode = currDialogueNode.NextDialogueNode;
        }
        else
        {
            //GameManager.Instance.AudioManager.StopSound(4);
            textBox.SetActive(false);
            OnConversationEnd.Invoke();
        }
    }

    public void ShowDialogue(DialogueNode rootDialogueNode)
    {
        currDialogueNode = rootDialogueNode;
        currVoiceAudioKey = rootDialogueNode.VoiceAudioKey;
        SetTextColor(rootDialogueNode.TextColor);
        GetNextDialogue();
    }

    private void SetTextColor(DialogueTextColor textColor)
    {
        switch (textColor)
        {
            case DialogueTextColor.Black:
                tmpText.color = Color.black;
                break;
            case DialogueTextColor.Red:
                tmpText.color = Color.red;
                break;
            case DialogueTextColor.Green:
                tmpText.color = Color.green;
                break;
            case DialogueTextColor.Blue:
                tmpText.color = Color.blue;
                break;
            case DialogueTextColor.Yellow:
                tmpText.color = Color.yellow;
                break;
        }
    }
}

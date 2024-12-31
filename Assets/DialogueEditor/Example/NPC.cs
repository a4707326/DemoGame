using UnityEngine;
using DialogueEditor;
using Unity.Properties;
using HeneGames.DialogueSystem;

public class NPC : MonoBehaviour
{
    public NPCConversation myConversation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("KeyCode.A)");
            ConversationManager.Instance.StartConversation(myConversation);
        }
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("KeyCode.B)");
            //DialogueUI.instance.StartDialogue();
        }
    }

    private void OnMouseOver()
    {
        Debug.Log("OnMouseOver");
        ConversationManager.Instance.StartConversation(myConversation);
    }
}

using System.Collections.Generic;

using UnityEngine;


namespace Game2
{
    public class Game2Mgr : MonoSingleton<Game2Mgr>
    {


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        void OnHelp()
        {
            Debug.Log("Player chose to help.");
            // Add more dialogue or gameplay logic here
        }

        void OnDecline()
        {
            Debug.Log("Player chose not to help.");
            // Add more dialogue or gameplay logic here
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("KeyCode.A)");

                //string[] dialogueLines = {
                //"Hello, adventurer!",
                //"Would you like to help us save the village?"
                //};

                //var options = new Dictionary<string, System.Action>
                //{
                //    { "Yes, I'll help!", OnHelp },
                //    { "No, I'm busy.", OnDecline }
                //};

                //DialogueMgr.Instance.StartDialogue(dialogueLines, options);



            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                //Dictionary<string, System.Action> options = new Dictionary<string, System.Action>
                //{
                //    { "讓他去吧(主動攻擊)", () => Debug.Log("") },
                //    { "口頭建議勇者進行友善交流", () => Debug.Log("") },
                //    { "施放洗腦術讓勇者進行友善交流", () => Debug.Log("") },
                //    { "施放火球術", () => Debug.Log("") }
                //};

                //DialogueMgr.Instance.StartDialogue("勇者A : 哈哈哈，又是個雜魚，看我收拾你", options);
            }
        }


        public void Talk()
        {

        }
    }
}

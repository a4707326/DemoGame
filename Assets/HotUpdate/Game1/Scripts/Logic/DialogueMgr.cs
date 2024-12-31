using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game1
{
    public class DialogueMgr : MonoSingleSingleton<DialogueMgr>
    {
        [Header("UI Elements")]
        public GameObject DialogueContainer;       // 顯示對話的容器
        public TMP_Text DialogueText;              // 顯示對話的文字
        public GameObject OptionsContainer;        // 選項按鈕的容器
        public GameObject OptionPrefab;            // 用於生成選項按鈕的Prefab

        [Header("Settings")]
        public float TypingSpeed = 0.05f;          // 控制文字顯示的速度
        public float OptionAppearDelay = 0.2f;     // 每個選項按鈕出現的延遲

        private bool isDialogueActive = false;     // 對話是否正在顯示
        private string currentDialogue;            // 當前的對話內容
        private Dictionary<string, System.Action> currentOptions; // 當前的選項

        private List<string> dialogueHistory = new List<string>(); // 記錄每次的對話
        private List<string> choiceHistory = new List<string>();   // 記錄每次選擇的選項

        public void StartDialogue(string dialogue, Dictionary<string, System.Action> options = null)
        {
            currentDialogue = dialogue;
            currentOptions = options ?? new Dictionary<string, System.Action>(); // 初始化選項

            dialogueHistory.Add(dialogue); // 記錄對話

            isDialogueActive = true;
            DialogueContainer.SetActive(true);
            OptionsContainer.SetActive(false); // 隱藏選項容器
            StartCoroutine(TypeDialogue());    // 開始逐字顯示文字
        }

        /// <summary>
        /// 逐字顯示對話文字
        /// </summary>
        private IEnumerator TypeDialogue()
        {
            DialogueText.text = string.Empty; // 清空對話文字

            foreach (char c in currentDialogue)
            {
                DialogueText.text += c; // 每次增加一個字
                yield return new WaitForSeconds(TypingSpeed); // 控制顯示速度
            }

            ShowOptions(); // 當文字顯示完畢後，顯示選項
        }

        /// <summary>
        /// 顯示選項按鈕
        /// </summary>
        private void ShowOptions()
        {
            OptionsContainer.SetActive(currentOptions.Count > 0); // 如果有選項，顯示容器

            // 清空舊的選項按鈕
            foreach (Transform child in OptionsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // 動態生成選項按鈕並逐步顯示
            StartCoroutine(ShowOptionsGradually());
        }

        /// <summary>
        /// 逐步顯示選項按鈕
        /// </summary>
        private IEnumerator ShowOptionsGradually()
        {
            foreach (var option in currentOptions)
            {
                GameObject optionButton = Instantiate(OptionPrefab, OptionsContainer.transform);
                optionButton.GetComponentInChildren<TMP_Text>().text = option.Key; // 設定按鈕文字
                optionButton.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option.Key, option.Value)); // 綁定點擊事件

                optionButton.SetActive(false); // 暫時隱藏按鈕
                yield return new WaitForSeconds(OptionAppearDelay); // 延遲顯示
                optionButton.SetActive(true);  // 顯示按鈕
            }
        }

        /// <summary>
        /// 當選項被選中時執行
        /// </summary>
        /// <param name="optionText">選項文字</param>
        /// <param name="optionAction">選項執行的動作</param>
        private void OnOptionSelected(string optionText, System.Action optionAction)
        {
            choiceHistory.Add(optionText); // 記錄選項
            optionAction?.Invoke(); // 執行選項的動作
            isDialogueActive = false; // 結束對話狀態
            OptionsContainer.SetActive(false); // 隱藏選項容器
        }

        /// <summary>
        /// 獲取對話記錄
        /// </summary>
        public List<string> GetDialogueHistory()
        {
            return dialogueHistory;
        }

        /// <summary>
        /// 獲取選項記錄
        /// </summary>
        public List<string> GetChoiceHistory()
        {
            return choiceHistory;
        }
    }
}

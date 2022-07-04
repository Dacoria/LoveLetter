using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Chibi.Free
{
    public enum DialogState
    {
        Show,
        Hide
    }

    public class Dialog : MonoBehaviour
    {

        private void OnEnable()
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        private void OnDisable()
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        private readonly int kDialogSort = 10100;
        private float alpha;
        private float addValue;
        private bool needCloseByTapBG;
        private List<ActionButton> actionButtons;
        private Action closedAction;
        private int fontSizeTitle;
        private int fontSizeOther;

        public DialogState state
        {
            get;
            private set;
        }
        public Text titleText;
        public Text messageText;
        public GameObject title;
        public GameObject message;
        public DialogButton dialogButton;
        public GameObject ifChild;
        public GameObject btnChild;
        public Action action;

        void Awake()
        {
            state = DialogState.Hide;
            alpha = 0;
            addValue = 5f;
            enabled = false;
            actionButtons = new List<ActionButton>();

            var size = Mathf.Min(Screen.width, Screen.height);
            fontSizeTitle = Mathf.Max(size / 15, 21);
            fontSizeOther = Mathf.Max(size / 20, 14);
            titleText.fontSize = fontSizeTitle;
            messageText.fontSize = fontSizeOther;

            ToBack();

        }
        void Update()
        {
            switch (state)
            {
                case DialogState.Show:
                    if (alpha < 1)
                    {
                        PlusAlpha(addValue);
                    }
                    break;
                case DialogState.Hide:
                    if (alpha > 0)
                    {
                        PlusAlpha(-addValue);
                        if (alpha < 0)
                        {
                            enabled = false;
                            ToBack();
                            DeleteButtons();
                            closedAction?.Invoke();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void ShowDialog(string txtTitle, string txtMessage, ActionButton[] acts = null, Action actClosed = null, bool needCloseByTapBG = false)
        {
            ToFront();

            enabled = true;

            titleText.text = txtTitle;
            messageText.text = txtMessage;

            title.SetActive(txtTitle != null);
            message.SetActive(txtMessage != null);

            closedAction = actClosed;

            this.needCloseByTapBG = needCloseByTapBG;

            state = DialogState.Show;

            actionButtons.Clear();            
            

            DeleteButtons();
            int idx = 0;

            foreach (var actButton in acts)
            {

                var btn = Instantiate(dialogButton);

                btn.transform.SetParent(btnChild.transform);

                btn.parent = this;

                btn.transform.localScale = new Vector3(1, 1, 1);

                btn.name = "btn_" + idx;
                btn.index = idx;
                idx += 1;

                var buttonText = btn.transform.Find("VLayout/Text").GetComponent<Text>();
                buttonText.text = actButton.text;

                buttonText.fontSize = fontSizeOther;

                buttonText.color = actButton.colorText;

                btn.GetComponent<Image>().color = actButton.colorBg;

                actionButtons.Add(actButton);
            }
        }


        private void PlusAlpha(float plus)
        {
            alpha += plus * Time.deltaTime;
            SetAlpha(alpha);
        }
        private void SetAlpha(float a)
        {
            var g = GetComponent<CanvasGroup>();
            g.alpha = a;
        }
       
        private void CloseDialog()
        {
            if (state == DialogState.Hide)
            {
                return;
            }
            alpha = 1f;
            state = DialogState.Hide;
        }
        
        public void OnClickButton(int idx)
        {
            ActionButton btn = actionButtons[idx];
            btn.action?.Invoke();
            CloseDialog();
        }
        
        public void OnClickBackground()
        {
            if (needCloseByTapBG)
            {
                CloseDialog();
            }
        }
        
        private void ToFront()
        {
            Sort(kDialogSort);
        }
        private void ToBack()
        {
            Sort(-kDialogSort);
        }
        private void Sort(int s)
        {
            var canvas = GetComponentInChildren<Canvas>();
            canvas.sortingOrder = s;
        }
        
        private void DeleteButtons()
        {
            GameObject child = btnChild;
            foreach (Transform btn in child.transform)
            {
                Destroy(btn.gameObject);
            }
        }
        
        public class ActionButton
        {
            public string text;
            public Action action;
            public Color colorBg;
            public Color colorText;

            public ActionButton(string text, Action action = null, Color? colorBg = null, Color? colorText = null)
            {
                this.text = text;
                this.action = action;
                this.colorBg = colorBg ?? Color.white;
                this.colorText = colorText ?? Color.black;
            }
        }
    }
}
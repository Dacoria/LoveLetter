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

        // Start is called before the first frame update
        void Start()
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

        // Update is called once per frame
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
                            // 閉じた後
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

        /// <summary>
        /// ダイアログをビューに追加
        /// どのボタンを押下してもダイアログは閉じられます。
        /// </summary>
        /// <param name="txtTitle">ダイアログタイトル</param>
        /// <param name="txtMessage">ダイアログ本文</param>
        /// <param name="acts">ボタンクリックコールバック（不要な場合は省略）</param>
        /// <param name="actClosed">ダイアログが閉じた後に返すコールバック</param>
        /// <param name="needCloseByTapBG">背景タップで閉じる場合はtrue（省略時：false）</param>
        public void ShowDialog(string txtTitle, string txtMessage, ActionButton[] acts = null, Action actClosed = null, bool needCloseByTapBG = false)
        {
            // 手前に表示
            ToFront();
            // タッチを受け付ける
            enabled = true;
            // タイトルと本文をセット
            titleText.text = txtTitle;
            messageText.text = txtMessage;
            // テキストが無い場合は非表示
            title.SetActive(txtTitle != null);
            message.SetActive(txtMessage != null);
            // ダイアログが閉じた際に返すコールバック
            closedAction = actClosed;
            // 背景タップで閉じる場合はtrue
            this.needCloseByTapBG = needCloseByTapBG;
            // アニメーションで表示開始
            state = DialogState.Show;
            // リストをクリア
            actionButtons.Clear();
            // ボタン削除
            DeleteButtons();
            int idx = 0;
            // ボタンを配置
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
        /// <summary>
        /// 全体にアルファを反映
        /// </summary>
        /// <param name="a">アルファ値</param>
        private void SetAlpha(float a)
        {
            var g = GetComponent<CanvasGroup>();
            g.alpha = a;
        }

        /// <summary>
        /// ダイアログを閉じる
        /// </summary>
        private void CloseDialog()
        {
            if (state == DialogState.Hide)
            {
                return;
            }
            alpha = 1f;
            state = DialogState.Hide;
        }

        /// <summary>
        /// 複製したボタンを押下した際に呼ばれる
        /// </summary>
        /// <param name="idx">押したボタンのインデックス</param>
        public void OnClickButton(int idx)
        {
            ActionButton btn = actionButtons[idx];
            btn.action?.Invoke();
            // どのボタンを押してもダイアログは閉じる
            CloseDialog();
        }

        /// <summary>
        /// バックグラウンドをタップ時に呼ばれる
        /// </summary>
        public void OnClickBackground()
        {
            if (needCloseByTapBG)
            {
                CloseDialog();
            }
        }


        /// <summary>
        /// ダイアログを手前に
        /// </summary>
        private void ToFront()
        {
            Sort(kDialogSort);
        }
        // ダイアログを奥に
        private void ToBack()
        {
            Sort(-kDialogSort);
        }
        private void Sort(int s)
        {
            var canvas = GetComponentInChildren<Canvas>();
            canvas.sortingOrder = s;
        }

        /// <summary>
        /// OKボタン等を削除する。
        /// </summary>
        private void DeleteButtons()
        {
            GameObject child = btnChild;
            foreach (Transform btn in child.transform)
            {
                // ボタン削除実行
                Destroy(btn.gameObject);
            }

        }

        /// <summary>
        /// ボタンのラベルとコールバック用
        /// </summary>
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
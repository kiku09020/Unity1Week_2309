using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.StageSelect
{
    public class StageFile : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] Button fileButton;
        [SerializeField] TextMeshProUGUI fileNameText;

        int stageNumber;
        string stageName;

        //--------------------------------------------------
        /// <summary> ステージファイルの初期化 </summary>
        public void SetStageFile(int stageNumber)
        {
            this.stageNumber = stageNumber;
            stageName = $"Stage_{this.stageNumber}.txt";

            fileNameText.text = stageName;

            // ボタン無効化
            SetButtonInteractable(false);
        }

        /// <summary> ボタンの有効化/無効化 </summary>
        public void SetButtonInteractable(bool value)
        {
            fileButton.interactable = value;
        }

        /// <summary> ステージファイルの読み込み </summary>
        public void LoadStageScene()
        {
            SceneManager.LoadScene(stageNumber);
        }
    }
}
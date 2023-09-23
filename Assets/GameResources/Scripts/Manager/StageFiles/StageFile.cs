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
        /// <summary> �X�e�[�W�t�@�C���̏����� </summary>
        public void SetStageFile(int stageNumber)
        {
            this.stageNumber = stageNumber;
            stageName = $"Stage_{this.stageNumber}.txt";

            fileNameText.text = stageName;

            // �{�^��������
            SetButtonInteractable(false);
        }

        /// <summary> �{�^���̗L����/������ </summary>
        public void SetButtonInteractable(bool value)
        {
            fileButton.interactable = value;
        }

        /// <summary> �X�e�[�W�t�@�C���̓ǂݍ��� </summary>
        public void LoadStageScene()
        {
            SceneManager.LoadScene(stageNumber);
        }
    }
}
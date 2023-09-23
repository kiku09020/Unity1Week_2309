using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StageSelect
{
	public class StageFileManager : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField] int stageCount = 3;        // �X�e�[�W��

		[Header("Generating")]
		[SerializeField] Transform parent;
		[SerializeField] StageFile stageFilePrefab;

		int stageClearCount;                        // �N���A�ς݂̃X�e�[�W��

		//--------------------------------------------------

		void Awake()
		{
			StageFile[] stageFiles = new StageFile[stageCount];

			// ����
			for (int i = 0; i < stageCount; i++) {
				stageFiles[i] = Instantiate(stageFilePrefab, parent);

				stageFiles[i].SetStageFile(i + 1);
			}

			// �N���A�ς݂̃X�e�[�W�����A�{�^����L����
			stageClearCount = PlayerPrefs.GetInt("StageClearCount", 0);

			for (int i = 0; i < stageClearCount + 1; i++) {
				stageFiles[i].SetButtonInteractable(true);
			}
		}
	}
}
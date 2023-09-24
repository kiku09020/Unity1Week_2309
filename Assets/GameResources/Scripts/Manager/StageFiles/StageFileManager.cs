using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StageSelect
{
	public class StageFileManager : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField] int stageCount = 3;        // ステージ数

		[Header("Generating")]
		[SerializeField] Transform parent;
		[SerializeField] StageFile stageFilePrefab;

		int stageClearCount;                        // クリア済みのステージ数

		//--------------------------------------------------

		public void SetStageFiles()
		{
			StageFile[] stageFiles = new StageFile[stageCount];

			// 生成
			for (int i = 0; i < stageCount; i++) {
				stageFiles[i] = Instantiate(stageFilePrefab, parent);

				stageFiles[i].SetStageFile(i + 1);
			}

			// クリア済みのステージ数分、ボタンを有効化
			stageClearCount = PlayerPrefs.GetInt("StageClearCount", 0);

			for (int i = 0; i < stageClearCount + 1; i++) {
				if (i < stageCount) {
					stageFiles[i]?.SetButtonInteractable(true);
				}
			}
		}

		private void Update()
		{
			// クリア数リセット
			if (Input.GetKeyDown(KeyCode.Delete) && Debug.isDebugBuild) {
				PlayerPrefs.DeleteKey("StageClearCount");
				print("Deleted StageClearCount.");
			}
		}
	}
}
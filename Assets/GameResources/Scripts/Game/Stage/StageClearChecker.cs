using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Stage
{
	/// <summary> ステージのクリア判定 </summary>
	public class StageClearChecker : MonoBehaviour
	{
		const int clearTileCount = 6;

		[SerializeField] TileBase[] targetTiles = new TileBase[clearTileCount];

		[SerializeField] StageManager stageManager;

		bool isContains;

		public event System.Action OnClearEvent;

		//--------------------------------------------------

		void Start()
		{
			stageManager.OnChangedEvent += CheckTiles;
		}

		public void CheckTiles()
		{
			var stageTiles = stageManager.GetStageTilesArray();

			// 列ごとに探索
			for (int x = 0; x < stageManager.Tilemap.size.x; x++) {
				for (int y = 0; y < stageManager.Tilemap.size.y; y++) {

					var tile = stageTiles[y, x];

					// タイルを含んでいたら、そのタイルを始点とする
					if (tile == targetTiles[0]) {
						if (!isContains) {
							isContains = true;
						}

						// 目標タイルと同じタイルだったら、次のタイルへ
						for (int i = 0; i < targetTiles.Length; i++) {
							if (stageTiles[y + i, x] != targetTiles[i]) {
								isContains = false;
								break;
							}
						}

						if (isContains) {
							print("clear");

							OnClearEvent?.Invoke();
						}

						break;
					}
				}

				if (isContains) {
					isContains = false;
					break;
				}
			}
		}
	}
}
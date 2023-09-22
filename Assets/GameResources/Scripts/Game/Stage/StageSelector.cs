using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Stage
{
	/// <summary> ステージの選択範囲 </summary>
	public class StageSelector : MonoBehaviour
	{
		[SerializeField] StageManager StageManager;
		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase selectTile;

		Vector3Int playerPrevTilePos;       // プレイヤーが前フレームにいたタイル座標

		//--------------------------------------------------

		public void SetSelectionTile(Vector3 playerPos)
		{
			var playerTilePos = tilemap.WorldToCell(playerPos);

			if (playerTilePos != playerPrevTilePos) {
				var bounds = StageManager.GetEnteredTilesBound(playerTilePos);

				// タイルを設置
				var length = Mathf.Abs(bounds.size.x * bounds.size.y);
				var tiles = Enumerable.Repeat(selectTile, length).ToArray();

				tilemap.ClearAllTiles();
				tilemap.SetTilesBlock(bounds, tiles);
			}

			playerPrevTilePos = playerTilePos;
		}
	}
}
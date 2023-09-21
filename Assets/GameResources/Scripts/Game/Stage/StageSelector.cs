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
		[SerializeField] Player.Player player;
		[SerializeField] StageManager StageManager;
		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase selectTile;

		Vector3Int playerPrevTilePos;       // プレイヤーが前フレームにいたタイル座標

		//--------------------------------------------------

		private void FixedUpdate()
		{
			var playerPos = tilemap.WorldToCell(player.transform.position);


			if (playerPos != playerPrevTilePos) {
				SetSelectionTile(playerPos);
			}

			playerPrevTilePos = playerPos;
		}

		void SetSelectionTile(Vector3Int playerPos)
		{
			var bounds = StageManager.GetEnteredTilesBound(playerPos);

			// タイルを設置
			var length = Mathf.Abs(bounds.size.x * bounds.size.y);
			var tiles = Enumerable.Repeat(selectTile, length).ToArray();

			tilemap.ClearAllTiles();
			tilemap.SetTilesBlock(bounds, tiles);
		}
	}
}
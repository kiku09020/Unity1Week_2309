using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Stage
{
	public class StageManager : MonoBehaviour
	{
		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase enterTile;

		[SerializeField] Vector3Int tilemapSize = new Vector3Int(18, 10);

		BoundsInt downRowBounds;

		//--------------------------------------------------

		public void OnChangedStage(Transform playerTransform)
		{
			// プレイヤーのタイル座標を取得
			var playerTilePos = tilemap.WorldToCell(playerTransform.position);

			// エンタータイル接地
			tilemap.SetTile(playerTilePos, enterTile);

			SetTiles(playerTilePos);
		}

		/// <summary> 改行対象タイルのBoundsを取得 </summary>
		public BoundsInt GetEnteredTilesBoound(Vector3Int playerTilePos)
		{
			var startTilePos = playerTilePos + Vector3Int.right;    // 改行開始タイル

			// 改行開始タイルが範囲外ならnullを返す
			if (startTilePos.x >= tilemapSize.x / 2) {
				return default;
			}

			var boundsSize = new Vector3Int((tilemapSize.x / 2) - startTilePos.x, 1, 1);
			var bounds = new BoundsInt(startTilePos, boundsSize);

			return bounds;
		}

		// 改行対象のタイルを取得
		TileBase[] GetEnteredTiles(Vector3Int playerTilePos)
		{
			var bounds = GetEnteredTilesBoound(playerTilePos);

			var tiles = tilemap.GetTilesBlock(bounds);

			tilemap.SetTilesBlock(bounds, new TileBase[bounds.size.x * bounds.size.y]);            // タイルを削除

			return tiles;
		}

		// 改行対象のタイル以降のタイルを取得
		TileBase[] GetEnteredDownRowTiles(Vector3Int playerTilePos)
		{
			var startTilePos = -tilemapSize / 2;

			var boundsSize = new Vector3Int(tilemapSize.x, playerTilePos.y - (-tilemapSize.y / 2), 1);

			// 改行対象の並びBounds
			var downRowBoundsPos = new Vector3Int(startTilePos.x, playerTilePos.y - 1);
			var downRowBoundsSize = new Vector3Int(tilemapSize.x, boundsSize.y, 1);
			var bounds = new BoundsInt(downRowBoundsPos, downRowBoundsSize);

			// タイル取得
			var tiles = new TileBase[bounds.size.x * bounds.size.y];

			for (int y = 0; y < bounds.size.y; y++) {
				for (int x = 0; x < bounds.size.x; x++) {
					tiles[x + y * bounds.size.x] = tilemap.GetTile(bounds.position + new Vector3Int(x, -y));
				}
			}

			downRowBounds = bounds;
			downRowBounds.position = new Vector3Int(bounds.position.x, bounds.position.y + bounds.size.y, 0);
			downRowBounds.size = new Vector3Int(bounds.size.x, -bounds.size.y, 1);

			return tiles;
		}

		void SetTiles(Vector3Int playerTilePos)
		{
			// 右端に到達していない場合のみ
			if (playerTilePos.x >= (tilemapSize.x / 2) - 1) {
				return;
			}

			List<TileBase> replacedTiles = new List<TileBase>();

			// タイル取得
			var enteredTiles = GetEnteredTiles(playerTilePos);
			var afterEnteredRowTiles = GetEnteredDownRowTiles(playerTilePos);

			replacedTiles.AddRange(enteredTiles);
			replacedTiles.AddRange(afterEnteredRowTiles);

			// タイルを設置	
			tilemap.SetTilesBlock(downRowBounds, replacedTiles.ToArray());

			// タイルを削除

		}
	}
}
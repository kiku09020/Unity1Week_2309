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

		/// <summary> ステージ変化時のイベント </summary>
		public event System.Action OnChangedEvent;

		public Tilemap Tilemap => tilemap;

		//--------------------------------------------------
		private void Awake()
		{
			// タイルマップのサイズを設定
			tilemap.origin = -tilemapSize / 2;
			tilemap.size = new Vector3Int(tilemapSize.x, tilemapSize.y, 1);
			tilemap.ResizeBounds();
		}

		//--------------------------------------------------
		/// <summary> ステージのタイルマップを二次元配列で取得 </summary>
		public TileBase[,] GetStageTilesArray()
		{
			var stageTiles = new TileBase[tilemap.size.x, tilemap.size.y];

			// 左上からの二次元配列
			for (int y = 0; y < tilemap.size.y; y++) {
				for (int x = 0; x < tilemap.size.x; x++) {

					var targetY = (tilemap.size.y - 1) + (tilemap.cellBounds.y - y);
					var cellPos = new Vector3Int(tilemap.cellBounds.x + x, targetY, tilemap.cellBounds.z);
					stageTiles[y, x] = tilemap.GetTile(cellPos);
				}
			}

			return stageTiles;
		}

		// タイル座標から配列のインデックスを取得
		Vector3Int GetArrayIndexFromTilePos(Vector3Int tilePos)
		{
			int x = tilePos.x - tilemap.cellBounds.x;
			int y = (tilemap.size.y - 1) + (tilemap.cellBounds.y - tilePos.y);

			return new Vector3Int(x, y, tilePos.z);
		}

		//--------------------------------------------------
		/// <summary> ステージ変化時の処理 </summary>
		public void OnChangedStage(Transform playerTransform)
		{
			// プレイヤーのタイル座標を取得
			var playerTilePos = tilemap.WorldToCell(playerTransform.position);

			SetTiles(playerTilePos);


			// エンタータイル設置
			if (CheckEnterPos(playerTilePos) != playerTilePos) {
				var pos = new Vector3Int(-tilemap.size.x / 2, playerTilePos.y - 1, playerTilePos.z);

				// タイルを下にずらす
				var tiles = GetEnteredDownRowTiles(playerTilePos);

				var boundsPos = new Vector3Int(pos.x, pos.y - (pos.y - (tilemapSize.y / 2) + 1), 0);
				var boundsSize = new Vector3Int(tilemapSize.x, (pos.y - (tilemapSize.y / 2)), 1);
				var bounds = new BoundsInt(boundsPos, boundsSize);

				tilemap.SetTilesBlock(bounds, tiles);

				// 改行対象削除
				var clearBoundsSize = new Vector3Int(tilemapSize.x, 1, 1);
				var clearBounds = new BoundsInt(pos, clearBoundsSize);
				tilemap.SetTilesBlock(clearBounds, new TileBase[clearBounds.size.x * clearBounds.size.y]);

				// エンタータイル設置
				tilemap.SetTile(pos, enterTile);
			}


			else {
				tilemap.SetTile(playerTilePos, enterTile);
			}



			// クリア判定
			OnChangedEvent?.Invoke();
		}

		//--------------------------------------------------
		// 改行タイルの位置をチェック
		Vector3Int CheckEnterPos(Vector3Int playerTilePos)
		{
			Vector3Int pos;

			// プレイヤーから左端までのタイルにエンタータイルがあれば、
			// そのタイルを始点として、左端までのタイルを改行対象とする
			for (int x = playerTilePos.x - 1; x >= -tilemapSize.x / 2; x--) {
				pos = new Vector3Int(x, playerTilePos.y, playerTilePos.z);
				var tile = tilemap.GetTile(pos);

				if (tile == enterTile) {
					return pos;
				}
			}

			return playerTilePos;
		}

		/// <summary> 改行対象タイルのBoundsを取得 </summary>
		public BoundsInt GetEnteredTilesBound(Vector3Int playerTilePos)
		{
			var startTilePos = playerTilePos + Vector3Int.right;    // 改行開始タイル
			var endTilePos = new Vector3Int(tilemapSize.x / 2, playerTilePos.y, playerTilePos.z);    // 改行終了タイル

			var pos = CheckEnterPos(playerTilePos);
			if (pos != playerTilePos) {
				startTilePos = pos - Vector3Int.right;
				endTilePos = pos + Vector3Int.up;
			}

			var boundsSize = new Vector3Int(endTilePos.x - startTilePos.x, 1, 1);
			var bounds = new BoundsInt(startTilePos, boundsSize);

			return bounds;
		}

		//--------------------------------------------------
		// 改行対象のタイルを取得
		TileBase[] GetEnteredTiles(Vector3Int playerTilePos)
		{
			var bounds = GetEnteredTilesBound(playerTilePos);
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
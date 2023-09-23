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

		//--------------------------------------------------
		/// <summary> ステージ変化時の処理 </summary>
		public void OnChangedStage(Transform playerTransform)
		{
			// プレイヤーのタイル座標を取得
			var playerTilePos = tilemap.WorldToCell(playerTransform.position);

			SetTiles(playerTilePos);

			// クリア判定
			OnChangedEvent?.Invoke();
		}

		// タイル配置
		void SetTiles(Vector3Int playerTilePos)
		{
			// 既存の改行タイルの位置をチェック
			var prevEnterTilePos = playerTilePos;
			bool isEnterOfEnter = CheckExistingEnterPos(playerTilePos, ref prevEnterTilePos);

			// 配置予定の改行タイルの位置を取得
			var nextEnterTilePos = GetNextEnterPos(playerTilePos, isEnterOfEnter);

			// 改行対象タイル + ずれ対象タイル
			List<TileBase> replacedTiles = new List<TileBase>();

			var enteredTilesBounds = GetEnteredTilesBound(playerTilePos, prevEnterTilePos, isEnterOfEnter);


			if (!isEnterOfEnter) {
				var enteredTiles = tilemap.GetTilesBlock(enteredTilesBounds);
				replacedTiles.AddRange(enteredTiles);
			}

			var offedTilesBouonds = GetOffedTIlesBounds(playerTilePos);
			var offedTiles = GetOffedTiles(offedTilesBouonds);
			replacedTiles.AddRange(offedTiles);

			// タイルを設置	
			SetReplacedTiles(nextEnterTilePos, replacedTiles.ToArray());

			// 改行タイルを設置
			SetEnterTile(enteredTilesBounds, nextEnterTilePos, isEnterOfEnter);
		}

		//--------------------------------------------------
		// 既存の改行タイルの位置をチェック
		bool CheckExistingEnterPos(Vector3Int playerTilePos, ref Vector3Int enterTilePos)
		{
			Vector3Int pos;

			// プレイヤーの左タイルから、左端まで探索
			// 改行タイルがあれば、タイル位置を返す
			for (int x = playerTilePos.x; x >= -tilemapSize.x / 2; x--) {
				pos = new Vector3Int(x, playerTilePos.y, playerTilePos.z);
				var tile = tilemap.GetTile(pos);

				if (tile == enterTile) {
					enterTilePos = pos;
					return true;
				}
			}

			return false;
		}

		/// <summary> 改行対象タイルの範囲を取得 </summary>
		public BoundsInt GetEnteredTilesBound(Vector3Int playerTilePos)
		{
			var enterTilePos = playerTilePos;
			var isEnterOfEnter = CheckExistingEnterPos(playerTilePos, ref enterTilePos);

			return GetEnteredTilesBound(playerTilePos, playerTilePos, isEnterOfEnter);
		}

		BoundsInt GetEnteredTilesBound(Vector3Int playerTilePos, Vector3Int enterTilePos, bool isEnterofEnter)
		{
			Vector3Int startTilePos = Vector3Int.zero;           // 改行開始タイル
			Vector3Int endTilePos = Vector3Int.zero;            // 改行終了タイル

			if (isEnterofEnter) {
				startTilePos = new Vector3Int(tilemap.cellBounds.x, enterTilePos.y - 1, enterTilePos.z);
				endTilePos = new Vector3Int(tilemap.cellBounds.max.x, enterTilePos.y - 1, 0);
			}

			else {
				startTilePos = playerTilePos;                                    // 改行開始タイル
				endTilePos = new Vector3Int(tilemap.cellBounds.max.x, playerTilePos.y, playerTilePos.z);   // 改行終了タイル

				// プレイヤーの右側からステージの右端までに改行タイルがあれば、
				// 終了位置をそのタイルとする
				for (int x = playerTilePos.x; x < tilemap.cellBounds.max.x; x++) {
					var pos = new Vector3Int(x, playerTilePos.y, playerTilePos.z);
					var tile = tilemap.GetTile(pos);

					if (tile == enterTile) {
						endTilePos = pos + Vector3Int.right;
						break;
					}
				}
			}

			var boundsSize = new Vector3Int(endTilePos.x - startTilePos.x, 1, 1);
			var bounds = new BoundsInt(startTilePos, boundsSize);

			return bounds;
		}

		// 配置予定の改行タイルの位置を取得
		Vector3Int GetNextEnterPos(Vector3Int playerTilePos, bool isEnterOfEnter)
		{
			// 改行の改行の場合、下の行の左端
			if (isEnterOfEnter) {
				return new Vector3Int(tilemap.cellBounds.x, playerTilePos.y - 1, playerTilePos.z);
			}

			// 改行の場合、プレイヤーのタイル位置
			return playerTilePos;
		}

		//--------------------------------------------------
		// ずれる対象のタイルの範囲を取得
		BoundsInt GetOffedTIlesBounds(Vector3Int playerTilePos)
		{
			var startTilePos = tilemap.cellBounds.position;

			var boundsPos = new Vector3Int(startTilePos.x, playerTilePos.y - 1);
			var boundsSize = new Vector3Int(tilemapSize.x, playerTilePos.y - startTilePos.y, 1);
			var bounds = new BoundsInt(boundsPos, boundsSize);

			return bounds;
		}

		// ずれる対象のタイルを取得
		TileBase[] GetOffedTiles(BoundsInt bounds)
		{
			var tiles = new TileBase[bounds.size.x * bounds.size.y];

			for (int y = 0; y < bounds.size.y; y++) {
				for (int x = 0; x < bounds.size.x; x++) {
					tiles[x + y * bounds.size.x] = tilemap.GetTile(bounds.position + new Vector3Int(x, -y));
				}
			}

			return tiles;
		}
		//--------------------------------------------------
		// 再配置されるタイルの範囲を取得
		BoundsInt GetReplacedTilesBounds(Vector3Int nextEnterTilePos)
		{
			var pos = new Vector3Int(tilemap.cellBounds.x, nextEnterTilePos.y, 0);
			var size = new Vector3Int(tilemapSize.x, -(pos.y - tilemap.cellBounds.y), 1);

			pos.y -= (size.y + 1);

			return new BoundsInt(pos, size);
		}

		// 再配置されるタイルを設置
		void SetReplacedTiles(Vector3Int nextEnterTilePos, TileBase[] replacedTiles)
		{
			var replacedTileBounds = GetReplacedTilesBounds(nextEnterTilePos);

			tilemap.SetTilesBlock(replacedTileBounds, replacedTiles);
		}

		//--------------------------------------------------
		// 改行タイル設置
		void SetEnterTile(BoundsInt enteredTilesBounds, Vector3Int nextEnterTilePos, bool isEnterOfEnter)
		{
			// 改行範囲のタイル削除
			var emptyTiles = new TileBase[enteredTilesBounds.size.x * enteredTilesBounds.size.y];

			if (isEnterOfEnter) {
				var boundsPos = new Vector3Int(tilemap.cellBounds.x, enteredTilesBounds.position.y);
				var boundsSize = new Vector3Int(tilemap.size.x, 1, 1);
				var bounds = new BoundsInt(boundsPos, boundsSize);

				emptyTiles = new TileBase[bounds.size.x * bounds.size.y];
				tilemap.SetTilesBlock(bounds, emptyTiles);
			}

			else {
				tilemap.SetTilesBlock(enteredTilesBounds, emptyTiles);
			}


			// エンタータイル設置
			tilemap.SetTile(nextEnterTilePos, enterTile);
		}
	}
}
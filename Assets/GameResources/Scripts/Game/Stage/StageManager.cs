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
			// �v���C���[�̃^�C�����W���擾
			var playerTilePos = tilemap.WorldToCell(playerTransform.position);

			// �G���^�[�^�C���ڒn
			tilemap.SetTile(playerTilePos, enterTile);

			SetTiles(playerTilePos);
		}

		/// <summary> ���s�Ώۃ^�C����Bounds���擾 </summary>
		public BoundsInt GetEnteredTilesBoound(Vector3Int playerTilePos)
		{
			var startTilePos = playerTilePos + Vector3Int.right;    // ���s�J�n�^�C��

			// ���s�J�n�^�C�����͈͊O�Ȃ�null��Ԃ�
			if (startTilePos.x >= tilemapSize.x / 2) {
				return default;
			}

			var boundsSize = new Vector3Int((tilemapSize.x / 2) - startTilePos.x, 1, 1);
			var bounds = new BoundsInt(startTilePos, boundsSize);

			return bounds;
		}

		// ���s�Ώۂ̃^�C�����擾
		TileBase[] GetEnteredTiles(Vector3Int playerTilePos)
		{
			var bounds = GetEnteredTilesBoound(playerTilePos);

			var tiles = tilemap.GetTilesBlock(bounds);

			tilemap.SetTilesBlock(bounds, new TileBase[bounds.size.x * bounds.size.y]);            // �^�C�����폜

			return tiles;
		}

		// ���s�Ώۂ̃^�C���ȍ~�̃^�C�����擾
		TileBase[] GetEnteredDownRowTiles(Vector3Int playerTilePos)
		{
			var startTilePos = -tilemapSize / 2;

			var boundsSize = new Vector3Int(tilemapSize.x, playerTilePos.y - (-tilemapSize.y / 2), 1);

			// ���s�Ώۂ̕���Bounds
			var downRowBoundsPos = new Vector3Int(startTilePos.x, playerTilePos.y - 1);
			var downRowBoundsSize = new Vector3Int(tilemapSize.x, boundsSize.y, 1);
			var bounds = new BoundsInt(downRowBoundsPos, downRowBoundsSize);

			// �^�C���擾
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
			// �E�[�ɓ��B���Ă��Ȃ��ꍇ�̂�
			if (playerTilePos.x >= (tilemapSize.x / 2) - 1) {
				return;
			}

			List<TileBase> replacedTiles = new List<TileBase>();

			// �^�C���擾
			var enteredTiles = GetEnteredTiles(playerTilePos);
			var afterEnteredRowTiles = GetEnteredDownRowTiles(playerTilePos);

			replacedTiles.AddRange(enteredTiles);
			replacedTiles.AddRange(afterEnteredRowTiles);

			// �^�C����ݒu	
			tilemap.SetTilesBlock(downRowBounds, replacedTiles.ToArray());

			// �^�C�����폜

		}
	}
}
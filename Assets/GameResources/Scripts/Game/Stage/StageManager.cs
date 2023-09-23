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

		/// <summary> �X�e�[�W�ω����̃C�x���g </summary>
		public event System.Action OnChangedEvent;

		public Tilemap Tilemap => tilemap;

		//--------------------------------------------------
		private void Awake()
		{
			// �^�C���}�b�v�̃T�C�Y��ݒ�
			tilemap.origin = -tilemapSize / 2;
			tilemap.size = new Vector3Int(tilemapSize.x, tilemapSize.y, 1);
			tilemap.ResizeBounds();
		}

		//--------------------------------------------------
		/// <summary> �X�e�[�W�̃^�C���}�b�v��񎟌��z��Ŏ擾 </summary>
		public TileBase[,] GetStageTilesArray()
		{
			var stageTiles = new TileBase[tilemap.size.x, tilemap.size.y];

			// ���ォ��̓񎟌��z��
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
		/// <summary> �X�e�[�W�ω����̏��� </summary>
		public void OnChangedStage(Transform playerTransform)
		{
			// �v���C���[�̃^�C�����W���擾
			var playerTilePos = tilemap.WorldToCell(playerTransform.position);

			SetTiles(playerTilePos);

			// �N���A����
			OnChangedEvent?.Invoke();
		}

		// �^�C���z�u
		void SetTiles(Vector3Int playerTilePos)
		{
			// �����̉��s�^�C���̈ʒu���`�F�b�N
			var prevEnterTilePos = playerTilePos;
			bool isEnterOfEnter = CheckExistingEnterPos(playerTilePos, ref prevEnterTilePos);

			// �z�u�\��̉��s�^�C���̈ʒu���擾
			var nextEnterTilePos = GetNextEnterPos(playerTilePos, isEnterOfEnter);

			// ���s�Ώۃ^�C�� + ����Ώۃ^�C��
			List<TileBase> replacedTiles = new List<TileBase>();

			var enteredTilesBounds = GetEnteredTilesBound(playerTilePos, prevEnterTilePos, isEnterOfEnter);


			if (!isEnterOfEnter) {
				var enteredTiles = tilemap.GetTilesBlock(enteredTilesBounds);
				replacedTiles.AddRange(enteredTiles);
			}

			var offedTilesBouonds = GetOffedTIlesBounds(playerTilePos);
			var offedTiles = GetOffedTiles(offedTilesBouonds);
			replacedTiles.AddRange(offedTiles);

			// �^�C����ݒu	
			SetReplacedTiles(nextEnterTilePos, replacedTiles.ToArray());

			// ���s�^�C����ݒu
			SetEnterTile(enteredTilesBounds, nextEnterTilePos, isEnterOfEnter);
		}

		//--------------------------------------------------
		// �����̉��s�^�C���̈ʒu���`�F�b�N
		bool CheckExistingEnterPos(Vector3Int playerTilePos, ref Vector3Int enterTilePos)
		{
			Vector3Int pos;

			// �v���C���[�̍��^�C������A���[�܂ŒT��
			// ���s�^�C��������΁A�^�C���ʒu��Ԃ�
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

		/// <summary> ���s�Ώۃ^�C���͈̔͂��擾 </summary>
		public BoundsInt GetEnteredTilesBound(Vector3Int playerTilePos)
		{
			var enterTilePos = playerTilePos;
			var isEnterOfEnter = CheckExistingEnterPos(playerTilePos, ref enterTilePos);

			return GetEnteredTilesBound(playerTilePos, playerTilePos, isEnterOfEnter);
		}

		BoundsInt GetEnteredTilesBound(Vector3Int playerTilePos, Vector3Int enterTilePos, bool isEnterofEnter)
		{
			Vector3Int startTilePos = Vector3Int.zero;           // ���s�J�n�^�C��
			Vector3Int endTilePos = Vector3Int.zero;            // ���s�I���^�C��

			if (isEnterofEnter) {
				startTilePos = new Vector3Int(tilemap.cellBounds.x, enterTilePos.y - 1, enterTilePos.z);
				endTilePos = new Vector3Int(tilemap.cellBounds.max.x, enterTilePos.y - 1, 0);
			}

			else {
				startTilePos = playerTilePos;                                    // ���s�J�n�^�C��
				endTilePos = new Vector3Int(tilemap.cellBounds.max.x, playerTilePos.y, playerTilePos.z);   // ���s�I���^�C��

				// �v���C���[�̉E������X�e�[�W�̉E�[�܂łɉ��s�^�C��������΁A
				// �I���ʒu�����̃^�C���Ƃ���
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

		// �z�u�\��̉��s�^�C���̈ʒu���擾
		Vector3Int GetNextEnterPos(Vector3Int playerTilePos, bool isEnterOfEnter)
		{
			// ���s�̉��s�̏ꍇ�A���̍s�̍��[
			if (isEnterOfEnter) {
				return new Vector3Int(tilemap.cellBounds.x, playerTilePos.y - 1, playerTilePos.z);
			}

			// ���s�̏ꍇ�A�v���C���[�̃^�C���ʒu
			return playerTilePos;
		}

		//--------------------------------------------------
		// �����Ώۂ̃^�C���͈̔͂��擾
		BoundsInt GetOffedTIlesBounds(Vector3Int playerTilePos)
		{
			var startTilePos = tilemap.cellBounds.position;

			var boundsPos = new Vector3Int(startTilePos.x, playerTilePos.y - 1);
			var boundsSize = new Vector3Int(tilemapSize.x, playerTilePos.y - startTilePos.y, 1);
			var bounds = new BoundsInt(boundsPos, boundsSize);

			return bounds;
		}

		// �����Ώۂ̃^�C�����擾
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
		// �Ĕz�u�����^�C���͈̔͂��擾
		BoundsInt GetReplacedTilesBounds(Vector3Int nextEnterTilePos)
		{
			var pos = new Vector3Int(tilemap.cellBounds.x, nextEnterTilePos.y, 0);
			var size = new Vector3Int(tilemapSize.x, -(pos.y - tilemap.cellBounds.y), 1);

			pos.y -= (size.y + 1);

			return new BoundsInt(pos, size);
		}

		// �Ĕz�u�����^�C����ݒu
		void SetReplacedTiles(Vector3Int nextEnterTilePos, TileBase[] replacedTiles)
		{
			var replacedTileBounds = GetReplacedTilesBounds(nextEnterTilePos);

			tilemap.SetTilesBlock(replacedTileBounds, replacedTiles);
		}

		//--------------------------------------------------
		// ���s�^�C���ݒu
		void SetEnterTile(BoundsInt enteredTilesBounds, Vector3Int nextEnterTilePos, bool isEnterOfEnter)
		{
			// ���s�͈͂̃^�C���폜
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


			// �G���^�[�^�C���ݒu
			tilemap.SetTile(nextEnterTilePos, enterTile);
		}
	}
}
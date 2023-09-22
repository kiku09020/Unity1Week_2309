using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Stage
{
	/// <summary> �X�e�[�W�̑I��͈� </summary>
	public class StageSelector : MonoBehaviour
	{
		[SerializeField] StageManager StageManager;
		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase selectTile;

		Vector3Int playerPrevTilePos;       // �v���C���[���O�t���[���ɂ����^�C�����W

		//--------------------------------------------------

		public void SetSelectionTile(Vector3 playerPos)
		{
			var playerTilePos = tilemap.WorldToCell(playerPos);

			if (playerTilePos != playerPrevTilePos) {
				var bounds = StageManager.GetEnteredTilesBound(playerTilePos);

				// �^�C����ݒu
				var length = Mathf.Abs(bounds.size.x * bounds.size.y);
				var tiles = Enumerable.Repeat(selectTile, length).ToArray();

				tilemap.ClearAllTiles();
				tilemap.SetTilesBlock(bounds, tiles);
			}

			playerPrevTilePos = playerTilePos;
		}
	}
}
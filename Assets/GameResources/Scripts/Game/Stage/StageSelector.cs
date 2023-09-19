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
		[SerializeField] Player.Player player;
		[SerializeField] StageManager StageManager;
		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase selectTile;

		Vector3Int playerPrevTilePos;       // �v���C���[���O�t���[���ɂ����^�C�����W

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
			var bounds = StageManager.GetEnteredTilesBoound(playerPos);

			// �^�C����ݒu
			var tiles = Enumerable.Repeat(selectTile, bounds.size.x * bounds.size.y).ToArray();

			tilemap.ClearAllTiles();
			tilemap.SetTilesBlock(bounds, tiles);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Stage
{
	/// <summary> �X�e�[�W�̃N���A���� </summary>
	public class StageClearChecker : MonoBehaviour
	{
		const int clearTileCount = 6;

		[SerializeField] Tilemap tilemap;
		[SerializeField] TileBase[] targetTiles = new TileBase[clearTileCount];

		bool isContains;

		public event System.Action OnClearEvent;

		TileBase[,] stageTiles;

		//--------------------------------------------------

		private void Start()
		{
			stageTiles = new TileBase[tilemap.size.x, tilemap.size.y];

			SetStageTiles();
		}

		void SetStageTiles()
		{
			// �s���ƂɒT���w��
			for (int y = 0; y < tilemap.size.y; y++) {
				for (int x = 0; x < tilemap.size.x; x++) {
					var cellPos = new Vector3Int(tilemap.cellBounds.x + x, tilemap.cellBounds.y + y, tilemap.cellBounds.z);
					stageTiles[y, x] = tilemap.GetTile(cellPos);
				}
			}
		}

		public void CheckTiles()
		{
			SetStageTiles();

			// �񂲂ƂɒT��
			for (int x = 0; x < tilemap.size.x; x++) {
				for (int y = tilemap.size.y - 1; y >= 0; y--) {

					var tile = stageTiles[y, x];

					// �^�C�����܂�ł�����A���̃^�C�����n�_�Ƃ���
					if (tile == targetTiles[0]) {
						if (!isContains) {
							isContains = true;
						}

						// �ڕW�^�C���Ɠ����^�C����������A���̃^�C����
						for (int i = 0; i < targetTiles.Length; i++) {
							if (stageTiles[y - i, x] != targetTiles[i]) {
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
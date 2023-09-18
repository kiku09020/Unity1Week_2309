using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	/// <summary> プレイヤーのコンポーネントの基底クラス </summary>
	public abstract class PlayerComponentBase : MonoBehaviour
	{
		protected Player player;

		//--------------------------------------------------

		void Start()
		{
			player = GetComponentInParent<Player>();

			Initialize();
		}

		/// <summary> 初期化処理 </summary>
		protected abstract void Initialize();
	}
}
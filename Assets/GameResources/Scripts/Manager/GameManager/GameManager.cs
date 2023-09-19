using Game.Stage;
using GameController.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] SEManager seManager;

	[SerializeField] StageClearChecker clearChecker;

	public static bool IsClear { get; private set; }

	//--------------------------------------------------

	void Awake()
	{
		IsClear = false;

		// クリア判定イベントにクリアフラグを立てる処理を登録
		clearChecker.OnClearEvent += () => IsClear = true;
	}
}

using Game.Player;
using Game.Stage;
using GameController.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] SEManager seManager;

	[SerializeField] StageClearChecker clearChecker;

	public static bool IsGameOver { get; private set; }
	public static bool IsClear { get; private set; }

	//--------------------------------------------------

	void Awake()
	{
		IsClear = false;
		IsGameOver = false;

		Player player = FindObjectOfType<Player>();

		// クリア判定イベントにクリアフラグを立てる処理を登録
		clearChecker.OnClearEvent += OnCleared;
		player.OnDeadEvent += OnGameOvered;
	}

	void OnCleared()
	{
		IsClear = true;

		seManager.PlayAudio("tada");
	}

	void OnGameOvered()
	{
		IsGameOver = true;

		seManager.PlayAudio("windowsError");
	}
}

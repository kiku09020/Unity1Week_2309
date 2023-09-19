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

		// �N���A����C�x���g�ɃN���A�t���O�𗧂Ă鏈����o�^
		clearChecker.OnClearEvent += () => IsClear = true;
	}
}

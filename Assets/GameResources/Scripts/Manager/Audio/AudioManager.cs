using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Audio
{
	[RequireComponent(typeof(AudioSource))]
    public abstract class AudioManager : MonoBehaviour
    {
		/* Fields */
		[Header("Components")]
		[SerializeField] protected AudioSource source;

		[SerializeField] protected AudioDataList dataList;

		static List<AudioManager> managers = new List<AudioManager>();

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Events */
		void Awake()
		{
			managers.Add(this);
		}

		protected virtual void OnDestroy()
		{
			managers.Clear();
		}

		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> 音声の再生処理用抽象メソッド </summary>
		protected abstract void PlayAudio_Derived(AudioClip clip);

		/// <summary> Clipを指定して、音声を再生する </summary>
		protected void PlayAudio(AudioClip clip, bool isParamReset = false)
		{
			// パラメータ再生
			if (isParamReset) {
				ResetSourceParameters();
			}

			PlayAudio_Derived(clip);
		}

		/// <summary> 音声名を指定して、音声を再生する </summary>
		public AudioManager PlayAudio(string audioName, bool isParamReset = false)
		{
			if( dataList.DataDictionary.TryGetValue(audioName, out var data)){
				PlayAudio(data.Data, isParamReset);
				return this;
			}

			throw new Exception("指定された名前の音声のデータがありません");
		}

		/// <summary> 音声を一時停止する </summary>
		public void Pause() { source.Pause(); }

		/// <summary> 音声の一時停止を解除する </summary>
		public void UnPause() { source.UnPause(); }

		/// <summary> ミュートにする </summary>
		public void Mute() { source.mute = true; }

		/// <summary> ミュート解除 </summary>
		public void Unmute() { source.mute = false; }

		//-------------------------------------------------------------------
		/* Audio Settings */

		/// <summary> ループ指定 </summary>
		public AudioManager SetLoop(bool loop = true) { source.loop = loop; return this; }

		/// <summary> 音量指定 </summary>
		public AudioManager SetVolume(float volume) { source.volume = volume; return this; }

		/// <summary> ピッチ指定 </summary>
		public AudioManager SetPitch(float pitch) { source.pitch = pitch; return this; }

		/// <summary> パラメータリセット </summary>
		public void ResetSourceParameters()
		{
			source.loop = false;
			source.volume = 1;
			source.pitch = 1;
		}

		//-------------------------------------------------------------------
		/* Static */
		/// <summary> 全ての音声を一時停止する </summary>
		public static void PauseAllAudio()
		{
			foreach (var manager in managers) {
				manager.Pause();
			}
		}

		/// <summary> 全ての音声の一時停止を解除する </summary>
		public static void UnPauseAllAudio()
		{
			foreach (var manager in managers) {
				manager.UnPause();
			}
		}

		/// <summary> 全ての音声をミュートにする </summary>
		public static void MuteAllAudio()
		{
			foreach (var audManager in managers) {
				audManager.Mute();
			}
		}

		/// <summary> 全ての音声のミュートを解除する </summary>
		public static void UnmuteAllAudio()
		{
			foreach (var audManager in managers) {
				audManager.Unmute();
			}
		}
	}
}
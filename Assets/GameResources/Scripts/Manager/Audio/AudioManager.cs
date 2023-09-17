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
		/// <summary> �����̍Đ������p���ۃ��\�b�h </summary>
		protected abstract void PlayAudio_Derived(AudioClip clip);

		/// <summary> Clip���w�肵�āA�������Đ����� </summary>
		protected void PlayAudio(AudioClip clip, bool isParamReset = false)
		{
			// �p�����[�^�Đ�
			if (isParamReset) {
				ResetSourceParameters();
			}

			PlayAudio_Derived(clip);
		}

		/// <summary> ���������w�肵�āA�������Đ����� </summary>
		public AudioManager PlayAudio(string audioName, bool isParamReset = false)
		{
			if( dataList.DataDictionary.TryGetValue(audioName, out var data)){
				PlayAudio(data.Data, isParamReset);
				return this;
			}

			throw new Exception("�w�肳�ꂽ���O�̉����̃f�[�^������܂���");
		}

		/// <summary> �������ꎞ��~���� </summary>
		public void Pause() { source.Pause(); }

		/// <summary> �����̈ꎞ��~���������� </summary>
		public void UnPause() { source.UnPause(); }

		/// <summary> �~���[�g�ɂ��� </summary>
		public void Mute() { source.mute = true; }

		/// <summary> �~���[�g���� </summary>
		public void Unmute() { source.mute = false; }

		//-------------------------------------------------------------------
		/* Audio Settings */

		/// <summary> ���[�v�w�� </summary>
		public AudioManager SetLoop(bool loop = true) { source.loop = loop; return this; }

		/// <summary> ���ʎw�� </summary>
		public AudioManager SetVolume(float volume) { source.volume = volume; return this; }

		/// <summary> �s�b�`�w�� </summary>
		public AudioManager SetPitch(float pitch) { source.pitch = pitch; return this; }

		/// <summary> �p�����[�^���Z�b�g </summary>
		public void ResetSourceParameters()
		{
			source.loop = false;
			source.volume = 1;
			source.pitch = 1;
		}

		//-------------------------------------------------------------------
		/* Static */
		/// <summary> �S�Ẳ������ꎞ��~���� </summary>
		public static void PauseAllAudio()
		{
			foreach (var manager in managers) {
				manager.Pause();
			}
		}

		/// <summary> �S�Ẳ����̈ꎞ��~���������� </summary>
		public static void UnPauseAllAudio()
		{
			foreach (var manager in managers) {
				manager.UnPause();
			}
		}

		/// <summary> �S�Ẳ������~���[�g�ɂ��� </summary>
		public static void MuteAllAudio()
		{
			foreach (var audManager in managers) {
				audManager.Mute();
			}
		}

		/// <summary> �S�Ẳ����̃~���[�g���������� </summary>
		public static void UnmuteAllAudio()
		{
			foreach (var audManager in managers) {
				audManager.Unmute();
			}
		}
	}
}
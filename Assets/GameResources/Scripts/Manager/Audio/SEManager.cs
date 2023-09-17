using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Audio
{
    public class SEManager : AudioManager
    {

		//--------------------------------------------------

		protected override void PlayAudio_Derived(AudioClip clip)
		{
			source.clip = clip;

			// ループが有効な場合、Play
			if (source.loop) {
				source.Play();
			}

			// そうでなければ、PlayOneShot
			else {
				source.PlayOneShot(clip);
			}
		}

		/// <summary> 完全ランダムな効果音を再生する </summary>
		public void PlayRandomSE(bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(0, dataList.DataDictionary.Count), isParamReset);
		}

		/// <summary> 範囲指定でランダムな効果音を再生する </summary>
		public void PlayRandomSE(int rangeMin, int rangeMax, bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(rangeMin, rangeMax), isParamReset);
		}

		//-------------------------------------------------------------------
		// 指定された範囲の効果音をランダムで取得する
		AudioClip GetRandomClip(int rangeMin, int rangeMax)
		{
			int randomIndex = Random.Range(rangeMin, rangeMax);
			AudioClip randomClip = dataList.DataList[randomIndex].Data;

			return randomClip;
		}

		/// <summary> ピッチを範囲指定でランダムにする </summary>
		public AudioManager SetRandomPitch(float min = 0.5f, float max = 1)
		{
			source.pitch = Random.Range(min, max);
			return this;
		}
	}
}
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

			// ���[�v���L���ȏꍇ�APlay
			if (source.loop) {
				source.Play();
			}

			// �����łȂ���΁APlayOneShot
			else {
				source.PlayOneShot(clip);
			}
		}

		/// <summary> ���S�����_���Ȍ��ʉ����Đ����� </summary>
		public void PlayRandomSE(bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(0, dataList.DataDictionary.Count), isParamReset);
		}

		/// <summary> �͈͎w��Ń����_���Ȍ��ʉ����Đ����� </summary>
		public void PlayRandomSE(int rangeMin, int rangeMax, bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(rangeMin, rangeMax), isParamReset);
		}

		//-------------------------------------------------------------------
		// �w�肳�ꂽ�͈͂̌��ʉ��������_���Ŏ擾����
		AudioClip GetRandomClip(int rangeMin, int rangeMax)
		{
			int randomIndex = Random.Range(rangeMin, rangeMax);
			AudioClip randomClip = dataList.DataList[randomIndex].Data;

			return randomClip;
		}

		/// <summary> �s�b�`��͈͎w��Ń����_���ɂ��� </summary>
		public AudioManager SetRandomPitch(float min = 0.5f, float max = 1)
		{
			source.pitch = Random.Range(min, max);
			return this;
		}
	}
}
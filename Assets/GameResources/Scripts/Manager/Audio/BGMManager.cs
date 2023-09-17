using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Audio
{
    public class BGMManager : AudioManager
    {

		//--------------------------------------------------

		protected override void PlayAudio_Derived(AudioClip clip)
		{
			source.clip = clip;
			source.Play();
		}
	}
}
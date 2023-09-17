using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameController.Audio
{
	[CreateAssetMenu(menuName = "GameController/Audio/AudioDataList", fileName = "AudioDataList")]
	public class AudioDataList : ScriptableObject
	{
		[SerializeField] List<AudioData> dataList = new List<AudioData>();

		Dictionary<string, AudioData> dataDictionary = new Dictionary<string, AudioData>();

		public IReadOnlyList<AudioData> DataList => dataList;
		public IReadOnlyDictionary<string, AudioData> DataDictionary => dataDictionary;

		[System.Serializable]
		public class AudioData
		{
			[SerializeField] string name;
			[SerializeField] AudioClip data;

			public string Name => name;
			public AudioClip Data => data;

			public void SetName()
			{
				name = data.name;
			}
		}

		//--------------------------------------------------

		void OnEnable()
		{
			dataDictionary = dataList.ToDictionary(data => data.Name);
		}

		private void OnValidate()
		{
			dataList.ForEach(data => data.SetName());
		}
	}
}
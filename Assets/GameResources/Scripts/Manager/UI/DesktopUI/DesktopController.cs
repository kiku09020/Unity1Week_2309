using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DesktopController : MonoBehaviour
{
	[Header("Timer")]
	[SerializeField] TextMeshProUGUI timerText;

	[Header("Audio")]
	[SerializeField] GameObject audioPanel;
	[SerializeField] Slider audioVolumeSlider;
	[SerializeField] TextMeshProUGUI audioVolumeText;
	[SerializeField] AudioMixer audioMixer;

	//--------------------------------------------------

	private void Awake()
	{
		var volume = PlayerPrefs.GetFloat("AudioVolume", 50);
		SetVolumeText(volume);
		audioVolumeSlider.value = volume;

		audioVolumeSlider.onValueChanged.AddListener(SetAudioVolume);
	}

	private void FixedUpdate()
	{
		SetTimerText();
	}

	// ���v
	void SetTimerText()
	{
		timerText.text = System.DateTime.Now.ToString("HH:mm:ss");
	}

	// ���ʐݒ�
	void SetAudioVolume(float value)
	{
		// �f�V�x���ɕϊ�
		var fixedValue = Mathf.Log10(value / 100) * 20;
		var clampedValue = Mathf.Clamp(fixedValue, -80, 0);

		audioMixer.SetFloat("Volume", clampedValue);

		SetVolumeText(value);

		PlayerPrefs.SetFloat("AudioVolume", value);
	}

	void SetVolumeText(float volume)
	{
		audioVolumeText.text = $"{volume}";
	}

	// �����{�^��
	public void AudioVolumeButton()
	{
		audioPanel.SetActive(!audioPanel.activeSelf);
	}

	public void TwitterButton()
	{
		Application.OpenURL("https://twitter.com/gamekk09020");
	}

	public void GitHubButton()
	{
		Application.OpenURL("https://github.com/kiku09020/Unity1Week_2309/blob/main/README.md");
	}
}

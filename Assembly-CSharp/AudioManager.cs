using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class AudioManager : MonoBehaviour
{
	// Token: 0x06000062 RID: 98 RVA: 0x00003734 File Offset: 0x00001934
	private void Awake()
	{
		if (AudioManager.instance == null)
		{
			AudioManager.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			foreach (Sound sound in this.sounds)
			{
				sound.source = base.gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;
				sound.source.volume = sound.volume;
				sound.source.pitch = sound.pitch;
				sound.source.loop = sound.loop;
			}
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000037DC File Offset: 0x000019DC
	private void Start()
	{
		this.GetSoundByName("Music").source.clip = this.soundtrack[this.ReturnOstIndex(this.ReturnOstId())];
		this.PlayOld("Music");
		this.currentScenario = this.ReturnOstId();
		this.currentOst = this.ReturnOstIndex(this.ReturnOstId());
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000383C File Offset: 0x00001A3C
	private void Update()
	{
		foreach (Sound sound in this.sounds)
		{
			if (sound.name == "Music")
			{
				if (GameManager.instance.music)
				{
					sound.source.volume = this.ostVolume;
				}
				else
				{
					sound.source.volume = 0f;
				}
			}
			else if (sound.ambience)
			{
				if (GameManager.instance.ambience)
				{
					sound.source.volume = sound.volume;
				}
				else
				{
					sound.source.volume = 0f;
				}
			}
			else if (GameManager.instance.sound)
			{
				sound.source.volume = sound.volume;
			}
			else
			{
				sound.source.volume = 0f;
			}
		}
		if (GameManager.instance.rain && !GameManager.instance.isInMines)
		{
			if (!this.GetSoundByName("Rain").source.isPlaying)
			{
				this.PlayOld("Rain");
			}
		}
		else if (this.GetSoundByName("Rain").source.isPlaying)
		{
			this.Stop("Rain");
		}
		if (this.currentScenario != this.ReturnOstId() && this.currentOst != this.ReturnOstIndex(this.ReturnOstId()))
		{
			this.ostVolume -= Time.deltaTime * 0.15f;
		}
		else
		{
			this.ostVolume += Time.deltaTime * 0.15f;
		}
		if ((this.ostVolume <= 0f && this.currentOst != this.ReturnOstIndex(this.ReturnOstId())) || (!this.GetSoundByName("Music").source.isPlaying && this.currentOst == this.ReturnOstIndex(this.ReturnOstId()) && Application.isFocused))
		{
			AudioClip clip = this.soundtrack[this.ReturnOstIndex(this.ReturnOstId())];
			this.GetSoundByName("Music").source.clip = clip;
			this.Stop("Music");
			this.PlayOld("Music");
			this.currentScenario = this.ReturnOstId();
			this.currentOst = this.ReturnOstIndex(this.ReturnOstId());
		}
		this.ostVolume = Mathf.Clamp(this.ostVolume, 0f, 1f);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003A94 File Offset: 0x00001C94
	private int ReturnOstId()
	{
		if (GameManager.instance.gameState == GameState.MainMenu || GameManager.instance.generatingWorld)
		{
			return 1;
		}
		if (GameManager.instance.invasionEvent == Invasion.CubicChaos)
		{
			return 5;
		}
		if (GameManager.instance.boss != null)
		{
			return GameManager.instance.boss.enemy.ReturnBossSoundtrackId();
		}
		if (GameManager.instance.isInMines)
		{
			return 6;
		}
		if (GameManager.instance.IsNight())
		{
			return 2;
		}
		return 0;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003B10 File Offset: 0x00001D10
	private int ReturnOstIndex(int id)
	{
		switch (id)
		{
		case 0:
		{
			int num = Random.Range(1, 3);
			if (num == 1)
			{
				return 0;
			}
			if (num != 2)
			{
				return 0;
			}
			return 9;
		}
		case 1:
			return 1;
		case 2:
			return 10;
		case 3:
			return 3;
		case 4:
			return 4;
		case 5:
			return 5;
		case 6:
			return 6;
		case 7:
			return 7;
		case 8:
			return 8;
		default:
			return 0;
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003B74 File Offset: 0x00001D74
	public void Play(string name)
	{
		Sound soundByName = this.GetSoundByName(name);
		if (soundByName == null)
		{
			return;
		}
		soundByName.source.PlayOneShot(soundByName.source.clip);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00003BA4 File Offset: 0x00001DA4
	public void PlayOld(string name)
	{
		Sound soundByName = this.GetSoundByName(name);
		if (soundByName == null)
		{
			return;
		}
		soundByName.source.Play();
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003BC8 File Offset: 0x00001DC8
	public void Stop(string name)
	{
		Sound soundByName = this.GetSoundByName(name);
		if (soundByName == null)
		{
			return;
		}
		soundByName.source.Stop();
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00003BEC File Offset: 0x00001DEC
	public Sound GetSoundByName(string name)
	{
		return Array.Find<Sound>(this.sounds, (Sound sound) => sound.name == name);
	}

	// Token: 0x0400004A RID: 74
	public static AudioManager instance;

	// Token: 0x0400004B RID: 75
	public Sound[] sounds;

	// Token: 0x0400004C RID: 76
	public AudioClip[] soundtrack;

	// Token: 0x0400004D RID: 77
	private int currentOst = -1;

	// Token: 0x0400004E RID: 78
	private int currentScenario = -1;

	// Token: 0x0400004F RID: 79
	private float ostVolume;
}

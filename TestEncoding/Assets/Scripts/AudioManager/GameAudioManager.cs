using UnityEngine;
using System.Collections;

public class GameAudioManager : MonoBehaviour 
{
	
	private AudioSource gameBgAudio =null;
	private AudioSource gameMagicSound =null;
	
	private bool gameBgAudioPlaying =false;
	private bool gameMagicPlaying =false;
	
	private string preBgAudioName;
	private GameObject MusicAllPosition =null;
	
	static AudioListener mListener;
	private static GameAudioManager _instance = null;
	public static GameAudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("GameAudioManager").AddComponent<GameAudioManager>();
				mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
				if (mListener == null)
				{
					GameObject cam =new GameObject("listerCamera");				
					cam.transform.parent =_instance.transform;
					if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
				}
			}
			return _instance;
		}
	}
	
	void Awake()
	{
		Debug.Log("GameAudioManager Awake");
		DontDestroyOnLoad(gameObject);
	}
	
	void InitData()
	{
		if(PlayerPrefs.GetInt("OpenGameBgAudio") >=0)
		{
			this.MuteGameBgAudio(false);
		}
		else 
		{
			this.MuteGameBgAudio(true);	
		}
		
		if(PlayerPrefs.GetInt("OpenMagicAudio") >=0)
		{
			this.MuteMagicAudio(false);
		}
		else 
		{
			this.MuteMagicAudio(true);	
		}		
	}
	
	#region Game Bg Audio
	public AudioSource GameBgAudio
	{
		get
		{
			if(this.gameBgAudio ==null)
			{
				GameObject curObject =new GameObject("GameBgAudio");
				curObject.transform.parent =this.transform;
				this.gameBgAudio =curObject.AddComponent<AudioSource>();
				this.gameBgAudio.volume =0.75f;
			}
			return this.gameBgAudio;
		}
	}

    public void playGameBgAudio ( string filename, float delay =0)
	{
		if(filename !="")
		{
			if(GameBgAudio.clip != null)
			{
			    preBgAudioName = GameBgAudio.clip.name;
			}
			AudioClip tempAudio = Resources.Load(AudioPath.GAME_BG_MUSIC + filename) as AudioClip;

			GameBgAudio.clip = tempAudio;
				
			GameBgAudio.playOnAwake =true;
			GameBgAudio.loop =true;
			
            GameBgAudio.PlayDelayed(delay);
			this.gameBgAudioPlaying =true;
			
			InitData();
		}
	}
	
	public void RevertGameBgAudio(){
		playGameBgAudio(preBgAudioName);
	}
	
	public void StopGameBgAudio()
	{
		if(this.gameBgAudioPlaying !=false)
		{
			GameBgAudio.Stop();	
			this.gameBgAudioPlaying =false;
		}
	}
	
	public void MuteGameBgAudio(bool state)
	{
		GameBgAudio.mute =state;	
	}
	#endregion
	
	#region Game Magic Sound
	public AudioSource GameMagicSound
	{
		get
		{
			if(this.gameMagicSound ==null)
			{
				GameObject curObject =new GameObject("GameMagicSound");
				curObject.transform.parent =this.transform;
				this.gameMagicSound =curObject.AddComponent<AudioSource>();
			}
			return this.gameMagicSound;
		}	
	}

    public void playMagicAudio ( string filename, float delay = 0 )
	{
		if(filename !="")
		{
			AudioClip tempAudio = Resources.Load(AudioPath.GAME_MAGIC_SOUND + filename) as AudioClip;	

			GameMagicSound.clip = tempAudio;
			GameMagicSound.playOnAwake =true;
			GameMagicSound.loop =false;
            GameMagicSound.PlayDelayed(delay);
			this.gameMagicPlaying =true;
			
			InitData();
		}
	}
	
	public void playMagicAudioEx(string filename, bool loop =false)
	{
		if(MusicAllPosition ==null)
		{
			MusicAllPosition =new GameObject("MusicAllPosition");
			MusicAllPosition.transform.parent =this.transform;
		}
		
		if(filename =="") return ;
		
		GameObject curObject =new GameObject(filename);
		curObject.transform.parent =MusicAllPosition.transform;
		
		AudioSource gameMagicSound =curObject.AddComponent<AudioSource>();
		AudioClip tempAudio = Resources.Load(AudioPath.GAME_MAGIC_SOUND + filename) as AudioClip;	

		gameMagicSound.clip =  tempAudio;
			
		gameMagicSound.playOnAwake =true;
		gameMagicSound.loop =loop;
		gameMagicSound.Play();
	}
	
	public void stopMagicAudioEx(string filename)
	{
		if(MusicAllPosition ==null) return ;
		for (int i = 0; i < MusicAllPosition.transform.childCount; ++i)
		{
			Transform tran = MusicAllPosition.transform.GetChild(i);
			if(tran.name == filename)
			{
				Destroy(tran.gameObject);
			}
		}
	}
	
	public void RepleatMagicAudio(string filename)
	{
		if(filename !="")
		{
			AudioClip tempAudio = Resources.Load(AudioPath.GAME_MAGIC_SOUND + filename) as AudioClip;	

			GameMagicSound.clip = tempAudio;
			GameMagicSound.playOnAwake =true;
			GameMagicSound.loop =true;
			GameMagicSound.Play();
			this.gameMagicPlaying =true;
			
			InitData();
		}
	}
	
	public AudioClip GetMagicAudio(string filename)
	{
		if(filename !="")
		{
			AudioClip tempAudio = Resources.Load(AudioPath.GAME_MAGIC_SOUND + filename) as AudioClip;	
			if(tempAudio != null)
			{
				return tempAudio;
			}
		}
		return null;
	}
	
	public void StopMagicAudio()
	{
		if(this.gameMagicPlaying !=false)
		{
			GameMagicSound.Stop();	
			this.gameMagicPlaying =false;
		}
	}
	
	public void MuteMagicAudio(bool state)
	{
		GameMagicSound.mute =state;	
	}
	#endregion
}

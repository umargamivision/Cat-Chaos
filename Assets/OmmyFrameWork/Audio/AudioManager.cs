//Ommy AudioManager
using UnityEngine;
using System.Collections.Generic;
using Ommy.SaveData;
namespace Ommy.Audio
{
    public enum Haptic
    {
        None = 0,           
        Standard = 100,      
        Light = 50,         
        Medium = 150,         
        Heavy = 300,          
    }
    public enum SFX
    {
        Click = 0,
        TaskComplete = 1,
        LevelComplete = 2,
        FishReward = 3,
        throwItem = 4,
        PlayerDeath = 5,
    }//enum end

    [System.Serializable]
    public sealed class SFXClip
    {
        //===================================================
        // FIELDS
        //===================================================
        [SerializeField] SFX _sfx;
        [SerializeField] AudioClip _clip = null;

        // Constructor
        public SFXClip(SFX sfx) => _sfx = sfx;

        //===================================================
        // PROPERTIES
        //===================================================
        public SFX SFX => _sfx;
        public AudioClip Clip => _clip;

    }//struct end

    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                Init();
            }
        }
        //===================================================
        // FIELDS
        //===================================================
        [SerializeField] AudioSource _bgSource = null;
        [SerializeField] AudioSource _sfxSource = null;
        [Space]
        [SerializeField] AudioClip _bgMusic = null;
        [Space]
        [SerializeField] List<SFXClip> _sfxClips = new List<SFXClip>();

        //===================================================
        // METHODS
        //===================================================
        internal void Init()
        {
            SetBGSetting(SaveData.SaveData.Instance.Music);
            SetSFXSetting(SaveData.SaveData.Instance.SFX);
            SetHapticSetting(SaveData.SaveData.Instance.Haptic);
            Create();
        }//Start() end

        /// <summary>
        /// Method for creating Array of SFX Clips.
        /// </summary>
        private void Create()
        {
            int length = System.Enum.GetValues(typeof(SFX)).Length;

            if (_sfxClips == null || _sfxClips.Count == 0)
            {
                _sfxClips = new List<SFXClip>();
                for (int i = 0; i < length; i++)
                {
                    _sfxClips.Add(new SFXClip((SFX)i));
                }//loop end
            }//if end
            else
            {
                for (int i = 0; i < length - _sfxClips.Count; i++)
                    _sfxClips.Add(new SFXClip((SFX)(_sfxClips.Count + i)));

                for (int i = 0; i < length; i++)
                {
                    if (i < _sfxClips.Count && _sfxClips[i].SFX != (SFX)i)
                        _sfxClips[i] = new SFXClip((SFX)i);
                }//loop end
            }//else end

        }//Create() end

        /// <summary>
        /// Toggle Background Music Audio Source.
        /// </summary>
        public void SetBGSetting(bool Toggle) => _bgSource.mute = !Toggle;

        /// <summary>
        /// Toggle SFX Audio Source.
        /// </summary>
        public void SetSFXSetting(bool Toggle) => _sfxSource.mute = !Toggle;
        public void SetHapticSetting(bool Toggle) => hasHaptic = Toggle;
        public bool hasHaptic;
        /// <summary>
        /// Call when Game Starts to play Background Music.
        /// </summary>
        public void StartGame()
        {
            if (_bgSource.isPlaying)
                return;

            _bgSource.clip = _bgMusic;
            _bgSource.loop = true;
            _bgSource.Play();
        }//StartGame() end

        /// <summary>
        /// Call when Game Starts to stop playing Background Music.
        /// </summary>
        public void GameEnd() => _bgSource.Stop();

        /// <summary>
        /// Call to play specific SFX clip against enum.
        /// </summary>
        // public void PlaySFX(SFX sfx, float volume = 1f) =>
        //     _sfxSource.PlayOneShot(_sfxClips[(int)sfx].Clip, volume);
        public void PlaySFX(SFX sfx, float volume = 1f)
        {
            _sfxSource.PlayOneShot(_sfxClips.Find(f => f.SFX == sfx).Clip, volume);
        }
        /// <summary>
        /// Call to play custom Audio Clip.
        /// </summary>
        public void PlaySFX(AudioClip clip, float volume = 1f) =>
            _sfxSource.PlayOneShot(clip, volume);
        public void Handheld(Haptic hapicType)
        {
            if (!hasHaptic) return;
            switch (hapicType)
            {
                case Haptic.Standard:
                    VibrationManager.Vibrate((int) hapicType);
                    break;
                case Haptic.Light:
                    VibrationManager.Vibrate((int) hapicType);
                    break;
                case Haptic.Medium:
                    VibrationManager.Vibrate((int) hapicType);
                    break;
                case Haptic.Heavy:
                    VibrationManager.Vibrate((int) hapicType);
                    break;
            }
        }
        public void Handheld(int milliseconds)
        {
            VibrationManager.Vibrate(milliseconds);
        }
        public void Handheld(long[] pattern, int loop)
        {
            VibrationManager.Vibrate(pattern, loop);
        }
    }//class end
}
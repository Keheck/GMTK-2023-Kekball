using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MusicStarter : MonoBehaviour {

    [SerializeField]
    private AudioClip music;

    async void Start(){
        AudioManager.PlayMusic(music);    
        await UniTask.Yield();
        AudioManager.SetMusicVolume(0.1f);
    }

}

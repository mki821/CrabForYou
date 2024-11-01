using UnityEngine;

public class Setasdf : MonoBehaviour
{
    private void Start() {
        int width = Screen.width;
        if(width >= 2560) Screen.SetResolution(2560, 1440, true);
        else if(width >= 1920) Screen.SetResolution(1920, 1080, true);
        else Screen.SetResolution(1280, 720, true);
        
        SoundManager.Instance.PlayBGM("Crab");
    }
}

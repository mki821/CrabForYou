using UnityEngine;

public class Setasdf : MonoBehaviour
{
    private void Awake() {
        int width = Screen.width;
        if(width >= 2650) Screen.SetResolution(2560, 1440, false);
        else if(width >= 1920) Screen.SetResolution(1920, 1080, false);
        else Screen.SetResolution(1280, 720, false);
    }
}

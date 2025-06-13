using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // フレームレートを60に固定する
        Application.targetFrameRate = 60;
    }
}

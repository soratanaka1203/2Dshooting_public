using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // �t���[�����[�g��60�ɌŒ肷��
        Application.targetFrameRate = 60;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject itemCanvas;
    [SerializeField] GameObject defaultCanvas;

    //�X�^�[�g�{�^���������ꂽ�Ƃ���ʂ�J�ڂ�����
    public void TapStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    }

    //�����L���O�{�^���������ꂽ�Ƃ���ʂ�J�ڂ�����
    public void TapRanking()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RankingScene");
    }

    //��������{�^���������ꂽ�Ƃ���ʂ�J�ڂ�����
    public void TapOperationExplanation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OperationExplanationScene");
    }

    public void BackTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

    public void LoadThisScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActiveItemCanvas()
    {
        defaultCanvas.SetActive(false);
        itemCanvas.SetActive(true);
    }

    public void BackDefaultCanvas()
    {
        defaultCanvas.SetActive(true);
        itemCanvas.SetActive(false);
    }
}

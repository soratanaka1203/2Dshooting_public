using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject player;

    private bool gameIsCleared = false;

    private string currentInput = ""; // ���͗�����ۑ�
    private string secretCommand = "tanaka"; // �B���R�}���h
    private bool isSecretCommand = false;

    private void Start()
    {
        Time.timeScale = 1;  // �Q�[����ʏ푬�x�ɖ߂�
    }

    //�B���R�}���h
    private void Update()
    {
        // �L�[���͂��`�F�b�N
        if (Input.GetKey(KeyCode.T)) currentInput += "t";
        if (Input.GetKeyDown(KeyCode.N)) currentInput += "n";
        if (Input.GetKeyDown(KeyCode.A)) currentInput += "a";
        if (Input.GetKeyDown(KeyCode.K)) currentInput += "k";

        // ���͗����̒����𐧌��i�R�}���h�ȏ�̕����͍폜�j
        if (currentInput.Length > secretCommand.Length)
        {
            currentInput = currentInput.Substring(currentInput.Length - secretCommand.Length);
        }

        // �R�}���h����v���邩�m�F
        if (currentInput == secretCommand)
        {
            ActivateSecret();
        }
    }

    public void OnGameClear()
    {
        gameIsCleared = true;  // �Q�[���N���A�t���O�𗧂Ă�
        // �Q�[���N���A��̏���
        StopGame();
    }

    private void StopGame()
    {
        // �Q�[���̐i�s���~
        Time.timeScale = 0;  // �Q�[�����ꎞ��~
    }

    private void ActivateSecret()
    {
        if (!isSecretCommand)
        {
            isSecretCommand = true;
            PlayerControl playerCO = player.GetComponent<PlayerControl>();
            playerCO.bulletCount = 10;
            playerCO.shieldObject.SetActive(true);
            playerCO.isShield = true;
        }
        
    }
}

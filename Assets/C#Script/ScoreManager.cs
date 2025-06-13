using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace MyNameSpace
{
    public class ScoreManager : MonoBehaviour
    {
        // �V���O���g���C���X�^���X
        public static ScoreManager Instance { get; private set; }

        // �X�R�A��ێ�����v���p�e�B
        public int score { get; private set; }
        public static int[] ranking = new int[4] { 0, 0, 0, 0 };
        [SerializeField] List<TextMeshProUGUI> rankingText;

        // ������
        private void Awake()
        {
            // ���ɃC���X�^���X���Ȃ��ꍇ�A���݂̃C���X�^���X��ݒ�
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // ���̃I�u�W�F�N�g���V�[�����ׂ��Ŕj�����Ȃ�
            }
            else
            {
                // ���ɃC���X�^���X�����݂���ꍇ�́A���݂̃I�u�W�F�N�g��j��
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            // �V�[�������[�h���ꂽ�Ƃ��̃C�x���g�o�^
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            // �C�x���g����
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // �X�R�A�����Z���郁�\�b�h
        public void AddScore(int points)
        {
            score += points;
        }

        // �X�R�A�����Z�b�g���郁�\�b�h
        public void ResetScore()
        {
            score = 0;
        }

        // �X�R�A�̕\�������郁�\�b�h
        public void SetDisplayScore(TextMeshProUGUI scoreText)
        {
            scoreText.text = "�X�R�A�F" + score;
        }

        // �����L���O�ɃX�R�A��ݒ�
        public void SetRanking(int score)
        {
            for (int i = 0; i < ranking.Length; i++)
            {
                if (ranking[i] <= score)
                {
                    // �X�R�A��}�����A�����̃X�R�A���V�t�g
                    int temp = ranking[i];
                    ranking[i] = score;

                    // ���ʂ̃����L���O���V�t�g������
                    for (int j = i + 1; j < ranking.Length; j++)
                    {
                        int nextTemp = ranking[j];
                        ranking[j] = temp;
                        temp = nextTemp;
                    }
                    break; // �V�����X�R�A��}�������烋�[�v���I��
                }
            }
        }


        // �V�[�������[�h���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // ���݂̃V�[���������L���O�̃V�[���Ȃ�e�L�X�g�ɔ��f������
            if (scene.name == "RankingScene")
            {
                rankingText = new List<TextMeshProUGUI>();

                for (int i = 0; i < ranking.Length; i++)
                {
                    var textObject = GameObject.Find("ranking" + (i + 1)); // �����L���O�e�L�X�g���Q��

                    if (textObject != null)
                    {
                        TextMeshProUGUI textMeshPro = textObject.GetComponent<TextMeshProUGUI>();
                        if (textMeshPro != null)
                        {
                            // �����L���O�ɐݒ�
                            rankingText.Add(textMeshPro);
                            textMeshPro.text = (i + 1) + "�ʁF" + ranking[i];
                            Debug.Log(textMeshPro.text);
                        }
                        else
                        {
                            Debug.LogError("TextMeshProUGUI component not found on ranking" + (i + 1));
                        }
                    }
                    else
                    {
                        Debug.LogError("Ranking text object ranking" + (i + 1) + " not found.");
                    }
                }
            }
        }
    }
}

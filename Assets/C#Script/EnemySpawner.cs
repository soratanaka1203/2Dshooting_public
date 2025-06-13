using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool; // �G�̃v�[�����Q��
    [SerializeField] private GameObject boss; // �{�X�̃I�u�W�F�N�g
    public float spawnInterval = 2.5f; // �����X�|�[���Ԋu
    public float minSpawnInterval = 0.3f; // �X�|�[���Ԋu�̍ŏ��l
    public float spawnIntervalDecreaseRate = 0.02f; // �X�|�[���Ԋu�̌�����

    private bool bossIs = false; // �{�X���o�ꂵ�����ǂ����̃t���O
    private float startTime; // �X�|�[���J�n���̎�����ێ�
    private float timeElapsed = 0; // �o�ߎ��Ԃ�ǐ�

    void Start()
    {
        // enemyPool�����ݒ�̏ꍇ�A�V�[�������玩���擾
        if (enemyPool == null)
        {
            enemyPool = GameObject.Find("EnemyPool").GetComponent<EnemyPool>();
        }

        InitializeBoss(); // �{�X�̏�����
        StartSpawner(); // �G�X�|�[���̊J�n
    }

    // �I�u�W�F�N�g���A�N�e�B�u�����ꂽ�Ƃ��̏���
    void OnEnable()
    {
        if (bossIs)
        {
            StartSpawner();
        }
    }

    // �{�X�̏����ݒ�
    public void InitializeBoss()
    {
        // boss�����ݒ�̏ꍇ�A�V�[�������玩���擾
        if (boss == null)
        {
            boss = GameObject.Find("Boss");
        }

        boss.SetActive(false); // ������ԂŔ�A�N�e�B�u�ɐݒ�
        bossIs = false; // �{�X�t���O�����Z�b�g
    }

    // �X�|�[���̃��Z�b�g���\�b�h
    public void ResetSpawner()
    {
        InitializeBoss(); // �{�X�̏�����
        spawnInterval = 2.5f; // �X�|�[���Ԋu�������l�Ƀ��Z�b�g
        StartSpawner(); // �X�|�[�����ċN��
    }

    // �X�|�[�����J�n���郁�\�b�h
    private void StartSpawner()
    {
        startTime = Time.time; // ���݂̎������L�^���A�o�ߎ��Ԃ����Z�b�g
        StartCoroutine(SpawnEnemies()); // �R���[�`���J�n
    }

    // �G�����I�ɃX�|�[������R���[�`��
    IEnumerator SpawnEnemies()
    {
        // �R���[�`���̃��[�v�B�i���ɌJ��Ԃ����
        while (true)
        {
            // �o�ߎ��Ԃ��X�V�BtimeElapsed�̓X�|�[���Ԋu���l�������o�ߎ���
            timeElapsed += spawnInterval;

            // �{�X���܂��o�ꂵ�Ă��Ȃ����X�|�[���J�n����120�b���o�߂����ꍇ�A�{�X��o�ꂳ����
            if (Time.time - startTime >= 120 && !bossIs)
            {
                // �{�X�̏����ʒu��ݒ�i��ʊO�ɔz�u�j
                boss.transform.position = new Vector3(0, 40, 0);
                boss.SetActive(true);
                bossIs = true; // �{�X���o�ꂵ�����Ƃ������t���O
                Debug.Log("Boss Active: " + boss.activeSelf); // �{�X���A�N�e�B�u�ɂȂ������Ƃ��f�o�b�O�o��
            }

            // �v�[������G���擾
            var enemyObject = enemyPool.GetEnemy();
            if (enemyObject != null)
            {
                // �G�̃X�|�[���ʒu�������_���ɐݒ�i��ʂ̏㕔�j
                enemyObject.transform.position = new Vector3(Random.Range(-50f, 50f), 55f, 0.0f);
                enemyObject.transform.rotation = Quaternion.identity; // ��]�����Z�b�g

                // �G�̐���p�R���|�[�l���g�ƃX�R�A�A�̗͊Ǘ��p�R���|�[�l���g���擾
                var enemyControl = enemyObject.GetComponent<EnemyControl>();
                var enemyBH = enemyObject.GetComponent<BulletHit>();
                SpriteRenderer spriteRenderer = enemyObject.GetComponent<SpriteRenderer>();

                // �����K�v�ȃR���|�[�l���g���S�đ����Ă�����
                if (enemyControl != null && enemyBH != null)
                {
                    // �G�̈ړ��p�^�[���������_���őI��
                    int randomMovement = Random.Range(0, 4); // 0~3�̃����_���Ȑ����𐶐�

                    // ��Փx�ɉ������{����ݒ�
                    float difficultyMultiplier = 1 + (timeElapsed / 120f); // timeElapsed�̌o�ߎ��ԂɊ�Â���Փx�𑝉�

                    // �ړ��p�^�[���ɉ����ď����𕪊�
                    switch (randomMovement)
                    {
                        // ���i�ړ�
                        case 0:
                            enemyControl.SetMovement(new StraightMovement());
                            spriteRenderer.color = Color.yellow; // �G�̐F��ݒ�
                            enemyBH.scorePoint = Mathf.RoundToInt(500 * difficultyMultiplier); // �X�R�A�ݒ�i��Փx�ɉ����đ����j
                            enemyBH.enemyHp = Mathf.CeilToInt(5 * difficultyMultiplier); // �̗͐ݒ�i��Փx�ɉ����đ����j
                            break;

                        // �W�O�U�O�ړ�
                        case 1:
                            enemyControl.SetMovement(new ZigzagMovement());
                            spriteRenderer.color = Color.red; // �G�̐F��ݒ�
                            enemyBH.scorePoint = Mathf.RoundToInt(350 * difficultyMultiplier);
                            enemyBH.enemyHp = Mathf.CeilToInt(4 * difficultyMultiplier);
                            break;

                        // �~�^���ړ�
                        case 2:
                            enemyControl.SetMovement(new CircularMovement());
                            spriteRenderer.color = Color.magenta; // �G�̐F��ݒ�
                            enemyBH.scorePoint = Mathf.RoundToInt(200 * difficultyMultiplier);
                            enemyBH.enemyHp = Mathf.CeilToInt(4 * difficultyMultiplier);
                            enemyControl.isShot = true;
                            break;

                        // �g��ړ��i�V�����ǉ������ړ��p�^�[���j
                        case 3:
                            enemyControl.SetMovement(new WaveMovement());
                            spriteRenderer.color = Color.cyan; // �G�̐F��ݒ�
                            enemyBH.scorePoint = Mathf.RoundToInt(600 * difficultyMultiplier);
                            enemyBH.enemyHp = Mathf.CeilToInt(6 * difficultyMultiplier);
                            enemyControl.isShot = true;
                            break;
                    }
                }
            }

            // �X�|�[���Ԋu�����������A�ŏ��l�ȉ��ɂȂ�Ȃ��悤�ɐ���
            spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecreaseRate, minSpawnInterval);

            // ���̃X�|�[���܂őҋ@
            yield return new WaitForSeconds(spawnInterval);
        }
    }

}

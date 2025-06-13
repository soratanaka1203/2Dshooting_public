using Cysharp.Threading.Tasks;
using MyNameSpace;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameManager gameManager;
    public int maxHealth = 400;                 // �{�X�̍ő�̗�
    private int currentHealth;                  // �{�X�̌��݂̗̑�
    public float moveSpeed = 2.0f;              // �{�X�̈ړ����x

    [SerializeField] private EnemyBulletPool enemyBulletPool;  // �e�̃v�[��
    public Transform firePoint;                 // �e�𔭎˂���ʒu
    public float bulletSpeed = 20f;             // �e�̑��x
    public float phaseChangeHealthThreshold = 0.5f;  // �t�F�[�Y�ύX�̗̑͊��� (50%)

    [SerializeField] private BulletPool playerBulletPool;     // �v���C���[�e�̃v�[��
    [SerializeField] private EffectPool effectPool;           // �G�t�F�N�g�v�[��

    [SerializeField] private TextMeshProUGUI scoreText;       // �X�R�A�\���p�e�L�X�g
    [SerializeField] private GameObject gameClearUI;          // �Q�[���N���A��UI
    [SerializeField] private TextMeshProUGUI resultGameClearText;//���U���g�e�L�X�g
    [SerializeField] private Slider healthBar;                //�{�X�̗̑̓o�[
    [SerializeField] private GameObject enemySpawner;         // �G�X�|�i�[�̎Q��

    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private AudioClip audioClip;
    public float volume;

    private bool isDead = false;                // �{�X�����S���Ă��邩�̃t���O
    private bool isPhaseChanged = false;        // �t�F�[�Y���ύX���ꂽ���̃t���O
    private int currentPhase = 1;               // ���݂̃t�F�[�Y

    void Start()
    {
        // �e�I�u�W�F�N�g��R���|�[�l���g�̎擾
        if (enemyBulletPool == null) enemyBulletPool = GameObject.Find("EnemyBulletPool").GetComponent<EnemyBulletPool>();
        if (effectPool == null) effectPool = GameObject.Find("EffectPool").GetComponent<EffectPool>();
        if (playerBulletPool == null) playerBulletPool = GameObject.Find("PlayerBulletPool").GetComponentInChildren<BulletPool>();
        if (scoreText == null) scoreText = GameObject.Find("scoreText").GetComponentInChildren<TextMeshProUGUI>();
        if (gameClearUI == null) gameClearUI = GameObject.Find("GameClearUI");
        if (enemySpawner == null) enemySpawner = GameObject.Find("EnemySpawner");

        enemySpawner.SetActive(false); // �G�X�|�i�[���A�N�e�B�u��
        healthBar.gameObject.SetActive(true);//�̗̓o�[��\��
        healthBar.value = maxHealth;  // ���݂̗̑͂�������
        currentHealth = maxHealth;     

        InvokeRepeating("ExecuteAttackPattern", 1f, 2.3f); // 2.3�b���ƂɍU���p�^�[�������s
    }

    void Update()
    {
        if (!isDead) // �{�X���������Ă���ꍇ�̂ݍX�V
        {
            Move();  // �{�X�̈ړ�����

            // �̗͂�臒l�������A�܂��t�F�[�Y�ύX���Ă��Ȃ��ꍇ
            if (currentHealth <= maxHealth * phaseChangeHealthThreshold && !isPhaseChanged)
            {
                ChangePhase(2);           // �t�F�[�Y�ύX
                enemySpawner?.SetActive(true);  // �G�X�|�i�[���A�N�e�B�u��
            }
        }
    }

    // �v���C���[�̒e�Ƃ̏Փ˂����m���A�_���[�W�������s��
    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet") // �v���C���[�e�Ƃ̏Փ˔���
        {
            TakeDamage(1, collision.gameObject);  // �_���[�W���󂯂�
            UpdateScore(200); // �X�R�A���X�V
        }
    }

    // �{�X�̈ړ��p�^�[�� (��: ���E�ɃX���[�Y�Ɉړ�)
    void Move()
    {
        float moveX = Mathf.Sin(Time.time * moveSpeed) * 30.0f; // ���E�ɃX���[�Y�Ɉړ�
        transform.position = new Vector3(moveX, transform.position.y, transform.position.z);
    }

    // �U���p�^�[���������_���Ɏ��s����
    void ExecuteAttackPattern()
    {
        switch (UnityEngine.Random.Range(1,4))
        {
            case 1:
                FireStraight(); // �����U��
                Debug.Log("���ڍU��");
                break;
            case 2:
                FireFanPattern(); // ���̍U��
                Debug.Log("���̍U��");
                break;
            case 3:
                StartFireCircularPattern(); //����
                Debug.Log("���ˍU��");
                break;
            default:
                FireStraight();
                break;
        }
    }

    // �����ɒe�𔭎˂���p�^�[��
    async void FireStraight()
    {
        int bulletCount = 4;
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = enemyBulletPool.GetEnemyBullet(); // �e���v�[������擾
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed); // �������ɔ���
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));  // 0.1�b�ԑҋ@
        }
    }

    // ���ɕ����̒e�𔭎˂���p�^�[��
    void FireFanPattern()
    {
        int bulletsCount = 6;                // ���˂���e��
        float angleStep = 45f / (bulletsCount - 1); // �e�̊p�x�X�e�b�v
        float startAngle = -18f;            // �J�n�p�x

        for (int i = 0; i < bulletsCount; i++)
        {
            GameObject bullet = enemyBulletPool.GetEnemyBullet();
            bullet.transform.position = firePoint.position;

            // �e�̔��˕�����ݒ�
            float angle = startAngle + angleStep * i;
            Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed / 2;
        }
    }

    //�Ԋu��������
    void StartFireCircularPattern()
    {
        int bulletCount = 20;
        float angleStep = -180f / bulletCount;

        // ���˂���^�C�~���O�����炵�Ēe���o��
        for (int i = 0; i < bulletCount; i++)
        {
            float delay = i * 0.1f; // 0.1�b���x�点�Ĕ���
            Invoke("FireBullet", delay); // FireBullet���\�b�h��x�������ČĂяo��
        }
    }
    //���˂���
    void FireBullet()
    {
        // �e���v�[������擾
        GameObject bullet = enemyBulletPool.GetEnemyBullet();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;

            // �~����̈ʒu���v�Z
            float angle = UnityEngine.Random.Range(0f, -180f);
            float angleInRadians = Mathf.Deg2Rad * angle;

            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletSpeed / 2;
        }
    }

    // �{�X�̃t�F�[�Y���ύX���ꂽ�Ƃ��̏���
    void ChangePhase(int newPhase)
    {
        currentPhase = newPhase;  // �t�F�[�Y���X�V
        isPhaseChanged = true;    // �t�F�[�Y�ύX�t���O��ݒ�
        Debug.Log("�{�X�̃t�F�[�Y���ύX����܂����I");

        // �t�F�[�Y�ύX���̋�������
        moveSpeed += 1.0f;  // �ړ����x�𑬂�����
        bulletSpeed += 5f;  // �e�̑��x�𑬂�����

        // ���ˊԊu���X�V
        CancelInvoke(); // ������Invoke���L�����Z��
        InvokeRepeating("ExecuteAttackPattern", 1f, Mathf.Max(0.5f, 3f - currentPhase)); // �V�����Ԋu�Ŕ���
    }

    // �{�X���_���[�W���󂯂��ۂ̏���
    public void TakeDamage(int damageAmount, GameObject gameObject)
    {
        currentHealth -= damageAmount; // ���݂̗̑͂�����
        PlayEffect(gameObject.transform, 0.2f).Forget(); // �q�b�g�G�t�F�N�g���Đ�
        UpdateHealthBar();//�̗̓o�[���X�V
        audioPlayer.PlayAudio(audioClip, volume);

        if (currentHealth <= 0 && !isDead) // �̗͂�0�ȉ��ɂȂ����玀�S����
        {
            Die();
        }
    }

    // �{�X�����S�����Ƃ��̏���
    void Die()
    {
        isDead = true;
        // �U���p�^�[�����~
        CancelInvoke("ExecuteAttackPattern");
        for (int i = 0; i < 15; i++)
        {
            // �G�t�F�N�g�������_���ʒu�ɕ\��
            Transform tr = new GameObject().transform;
            tr.position = gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-10, 11), UnityEngine.Random.Range(-10, 11), 0);
            PlayEffect(tr, 2f).Forget();
            audioPlayer.PlayAudio(audioClip, volume);
        }
        enemySpawner.SetActive(false);
        gameObject.SetActive(false); // �{�X���A�N�e�B�u��
        UpdateScore(100000);         // �X�R�A�����Z
        ScoreManager.Instance.SetDisplayScore(resultGameClearText);//���ʂ�\��
        ScoreManager.Instance.SetRanking(ScoreManager.Instance.score); // �X�R�A�������L���O�ɃZ�b�g
        ScoreManager.Instance.ResetScore(); // �X�R�A�����Z�b�g
        gameClearUI.SetActive(true); // �Q�[���N���A��UI��\��
        gameManager.OnGameClear();

    }

    // �q�b�g�G�t�F�N�g��\��
    async UniTask PlayEffect(Transform effectTransform, float delay)
    {
        GameObject effect = effectPool.GetEffect(); // �G�t�F�N�g���擾
        effect.transform.position = effectTransform.position; // �G�t�F�N�g�ʒu�ݒ�

        // ��莞�Ԍ�ɃG�t�F�N�g���v�[���ɖ߂�
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        effectPool.ReleaseEffect(effect);
    }

    // �X�R�A�̍X�V
    private void UpdateScore(int amount)
    {
        ScoreManager.Instance.AddScore(amount);         // �X�R�A�����Z
        ScoreManager.Instance.SetDisplayScore(scoreText); // �X�R�A�e�L�X�g���X�V
    }

    //�̗̓o�[�̍X�V
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }
}

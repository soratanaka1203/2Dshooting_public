using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MyNameSpace;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    // �v���C���[�̈ړ����x
    public float speed = 120f;
    Rigidbody2D rb; // Rigidbody2D�R���|�[�l���g���Q��
    [SerializeField] GameObject shotPoint; // �e�𔭎˂���ʒu
    [SerializeField] BulletPool bulletPool; // �e�̃v�[��
    public float bulletSpeed = 300f; // �e�̑��x
    public int bulletCount = 1; // ���˂���e�̐�
    float fireRate = 0.1f; // ���ˊԊu
    float nextFireTime = 0f; // ���̔��ˎ���

    // �J�����̋��E���`���邽�߂̕ϐ�
    private Vector3 minBounds; // �J�����̍����̋��E
    private Vector3 maxBounds; // �J�����̉E��̋��E
    private float objectWidth; // �v���C���[�̕�
    private float objectHeight; // �v���C���[�̍���

    [SerializeField] AudioClip shotSe; // �e���ˎ��̉�
    [SerializeField] AudioClip dethSe; // �v���C���[���S���̉�
    [SerializeField] AudioPlayer audioPlayer; // �I�[�f�B�I�Ǘ��N���X
    public float volum = 5f; // ����

    [SerializeField] GameObject gameOverUi; // �Q�[���I�[�o�[UI
    [SerializeField] GameObject enemySpawner; // �G�̃X�|�i�[

    [SerializeField] TextMeshProUGUI ResultGameOver;

    public bool isShield = false;
    public GameObject shieldObject; // �V�[���h�̃v���n�u�܂��͎q�I�u�W�F�N�g

    void Start()
    {
        // �Q�[���I�[�o�[UI���\���ɐݒ�
        gameOverUi.SetActive(false);
        // �G�̃X�|�i�[���A�N�e�B�u�ɐݒ�
        enemySpawner.SetActive(true);
        shieldObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D���擾

        // �e�̃v�[�����ݒ肳��Ă��Ȃ���ΒT���Ď擾
        if (bulletPool == null)
        {
            bulletPool = GameObject.Find("PlayerBulletPool").GetComponent<BulletPool>();
        }

        // �J�����̃r���[�|�[�g���E���v�Z
        Camera cam = Camera.main;
        minBounds = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)); // �J��������
        maxBounds = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)); // �J�����E��

        // �v���C���[�̃T�C�Y���擾�iCollider���K�v�j
        objectWidth = GetComponent<SpriteRenderer>().bounds.extents.x; // �I�u�W�F�N�g�̕��̔���
        objectHeight = GetComponent<SpriteRenderer>().bounds.extents.y; // �I�u�W�F�N�g�̍����̔���
    }

    void Update()
    {
        // �v���C���[�̈ړ�
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // X���̈ړ�
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime; // Y���̈ړ�

        // �V�����ʒu���v�Z
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // �J�����̋��E���Ƀv���C���[�𐧌�
        float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + objectWidth, maxBounds.x - objectWidth);
        float clampedY = Mathf.Clamp(newPosition.y, minBounds.y + objectHeight, maxBounds.y - objectHeight);

        // �v���C���[�̐V�����ʒu��ݒ�
        transform.position = new Vector3(clampedX, clampedY, newPosition.z);

        // �X�y�[�X�L�[��������Ă��āA���̔��ˎ��Ԃ𒴂�����e�𔭎�
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // ���̔��ˎ��Ԃ�ݒ�
            Shot().Forget(); // UniTask���g�p���Ĕ񓯊�����
        }
    }

    // �e��G�������������̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isShield)
            {
                SetShiled(false);
            }
            else
            {

                StartCoroutine(HandleDeath());
            }
        }
    }

    //�_���[�W���󂯂���
    private IEnumerator HandleDeath()
    {
        audioPlayer.PlayAudio(dethSe, volum); // ���S���̉����Đ�
        yield return new WaitForSeconds(0.1f); // ���Đ��̂���1�b�ҋ@

        ScoreManager.Instance.SetDisplayScore(ResultGameOver); // ���ʂ�\��
        ScoreManager.Instance.SetRanking(ScoreManager.Instance.score); // �X�R�A�������L���O�ɃZ�b�g
        ScoreManager.Instance.ResetScore(); // �X�R�A�����Z�b�g

        gameOverUi.SetActive(true); // �Q�[���I�[�o�[UI��\��
        enemySpawner.SetActive(false); // �G�̃X�|�i�[���A�N�e�B�u��

        Destroy(gameObject); // �v���C���[��j��
    }

    // �e��ł�
    private async UniTaskVoid Shot()
    {
        // �e�̃v�[���Ɣ��˓_���ݒ肳��Ă��Ȃ��ꍇ�͏I��
        if (bulletPool == null || shotPoint == null)
        {
            Debug.LogError("�e�̃v�[���܂��͔��˓_���ݒ肳��Ă��܂���B");
            return;
        }

        float spreadAngle = 15f; // �e�̊g�U�p�x

        for (int i = 0; i < bulletCount; i++)
        {
            // �v�[������e�ۂ��擾
            var bulletGB = bulletPool.GetBullet();
            if (bulletGB == null)
            {
                Debug.LogError("�e���擾�ł��܂���ł����B");
                continue; // ���̒e������
            }

            // ���ˈʒu�Ə����ݒ�
            bulletGB.transform.position = shotPoint.transform.position;

            // �e�̊p�x�𒲐�
            float angle = -spreadAngle * (bulletCount - 1) / 2 + spreadAngle * i;
            bulletGB.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            var bulletRB = bulletGB.GetComponent<Rigidbody2D>();
            bulletRB.velocity = Vector2.zero; // �O��̓��������Z�b�g

            // ���˕������v�Z
            Vector2 shotDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
            bulletRB.velocity = shotDirection * bulletSpeed;

            // �e���ˉ����Đ�
            audioPlayer.PlayAudio(shotSe, volum);
        }
    }


    //�A�C�e���̃V�[���h
    public void SetShiled(bool value)
    {
        isShield = value;
        if (isShield)
        {
            shieldObject.SetActive(true);
        }
        else
        {
            shieldObject.SetActive(false);
        }
    }
}

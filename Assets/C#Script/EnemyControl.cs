using UnityEngine;
using static IEnemy;

public class EnemyControl : MonoBehaviour
{
    public Transform shotPoint; // �e�̔��ˈʒu
    public IMovement movement; // �ړ��p�^�[�����Ǘ�
    public EnemyBulletPool enemyBulletPool; // �e�̃v�[���i�ʂ̃X�N���v�g�ŊǗ��j
    public bool isShot=false;//�e��ł��ǂ���

    public float fireRate = 3.5f; // �e�̔��ˊԊu
    private float fireCooldown = 0f; // ���˂̃N�[���_�E���^�C�}�[

    public Transform player; // �v���C���[�̈ʒu

    void FixedUpdate()
    {
        movement.Move(transform); // �G�̈ړ�

        fireCooldown -= Time.deltaTime; // �N�[���_�E��������

        if (isShot)//true��������e��ł�
        {
            // ���ˊԊu���߂�����e�𔭎�
            if (fireCooldown <= 0f)
            {
                FireBullet(); // �e�𔭎�
                fireCooldown = fireRate; // �N�[���_�E�������Z�b�g
            }
        }
    }

    // �ړ��p�^�[���̐ݒ�
    public void SetMovement(IMovement movement)
    {
        this.movement = movement;
    }

    // �e�𔭎˂��郁�\�b�h
    private void FireBullet()
    {
        if (enemyBulletPool != null && shotPoint != null && player != null)
        {
            GameObject bullet = enemyBulletPool.GetEnemyBullet(); // �e���v�[������擾

            if (bullet != null)
            {
                bullet.transform.position = shotPoint.position; // ���ˈʒu�ɐݒ�
                bullet.SetActive(true); // �e���A�N�e�B�u�ɂ���

                // �v���C���[�ւ̕������v�Z
                Vector2 direction = (player.position - shotPoint.position).normalized;

                // �e�̑��x��ݒ�
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = direction * 30f; // �v���C���[�����ɑ��x��ݒ�
                
            }
        }
    }
}

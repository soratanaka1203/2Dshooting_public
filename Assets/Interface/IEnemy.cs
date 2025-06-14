using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class IEnemy : MonoBehaviour
{
    public interface IMovement
    {
        void Move(Transform transform);
    }
}

// 下に直線運動
public class StraightMovement : IMovement
{
    public float speed = 30f;

    public void Move(Transform transform)
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

// ジグザグ運動
public class ZigzagMovement : IMovement
{
    public float speed = 23f;         
    public float frequency = 5f;     // ジグザグの速さ
    public float magnitude = 80f;   // ジグザグの幅

    public void Move(Transform transform)
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        float x = Mathf.Sin(Time.time * frequency) * magnitude;
        transform.Translate(Vector3.right * x * Time.deltaTime);
    }
}

// 円運動
public class CircularMovement : IMovement
{
    public float speed = 2f;
    public float radius = 30f;

    private float angle = 0f;
    private Vector3 centerPosition;
    private bool initialized = false;

    public void Move(Transform transform)
    {
        if (!initialized)
        {
            centerPosition = transform.position;
            initialized = true;
        }

        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = centerPosition + new Vector3(x, -Mathf.Abs(y), 0f); // 下方向に円を描く
    }
}

// 波状移動
public class WaveMovement : IMovement
{
    private float speed = 22f;
    private float waveFrequency = 7f;
    private float waveAmplitude = 5f;
    private float elapsedTime = 0f;

    public void Move(Transform transform)
    {
        elapsedTime += Time.deltaTime;

        float xOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
        float yOffset = -speed * Time.deltaTime;

        Vector3 movement = new Vector3(xOffset * Time.deltaTime, yOffset, 0);
        transform.Translate(movement, Space.World);
    }
}

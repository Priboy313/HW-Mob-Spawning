using System;
using UnityEngine;

static class DevUtils
{
    private static float s_chanceMax = 100f;

    public static int GetRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(Math.Min(min, max), Math.Max(min, max));
    }

    public static int GetRandomNumber(int max)
    {
        return GetRandomNumber(0, max);
    }

    public static float GetRandomNumber(float min, float max)
    {
        return UnityEngine.Random.Range(Math.Min(min, max), Math.Max(min, max));
    }

    public static float GetRandomNumber(float max)
    {
        return GetRandomNumber(0f, max);
    }

    public static Vector3 GetRandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(
            GetRandomNumber(min.x, max.x),
            GetRandomNumber(min.y, max.y),
            GetRandomNumber(min.z, max.z)
        );
    }

    public static Vector3 GetRandomDirection2D()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized;

        return new Vector3(
            randomCircle.x,
            0,
            randomCircle.y
        );
    }

    public static bool IsChanceSuccess(int chancePercent)
    {
        return UnityEngine.Random.value < (chancePercent / s_chanceMax);
    }
}

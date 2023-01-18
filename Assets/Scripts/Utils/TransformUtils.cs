using UnityEngine;

namespace Utils.transforms
{
    public static class TransformUtils
    {
        public static bool Approximately(Quaternion quatA, Quaternion value, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }

        public static bool Approximately(Vector3 vec1, Vector3 vec2, float acceptableRange)
        {
            Quaternion quatA = Quaternion.Euler(vec1);
            Quaternion value = Quaternion.Euler(vec2);

            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }
    }
}


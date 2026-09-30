using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public float fieldLength = 120f;
    public float fieldWidth = 70f;
    public float tryLineA = -50f;
    public float tryLineB = 50f;

    // Retourne 0 si essai Team A, 1 si essai Team B, -1 sinon
    public int CheckTry(Vector3 ballPos)
    {
        if (ballPos.z < tryLineA) return 1;
        if (ballPos.z > tryLineB) return 0;
        return -1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(fieldWidth, 0.1f, fieldLength));
    }
}
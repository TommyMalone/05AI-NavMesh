using UnityEngine;

public class Waypoint : MonoBehaviour
{
    void OnValidate()
    {
        GetComponent<TextMesh>().text = gameObject.transform.parent.gameObject.name;
    }
}

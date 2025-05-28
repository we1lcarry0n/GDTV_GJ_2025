using UnityEngine;

public class Decoration : MonoBehaviour
{
    [field: SerializeField] public int DecorationLength { get; private set; }
    [field : SerializeField] public float XOffset { get; private set; }
    [field: SerializeField] public float YOffset { get; private set; } 
}

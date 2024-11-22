using UnityEngine;

namespace GraphAndDjikstra
{
    public class Node : MonoBehaviour
    {
        public int id; // Identificador único del nodo
        public Vector3 Position => transform.position; // Posición en el mundo
    }
}
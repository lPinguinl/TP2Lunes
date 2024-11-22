using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace GraphAndDjikstra
{
    public class NpcMovement : MonoBehaviour
    {
        public GraphManager graphManager;
        public int startNodeId;
        public int targetNodeId;
        public float speed = 2f;

        private Queue<Vector3> waypoints;

        void Start()
        {
            if (graphManager == null)
            {
                Debug.LogError("GraphManager no asignado al NPC.");
                return;
            }

            Debug.Log($"Start Node ID: {startNodeId}, Target Node ID: {targetNodeId}");
            var path = graphManager.CalculateShortestPath(startNodeId, targetNodeId);

            if (path == null || path.Count == 0)
            {
                Debug.LogError("No se pudo calcular un camino válido.");
                return;
            }

            waypoints = new Queue<Vector3>();

            foreach (var nodeId in path)
            {
                var node = graphManager.nodes.Find(n => n.id == nodeId);
                if (node != null)
                {
                    waypoints.Enqueue(node.Position);
                }
            }

            StartCoroutine(MoveAlongPath());
        }

        IEnumerator MoveAlongPath()
        {
            while (waypoints.Count > 0)
            {
                var targetPosition = waypoints.Dequeue();
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}


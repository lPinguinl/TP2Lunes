using System.Collections.Generic;
using GraphAndDjikstra.Imported;
using UnityEngine;

namespace GraphAndDjikstra
{
    public class GraphManager : MonoBehaviour
    {
        public List<Node> nodes; // Nodos en la escena
        private GrafoMA graph; // Estructura del grafo

        void Awake()
        {
            graph = new GrafoMA();
            graph.InicializarGrafo();
            Debug.Log("Grafo inicializado");

            foreach (var node in nodes)
            {
                graph.AgregarVertice(node.id);
                Debug.Log($"Nodo agregado al grafo: {node.id}");
            }

            // Conexiones entre nodos (definir manualmente o dinámicamente)
            // graph.AgregarArista(idArista, nodoOrigen, nodoDestino, peso);
            graph.AgregarArista(1,1,2,1);
            graph.AgregarArista(2,2,3,3);
            graph.AgregarArista(3,3,4,5);
            graph.AgregarArista(4,4,5,6);
            graph.AgregarArista(5,5,6,7);
            
        }

        // Método para calcular el camino más corto
        public List<int> CalculateShortestPath(int sourceId, int targetId)
        {
            if (graph == null)
            {
                Debug.LogError("El grafo no ha sido inicializado.");
                return null;
            }

            if (!graph.Vertices().Pertenece(sourceId))
            {
                Debug.LogError($"El nodo de origen {sourceId} no existe en el grafo.");
                return null;
            }
            
            // Ejecutar Dijkstra
            AlgDijkstra.Dijkstra(graph, sourceId);

            // Obtener el índice del nodo destino
            int targetIndex = graph.Vert2Indice(targetId);

            // Reconstruir el camino
            string path = AlgDijkstra.nodos[targetIndex];
            List<int> pathNodes = new List<int>();
            foreach (var id in path.Split(','))
            {
                pathNodes.Add(int.Parse(id));
            }

            return pathNodes;
        }
    }
}




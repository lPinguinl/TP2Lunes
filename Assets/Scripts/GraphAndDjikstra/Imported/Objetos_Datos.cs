namespace GraphAndDjikstra.Imported
{
    public class Obj_Nodos
    {
        // dato de identificación del nodo
        public int id;

        // datos y métodos del objeto nodo
        public object dato1;

        public object dato2;

        public string descripcion()
        {
            return "Nodo id: " + id.ToString();
        }
    }

    public class Obj_Aristas
    {
        // datos de identificación y definición de la arista
        public int id;

        public int idOrig;
        public int idDest;
        public int peso;

        // datos y métodos del objeto arista
        public object dato1;

        public object dato2;

        public string descripcion()
        {
            return "Arista id: " + id.ToString() + ", Nodo Origen: " + idOrig.ToString() + ", Nodo Destino: " + idDest.ToString() + ", Peso: " + peso.ToString();
        }
    }

}

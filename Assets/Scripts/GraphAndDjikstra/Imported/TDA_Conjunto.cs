﻿namespace GraphAndDjikstra.Imported
{
    public interface ConjuntoTDA
    {
        void InicializarConjunto();
        bool ConjuntoVacio();
        void Agregar(int x);
        int Elegir();
        void Sacar(int x);
        bool Pertenece(int x);
    }

    public class Nodo
    {
        public int info;
        public Nodo sig;
    }

    // IMPLEMENTACIÓN DINÁMICA //
    public class ConjuntoLD : ConjuntoTDA
    {
        Nodo c;
        public void InicializarConjunto()
        {
            c = null;

        }
        public bool ConjuntoVacio()
        {
            return (c == null);
        }
        public void Agregar(int x)
        {
            /* Verifica que x no este en el conjunto */
            if (!this.Pertenece(x))
            {
                Nodo aux = new Nodo();
                aux.info = x;
                aux.sig = c;
                c = aux;
            }
        }
        public int Elegir()
        {
            return c.info;
        }
        public void Sacar(int x)
        {
            if (c != null)
            {
                // si es el primer elemento de la lista
                if (c.info == x)
                {
                    c = c.sig;
                }
                else
                {
                    Nodo aux = c;
                    while (aux.sig != null && aux.sig.info != x)
                        aux = aux.sig;
                    if (aux.sig != null)
                        aux.sig = aux.sig.sig;
                }
            }
        }
        public bool Pertenece(int x)
        {
            Nodo aux = c;
            while ((aux != null) && (aux.info != x))
            {
                aux = aux.sig;
            }
            return (aux != null);
        }
    }

    // IMPLEMENTACIÓN ESTÁTICA //
    public class ConjuntoTA : ConjuntoTDA
    {
        int[] a;
        int cant;

        public void Agregar(int x)
        {
            if (!this.Pertenece(x))
            {
                a[cant] = x;
                cant++;
            }
        }

        public bool ConjuntoVacio()
        {
            return cant == 0;
        }

        public int Elegir()
        {
            return a[cant - 1];
        }

        public void InicializarConjunto()
        {
            a = new int[100];
            cant = 0;
        }

        public bool Pertenece(int x)
        {
            int i = 0;
            while (i < cant && a[i] != x)
            {
                i++;
            }
            return (i < cant);
        }

        public void Sacar(int x)
        {
            int i = 0;
            while (i < cant && a[i] != x)
            {
                i++;
            }
            if (i < cant)
            {
                a[i] = a[cant - 1];
                cant--;
            }
        }
    }
}

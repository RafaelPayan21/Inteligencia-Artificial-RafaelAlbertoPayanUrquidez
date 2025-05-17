
using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoNumeros
{
    internal class Nodo : IComparable<Nodo>
    {
        public string Estado { get; private set; }
        public Nodo Padre { get; set; }
        public int Costo { get; set; }

        // Constructor para el nodo inicial
        public Nodo(string estadoString)
        {
            this.Estado = estadoString;
            this.Padre = null;
            this.Costo = 0;
        }

        // Constructor para nodos hijos
        public Nodo(string estadoString, Nodo padre)
        {
            this.Estado = estadoString;
            this.Padre = padre;
            if (padre != null)
            {
                this.Costo = padre.Costo + 1; 
            }
            else
            {
                this.Costo = 0; 
            }
        }

        public List<Nodo> GetHijos()
        {
            List<Nodo> hijos = new List<Nodo>();
            int[] dx = new int[] { 0, 1, 0, -1 }; 
            int[] dy = new int[] { -1, 0, 1, 0 };

            int indiceVacio = Estado.IndexOf('0');
            if (indiceVacio == -1)
            {

                return hijos;
            }

            (int filaVacio, int colVacio) = ConvertirIndiceACoordenadas(indiceVacio);

            for (int i = 0; i < dx.Length; i++)
            {
                int nuevaFila = filaVacio + dx[i];
                int nuevaCol = colVacio + dy[i];

                if (nuevaFila >= 0 && nuevaFila < 3 && nuevaCol >= 0 && nuevaCol < 3)
                {
                    int indiceIntercambio = ConvertirCoordenadasAIndice(nuevaFila, nuevaCol);
                    StringBuilder nuevoEstadoSb = new StringBuilder(Estado);

                    // Intercambio
                    char temp = nuevoEstadoSb[indiceVacio];
                    nuevoEstadoSb[indiceVacio] = nuevoEstadoSb[indiceIntercambio];
                    nuevoEstadoSb[indiceIntercambio] = temp;

                    hijos.Add(new Nodo(nuevoEstadoSb.ToString(), this));
                }
            }
            return hijos;
        }

        // conversión
        private (int, int) ConvertirIndiceACoordenadas(int index)
        {
            return (index / 3, index % 3);
        }

        private int ConvertirCoordenadasAIndice(int fila, int columna)
        {
            return fila * 3 + columna;
        }

        // imprimir el estado del nodo actual
        public void ImprimirEstadoActual()
        {
            Console.WriteLine("-------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"|{Estado[i * 3 + 0]}|{Estado[i * 3 + 1]}|{Estado[i * 3 + 2]}|");
            }
            Console.WriteLine("-------");
        }
        public static int[,] ConvertirStringAMatriz(string estado)
        {
            int[,] matriz = new int[3, 3];
            if (estado == null || estado.Length != 9)
                throw new ArgumentException("La cadena de estado debe tener 9 caracteres.");

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!int.TryParse(estado[i * 3 + j].ToString(), out matriz[i, j]))
                        throw new ArgumentException("La cadena de estado contiene caracteres no numéricos.");
                }
            }
            return matriz;
        }

        public static string ConvertirMatrizAString(int[,] matriz)
        {
            if (matriz == null || matriz.GetLength(0) != 3 || matriz.GetLength(1) != 3)
                throw new ArgumentException("La matriz debe ser de 3x3.");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sb.Append(matriz[i, j]);
                }
            }
            return sb.ToString();
        }

        public static void ImprimirMatriz(int[,] matriz)
        {
            Console.WriteLine("Estado del tablero:");
            for (int k = 0; k < 3; k++)
            {
                Console.Write("[");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(matriz[k, j]);
                    if (j < 2) Console.Write(", ");
                }
                Console.Write("]");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Nodo otro = (Nodo)obj;
            return Estado.Equals(otro.Estado);
        }

        public override int GetHashCode()
        {
            return Estado.GetHashCode();
        }
        public int CompareTo(Nodo other)
        {
            if (other == null) return 1;
            return this.Costo.CompareTo(other.Costo);
        }
    }
}
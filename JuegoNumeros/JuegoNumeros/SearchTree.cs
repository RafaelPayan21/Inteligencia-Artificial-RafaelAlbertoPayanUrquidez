
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JuegoNumeros
{
    internal class SearchTree
    {
        private Nodo nodoInicial;
        private string estadoObjetivo;

        public SearchTree(string estadoInicialString, string estadoObjetivoString)
        {
            this.nodoInicial = new Nodo(estadoInicialString);
            this.estadoObjetivo = estadoObjetivoString;
        }

        private List<Nodo> ReconstruirCamino(Nodo nodoFinal)
        {
            List<Nodo> camino = new List<Nodo>();
            Nodo actual = nodoFinal;
            while (actual != null)
            {
                camino.Add(actual);
                actual = actual.Padre;
            }
            camino.Reverse(); 
            return camino;
        }

        private void GenerarReporte(string nombreAlgoritmo, long tiempoMs, int nodosVisitadosHashSet, Nodo nodoSolucion)
        {
            Console.WriteLine($"--- Reporte para {nombreAlgoritmo} ---");
            Console.WriteLine($"Tiempo de ejecución: {tiempoMs} ms");
            Console.WriteLine($"Estados únicos explorados (tamaño del HashSet): {nodosVisitadosHashSet}");
            if (nodoSolucion != null)
            {
                List<Nodo> camino = ReconstruirCamino(nodoSolucion);
                Console.WriteLine($"Solución encontrada a profundidad/costo: {nodoSolucion.Costo}");
                Console.WriteLine($"Número de movimientos en la solución: {camino.Count - 1}");
            }
            else
            {
                Console.WriteLine("No se encontró solución.");
            }
            Console.WriteLine("--------------------------------------\n");
        }


        public List<Nodo> BusquedaEnProfundidad()
        {
            Stopwatch cronometro = Stopwatch.StartNew();
            Stack<Nodo> frontera = new Stack<Nodo>();
            HashSet<string> explorados = new HashSet<string>(); 

            frontera.Push(nodoInicial);

            while (frontera.Count > 0)
            {
                Nodo actual = frontera.Pop();

                if (explorados.Contains(actual.Estado))
                {
                    continue; 
                }
                explorados.Add(actual.Estado);

                // Console.WriteLine($"explorando (profundidad {actual.Costo}): {actual.Estado}");

                if (actual.Estado.Equals(estadoObjetivo))
                {
                    cronometro.Stop();
                    GenerarReporte("Búsqueda en Profundidad (DFS)", cronometro.ElapsedMilliseconds, explorados.Count, actual);
                    return ReconstruirCamino(actual);
                }

                List<Nodo> hijos = actual.GetHijos();
                hijos.Reverse();
                foreach (Nodo hijo in hijos)
                {
                    if (!explorados.Contains(hijo.Estado)) // Solo añadir si no ha sido expandido
                    {
                        frontera.Push(hijo);
                    }
                }
            }

            cronometro.Stop();
            GenerarReporte("Búsqueda en Profundidad (DFS)", cronometro.ElapsedMilliseconds, explorados.Count, null);
            return new List<Nodo>();
        }

        public List<Nodo> BusquedaEnAnchura() 
        {
            Stopwatch cronometro = Stopwatch.StartNew();
            Queue<Nodo> frontera = new Queue<Nodo>();
            HashSet<string> explorados = new HashSet<string>(); 

            frontera.Enqueue(nodoInicial);
            explorados.Add(nodoInicial.Estado);

            while (frontera.Count > 0)
            {
                Nodo actual = frontera.Dequeue();
                if (actual.Estado.Equals(estadoObjetivo))
                {
                    cronometro.Stop();
                    GenerarReporte("Búsqueda en Anchura (BFS)", cronometro.ElapsedMilliseconds, explorados.Count, actual);
                    return ReconstruirCamino(actual);
                }

                foreach (Nodo hijo in actual.GetHijos())
                {
                    if (!explorados.Contains(hijo.Estado))
                    {
                        explorados.Add(hijo.Estado);
                        frontera.Enqueue(hijo);
                    }
                }
            }
            cronometro.Stop();
            GenerarReporte("Búsqueda en Anchura (BFS)", cronometro.ElapsedMilliseconds, explorados.Count, null);
            return new List<Nodo>();
        }
        public List<Nodo> BusquedaProfundidadLimitada(int limite)
        {
            Stopwatch cronometro = Stopwatch.StartNew();
            Stack<Nodo> frontera = new Stack<Nodo>();
            HashSet<string> explorados = new HashSet<string>();

            frontera.Push(nodoInicial);

            while (frontera.Count > 0)
            {
                Nodo actual = frontera.Pop();

                //if (explorados.Contains(actual.Estado) && actual.Costo >= (explorados.FirstOrDefault(s => s == actual.Estado) != null ? nodoInicial.Costo : int.MaxValue)) // Pequeña heurística para DLS
                //{
                //     continue;
                //}
                if (explorados.Contains(actual.Estado))
                {
                    continue;
                }
                explorados.Add(actual.Estado);


                if (actual.Estado.Equals(estadoObjetivo))
                {
                    cronometro.Stop();
                    GenerarReporte($"DLS (Límite {limite})", cronometro.ElapsedMilliseconds, explorados.Count, actual);
                    return ReconstruirCamino(actual);
                }

                if (actual.Costo < limite) // Solo expandir si no hemos superado el límite
                {
                    List<Nodo> hijos = actual.GetHijos();
                    hijos.Reverse();
                    foreach (Nodo hijo in hijos)
                    {
                        if (!explorados.Contains(hijo.Estado))
                        {
                            frontera.Push(hijo);
                        }
                    }
                }
            }

            cronometro.Stop();
            GenerarReporte($"DLS (Límite {limite}) - Fallido", cronometro.ElapsedMilliseconds, explorados.Count, null);
            return new List<Nodo>(); 
        }
    }
}
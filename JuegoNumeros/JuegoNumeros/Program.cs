using System;
using System.Collections.Generic;
using System.Linq;

namespace JuegoNumeros
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputString = "364017852";
            //string inputString = "102345678";
            string outputString = "012345678";

            Console.WriteLine("Estado Inicial:");
            try
            {
                Nodo.ImprimirMatriz(Nodo.ConvertirStringAMatriz(inputString));
            }
            catch (ArgumentException e) { Console.WriteLine(e.Message); return; }

            Console.WriteLine("Estado Objetivo:");
            try
            {
                Nodo.ImprimirMatriz(Nodo.ConvertirStringAMatriz(outputString));
            }
            catch (ArgumentException e) { Console.WriteLine(e.Message); return; }


            SearchTree arbolBusqueda = new SearchTree(inputString, outputString);

            Console.WriteLine("\n--- Ejecutando Búsqueda en Profundidad (DFS) ---");
            List<Nodo> solucionDFS = arbolBusqueda.BusquedaEnProfundidad();
            if (solucionDFS.Any())
            {
                 ImprimirSolucion(solucionDFS, "DFS"); // Descomentar para ver los pasos
            }

            Console.WriteLine("\n--- Ejecutando Búsqueda en Anchura (BFS) ---");
            List<Nodo> solucionBFS = arbolBusqueda.BusquedaEnAnchura();
            if (solucionBFS.Any())
            {
                // ImprimirSolucion(solucionBFS, "BFS"); // Descomentar para ver los pasos
            }

            Console.WriteLine("\n--- Ejecutando Búsqueda en Profundidad Limitada (DLS) ---");
            int limiteDLS = solucionBFS.Any() ? solucionBFS.Last().Costo : 5;
            Console.WriteLine($"Usando límite para DLS: {limiteDLS}");
            List<Nodo> solucionDLS = arbolBusqueda.BusquedaProfundidadLimitada(limiteDLS);
            if (solucionDLS.Any())
            {
                // ImprimirSolucion(solucionDLS, $"DLS (Límite {limiteDLS})"); // Descomentar para ver los pasos
            }


            Console.WriteLine("\nPresiona cualquier tecla para salir.");
            Console.ReadKey();
        }

        public static void ImprimirSolucion(List<Nodo> camino, string nombreAlgoritmo)
        {
            if (camino == null || !camino.Any())
            {
                Console.WriteLine($"No se encontró solución con {nombreAlgoritmo}.");
                return;
            }

            Console.WriteLine($"\nPasos de la solución encontrados por {nombreAlgoritmo} ({camino.Count - 1} movimientos):");
            int pasoNum = 0;
            foreach (Nodo nodo in camino)
            {
                Console.WriteLine($"Paso {pasoNum++} (Costo/Profundidad: {nodo.Costo}):");
                nodo.ImprimirEstadoActual();
                Console.WriteLine();
            }
            Console.WriteLine($"Fin de la solución de {nombreAlgoritmo}.\n");
        }
    }
}
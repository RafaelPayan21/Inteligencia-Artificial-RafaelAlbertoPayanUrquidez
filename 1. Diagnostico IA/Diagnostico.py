# Examen Diagnóstico de un árbol binario de búsqueda
# Autor: Rafael Payan 
# Representa un nodo en un árbol binario de búsqueda.
class Nodo:
    def __init__(self, nombre):
        self.nombre = nombre   
        self.izquierda = None
        self.derecha = None

# Implementación mejorada de árbol binario con operaciones básicas.
class ArbolBinario:
    def __init__(self):
        self.raiz = None
    
    # Verifica si el árbol no contiene nodos.
    def esta_vacio(self):
        return self.raiz is None
    
    # Inserta nuevos nodos de forma iterativa evitando duplicados.
    def insertar(self, nombre):
        if self.raiz is None:
            self.raiz = Nodo(nombre)
            return True
        
        actual = self.raiz
        while True:
            if nombre < actual.nombre:
                if actual.izquierda is None:
                    actual.izquierda = Nodo(nombre)
                    return True
                actual = actual.izquierda
            elif nombre > actual.nombre:
                if actual.derecha is None:
                    actual.derecha = Nodo(nombre)
                    return True
                actual = actual.derecha
            else:
                # Valor duplicado no se inserta
                return False
    
    # Búsqueda iterativa de nodos por su nombre.  
    def buscar(self, nombre):
        actual = self.raiz
        while actual:
            if actual.nombre == nombre:
                return actual
            actual = actual.izquierda if nombre < actual.nombre else actual.derecha
        return None
    
    # Devuelve los nombres en orden ascendente.
    def recorrido_inorden(self):
        resultado = []
        pila = []
        actual = self.raiz
        
        while pila or actual:
            while actual:
                pila.append(actual)
                actual = actual.izquierda
            actual = pila.pop()
            resultado.append(actual.nombre)
            actual = actual.derecha
        
        return resultado
    
    # Calcula la altura del árbol usando recursividad.
    def altura(self):
        def _calcular_altura(nodo):
            if nodo is None:
                return 0
            return 1 + max(_calcular_altura(nodo.izquierda), _calcular_altura(nodo.derecha))
        
        return _calcular_altura(self.raiz) - 1 if self.raiz else 0


arbol_familiar = ArbolBinario()

print("El árbol está vacío" if arbol_familiar.esta_vacio() else "El árbol no está vacío")

# Insertar miembros
for nombre in ["Jose", "Ana", "Pedro", "Ana", "Rafael", "Nestor", "Pedro"]:
    if arbol_familiar.insertar(nombre):
        print(f"{nombre} insertado correctamente")
    else:
        print(f"{nombre} duplicado, no se insertó")

# Resultados de búsqueda
nombres_buscar = ["Pedro", "Ana", "Manuel"]
for nombre in nombres_buscar:
    encontrado = "Encontrado" if arbol_familiar.buscar(nombre) else "No encontrado"
    print(f"Búsqueda de {nombre}: {encontrado}")

print("\nRecorrido inorden:", arbol_familiar.recorrido_inorden())
print("Altura del árbol:", arbol_familiar.altura())
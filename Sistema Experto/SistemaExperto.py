import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl
import matplotlib.pyplot as plt

# AUTOR: RAFAEL ALBERTO PAYAN URQUIDEZ

aceleracion = ctrl.Antecedent(np.arange(1.5, 4.6, 0.1), 'aceleracion')
aceleracion['muy_lenta'] = fuzz.trapmf(aceleracion.universe, [3.5, 4.0, 4.5, 4.5])
aceleracion['rapida'] = fuzz.trimf(aceleracion.universe, [2.5, 3.0, 3.5])
aceleracion['muy_rapida'] = fuzz.trapmf(aceleracion.universe, [1.5, 1.5, 2.0, 2.5])

nurburgring = ctrl.Antecedent(np.arange(6.5, 8.1, 0.1), 'nurburgring')
nurburgring['lento'] = fuzz.trapmf(nurburgring.universe, [7.5, 7.8, 8.0, 8.0])
nurburgring['bueno'] = fuzz.trimf(nurburgring.universe, [7.0, 7.25, 7.5])
nurburgring['excelente'] = fuzz.trapmf(nurburgring.universe, [6.5, 6.5, 6.8, 7.0])


peso = ctrl.Antecedent(np.arange(1000, 2501, 10), 'peso')
peso['ligero'] = fuzz.trapmf(peso.universe, [1000, 1000, 1200, 1400])
peso['medio'] = fuzz.trimf(peso.universe, [1300, 1500, 1700])
peso['pesado'] = fuzz.trapmf(peso.universe, [1600, 1800, 2500, 2500])


evaluacion_difusa = ctrl.Consequent(np.arange(0, 101, 1), 'evaluacion_difusa')
evaluacion_difusa['baja'] = fuzz.trimf(evaluacion_difusa.universe, [0, 30, 50])
evaluacion_difusa['media'] = fuzz.trimf(evaluacion_difusa.universe, [30, 50, 70])
evaluacion_difusa['alta'] = fuzz.trimf(evaluacion_difusa.universe, [50, 70, 90])
evaluacion_difusa['excelente'] = fuzz.trimf(evaluacion_difusa.universe, [70, 90, 100])

Combinadas = [
    ctrl.Rule(aceleracion['muy_rapida'] & nurburgring['excelente'], evaluacion_difusa['excelente']),
    ctrl.Rule(peso['ligero'] & aceleracion['rapida'], evaluacion_difusa['alta']),
    ctrl.Rule(nurburgring['lento'] | peso['pesado'], evaluacion_difusa['baja']),
    ctrl.Rule(aceleracion['rapida'] & nurburgring['bueno'], evaluacion_difusa['media']),
    ctrl.Rule(aceleracion['muy_lenta'], evaluacion_difusa['baja']),
    ctrl.Rule(nurburgring['excelente'] & peso['medio'], evaluacion_difusa['alta']),
    ctrl.Rule(peso['medio'] & aceleracion['rapida'], evaluacion_difusa['media']),
]

def calcular_puntuacion_numerica(motor, marchas, traccion, consumo, unidad_consumo):
    puntos = 0
    

    if motor == "Eléctrico": puntos += 10
    elif motor == "Híbrido": puntos += 15
    else: puntos += 5  
    

    puntos += 10 if marchas == "Manual" else 5

    puntos += 5 if traccion == "AWD" else 2

    if unidad_consumo == "kWh/100km":
        puntos += 10 if consumo < 20 else 5 if consumo < 30 else 0
    else:
        puntos += 10 if consumo < 12 else 5 if consumo < 20 else 0
        
    return puntos

def evaluar_superdeportivo(*args):
    _, aceleracion_val, nurburgring_val, peso_val, motor, marchas, traccion, consumo, unidad = args
    
    aceleracion_val = np.clip(aceleracion_val, 1.5, 4.5)
    nurburgring_val = np.clip(nurburgring_val, 6.5, 8.0)
    peso_val = np.clip(peso_val, 1000, 2500)
    
    sistema = ctrl.ControlSystem(Combinadas)
    simulador = ctrl.ControlSystemSimulation(sistema)
    
    try:
        simulador.input['aceleracion'] = aceleracion_val
        simulador.input['nurburgring'] = nurburgring_val
        simulador.input['peso'] = peso_val
        simulador.compute()
        difuso = simulador.output['evaluacion_difusa']
    except:
        difuso = 0
        
    numerico = calcular_puntuacion_numerica(motor, marchas, traccion, consumo, unidad)
    
    return min(100, max(0, (difuso * 0.7) + (numerico * 0.75))) 

autos = {
    "Ferrari SF90": (340, 2.5, 6.8, 1570, "Híbrido", "Automático", "AWD", 12.5, "L/100km"),
    "Rimac Nevera": (412, 1.85, 7.1, 2150, "Eléctrico", "Automático", "AWD", 25, "kWh/100km"),
    "Porsche 911 GT3 RS": (296, 3.2, 6.82, 1430, "Combustión", "Manual", "RWD", 18, "L/100km")
}

resultados = {nombre: evaluar_superdeportivo(*datos) for nombre, datos in autos.items()}

plt.figure(figsize=(12, 6))


for termino in evaluacion_difusa.terms:
    plt.plot(evaluacion_difusa.universe, 
             fuzz.interp_membership(evaluacion_difusa.universe, 
                                  evaluacion_difusa[termino].mf, 
                                  evaluacion_difusa.universe),
             linewidth=2,
             label=termino.capitalize())


colores = ['#FF0000', '#00CC00', '#0000FF']
for (nombre, puntuacion), color in zip(resultados.items(), colores):
    plt.axvline(puntuacion, color=color, linestyle='--', linewidth=3,
                label=f'{nombre} ({puntuacion:.1f})')

plt.title('Evaluación Comparativa de Superdeportivos', fontsize=14, pad=20)
plt.xlabel('Puntuación Total (0-100)', fontsize=12)
plt.ylabel('Grado de Membresía', fontsize=12)
plt.legend(loc='upper right', fontsize=10)
plt.grid(True, alpha=0.3)
plt.xlim(0, 100)
plt.ylim(0, 1)
plt.tight_layout()

# Mostrar resultados en consola
print("\n=== RESULTADOS FINALES ===")
for nombre, puntuacion in resultados.items():
    print(f"{nombre:<20}: {puntuacion:.1f}/100")
print()

plt.show()
# Importar librerías necesarias
import tensorflow as tf
import numpy as np
import matplotlib.pyplot as plt

# 1. Cargar el dataset MNIST
mnist = tf.keras.datasets.mnist
(x_train, y_train), (x_test, y_test) = mnist.load_data()

# 2. Preprocesamiento de datos
# Normalizar los valores de píxeles (0-255) a (0-1)
x_train = x_train / 255.0
x_test = x_test / 255.0

# Convertir etiquetas a one-hot encoding
y_train = tf.keras.utils.to_categorical(y_train, 10)
y_test = tf.keras.utils.to_categorical(y_test, 10)

# 3. Construir la red neuronal
model = tf.keras.Sequential([
    # Aplanar las imágenes 28x28 a un vector de 784 elementos
    tf.keras.layers.Flatten(input_shape=(28, 28)),
    
    # Capa oculta 1 con 512 neuronas y activación ReLU
    tf.keras.layers.Dense(512, activation='relu'),
    
    # Regularización con Dropout
    tf.keras.layers.Dropout(0.2),
    
    # Capa oculta 2 con 256 neuronas
    tf.keras.layers.Dense(256, activation='relu'),
    
    # Capa de salida con 10 neuronas (una por dígito) y activación softmax
    tf.keras.layers.Dense(10, activation='softmax')
])

# 4. Compilar el modelo
model.compile(optimizer='adam',
              loss='categorical_crossentropy',
              metrics=['accuracy'])

# 5. Entrenar la red
history = model.fit(x_train, y_train,
                    epochs=10,
                    batch_size=128,
                    validation_split=0.2)

# 6. Evaluar el modelo
test_loss, test_acc = model.evaluate(x_test, y_test, verbose=2)
print(f'\nPrecisión en prueba: {test_acc:.4f}')

# 7. Visualizar resultados (opcional)
# Gráfico de precisión durante el entrenamiento
plt.plot(history.history['accuracy'], label='Precisión entrenamiento')
plt.plot(history.history['val_accuracy'], label='Precisión validación')
plt.xlabel('Épocas')
plt.ylabel('Precisión')
plt.legend()
plt.show()

# Hacer predicciones en ejemplos individuales
def predict_example(img_index):
    img = x_test[img_index]
    prediction = model.predict(img[np.newaxis, ...])
    predicted_label = np.argmax(prediction)
    true_label = np.argmax(y_test[img_index])
    
    plt.imshow(img, cmap='gray')
    plt.title(f'Predicción: {predicted_label} | Real: {true_label}')
    plt.axis('off')
    plt.show()

# Probar con una imagen de ejemplo
predict_example(0)  # Cambia el índice para probar diferentes ejemplos
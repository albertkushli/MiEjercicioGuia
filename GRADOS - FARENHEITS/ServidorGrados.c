#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>

double celsius_to_fahrenheit(double celsius) {
    return (celsius * 9 / 5) + 32;
}

double fahrenheit_to_celsius(double fahrenheit) {
    return (fahrenheit - 32) * 5 / 9;
}

int main() {
    int sock_listen, sock_conn, ret;
    struct sockaddr_in serv_addr;
    char buffer[512];

    // Crear el socket
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        perror("Error creando socket");
        exit(EXIT_FAILURE);
    }

    memset(&serv_addr, 0, sizeof(serv_addr));
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    serv_addr.sin_port = htons(8500);  // Puedes cambiar el puerto según sea necesario

    // Vincular el socket
    if (bind(sock_listen, (struct sockaddr*)&serv_addr, sizeof(serv_addr)) < 0) {
        perror("Error al vincular el socket");
        exit(EXIT_FAILURE);
    }

    // Escuchar por conexiones
    if (listen(sock_listen, 3) < 0) {
        perror("Error en el Listen");
        exit(EXIT_FAILURE);
    }

    printf("Servidor esperando conexiones...\n");

    // Bucle infinito para aceptar conexiones
    for (;;) {
        sock_conn = accept(sock_listen, NULL, NULL);
        if (sock_conn < 0) {
            perror("Error aceptando conexión");
            exit(EXIT_FAILURE);
        }

        // Recibir datos del cliente
        ret = recv(sock_conn, buffer, sizeof(buffer), 0);
        if (ret < 0) {
            perror("Error al recibir datos");
            exit(EXIT_FAILURE);
        }

        // Analizar la solicitud del cliente
        char* token = strtok(buffer, "/");
        int option = atoi(token);

        // Convertir grados
        double result;
        token = strtok(NULL, "/");
        double value = atof(token);

        if (option == 1) {
            // Celsius a Fahrenheit
            result = celsius_to_fahrenheit(value);
        } else if (option == 2) {
            // Fahrenheit a Celsius
            result = fahrenheit_to_celsius(value);
        } else {
            // Opción no válida
            printf("Opción no válida\n");
            close(sock_conn);
            continue;
        }

        // Enviar la respuesta al cliente
        snprintf(buffer, sizeof(buffer), "%.2f", result);
        send(sock_conn, buffer, strlen(buffer), 0);

        // Cerrar la conexión
        close(sock_conn);
    }

    close(sock_listen);

    return 0;
}

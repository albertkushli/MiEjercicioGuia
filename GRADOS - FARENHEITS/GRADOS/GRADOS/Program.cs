using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // Establecer conexión con el servidor
        TcpClient client = new TcpClient("192.168.56.101", 8500);  // Ajusta la dirección IP y el puerto según sea necesario
        NetworkStream stream = client.GetStream();

        Console.WriteLine("Seleccione la conversión:");
        Console.WriteLine("1. Celsius a Fahrenheit");
        Console.WriteLine("2. Fahrenheit a Celsius");
        int option = int.Parse(Console.ReadLine());

        Console.Write("Ingrese el valor: ");
        double value = double.Parse(Console.ReadLine());

        // Enviar datos al servidor
        string message = $"{option}/{value}";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        // Recibir la respuesta del servidor
        byte[] buffer = new byte[512];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string result = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        Console.WriteLine($"Resultado: {result}");

        // Cerrar la conexión
        stream.Close();
        client.Close();
    }
}

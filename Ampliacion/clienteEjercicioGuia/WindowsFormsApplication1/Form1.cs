using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);
            

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Longitud.Checked)
            {
                string mensaje = "1/" + nombre.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("La longitud de tu nombre es: " + mensaje);
            }
            else if (Bonito.Checked)
            {
                string mensaje = "2/" + nombre.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                if (mensaje == "SI")
                    MessageBox.Show("Tu nombre ES bonito.");
                else
                    MessageBox.Show("Tu nombre NO es bonito. Lo siento.");
            }
            else
            {
                // Enviamos nombre y altura
                string mensaje = "3/" + nombre.Text + "/" + alturaBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show(mensaje);
            }

            // Solicitar si el nombre es un palíndromo
            string palindromoMensaje = "4/" + nombre.Text; // Código 4 para verificar si es palíndromo
            byte[] palindromoMsg = System.Text.Encoding.ASCII.GetBytes(palindromoMensaje);
            server.Send(palindromoMsg);

            byte[] palindromoMsg2 = new byte[80];
            server.Receive(palindromoMsg2);
            palindromoMensaje = Encoding.ASCII.GetString(palindromoMsg2).Split('\0')[0];

            if (palindromoMensaje == "SI")
                MessageBox.Show("El nombre es un palíndromo.");
            else
                MessageBox.Show("El nombre NO es un palíndromo.");

            // Solicitar el nombre en mayúsculas
            string mayusculasMensaje = "5/" + nombre.Text; // Código 5 para obtener el nombre en mayúsculas
            byte[] mayusculasMsg = System.Text.Encoding.ASCII.GetBytes(mayusculasMensaje);
            server.Send(mayusculasMsg);

            byte[] mayusculasMsg2 = new byte[80];
            server.Receive(mayusculasMsg2);
            mayusculasMensaje = Encoding.ASCII.GetString(mayusculasMsg2).Split('\0')[0];

            MessageBox.Show("Nombre en mayúsculas: " + mayusculasMensaje);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";
        
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();


        }

        private void altura_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Bonito_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void nombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

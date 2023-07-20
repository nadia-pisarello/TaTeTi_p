using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaTeTi_p
{
    public partial class Form1 : Form
    {
        string turno;
        string ganador;
        bool iniciar = false;
        List<Button> buttonList = new List<Button>();
        int moves;
        DateTime inicioPartida;
        DateTime finalPartida;

        public Form1()
        {
            InitializeComponent();

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            iniciar = true;
            turno = "O";
            ListarBotones();
            LimpiarBotones();
            tb1.ReadOnly = true;
            tb2.ReadOnly = true;
            moves = 9;
            inicioPartida = DateTime.Now;
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Seguro quieren reinicar?", "Reiniciar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;
            Reiniciar();
        }

        private void Jugada_Click(object sender, EventArgs e)
        {
            if (!iniciar)
            {
                MessageBox.Show("Ingresa al menú y selecciona Iniciar");
                return;
            }
            if (moves > 0 && !Ganador())
            {
                Button btn = (Button)sender;
                turno = turno == "O" ? "X" : "O";
                btn.Text = turno;
                moves--;
                finalPartida = DateTime.Now;
            }
            if (Ganador())
            {
                ganador = turno == "O" ? tb2.Text : tb1.Text;
                finalPartida = DateTime.Now;
                MessageBox.Show(string.Format("¡{0} ha ganado!", ganador), "Ganador");

            }
            if (moves == 0 && !Ganador())
            {
                finalPartida = DateTime.Now;
                MessageBox.Show("Han empatado", "Empate");
            }
        }

        private void ListarBotones()
        {
            foreach (Control control in tablero.Controls)
            {
                Button button = control as Button;
                buttonList.Add(button);
            }
        }

        private bool Ganador()
        {
            return (buttonList[0].Text == turno && buttonList[0].Text == buttonList[1].Text && buttonList[1].Text == buttonList[2].Text) ||
          (buttonList[3].Text == turno && buttonList[3].Text == buttonList[4].Text && buttonList[4].Text == buttonList[5].Text) ||
          (buttonList[6].Text == turno && buttonList[6].Text == buttonList[7].Text && buttonList[7].Text == buttonList[8].Text) ||
          (buttonList[0].Text == turno && buttonList[0].Text == buttonList[3].Text && buttonList[3].Text == buttonList[6].Text) ||
          (buttonList[1].Text == turno && buttonList[1].Text == buttonList[4].Text && buttonList[4].Text == buttonList[7].Text) ||
          (buttonList[2].Text == turno && buttonList[2].Text == buttonList[5].Text && buttonList[5].Text == buttonList[8].Text) ||
          (buttonList[0].Text == turno && buttonList[0].Text == buttonList[4].Text && buttonList[4].Text == buttonList[8].Text) ||
          (buttonList[2].Text == turno && buttonList[2].Text == buttonList[4].Text && buttonList[4].Text == buttonList[6].Text);
        }

        private void Reiniciar()
        {
            turno = "O";
            moves = 9;
            LimpiarBotones();
        }

        private void LimpiarBotones()
        {
            foreach (Button button in buttonList)
            {
                button.Text = "";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void Guardar()
        {
            string path = string.Empty;
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Archivo de texto (*.txt) | *.txt";
            saveFile.ShowDialog();
            path = saveFile.FileName;
            if (!String.IsNullOrEmpty(path))
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    int contador = 0;
                    List<Button> listaDeBotones = new List<Button>();
                    listaDeBotones = buttonList;
                    listaDeBotones.Reverse();
                    sw.WriteLine("Judagores: {0}, {1}", tb1.Text, tb2.Text);
                    sw.WriteLine("Ganador {0}", ganador);
                    sw.WriteLine("Jugada: \r\n");
                    sw.WriteLine("\r\n-- -- -- -- -- -- -- -- --");

                    foreach (Button button in listaDeBotones)
                    {

                        sw.Write("   " + button.Text + "  |  ");
                        contador++;
                        if (contador == 3)
                        {
                            contador = 0;
                            sw.WriteLine("\r\n-- -- -- -- -- -- -- -- --");
                        }

                    }
                    sw.WriteLine("\r\nInicio Partida: " + inicioPartida.ToString());
                    sw.WriteLine("Final Partida: " + finalPartida.ToString());
                    sw.WriteLine("Duracion Partida: {0:F2} segundos", (finalPartida - inicioPartida).TotalSeconds);
                }

            }
        }
    }
}

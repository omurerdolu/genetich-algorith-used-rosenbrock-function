using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MehmetOmurErdolu_153311035
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int populasyonBoyut = (int)numericUpDown1.Value;
            int iterasyonSayisi = (int)numericUpDown2.Value;
            double min = 1;//başlangıç değeri
            double max = 30;//Soruda verilen bitiş değeri
            double caprazlamaOrani = (double)numericUpDown5.Value;
            double mutasyonOrani = (double)numericUpDown6.Value;
            double secim = 1; 
            int maxGen = 5000;
            double seckinlik = (double)numericUpDown9.Value; 
            Genetic cozum = new Genetic();
            double[] enIyiCozum = cozum.Cozum(populasyonBoyut, iterasyonSayisi, min, max, caprazlamaOrani, mutasyonOrani, secim, maxGen, seckinlik);
            for (int i = 0; i < enIyiCozum.Length; i++)
            {
                chart1.Series["Series1"].Points.Add(enIyiCozum[i]);
            }
            double hata = Genetic.Hata(enIyiCozum);
            label11.Text = "En iyi hata(penaltı) değeri: " + hata;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void listBirey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genetic sonuclar = new Genetic();

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

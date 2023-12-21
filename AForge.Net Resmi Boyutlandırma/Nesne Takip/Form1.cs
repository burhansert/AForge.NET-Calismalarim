using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.ComplexFilters;
using AForge.Imaging.ColorReduction;
using AForge.Imaging.Filters;

namespace Nesne_Takip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap resim;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                resim = (Bitmap)System.Drawing.Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = resim;
            }
            pictureBox2.Image = resim;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //trackBar1 ın maksimum ve minumum değerleri belirlendi
            trackBar1.Minimum = -300;
            trackBar1.Maximum = 300;

            comboBox1.Items.Add("ResizeNearestNeighbor");
            comboBox1.Items.Add("ResizeBicubic");
            comboBox1.Items.Add("ResizeBilinear");
        }

        void goruntuGuncelle()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //resim boyut değiştirme filtresi tanımlandı
                //bu filtre sistemi yormuyor
                //diğerleri sistemi zorluyor
                ResizeNearestNeighbor boyutlandirmaFiltresi = new ResizeNearestNeighbor(resim.Width + trackBar1.Value, resim.Height + trackBar1.Value);
                //resim dosyasına filtre uygulandı
                pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                //resim boyut değiştirme filtresi tanımlandı
                ResizeBicubic boyutlandirmaFiltresi = new ResizeBicubic(resim.Width + trackBar1.Value, resim.Height + trackBar1.Value);
                //resim dosyasına filtre uygulandı
                pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
            }
            if (comboBox1.SelectedIndex == 2)
            {
                //resim boyut değiştirme filtresi tanımlandı
                ResizeBilinear boyutlandirmaFiltresi = new ResizeBilinear(resim.Width + trackBar1.Value, resim.Height + trackBar1.Value);
                //resim dosyasına filtre uygulandı
                pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            goruntuGuncelle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            goruntuGuncelle();
        }
    }
}
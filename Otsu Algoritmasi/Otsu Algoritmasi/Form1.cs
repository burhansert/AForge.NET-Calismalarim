using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;

namespace Otsu_Algoritmasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap kaynakResmi;
            OtsuThreshold otsuFiltre = new OtsuThreshold();
   
            kaynakResmi = (Bitmap)System.Drawing.Image.FromFile("resim.jpg");

            //orjinal resim gösteriliyor
            pictureBox1.Image = kaynakResmi;

            //resmi eğer renkliyse önce griye çeviriyor sonra filtre uyguluyor
            //resim zaten griyse direk filtre uyguluyor
            kaynakResmi = otsuFiltre.Apply(kaynakResmi.PixelFormat != PixelFormat.Format8bppIndexed ? Grayscale.CommonAlgorithms.BT709.Apply(kaynakResmi) : kaynakResmi);

            //filtre uygulanan resim gösteriliyor
            pictureBox2.Image = kaynakResmi;
            
            //Uygulanan Threshold Değeri form başlığında görünüyor
            this.Text = "Threshold Değeri : " + otsuFiltre.ThresholdValue.ToString();
        }
    }
}

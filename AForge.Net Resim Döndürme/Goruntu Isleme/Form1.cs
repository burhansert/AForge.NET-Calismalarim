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

namespace Goruntu_Isleme
{
    public partial class Form1 : Form
    {
        bool tamEkran;
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap resim;

        private void Form1_Load(object sender, EventArgs e)
        {
            //trackBar1 ın maksimum ve minumum değerleri belirlendi
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 360;

            comboBox1.Items.Add("RotateNearestNeighbor");
            comboBox1.Items.Add("RotateBilinear");
            comboBox1.Items.Add("RotateBicubic");
        }

        void goruntuGuncelle()
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    //resim döndürme filtresi tanımlandı
                    //bu filtre sistemi yormuyor
                    //diğerleri sistemi zorluyor
                    //resim döndürme saat yönünün tersine doğru yapılıyor

                    //fonksiyona paremetre olarak true verilirse resmin tamamı ekrana sığdırılmıyor
                    //bazı yerler kırpılıyor
                    //fakat resim boyutu (genişliği ve yükseliği) değişmiyor
                    //görüntü daha güzel görünüyor

                    //eğer false olursa resim küçültme büyütme işlemelerinde resim boyutuda (genişliği ve yükseliği) değişiyor
                    //yani false olunca resim daima ekrana sığdırılıyor
                    RotateNearestNeighbor boyutlandirmaFiltresi = new RotateNearestNeighbor(trackBar1.Value,tamEkran);
                    //resim dosyasına filtre uygulandı
                    pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    //resim döndürme filtresi tanımlandı
                    RotateBilinear boyutlandirmaFiltresi = new RotateBilinear(trackBar1.Value, tamEkran);
                    //resim dosyasına filtre uygulandı
                    pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    //resim döndürme filtresi tanımlandı
                    RotateBicubic boyutlandirmaFiltresi = new RotateBicubic(trackBar1.Value, tamEkran);
                    //resim dosyasına filtre uygulandı
                    pictureBox2.Image = boyutlandirmaFiltresi.Apply((Bitmap)pictureBox1.Image);
                }
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                resim = (Bitmap)System.Drawing.Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = resim;
            }
            pictureBox2.Image = resim;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            goruntuGuncelle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            goruntuGuncelle();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tamEkran = !checkBox1.Checked; //dikkat, burada değili alındı
            goruntuGuncelle();
        }
    }
}

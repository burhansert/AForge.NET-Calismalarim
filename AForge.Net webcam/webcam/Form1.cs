using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace webcam
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection cihazlar;
        private VideoCaptureDevice kamera;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (kamera.IsRunning)
            {
                kamera.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
            }
            else
            {
                kamera = new VideoCaptureDevice(cihazlar[comboBox1.SelectedIndex].MonikerString);
                kamera.NewFrame += kamera_NewFrame;
                kamera.Start();
            }
        }

        private void kamera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cihazlar = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo cihaz in cihazlar)
            {
                comboBox1.Items.Add(cihaz.Name);
            }

            kamera = new VideoCaptureDevice();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (kamera.IsRunning)
                kamera.Stop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

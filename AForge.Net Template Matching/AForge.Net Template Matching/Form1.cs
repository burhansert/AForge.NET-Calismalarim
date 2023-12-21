using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AForge.Net_Template_Matching
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //Bitmap templateImage;
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap sourceImage = (Bitmap)System.Drawing.Image.FromFile("YoklamaListe.jpg");

            Bitmap template = (Bitmap)System.Drawing.Image.FromFile("sablon.jpg");

            // create template matching algorithm's instance
            // (set similarity threshold to 92.5%)
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.925f);
            // find all matchings with specified above similarity
            TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template);
            // highlight found matchings
            BitmapData data = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadWrite, sourceImage.PixelFormat);
            foreach (TemplateMatch m in matchings)
            {
                Drawing.Rectangle(data, m.Rectangle, Color.Red);
                // do something else with matching
            }
            sourceImage.UnlockBits(data);

            pictureBox1.Image = sourceImage;
            MessageBox.Show(matchings.Count().ToString());
        }
    }
}

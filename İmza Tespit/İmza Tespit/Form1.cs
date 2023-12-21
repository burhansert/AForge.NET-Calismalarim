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
using AForge.Imaging;

namespace İmza_Tespit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap kaynakResim;
        Bitmap kaynakSablon;

        //gerekli filtreler uygulandıktan sonra elde edilen ve 
        //imza tespitine hazır olan son görüntü
        Bitmap analizEdilmisResim;
        private void Form1_Load(object sender, EventArgs e)
        {
            kaynakResim = (Bitmap)System.Drawing.Image.FromFile("YoklamaListe.jpg");//(Bitmap)System.Drawing.Image.FromFile(openFileDialog1.FileName);
            kaynakSablon = (Bitmap)System.Drawing.Image.FromFile("sablon.jpg");

            analiz();
        }

        void analiz()
        {
            FiltersSequence filtreListesi = new FiltersSequence();
            filtreListesi.Add(Grayscale.CommonAlgorithms.BT709);  //First add  GrayScaling filter
            filtreListesi.Add(new OtsuThreshold()); //Then add binarization(thresholding) filter

            Bitmap yeniResim = kaynakResim.Clone() as Bitmap; //Clone image to keep original image
            yeniResim = filtreListesi.Apply(kaynakResim); // Apply filters on source image

            Bitmap yeniSablon = kaynakSablon.Clone() as Bitmap; //Clone image to keep original image
            yeniSablon = filtreListesi.Apply(kaynakSablon); // Apply filters on source image

            double kucultmeKatSayisi = Convert.ToDouble(yeniResim.Width) / 600;

            int yeniResimGenislik = Convert.ToInt32(yeniResim.Width / kucultmeKatSayisi);
            int yeniResimYukseklik = Convert.ToInt32(yeniResim.Height / kucultmeKatSayisi);
            int yeniSablonGenislik = Convert.ToInt32(yeniSablon.Width / kucultmeKatSayisi);
            int yeniSablonYukseklik = Convert.ToInt32(yeniSablon.Height / kucultmeKatSayisi);

            ResizeBilinear resmiKucult = new ResizeBilinear(yeniResimGenislik, yeniResimYukseklik);
            yeniResim = resmiKucult.Apply(yeniResim);

            ResizeBilinear sablonuKucult = new ResizeBilinear(yeniSablonGenislik, yeniSablonYukseklik);
            yeniSablon = sablonuKucult.Apply(yeniSablon);


            //şablon bulundu        
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.925f);
            TemplateMatch[] matchings = tm.ProcessImage(yeniResim, yeniSablon);
            BitmapData data = yeniResim.LockBits(
                new Rectangle(0, 0, yeniResim.Width, yeniResim.Height),
                ImageLockMode.ReadWrite, yeniResim.PixelFormat);

            yeniResim.UnlockBits(data);

            this.Text = "Bulunan şablon sayısı = " + (matchings.Count().ToString());

            int dikkateAlinacakGenislik = 570;
            int dikkateAlinacakYukseklik = 656;
            int konumX = matchings[0].Rectangle.X;
            int konumY = matchings[0].Rectangle.Bottom;

            Crop filter = new Crop(new Rectangle(konumX, konumY, dikkateAlinacakGenislik, dikkateAlinacakYukseklik));
            analizEdilmisResim = filter.Apply(yeniResim);

            pictureBox3.Image = analizEdilmisResim;

            label1.Text = "Resim Genişliği   = " + dikkateAlinacakGenislik.ToString() +
            "\nResim Yüksekliği = " + dikkateAlinacakYukseklik.ToString();

            imzaGoster(1);
        }
        int imzaKonumX;
        int imzaKonumY;
        int imzaGenislik;
        int imzaYukseklik;
        void imzaGoster(int imza)
        {
            label2.Text = "İmza " + imza.ToString() + " / 75";

            imzaGenislik = 47;
            imzaYukseklik = 16;

            imzaKonumX = ((imza - 1) / 25) * (142 + 1 + imzaGenislik) + 142;
            imzaKonumY = ((imza - 1) % 25) * (imzaYukseklik + 8 + 2) + 14;

            Bitmap tempBitmap = new Bitmap(analizEdilmisResim.Width, analizEdilmisResim.Height);
            using (Graphics g = Graphics.FromImage(tempBitmap))
            {
                g.DrawImage(analizEdilmisResim, 0, 0);
                Pen myPen1 = new Pen(System.Drawing.Color.Red, 5);
                Rectangle myRectangle1 = new Rectangle(imzaKonumX, imzaKonumY, imzaGenislik, imzaYukseklik);
                g.DrawRectangle(myPen1, myRectangle1);
            }
            pictureBox3.Image = tempBitmap;

            Crop filter = new Crop(new Rectangle(imzaKonumX, imzaKonumY, imzaGenislik, imzaYukseklik));
            Bitmap imzax = filter.Apply(analizEdilmisResim);
            pictureBox1.Image = imzax;

            Color pixelColor;
            int rToplami = 0; //sadece kırmızı renge bakıldı
            for (int i = 0; i < imzax.Width; i++)
            {
                for (int j = 0; j < imzax.Height; j++)
                {
                    pixelColor = imzax.GetPixel(i, j);
                    rToplami += pixelColor.R;
                }
            }
            int enBuyukDeger = (imzax.Width * imzax.Height * 255);
            label3.Text = "Renk Yogunluğu " + (enBuyukDeger-rToplami).ToString() + " / " + enBuyukDeger.ToString();
            int yuzdelikDeger = Convert.ToInt32(((enBuyukDeger-rToplami) /Convert.ToDouble(enBuyukDeger))*100);

            label4.Text = "Renk Yogunluğu %"+yuzdelikDeger.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            analizEdilmisResim.Save(Application.StartupPath + @"\imzaListesi.jpg", ImageFormat.Jpeg);


        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

            imzaGoster(hScrollBar1.Value);

        }
    }
}

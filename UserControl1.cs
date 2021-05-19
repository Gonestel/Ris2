using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
    public partial class UserControl1: UserControl
    {
        Bitmap bm;
        Graphics gr;

        int CellX = 1;
        int CellY = 1;
        bool FlagCell = false;
        Pen PenCell;

        bool FlagGraphic = false;
        int degree;
        double[] Coeff;
        Pen PenGraph;
        double XminB = -1, XmaxB = 1, YminB = -1, YmaxB = 1, Xmin, Xmax, Ymin, Ymax, Kx, Ky, x, y, xxx;

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ReDrawPicture();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ReDrawPicture();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        int X1scr, Y1scr, X2scr, Y2scr;
        double Yscr;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            pictureBox1.Width = this.Width - 50;
            pictureBox1.Height = this.Height - 50;

            trackBar1.Left = 0;
            trackBar1.Top = pictureBox1.Height + 1;
            trackBar1.Width = pictureBox1.Width;
            trackBar2.Left = pictureBox1.Width + 5;
            trackBar2.Top = 0;
            trackBar2.Height = pictureBox1.Height;
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            pictureBox1.Width = this.Width - 50;
            pictureBox1.Height = this.Height - 50;

            trackBar1.Left = 0;
            trackBar1.Top = pictureBox1.Height + 1;
            trackBar1.Width = pictureBox1.Width;
            trackBar2.Left = pictureBox1.Width + 5;
            trackBar2.Top = 0;
            trackBar2.Height = pictureBox1.Height;
        }


        public void InitGraph()
        {
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(bm);
        }

        private void ReDrawPicture()
        {
            gr.Clear(pictureBox1.BackColor);

            if (FlagCell)
            {
                for (int i = 0; i <= CellX; i++) gr.DrawLine(PenCell, (int)(i * bm.Width / CellX), 0, bm.Height, (int)(i * bm.Height / CellY));
                for (int i = 0; i <= CellY; i++) gr.DrawLine(PenCell, 0, (int)(i * bm.Height / CellY), bm.Width, (int)(i * bm.Width / CellX));  
            }

            if (FlagGraphic)
            {
                Xmin = ((XmaxB + XminB) / 2) - ((XmaxB - XminB) / 2)*Math.Pow(1.1, trackBar1.Value) ;         //(YmaxB+XmaxB)/2 - середина
                Xmax = (XmaxB + XminB) / 2 + ((XmaxB - XminB) / 2) * Math.Pow(1.1, trackBar1.Value);          // Xscr = Kx * (x - Xmin)     x = Xscr/Kx + Xmin
                                                                                                              // Ysc = bm.Height - Ky * (y - Ymin)

                Ymin = ((YmaxB + YminB) / 2) - ((YmaxB - YminB) / 2) * Math.Pow(1.1, trackBar2.Value);
                Xmax = (YmaxB + YminB) / 2 + ((YmaxB - YminB) / 2) * Math.Pow(1.1, trackBar2.Value);

                Kx = bm.Width / (Xmax - Xmin);
                Ky = bm.Height / (Ymax - Ymin);

                X1scr = 0;
                x = Xmin;
                xxx = 1;
                y = 0;
                for (int j = degree; j >= 0; j--)
                {
                    y += Coeff[j] * xxx;
                    xxx *= x;
                }

                Yscr = (int)(bm.Height - Ky * (y - Ymin));
                if (Yscr < 0) Y1scr = -1;
                else
                {
                    if (Yscr > bm.Height) Y1scr = bm.Height + 1;
                    else Y1scr = Yscr;
                }

                for (int i = 1; i <= bm.Width; i++)
                {
                    X2scr = i;
                    x = Xmin + X2scr / Kx;

                    xxx = 1;
                    y = 0;
                    for (int j = degree; j >= 0; j--)
                    {
                        y += Coeff[j] * xxx;
                        xxx *= x;
                    }

                    Yscr = bm.Height - Ky * (y - Ymin);
                    if (Yscr < 0) Y2scr = -1;
                    else
                    {
                        if (Yscr > bm.Height) Y2scr = bm.Height + 1;
                        else Y2scr = (int)Yscr;
                    }

                    gr.DrawLine(PenGraph, X1scr, Y1scr, X2scr, Y2scr);
                    X1scr = X2scr;
                    Y1scr = Y2scr;
                }

            }










            pictureBox1.Image = bm;
        }

        public void SetBackColor(Color cl)
        {
            pictureBox1.BackColor = cl; 
        }

        public void AddCell(int Nx, int Ny, Color cl, int w)
        {
            FlagCell = true;
            CellX = Nx;
            CellY = Ny;
            PenCell = new Pen(cl, w);
            ReDrawPicture();
        }

        public void DeleteCell()
        {
            FlagCell = false;
            PenCell.Dispose();
            ReDrawPicture();

        }

        public void AddPolinomGraphic(int n, double[] A, Color cl, int w)
        {
            degree = n;
            Coeff = new double[n + 1];
            for (int i = 0; i <= n; i++) Coeff[i] = A[i];
            PenGraph = new Pen(cl, w);
            FlagGraphic = true;
            ReDrawPicture();

        }

        public void DeletePolinomGraphic()
        {
            FlagGraphic = false;
            PenGraph.Dispose();
            Array.Clear(Coeff, 0, degree+1);
            ReDrawPicture();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeImageViewer
{
    public partial class frmMain : Form
    {
        private Bitmap mobjFormBitmap;
        private Graphics mobjBitmapGraphics;
        private int mintFormWidth;
        private int mintFormHeight;
        private Boolean mblnDoneOnce = false;
        private string mstrFileName = "";
        private string[] marrFileNames;
        private Int32 mintFileOrder;
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (!mblnDoneOnce)
            {
                mblnDoneOnce = true;
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);

                marrFileNames = Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + "\\images", "*.jpg", SearchOption.TopDirectoryOnly);
                if (marrFileNames.Length > 0)
                {
                    mintFileOrder = 0;
                    mstrFileName = marrFileNames[mintFileOrder];
                }
                RefreshDisplay();
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);
                RefreshDisplay();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            if (mobjFormBitmap != null)
                e.Graphics.DrawImage(mobjFormBitmap, 0, 0);
        }

        private void RefreshDisplay()
        {
            Font objFont;
            int intX;
            int intY;
            Image objImage;
            string strTimeStamp;
            FileInfo objFileInfo;

            mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight);

            if (mstrFileName != "") // if there are any image files in the folder
            {
                objImage = Image.FromFile(mstrFileName);
                if (mnuRotate.Checked)
                    objImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                intX = (mintFormWidth - objImage.Width) / 2;
                intY = (mintFormHeight - objImage.Height) / 2;
                mobjBitmapGraphics.DrawImage(objImage, intX, intY);
                objFileInfo = new FileInfo(mstrFileName);
                strTimeStamp = string.Format("{0:ddd MMM dd, yyyy h:mm tt}", objFileInfo.CreationTime);
                objFont = new Font("MS Sans Serif", 14, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
                mobjBitmapGraphics.DrawString(strTimeStamp, objFont, Brushes.Black, (mintFormWidth - 250) / 2, 10);
                mobjBitmapGraphics.DrawString((mintFileOrder + 1).ToString() + " of " + marrFileNames.Length.ToString(), objFont, Brushes.Black, (mintFormWidth - 60) / 2, 30);
            }

            this.Invalidate();
        }

        private void rotate90DegreesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (mintFileOrder < marrFileNames.Length - 1)
                        mintFileOrder++;
                    break;
                
                case Keys.Left:
                    if (mintFileOrder > 0)
                        mintFileOrder--;
                    break;

                case Keys.Home:
                    mintFileOrder = 0;
                    break;

                case Keys.End:
                    mintFileOrder = marrFileNames.Length - 1;
                    break;
            }
            
            mstrFileName = marrFileNames[mintFileOrder];
            RefreshDisplay();

        }

        
    }
}

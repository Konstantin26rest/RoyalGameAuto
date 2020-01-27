using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.Structure;

namespace GameAuto
{
    public partial class MainWindow : Form
    {
        private static CvBlobDetector _blobDetector;
        private List<int> m_LstCharacter = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSetROI_Click(object sender, EventArgs e)
        {
            Point pt = Location;
            Location = new Point(-2000, -2000);

            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            ROIWindow roiWnd = new ROIWindow();
            roiWnd.m_bmpScr = bitmap;
            roiWnd.ShowDialog();

            Location = pt;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            _blobDetector = new CvBlobDetector();

            //try
            //{
            //    NameValueCollection appSettings = ConfigurationManager.AppSettings;
            //    lbconfig.Text = "ROI not set";

            //    if (appSettings.Count < 1) return;

            //    int X = 0, Y = 0, W = 0, H = 0;
            //    for (int i = 0; i < appSettings.Count; i++)
            //    {
            //        string key = appSettings.GetKey(i);
            //        if (key.CompareTo("ROIx") == 0)
            //            X = Convert.ToInt32(appSettings[i]);
            //        else if (key.CompareTo("ROIy") == 0 )
            //            Y = Convert.ToInt32(appSettings[i]);
            //        else if (key.CompareTo("ROIw") == 0 )
            //            W = Convert.ToInt32(appSettings[i]);
            //        else if (key.CompareTo("ROIh") == 0)
            //            H = Convert.ToInt32(appSettings[i]);
            //    }

            //    if (X * Y * W * H == 0) return;

            //    Global.g_rcROI = new Rectangle(X, Y, W, H);
            //    lbconfig.Text = "ROI load, (" + X + ", " + Y + ", " + W + ", " + H + ")";
            //}
            //catch (ConfigurationErrorsException){}

            //Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - 5, Screen.PrimaryScreen.Bounds.Height - Height - 25);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            timer_process.Enabled = true;
        }

        private MCvScalar[] cols = { new MCvScalar(0,0,0),
            new MCvScalar(255,255,255), new MCvScalar(0,125,255),
            new MCvScalar(125,0,255), new MCvScalar(125,125,0),
            new MCvScalar(255,0,0), new MCvScalar(0,255,0),
            new MCvScalar(0,0,255), new MCvScalar(0,255,255), 
            new MCvScalar(255,255,0), new MCvScalar(255,0,255),};

        private void timer_process_Tick(object sender, EventArgs e)
        {
            timer_process.Enabled = false;

            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save("4.png");

            long Ticks = DateTime.Now.Millisecond;
            Mat mat = new Mat("4.png", Emgu.CV.CvEnum.LoadImageType.Color);
            Image<Bgr, Byte> imgBgr = mat.ToImage<Bgr, Byte>();
            Image<Gray, Byte> imgGray = mat.ToImage<Gray, Byte>();
            int nWid = imgGray.Width; int nHei = imgGray.Height;

            byte[,,] pData = imgGray.Data;
            for (int y = 0; y < nHei; y++)
            {
                for (int x = 0; x < nWid; x++)
                {
                    byte c = pData[y, x, 0];
                    if (c > 5) pData[y, x, 0] = 0;
                    else pData[y, x, 0] = 255;
                }
            }

            CvBlobs blobs = new CvBlobs();
            _blobDetector.Detect(imgGray, blobs);
            blobs.FilterByArea(100, int.MaxValue);
            //_tracker.Process(smoothedFrame, forgroundMask);
            if (blobs.Count < 1)
            {
                timer_process.Enabled = true;
                return;
            }

            //-------------------------------
            Rectangle rc = Rectangle.Empty;
            foreach (var pair in blobs)
            {
                CvBlob b = pair.Value;
                rc = b.BoundingBox;
                //CvInvoke.Rectangle(imgBgr, b.BoundingBox, new MCvScalar(255.0, 0, 0), 2);
                break;
            }

            // -------Detect Blue Region ---- /
            imgGray = imgBgr.Convert<Gray, Byte>();
            pData = imgGray.Data;
            for (int y = 0; y < nHei; y++)
            {
                for (int x = 0; x < nWid; x++)
                {
                    if (!rc.Contains(x, y))
                    {
                        pData[y, x, 0] = 0;
                        continue;
                    }

                    byte c = pData[y, x, 0];
                    if (c >= 100 && c <= 120 ) pData[y, x, 0] = 255;
                    else pData[y, x, 0] = 0;
                }
            }
                        
            blobs.Clear();
            _blobDetector.Detect(imgGray, blobs);
            blobs.FilterByArea(100, int.MaxValue);
            //_tracker.Process(smoothedFrame, forgroundMask);
            if (blobs.Count < 1)
            {
                timer_process.Enabled = true;
                return;
            }

            //-------------------------------
            rc = Rectangle.Empty;

            int nSizeMax = 0;
            foreach (var pair in blobs)
            {
                CvBlob b = pair.Value;
                if( b.BoundingBox.Width * b.BoundingBox.Height > nSizeMax)
                {
                    rc = b.BoundingBox;
                    nSizeMax = rc.Width * rc.Height;
                }
                //break;
            }
            
            CvInvoke.Rectangle(imgBgr, rc, new MCvScalar(255, 255, 0), 2);

            Global.g_rcROI = rc;

            Global.DEF_MAIN_BOARD_X = 238;
            Global.DEF_MAIN_BOARD_Y = 42;
            Global.DEF_MAIN_BOARD_W = 570;
            Global.DEF_MAIN_BOARD_H = 570;

            int nGameBoardX = Global.DEF_MAIN_BOARD_X + rc.X;
            int nGameBoardY = Global.DEF_MAIN_BOARD_Y + rc.Y;

            Global.GetRatioCalcedValues(rc.Width, rc.Height, ref nGameBoardX, ref nGameBoardY);
            Global.GetRatioCalcedValues(rc.Width, rc.Height, ref Global.DEF_MAIN_BOARD_W, ref Global.DEF_MAIN_BOARD_H);

            CvInvoke.Rectangle(imgBgr, new Rectangle(nGameBoardX, nGameBoardY, Global.DEF_MAIN_BOARD_W, Global.DEF_MAIN_BOARD_H), new MCvScalar(255, 255, 0), 2);

            Global.DEF_MARKS_X = 15;
            Global.DEF_MARKS_Y = 204;
            Global.DEF_MARKS_W = 189;
            Global.DEF_MARKS_H = 69;

            int nMarksX = Global.DEF_MARKS_X + rc.X;
            int nMarksY = Global.DEF_MARKS_Y + rc.Y;

            Global.GetRatioCalcedValues(rc.Width, rc.Height, ref nMarksX, ref nMarksY);
            Global.GetRatioCalcedValues(rc.Width, rc.Height, ref Global.DEF_MARKS_W, ref Global.DEF_MARKS_H);
            CvInvoke.Rectangle(imgBgr, new Rectangle(nMarksX, nMarksY, Global.DEF_MARKS_W, Global.DEF_MARKS_H), new MCvScalar(255, 255, 0), 2);

            int nStepX = Global.DEF_MAIN_BOARD_W / 8;
            int nStepY = Global.DEF_MAIN_BOARD_H / 8;

            var rois = new List<Rectangle>(); // List of rois
            var imageparts = new List<Image<Bgr, byte>>(); // List of extracted image parts

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle roi = new Rectangle(nGameBoardX + j * nStepX, nGameBoardY + i * nStepY, Global.DEF_ITEM_W, Global.DEF_ITEM_H);
                    rois.Add(roi);
                    imgBgr.ROI = roi;
                    imageparts.Add(imgBgr.Copy());
                }
            }

            imgBgr.ROI = Rectangle.Empty;

            m_LstCharacter.Clear();

            bool bCanProcess = true;
            int k = 0, nRow = 0, nCol = 0;
            foreach (Image<Bgr, Byte> img in imageparts)
            {
                int nCharac = (int)ImageMatcher.DetermineCharacter(img);
                m_LstCharacter.Add(nCharac);

                MovementDecision.g_AllocCharacters[nRow, nCol] = nCharac;
                nCol++;

                if (nCol >= 8)
                { nRow ++; nCol = 0; }

                //if (nCharac != 0)
                CvInvoke.Rectangle(imgBgr, rois[k], new MCvScalar(255, 255, 0), 2);
                    //CvInvoke.Rectangle(imgBgr, rois[k], cols[nCharac - 1], 2);
                
                if (nCharac == 0)
                    bCanProcess = false;

                k++;
            }

            string szLine = "";
            lstBox.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                szLine = "";
                for (int j = 0; j < 8; j++)
                {
                    szLine += "" + MovementDecision.g_AllocCharacters[i, j] + " ";
                }
                lstBox.Items.Add(szLine);
            }

            //imgBgr.Save("processed.png");
            picScr.Image = imgBgr.Bitmap;
            if (!bCanProcess)
            {
                timer_process.Enabled = true;
                return;
            }

            MovementDecision.Process();

            long Ticks2 = DateTime.Now.Millisecond;
            lbProcessTime.Text = "" + (Ticks2 - Ticks);

            timer_process.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

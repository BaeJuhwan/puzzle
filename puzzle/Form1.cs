using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            thePen = new Pen(Color.Red);
            theBrush = new SolidBrush(Color.Green);
            theFont = new Font("굴림", 15);

            imgList = new List<Image>();

            int i;
            for (i = 0; i < 16; i++)
            {
                String tmpName = String.Format("pic_{0}.png", (char)(97 + i));
                Image tmpI =Image.FromFile(tmpName);
                imgList.Add(tmpI);
            }
            theTick = 0;
            timer1.Start();
            theGameTick = 0;

            Random tmpR = new Random();
            theviewIndices = new int[16];
            for(i = 0;i < 16;i++)
            {
                theviewIndices[i] = i;
            }
            for(i = 0; i < 1000; i++)
            {
                int tmpSwap;
                int tmpRand1 = tmpR.Next(16);
                int tmpRand2 = tmpR.Next(16);
                tmpSwap = theviewIndices[tmpRand1];
                theviewIndices[tmpRand1] = theviewIndices[tmpRand2];
                theviewIndices[tmpRand2] = tmpSwap;
            }
            theEmptyIndex = 15;
            theTouchIndex = -1;

            bSuccess = 0;
        }
        Pen thePen;
        Brush theBrush;
        Font theFont;

        List<Image> imgList;

        int scrX = 50;
        int scrY = 50;
        int theTick;
        int theGameTick;

        int[] theviewIndices;
        int theEmptyIndex;
        int theTouchIndex;
        int bSuccess;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            String strTime = String.Format("Time:{0:D3}",99 - (theTick - theGameTick)/20);

            e.Graphics.DrawRectangle(thePen, 0, 0, 99 - (theTick - theGameTick)/20, 10);
            e.Graphics.DrawString(strTime, theFont, theBrush, 0, 10);

            int i;
            for(i = 0; i < 16;i++)
            {
               if(theviewIndices[i] == theEmptyIndex)
                {
                    continue;
                }
                e.Graphics.DrawImage(imgList[theviewIndices[i]], scrX + (i % 4) * 100, scrY + (i / 4) * 100, 100, 100);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            theTick++;
            Invalidate();
        }
        int[][] theWays =
        {
            new int[]{1, 4},
            new int[]{0, 2, 5},
            new int[]{1, 3, 6},
            new int[]{2, 7},
            new int[]{0, 5, 8},
            new int[]{1, 4, 6, 9},
            new int[]{2, 5, 7, 10},
            new int[]{3, 6, 11},
            new int[]{4, 9, 12},
            new int[]{5, 8, 10, 13},
            new int[]{6, 9, 11, 14},
            new int[]{7, 10, 15},
            new int[]{8, 13},
            new int[]{9, 12, 14},
            new int[] {10, 13, 15},
            new int[] {11, 14}
        };

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int i;
            int tmpX = e.X;
            int tmpY = e.Y;
            int tmpDP = 100;
            theTouchIndex = -1;
            for (i = 0; i < 16; i++)
            {
                if ( scrX + (i % 4) * tmpDP < tmpX && tmpX < scrX + (i % 4) * tmpDP + tmpDP)
                {
                    if( scrY + (i / 4) * tmpDP < tmpY && tmpY < scrY + (i / 4) * tmpDP +  tmpDP)
                    {
                        theTouchIndex = i;
                        break;
                    }
                }
            }
            if(0 <= theTouchIndex && theTouchIndex < 16)
            {
                for(i = 0; i < theWays[theTouchIndex].Length; i++)
                {
                    int tmpWayPos =theWays[theTouchIndex][i];
                    if(theviewIndices[tmpWayPos] == theEmptyIndex)
                    {
                        int tmpSwap;
                        tmpSwap = theviewIndices[tmpWayPos];
                        theviewIndices[tmpWayPos] = theviewIndices[theTouchIndex];
                        theviewIndices[theTouchIndex] = tmpSwap;
                    }
                }
                bSuccess = CheckSuccess();
            }
            Invalidate();
            if(bSuccess == 1)
            {
                MessageBox.Show("완료했습니다.");
            }
        }

        public int CheckSuccess()
        {
            int i;
            for(i = 0; i < 16; i++)
            {
                if(theviewIndices[i] == theEmptyIndex)
                {
                    continue;
                }
                if(theviewIndices[i] == i)
                {
                    continue;
                }
                break;
            }
            if(i == 16)
            {
                return (1);
            }
            return (0);
        }
    }
}

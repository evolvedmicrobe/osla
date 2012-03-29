using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Fit_Growth_Curves
{
    public class ColorMapPlateMap : SelectablePlateMap
    {
        double[] cData;
        ColorMap Cmap;
        Color[] Colors;
        bool AddText = true;
        public ColorMapPlateMap(double[] DataForColors)
        {
            cData = DataForColors;
            Colors = new Color[DataForColors.Length];
            var Data = from x in cData where SimpleFunctions.IsARealNumber(x) select x;
            var realData = Data.ToArray();
            Cmap = new ColorMap(SimpleFunctions.MinNotZero(realData), SimpleFunctions.Max(realData), true);
            for (int i = 0; i < DataForColors.Length; i++)
            {
                if (!SimpleFunctions.IsARealNumber(DataForColors[i]))
                { DataForColors[i] = 0.0; }
                Colors[i] = Cmap.GetColor(DataForColors[i]);
            }
            makeSquares();
            this.Refresh();

        }
        public ColorMapPlateMap()
            :this(new double[48])
        {
            
        }
        internal override void makeSquares()
        {

            Graphics g = Graphics.FromImage(TheColoredSquaresBM);
            g.Clear(System.Drawing.Color.White);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);
            Pen P = new Pen(Color.Black, 5);
            SolidBrush sb = new SolidBrush(Color.Blue);
            try
            {

                for (int i = 1; i <= NumCols; i++)
                {
                    g.DrawString(i.ToString(), this.Font, BlackBrush, ((float)(RoomForText + (SideLength * .25) + SideLength * (i - 1))), 0);
                }
                string Rows = "ABCDEFGH";
                Rows = Rows.Substring(0, NumRows);
                for (int i = 0; i < NumRows; i++)
                {
                    g.DrawString(Rows[i].ToString(), this.Font, BlackBrush, 0, ((float)(RoomForText + (SideLength * .35) + SideLength * i)));
                }

                int[,] TextPos = new int[NumWells, 2];
                Rectangle[] PlateWells = new Rectangle[NumWells];

                for (int i = 0; i < NumWells; i++)
                {
                    SolidBrush toPaintWith = new SolidBrush(Colors[i]);
                    int ColPos = i % NumCols;
                    int rowPos = Convert.ToInt32((i / NumCols));
                    int RecYPos = RowPositions[rowPos];
                    int RecXPos = ColumnPositions[ColPos];
                    //int RecYPos = YPos + SideLength * rowPos;
                    //int RecXPos = XPos + SideLength * ColPos;
                    TextPos[i, 0] = (int)(RecXPos + SideLength * .15);
                    TextPos[i, 1] = (int)(RecYPos + (SideLength / 2) - (this.Font.Height / 2));
                    PlateWells[i] = new Rectangle(RecXPos, RecYPos, SideLength, SideLength);
                    g.FillRectangle(toPaintWith, PlateWells[i]);
                    toPaintWith.Dispose();
                }
                g.DrawRectangles(P, PlateWells);
                //now to add text over it
                if (AddText)
                {
                    for (int i = 0; i < NumWells; i++)
                    {
                        string toWrite = cData[i].ToString("n4");
                        if (cData[i] > 100)
                        {
                            int Value = (int)cData[i];
                            toWrite = Value.ToString();
                        }
                        g.DrawString(toWrite, this.Font, BlackBrush, TextPos[i, 0], TextPos[i, 1]);
                    }
                }
            }
            finally { g.Dispose(); BlackBrush.Dispose(); P.Dispose(); sb.Dispose(); }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace Fit_Growth_Curves
{
    public class ColorMap
    {
        private int colormapLength = 64;
        private int alphaValue = 255;
        private double min;
        private double max;
        private bool darkoutsiderange;
        private int[,] ColorArray;//the array with the color map in it
        public ColorMap(double Min, double Max,bool DarkBelowRange)
        {
            darkoutsiderange = DarkBelowRange;//This determines if colors below the minimum should be set at the lowest color value, or appear white
            min = Min;
            max = Max;
            ColorArray = Spring();
        }
        //public ColorMap(int colorLength)
        //{
        //    colormapLength = colorLength;
        //}
        //public ColorMap(int colorLength, int alpha)
        //{
        //    colormapLength = colorLength;
        //    alphaValue = alpha;
        //}
        public int[,] Spring()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] spring = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                spring[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (int)(255 * spring[i]);
                cmap[i, 3] = 255 - cmap[i, 1];
            }
            return cmap;
        }
        public Color GetColor(double value)
        {
            int m = 64;//the slope for the color
            int colorIndex = 0;
            if (value < min && !darkoutsiderange) { colorIndex = 0; }
            else if (value < min && darkoutsiderange) { return Color.GhostWhite; }
            else if (value >= max) { colorIndex = colormapLength - 1; }
            else { colorIndex = (int)((value - min) * colormapLength / (max - min)); }
            Color ColorToReturn = Color.FromArgb(ColorArray[colorIndex, 0], ColorArray[colorIndex, 1], ColorArray[colorIndex, 2], ColorArray[colorIndex, 3]);
            return ColorToReturn;
        }
    }
    public class BinaryColorMap
    {
        //This class has no nuance, it is designed to return either white or red , for values zero or 1
        public static Color GetColor(double value)
        {
            if(value==1)
            {
                return Color.Red;
            }
            else
            {
                return Color.GhostWhite;
            }
        }
    }
    public class Plate_Tools
    {
        /// This will be class of tools for interacting with Plate Data
        /// It will mostly be designed for grabbing data or not

        //A bitmap to store the colored squares
        public System.Drawing.Bitmap TheColoredSquaresBM;
        int SideLength;
        int RoomForText;
        bool IS_48WELL_PLATE;
        int totalLength;
        int totalHeight;
        private Font FontofControl;
        bool AddText = true;
        int NumRows;
        int NumCols;
        int NumWells;
        bool UseBinaryColorScheme = false;//The binary color scheme is designed to show either blanks or not blanks
      
        public Plate_Tools(Fit_Growth_Curves.Form1 curForm)
            : this(curForm, 50, false, false)
        {
            AddText = true;
        }
        public Plate_Tools(Fit_Growth_Curves.Form1 curForm, bool Is48WellPlate)
            : this(curForm, 50, false, Is48WellPlate)
        {
            AddText = true;
        }
        public Plate_Tools(Fit_Growth_Curves.Form1 curForm, int sidelength, bool BinaryColors,bool Is48WellPlate)
        {
            UseBinaryColorScheme = BinaryColors;
            AddText=false;
            IS_48WELL_PLATE = Is48WellPlate;
            SetPlateValues();
            //This is a constructor for making much smaller boxes with no text, it is designed to graphically
            //represent the location of the blanks
            MakeIntsToCellName();
            MakeRowColArray();
            FontofControl = curForm.Font;
            SideLength = sidelength;
            RoomForText = 15;
            totalLength = (int)(RoomForText + 2 + NumCols * SideLength);
            totalHeight = (int)(RoomForText + 2 + NumCols * SideLength);
            TheColoredSquaresBM = new System.Drawing.Bitmap(totalLength, totalHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
  
        }

        public Dictionary<int, string> IntsToCellName = new Dictionary<int, string>();
        public Dictionary<string, int> CellNameToInts = new Dictionary<string, int>();

        public int[,] RowColArray;
        private void MakeRowColArray()
        {
            ///This method will create a row column array, the first element will be the row and the second the col
            ///Thus rowcolarray[i,0]=row of index 1 and rowcolarray[i,1]=col of index 1, zero order
            
            RowColArray = new int[NumWells, 2];//first element row, second column, indexed by row position
            int index = 0;
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumCols; col++)
                {
                    RowColArray[index, 0] = row;
                    RowColArray[index, 1] = col;
                    index++;
                }
            }
        }
        private void MakeIntsToCellName()
        {
            string RowNames = "ABCDEFGH";
            int count = 0;
            RowNames = RowNames.Substring(0, NumRows);
            foreach (char c in RowNames)
            {
                for (int i = 1; i <= NumCols; i++)
                {
                    IntsToCellName.Add(count, c.ToString()  + i.ToString());
                    CellNameToInts.Add(c.ToString() + i.ToString(), count);
                    count++;
                }
            }
        }
        public int[] ReturnIndexesInRectangle(int TopLeftIndex, int BottomRightIndex)
        {
            //This method will return the indexes in a rectangle defined by the inputs
            int startrow = RowColArray[TopLeftIndex, (int)0];
            int startcol = RowColArray[TopLeftIndex, (int)1];
            int endrow = RowColArray[BottomRightIndex, (int)0];
            int endcol = RowColArray[BottomRightIndex, (int)1];
            int sizeofArray = (endrow - startrow + 1) * (endcol - startcol + 1);
            int[] IndexesToReturn = new int[sizeofArray];
            int spot = 0;
            for (int i = startrow; i < endrow + 1; i++)
            {
                for (int j = startcol; j < endcol + 1; j++)
                {

                    IndexesToReturn[spot] = i * NumCols + j;
                    spot++;
                }
            }
            return IndexesToReturn;

        }
        private int[] ReturnIndexesInRows(int[] rows)
        {
            //This method will return the indexes for a particular set of rows
            //So for example if given rows 2-4, it will return the indexes of all the cells there
            int[] indexes = new int[NumCols * rows.Length];
            int spot = 0;
            foreach (int i in rows)
            {
                for (int j = 0; j < NumCols; j++)
                {
                    indexes[spot] = NumCols * i + j;
                    spot++;
                }
            }
            return indexes;
        }
        public void makeSquares(double[] DataToSquare)
        {

            Graphics g = Graphics.FromImage(TheColoredSquaresBM);
            g.Clear(System.Drawing.Color.White);
            //ColorMap Cmap = new ColorMap(0,1);

            //ColorMap Cmap = new ColorMap(SimpleFunctions.Min(DataToSquare), SimpleFunctions.Max(DataToSquare),false);
            //USE THE ONE UNDERNEATH
            //now going to make data so it excludes NaN
            var Data = from x in DataToSquare where SimpleFunctions.IsARealNumber(x) select x;
            var realData=Data.ToArray();
            ColorMap Cmap = new ColorMap(SimpleFunctions.MinNotZero(realData), SimpleFunctions.Max(realData),true);
            //ColorMap Cmap = new ColorMap(0, 1, true);
            
            //Color[] ColorNums =new Color[96];
            //for(int i=0;i<96;i++)
            //{ ColorNums[i]=Cmap.GetColor(i);}
            SolidBrush BlackBrush = new SolidBrush(Color.Black);
            Pen P = new Pen(Color.Black, 5);
            SolidBrush sb = new SolidBrush(Color.Blue);
            try
            {
                int YPos = RoomForText;
                int XPos = RoomForText;
              
                for (int i = 1; i <=NumCols; i++)
                {
                    g.DrawString(i.ToString(), FontofControl, BlackBrush, ((float)(RoomForText + (SideLength * .25) + SideLength * (i - 1))), 0);
                }
                string Rows = "ABCDEFGH";
                Rows = Rows.Substring(0, NumRows);
                for (int i = 0; i < NumRows; i++)
                {
                    g.DrawString(Rows[i].ToString(), FontofControl, BlackBrush, 0, ((float)(RoomForText + (SideLength * .35) + SideLength * i)));
                }            

                int[,] TextPos = new int[NumWells, 2];
                Rectangle[] PlateWells = new Rectangle[NumWells];
                
                for (int i = 0; i < NumWells; i++)
                {
                    double colToSquare = DataToSquare[i];
                    if (!SimpleFunctions.IsARealNumber(colToSquare))
                    { colToSquare = 0.0; }
                    sb = new SolidBrush(Cmap.GetColor(colToSquare));
                    if (UseBinaryColorScheme)
                    {
                        sb = new SolidBrush(BinaryColorMap.GetColor(colToSquare));
                    }
                    int ColPos = i % NumCols;
                    int rowPos = Convert.ToInt32((i / NumCols));
                    int RecYPos = YPos + SideLength * rowPos;
                    int RecXPos = XPos + SideLength * ColPos;
                    TextPos[i, 0] = (int)(RecXPos + SideLength * .15);
                    TextPos[i, 1] = (int)(RecYPos + (SideLength / 2) - (FontofControl.Height / 2));
                    PlateWells[i] = new Rectangle(RecXPos, RecYPos, SideLength, SideLength);
                    g.FillRectangle(sb, PlateWells[i]);
                }
                g.DrawRectangles(P, PlateWells);
                //now to add text over it
                if (AddText)
                {
                    for (int i = 0; i < NumWells; i++)
                    {
                        string toWrite=DataToSquare[i].ToString("n4");
                        if (DataToSquare[i] > 100)
                        {
                            int Value=(int)DataToSquare[i];
                            toWrite=Value.ToString();
                        }
                        g.DrawString(toWrite, FontofControl, BlackBrush, TextPos[i, 0], TextPos[i, 1]);
                    }
                }  
            }
            finally { g.Dispose(); BlackBrush.Dispose(); P.Dispose(); sb.Dispose(); }
        }
        public int[] GetInverseSquares(int[] WellsCurrent)
        {
            //This will take a list of well locations, and return a new list that is the opposite of it
            //for example if the old list had well B1, the new list would have everything but B1
            List<int> OldWells = new List<int>(WellsCurrent);
            List<int> NewWells = new List<int>();
            for (int i = 0; i < NumWells; i++)
            {
                if (!OldWells.Contains(i))
                {
                    NewWells.Add(i);
                }
            }
            int[] toReturn = new int[NewWells.Count];
            NewWells.CopyTo(toReturn);
            return toReturn;
            

           

        }
        private void SetPlateValues()
        {
            if (IS_48WELL_PLATE)
            {
                NumRows = 6;
                NumCols = 8;
                NumWells = 48;
            }
            else
            {
                NumRows = 8;
                NumCols = 12;
                NumWells = 96;
            }
        }
    }
}

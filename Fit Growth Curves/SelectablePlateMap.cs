using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fit_Growth_Curves
{
    
    public partial class SelectablePlateMap : UserControl
    {
        protected System.Drawing.Bitmap TheColoredSquaresBM;
        protected int SideLength;
        protected int RoomForText;
        bool IS_48WELL_PLATE=true;
        public bool SHOW_GROUP_NUMBER = true;
        protected int NumRows;
        protected int NumCols;
        protected int NumWells;
        static public int MAX_GROUP_ASSIGNMENTS = 17;//0 is considered unselected
        protected static Color[] ColorAssignments;
        protected int[] CurWellAssignment;
        protected int[] RowPositions, ColumnPositions;
        protected int pCurGroupToSelect = 1;
        public int CurGroupToSelect
        {
            get { return pCurGroupToSelect; }
            set {
                if (value >= MAX_GROUP_ASSIGNMENTS || value < 0) { throw new Exception("Cannot assign a group higher than the plate map has colors for"); }
                pCurGroupToSelect=value;
                }
        }
        public Dictionary<int, string> IntsToCellName = new Dictionary<int, string>();
        public Dictionary<string, int> CellNameToInts = new Dictionary<string, int>();
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public delegate void WellChangedEventHandler(object sender, SelectablePlateEventArgs e);
        public event ChangedEventHandler GroupsChanged;
        public event ChangedEventHandler IndividualWellChanged;
        public SelectablePlateMap()
        {
            this.Size=new Size(225,225);
            SetColors();            
            SetPlateValues();
            MakeRowColArray();
            MakeIntsToCellName();            
            RoomForText = 15;
            int totalHeight = this.Height;
            int totalWidth = this.Width;
            
            SideLength=(int)((totalHeight-2-RoomForText)/Convert.ToDouble(NumCols));
            TheColoredSquaresBM = new System.Drawing.Bitmap(totalWidth, totalHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CurWellAssignment = new int[NumCols * NumRows];
            MakeRectanglePostions();
            makeSquares();
            InitializeComponent();
        }
        protected void OnInidividualWellChanged(int Well,int GroupAssignedTo)
        {
            RecreateImage();
            if (IndividualWellChanged != null)
            {
                string WellName = IntsToCellName[Well];
                SelectablePlateEventArgs args = new SelectablePlateEventArgs(WellName, Well, GroupAssignedTo);
                IndividualWellChanged(this, args);
            }
            OnAssignmentsChanged();
        }
        protected void OnAssignmentsChanged()
        {
            if (GroupsChanged != null)
            {
                GroupsChanged(this, EventArgs.Empty);
            }
        }
        void SetColors()
        {
            ColorAssignments = new Color[MAX_GROUP_ASSIGNMENTS];
            ColorAssignments[0] = this.BackColor;
            ColorAssignments[1] = Color.Green;
            ColorAssignments[2] = Color.Red;
            ColorAssignments[3] = Color.Blue;
            ColorAssignments[4] = Color.Black;
            ColorAssignments[5] = Color.Purple;
            ColorAssignments[6] = Color.Brown;
            ColorAssignments[7] = Color.Aquamarine;
            ColorAssignments[8] = Color.Chartreuse;
            ColorAssignments[9] = Color.DarkGoldenrod;
            ColorAssignments[10] = Color.Gray;
            ColorAssignments[11] = Color.Orange;
            ColorAssignments[12] = Color.Indigo;
            ColorAssignments[13] = Color.ForestGreen;
            ColorAssignments[14] = Color.Crimson;
            ColorAssignments[15] = Color.DeepSkyBlue;
            ColorAssignments[16] = Color.Wheat;
        }
        protected override void OnClick(EventArgs e)
        {
            MouseEventArgs mea = e as MouseEventArgs;
            int? selectedWell = ReturnCellIndexOfMouseClick(mea);
            if (selectedWell.HasValue)
            {
                int well = selectedWell.Value;
                int curAssignment = CurWellAssignment[well];
                int FutureAssignment;
                if (curAssignment==pCurGroupToSelect)
                { FutureAssignment = 0; }
                else
                { FutureAssignment = pCurGroupToSelect; }
                CurWellAssignment[well]=FutureAssignment;
                OnInidividualWellChanged(selectedWell.Value,FutureAssignment); 
            }            
            base.OnClick(e);
        }
        public void RecreateImage()
        {
            makeSquares();
            this.Refresh();
        }
        protected int? ReturnCellIndexOfMouseClick(MouseEventArgs mea)
        {
            int x = mea.X;
            int y = mea.Y;
            int? RowPosition = DetermineIntervalValueIsIn(RowPositions, y);
            int? ColumnPosition = DetermineIntervalValueIsIn(ColumnPositions, x);
            int? toReturn = new int?();
            if (RowPosition.HasValue && ColumnPosition.HasValue)
            {
                toReturn = RowColInverseArray[RowPosition.Value, ColumnPosition.Value];
                return toReturn;
            }
            return toReturn;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;
            graphicsObj.DrawImage(TheColoredSquaresBM,0, 0,TheColoredSquaresBM.Width, TheColoredSquaresBM.Height);
            base.OnPaint(e);
        } 
        public int[,] RowColArray;
        public int[,] RowColInverseArray;
        private void MakeRowColArray()
        {
            ///This method will create a row column array, the first element will be the row and the second the col
            ///Thus rowcolarray[i,0]=row of index 1 and rowcolarray[i,1]=col of index 1, zero order
            
            RowColArray = new int[NumWells, 2];//first element row, second column, indexed by row position\
            RowColInverseArray = new int[NumRows, NumCols];
            int index = 0;
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumCols; col++)
                {
                    RowColArray[index, 0] = row;
                    RowColArray[index, 1] = col;
                    RowColInverseArray[row, col] = index;
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
                    IntsToCellName[count]=c.ToString()  + i.ToString();
                    CellNameToInts[c.ToString() + i.ToString()]= count;
                    count++;
                }
            }
        }
        protected int[] ReturnIndexesInRectangle(int TopLeftIndex, int BottomRightIndex)
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
        protected int[] ReturnIndexesInRows(int[] rows)
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
        protected void MakeRectanglePostions()
        {
          
           //Add two to make it the 
            RowPositions = new int[NumRows+1];
            ColumnPositions = new int[NumCols + 1];
            RowPositions[0] = ColumnPositions[0] = RoomForText;
            for (int i = 1; i < NumRows+1; i++)
            {
                RowPositions[i]= RowPositions[0] + SideLength * i;
            }
            for (int i = 1; i < NumCols + 1; i++)
            {
                ColumnPositions[i]=ColumnPositions[0]+ SideLength * i;
            }
        }
        protected int? DetermineIntervalValueIsIn(int[] Positions, int Position)
        {
            for (int i = 0; i < Positions.Length-1; i++)
            {
                if (Position >= Positions[i] && Position < Positions[i + 1])
                {int? FoundValue=i;return FoundValue;}
            }
            int? toReturn=new int?();
            return toReturn;
        }
        internal virtual void makeSquares()
        {
            Graphics g = Graphics.FromImage(TheColoredSquaresBM);
            g.Clear(System.Drawing.Color.White);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);
            Pen P = new Pen(Color.Black, 5);
            SolidBrush[] Brushes = new SolidBrush[MAX_GROUP_ASSIGNMENTS];
            int index=0;
            foreach (Color c in ColorAssignments)
            { Brushes[index] = new SolidBrush(c); index++; }
            try
            {
                //int YPos = RoomForText;
                //int XPos = RoomForText;
                for (int i = 1; i <=NumCols; i++)
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
                SolidBrush toPaintWith;
                for (int i = 0; i < NumWells; i++)
                {   
                    toPaintWith = Brushes[CurWellAssignment[i]];
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
                }
                g.DrawRectangles(P, PlateWells);
                //now to add text over it
                if (SHOW_GROUP_NUMBER)
                {
                    for (int i = 0; i < NumWells; i++)
                    {
                        if (CurWellAssignment[i] > 0)
                        {
                            g.DrawString(CurWellAssignment[i].ToString(), this.Font, BlackBrush, TextPos[i, 0], TextPos[i, 1]);
                        }
                    }
                }
            }
            finally { g.Dispose(); BlackBrush.Dispose(); P.Dispose(); foreach (SolidBrush sb in Brushes) { sb.Dispose(); } }
        }
        protected int[] GetInverseSquares(int[] WellsCurrent)
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
        protected void SetPlateValues()
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
        public IEnumerable<string> GetNamesOfWellsAssignedToGroup(int TreatmentNumber, out Color GroupColor)
        {
            GroupColor = ColorAssignments[TreatmentNumber];
            List<int> Indexes = new List<int>();
            for (int i = 0; i < CurWellAssignment.Length; i++)
            { if (CurWellAssignment[i] == TreatmentNumber)Indexes.Add(i); }
            var Names = from x in Indexes select IntsToCellName[x];
            return Names;
        }
        public void ClearAllAssignments()
        {
            ClearAllAssignments(true);
        }
        public void ClearAllAssignments(bool AlsoRedraw)
        {
            for (int i = 0; i < CurWellAssignment.Length; i++)
            {
                CurWellAssignment[i] = 0;
            }
            OnAssignmentsChanged();
            if (AlsoRedraw)
            { RecreateImage(); }
        }
        /// <summary>
        /// Does not fire event currently
        /// </summary>
        /// <param name="WellName"></param>
        /// <param name="GroupIndex"></param>
        public void AssignWellToGroup(string WellName, int GroupIndex)
        {
            if (!CellNameToInts.ContainsKey(WellName))
            { throw new Exception("Well Name Does Not Exist" + WellName); }
            if (GroupIndex >= MAX_GROUP_ASSIGNMENTS)
            { throw new Exception("Tried to Assign Well to non-existant group"); }
            int wellIndex = CellNameToInts[WellName];
            CurWellAssignment[wellIndex] = GroupIndex;                
        }
        public bool isValidWellName(string Name)
        {
            return CellNameToInts.ContainsKey(Name);
        }
        public int GetGroupAssignmentForWell(string Well)
        {
            int wellIndex = CellNameToInts[Well];
            return CurWellAssignment[wellIndex];
        }

        private void SelectablePlateMap_Load(object sender, EventArgs e)
        {

        }
    }
    public class SelectablePlateEventArgs : EventArgs
    {
        public string WellID;
        public int WellIndex;
        public int GroupAssignedTo;
        public SelectablePlateEventArgs(string WellID, int index, int GroupAssignedTo)
        {
            this.WellID = WellID;
            this.WellIndex = index;
            this.GroupAssignedTo = GroupAssignedTo;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Game
{
    enum RotationEnum
    {
        deg0   = 0,
        deg90  = 1,
        deg180 = 2,
        deg270 = 3
    }

    enum BlockTypeNum
    {
        block01 = 0,
        block02 = 1,
        block03 = 2,
        block04 = 3,
        block05 = 4,
        block06 = 5,
        block07 = 6,
        block08 = 7,
        block09 = 8,
        block10 = 9,
        block11 = 10,
        block12 = 11
    }

    enum DifficultyEnum
    {
        Easy = 0,
        Hard = 1
    }

    struct StructBlock
    {
        public RotationEnum angle;
        public BlockTypeNum type;

        public StructBlock(RotationEnum newAngle, BlockTypeNum newType)
        {
            this.angle = newAngle;
            this.type  = newType;
        }
    }

    struct StructBlockStyle
    {
        public System.Drawing.Color color;
        public Boolean              isBlock;

        public StructBlockStyle(System.Drawing.Color newColor, Boolean newIsBlock)
        {
            this.color   = newColor;
            this.isBlock = newIsBlock;
        }
    }

    public delegate void TetrisHandler(object o, EventArgs e);

    public class EventArgs
    {
        public readonly int RowsCompleted;

        public EventArgs(int r)
        {
            RowsCompleted = r;
        }
    }

    class BaseClass
    {
        protected static int BLOCK_SIZE = 4; // size of the block(4v4)

        protected static bool[]             arrBlock    = new bool[BLOCK_SIZE << 2];
        protected static WindowRect         TetrisField = new WindowRect();
        protected static Point              m_blockpos  = new Point();
        protected static StructBlock        m_block     = new StructBlock();
        protected static int                m_Width;
        protected static int                m_Height;
        protected static StructBlockStyle[] arrField;
    }

    class BlockClass : BaseClass
    {
        public System.Windows.Forms.ImageList imlBlockImage = 
            new System.Windows.Forms.ImageList();

        public BlockClass()
        {
            String AppPath = String.Concat(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, 
                                @"\Images\Blocks\");

            imlBlockImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imlBlockImage.ImageSize = new Size(this.Width, this.Height);
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockred.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockblue.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockgreen.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockcyan.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockyellow.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockorange.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockmagenta.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockbrown.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockdarkblue.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockgreenyellow.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockpink.jpg")));
            imlBlockImage.Images.Add(Image.FromFile(String.Concat(AppPath, "blockwhite.jpg")));
        }

        public int Width
        {
            get
            {
                if (m_Width.Equals(0))
                    return 24;
                else
                    return m_Width;
            }
            set
            {
                m_Width = value;
            }
        }

        public int Height
        {
            get
            {
                if (m_Height.Equals(0))
                    return 24;
                else
                    return m_Height;
                
            }
            set
            {
                m_Height = value;
            }
        }

        public RotationEnum Angle
        {
            get
            {
                return m_block.angle;
            }
            set
            {
                m_block.angle = value;
            }
        }

        public BlockTypeNum Type
        {
            get
            {
                return m_block.type;
            }
            set
            {
                m_block.type = value;
            }
        }

        public int Size
        {
            get
            {
                return BLOCK_SIZE;
            }
        }

        public System.Drawing.Color Color(BlockTypeNum typBlock)
        {
            // this function returns the color of the block.
            switch (typBlock)
            {
                case BlockTypeNum.block01:
                    return System.Drawing.Color.Red;
                case BlockTypeNum.block02:
                    return System.Drawing.Color.Blue;
                case BlockTypeNum.block03:
                    return System.Drawing.Color.Green;
                case BlockTypeNum.block04:
                    return System.Drawing.Color.Cyan;
                case BlockTypeNum.block05:
                    return System.Drawing.Color.Yellow;
                case BlockTypeNum.block06:
                    return System.Drawing.Color.Orange;
                case BlockTypeNum.block07:
                    return System.Drawing.Color.Magenta;
                case BlockTypeNum.block08:
                    return System.Drawing.Color.Brown;
                case BlockTypeNum.block09:
                    return System.Drawing.Color.DarkBlue;
                case BlockTypeNum.block10:
                    return System.Drawing.Color.GreenYellow;
                case BlockTypeNum.block11:
                    return System.Drawing.Color.Pink;
                default:
                    return System.Drawing.Color.White;
            }
        }

        public StructBlock Generate(DifficultyEnum d)
        {
            Random rnd = new Random();

            if(d.Equals(DifficultyEnum.Easy))
                return new StructBlock((RotationEnum)rnd.Next(0, Enum.GetNames(typeof(RotationEnum)).Length),
                                       (BlockTypeNum)rnd.Next(0, 7));
            else
                return new StructBlock((RotationEnum)rnd.Next(0, Enum.GetNames(typeof(RotationEnum)).Length),
                                       (BlockTypeNum)rnd.Next(0, Enum.GetNames(typeof(BlockTypeNum)).Length));
        }

        public WindowRect Rotate(RotationEnum newAngle)
        {
            WindowRect wrBlock = new WindowRect();

            Angle = newAngle;
            Build();
            Adjustment(ref wrBlock);

            return wrBlock;
        }

        public void Build()
        {
            // Get the data for the block.
            arrBlock = GetBlockData(new StructBlock(Angle, Type));
        }

        public Boolean[] GetBlockData(StructBlock structBlock)
        {
            // 0123
            // 4567
            // 8901
            // 2345

            // data for 4v4 block shapes
            bool[] arrData = new bool[BLOCK_SIZE << 2];

            switch (structBlock.type)
            {
                case BlockTypeNum.block01:
                    if (structBlock.angle.Equals(RotationEnum.deg0) ||
                        structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[2]  = true; // ..#. 0123
                        arrData[6]  = true; // ..#. 4567
                        arrData[10] = true; // ..#. 8901
                        arrData[14] = true; // ..#. 2345
                    }
                    else
                    {
                        arrData[12] = true; // .... 0123
                        arrData[13] = true; // .... 4567
                        arrData[14] = true; // .... 8901
                        arrData[15] = true; // #### 2345
                    }

                    break;
                case BlockTypeNum.block02:
                    arrData[0] = true; // ##.. 0123
                    arrData[1] = true; // ##.. 4567
                    arrData[4] = true; // .... 8901
                    arrData[5] = true; // .... 2345
                    break;
                case BlockTypeNum.block03:
                    if (structBlock.angle.Equals(RotationEnum.deg0) ||
                        structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[5] = true; // .... 0123
                        arrData[6] = true; // .##. 4567
                        arrData[8] = true; // ##.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    else
                    {
                        arrData[1]  = true; // .#.. 0123
                        arrData[5]  = true; // .##. 4567
                        arrData[6]  = true; // ..#. 8901
                        arrData[10] = true; // .... 2345
                    }
                    break;
                case BlockTypeNum.block04:
                    if (structBlock.angle.Equals(RotationEnum.deg0) ||
                        structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[4]  = true; // .... 0123
                        arrData[5]  = true; // ##.. 4567
                        arrData[9]  = true; // .##. 8901
                        arrData[10] = true; // .... 2345
                    }
                    else
                    {
                        arrData[2] = true; // ..#. 0123
                        arrData[5] = true; // .##. 4567
                        arrData[6] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    break;
                case BlockTypeNum.block05:
                    if (structBlock.angle.Equals(RotationEnum.deg0))
                    {
                        arrData[4] = true; // .... 0123
                        arrData[5] = true; // ###. 4567
                        arrData[6] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg90))
                    {
                        arrData[1] = true; // .#.. 0123
                        arrData[4] = true; // ##.. 4567
                        arrData[5] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[5]  = true; // .... 0123
                        arrData[8]  = true; // .#.. 4567
                        arrData[9]  = true; // ###. 8901
                        arrData[10] = true; // .... 2345
                    }
                    else
                    {
                        arrData[1] = true; // .#.. 0123
                        arrData[5] = true; // .##. 4567
                        arrData[6] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    break;
                case BlockTypeNum.block06:
                    if (structBlock.angle.Equals(RotationEnum.deg0))
                    {
                        arrData[4] = true; // .... 0123
                        arrData[5] = true; // ###. 4567
                        arrData[6] = true; // #... 8901
                        arrData[8] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg90))
                    {
                        arrData[0] = true; // ##.. 0123
                        arrData[1] = true; // .#.. 4567
                        arrData[5] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[6]  = true; // .... 0123
                        arrData[8]  = true; // ..#. 4567
                        arrData[9]  = true; // ###. 8901
                        arrData[10] = true; // .... 2345
                    }
                    else
                    {
                        arrData[1]  = true; // .#.. 0123
                        arrData[5]  = true; // .#.. 4567
                        arrData[9]  = true; // .##. 8901
                        arrData[10] = true; // .... 2345
                    }
                    break;
                case BlockTypeNum.block07:
                    if (structBlock.angle.Equals(RotationEnum.deg0))
                    {
                        arrData[4]  = true; // .... 0123
                        arrData[5]  = true; // ###. 4567
                        arrData[6]  = true; // ..#. 8901
                        arrData[10] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg90))
                    {
                        arrData[1] = true; // .#.. 0123
                        arrData[5] = true; // .#.. 4567
                        arrData[8] = true; // ##.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[4]  = true; // .... 0123
                        arrData[8]  = true; // #... 4567
                        arrData[9]  = true; // ###. 8901
                        arrData[10] = true; // .... 2345
                    }
                    else
                    {
                        arrData[1] = true; // .##. 0123
                        arrData[2] = true; // .#.. 4567
                        arrData[5] = true; // .#.. 8901
                        arrData[9] = true; // .... 2345
                    }
                    break;
                case BlockTypeNum.block08:
                    arrData[0] = true; // #... 0123
                                       // .... 4567
                                       // .... 8901
                                       // .... 2345
                    break;
                case BlockTypeNum.block09:
                    if (structBlock.angle.Equals(RotationEnum.deg0) ||
                        structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[1] = true; // .##. 0123
                        arrData[2] = true; // .#.. 4567
                        arrData[5] = true; // ##.. 8901
                        arrData[8] = true; // .... 2345
                        arrData[9] = true; // .... 2345
                    }
                    else 
                    {
                        arrData[0]  = true;  // #... 0123
                        arrData[4]  = true;  // ###. 4567
                        arrData[5]  = true;  // ..#. 8901
                        arrData[6]  = true;  // .... 2345
                        arrData[10] = true;
                    }
                    break;
                case BlockTypeNum.block10:
                    if (structBlock.angle.Equals(RotationEnum.deg0) ||
                        structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[0]  = true; // ##.. 0123
                        arrData[1]  = true; // .#.. 4567
                        arrData[5]  = true; // .##. 8901
                        arrData[9]  = true; // .... 2345
                        arrData[10] = true;
                    }
                    else
                    {
                        arrData[3] = true; // ...# 0123
                        arrData[5] = true; // .### 4567
                        arrData[6] = true; // .#.. 8901
                        arrData[7] = true; // .... 2345
                        arrData[9] = true;
                    }
                    break;
                case BlockTypeNum.block11:
                    if (structBlock.angle.Equals(RotationEnum.deg0))
                    {
                        arrData[0] = true; // ###. 0123
                        arrData[1] = true; // .#.. 4567
                        arrData[2] = true; // .#.. 8901
                        arrData[5] = true; // .... 2345
                        arrData[9] = true;
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg90))
                    {
                        arrData[2]  = true; // ..#. 0123
                        arrData[4]  = true; // ###. 4567
                        arrData[5]  = true; // ..#. 8901
                        arrData[6]  = true; // .... 2345
                        arrData[10] = true;
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[1]  = true;  // .#.. 0123
                        arrData[5]  = true;  // .#.. 4567
                        arrData[8]  = true;  // ###. 8901
                        arrData[9]  = true;  // .... 2345
                        arrData[10] = true;
                    }
                    else
                    {
                        arrData[1] = true; // .#.. 0123
                        arrData[5] = true; // .### 4567
                        arrData[6] = true; // .#.. 8901
                        arrData[7] = true; // .... 2345
                        arrData[9] = true;
                    }
                    break;
                case BlockTypeNum.block12:
                    if (structBlock.angle.Equals(RotationEnum.deg0))
                    {
                        arrData[2] = true; // ..#. 0123
                        arrData[5] = true; // .##. 4567
                        arrData[6] = true; // .... 8901
                                           // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg90))
                    {
                        arrData[1] = true; // .#.. 0123
                        arrData[4] = true; // .##. 4567
                        arrData[5] = true; // .... 8901
                                           // .... 2345
                    }
                    else if (structBlock.angle.Equals(RotationEnum.deg180))
                    {
                        arrData[1] = true;  // .##. 0123
                        arrData[2] = true;  // ..#. 4567
                        arrData[6] = true;  // .... 8901
                                            // .... 2345
                    }
                    else
                    {
                        arrData[1] = true; // .##. 0123
                        arrData[2] = true; // .#.. 4567
                        arrData[5] = true; // .... 8901
                                           // .... 2345
                    }
                    break;
            }

            return arrData;
        }

        public void Adjustment(ref WindowRect wrBlock)
        {
            Adjustment(ref wrBlock, arrBlock);
        }

        public void Adjustment(ref WindowRect wrBlock, bool[] arrData)
        {
            //  This function returns the exact measurement of the block. 

            wrBlock = new WindowRect();

            int  col;
            int  row;
            bool isAdj;

            //  Check empty colums from the left-side of the block, and if found, 
            // increase the left margin.
            isAdj = true;
            for (col = 0; col < BLOCK_SIZE; col++)
            {
                for (row = 0; row < BLOCK_SIZE; row++)
                    if (arrData[col + row * BLOCK_SIZE])
                    {
                        isAdj = false;
                        break;
                    }

                if (isAdj)
                    // left margin
                    wrBlock.left++;
                else
                    break;
            }
            // end left adjustment

            //  Check empty rows from the top-side of the block, and if found, 
            // increse the top margin. 
            isAdj = true;
            for (row = 0; row < BLOCK_SIZE; row++)
            {
                for (col = 0; col < BLOCK_SIZE; col++)
                    if (arrData[col + row * BLOCK_SIZE])
                    {
                        isAdj = false;
                        break;
                    }

                if (isAdj)
                    wrBlock.top++;
                else
                    break;
            }
            // end top adjustment

            //  Check empty columns from the right-side of the block, and if found, 
            // increase the right margin.
            isAdj = true;
            for (col = BLOCK_SIZE - 1; col >= 0; col--)
            {
                for (row = 0; row < BLOCK_SIZE; row++)
                    if (arrData[col + row * BLOCK_SIZE])
                    {
                        isAdj = false;
                        break;
                    }

                if (isAdj)
                    wrBlock.width++;
                else
                    break;
            }

            // get the exact width of the block
            wrBlock.width = BLOCK_SIZE - (wrBlock.left + wrBlock.width);
            // end right adjustment

            //  Check empty rows from the bottom-side of the block, and if found, 
            // increase the bottom.
            isAdj = true;
            for (row = BLOCK_SIZE - 1; row >= 0; row--)
            {
                for (col = 0; col < BLOCK_SIZE; col++)
                    if (arrData[col + row * BLOCK_SIZE])
                    {
                        isAdj = false;
                        break;
                    }

                if (isAdj)
                    // bottom margin
                    wrBlock.height++;
                else
                    break;
            }

            // get the exact height of the block.
            wrBlock.height = BLOCK_SIZE - (wrBlock.top + wrBlock.height);
            // end top adjustment;
        }

        public Image Draw(Graphics e, Point pt, WindowRect wrBlockAdj, Boolean allBlocks = false)
        {
            if (allBlocks)
                return TetrisClass.DrawField(e, wrBlockAdj);
            else
            {
                if ((wrBlockAdj.width > 0) && (wrBlockAdj.height > 0))
                {
                    System.Drawing.Image tmpBitmap = new Bitmap(wrBlockAdj.width * this.Width,
                                                                wrBlockAdj.height * this.Height);
                    System.Drawing.Graphics tmpGraphics = System.Drawing.Graphics.FromImage(tmpBitmap);

                    for (int row = wrBlockAdj.top; row < wrBlockAdj.top + wrBlockAdj.height; row++)
                        for (int col = wrBlockAdj.left; col < wrBlockAdj.left + wrBlockAdj.width; col++)
                            if (arrBlock[col + row * BLOCK_SIZE])
                                imlBlockImage.Draw(tmpGraphics, new System.Drawing.Point(this.Width * (col - wrBlockAdj.left),
                                                                                         this.Height * (row - wrBlockAdj.top)),
                                                   GetImageByType(Type));
                    e.DrawImage(tmpBitmap, new PointF(this.Width * (pt.x + wrBlockAdj.left - wrBlockAdj.left),
                                                      this.Height * (pt.y + wrBlockAdj.top - wrBlockAdj.top)));
                    tmpGraphics.Dispose();
                    tmpBitmap.Dispose();
                }

                return null;
            }
        }

        public int GetImageByType(BlockTypeNum typBlock)
        {
            switch (typBlock)
            {
                case BlockTypeNum.block01:
                    return 0;
                case BlockTypeNum.block02:
                    return 1;
                case BlockTypeNum.block03:
                    return 2;
                case BlockTypeNum.block04:
                    return 3;
                case BlockTypeNum.block05:
                    return 4;
                case BlockTypeNum.block06:
                    return 5;
                case BlockTypeNum.block07:
                    return 6;
                case BlockTypeNum.block08:
                    return 7;
                case BlockTypeNum.block09:
                    return 8;
                case BlockTypeNum.block10:
                    return 9;
                case BlockTypeNum.block11:
                    return 10;
                default:
                    return 11;
            }
        }

        public int GetImageByColor(System.Drawing.Color c)
        {
            if (c.Equals(System.Drawing.Color.Red))
                return 0; 
            else if (c.Equals(System.Drawing.Color.Blue))
                return 1;
            else if (c.Equals(System.Drawing.Color.Green))
                return 2; 
            else if (c.Equals(System.Drawing.Color.Cyan))
                return 3; 
            else if (c.Equals(System.Drawing.Color.Yellow))
                return 4;
            else if (c.Equals(System.Drawing.Color.Orange))
                return 5;
            else if (c.Equals(System.Drawing.Color.Magenta))
                return 6; 
            else if (c.Equals(System.Drawing.Color.Brown))
                return 7; 
            else if (c.Equals(System.Drawing.Color.DarkBlue))
                return 8;
            else if (c.Equals(System.Drawing.Color.GreenYellow))
                return 9; 
            else if (c.Equals(System.Drawing.Color.Pink))
                return 10;
            else
                return 11; 
        }

        public void Preview(Graphics e, WindowRect wrBound, StructBlock structBlock)
        {
            // shows a preview of a block
            WindowRect wrBlockAdj = new WindowRect();
            bool[] arrData        = GetBlockData(structBlock);
            Point pt              = new Point();
            //  retrieve the exact measurement of the block
            // so we can able to draw the block in a correct position.
            Adjustment(ref wrBlockAdj, arrData);

            pt.x = (wrBound.width - wrBlockAdj.width* this.Width) / 2 ;
            pt.y = (wrBound.height - wrBlockAdj.height* this.Height) / 2 ;

            for (int row = wrBlockAdj.top; row < wrBlockAdj.top + wrBlockAdj.height; row++)
                for (int col = wrBlockAdj.left; col < wrBlockAdj.left + wrBlockAdj.width; col++)
                    if (arrData[col + row * BLOCK_SIZE])
                        imlBlockImage.Draw(e, new System.Drawing.Point((pt.x + this.Width * (col - wrBlockAdj.left)),
                                                                       (pt.y + this.Height * (row - wrBlockAdj.top))),
                                           GetImageByType(structBlock.type));
        }

        public RotationEnum getNextAngle(int rotateOption)
        {
            if (rotateOption.Equals(0))
                // clockwise
                switch (Angle)
                {
                    case RotationEnum.deg0:
                        return RotationEnum.deg90;
                    case RotationEnum.deg90:
                        return RotationEnum.deg180;
                    case RotationEnum.deg180:
                        return RotationEnum.deg270;
                    default:
                        return RotationEnum.deg0;
                }
            else
                // counter-clockwise
                switch (Angle)
                {
                    case RotationEnum.deg0:
                        return RotationEnum.deg270;
                    case RotationEnum.deg270:
                        return RotationEnum.deg180;
                    case RotationEnum.deg180:
                        return RotationEnum.deg90;
                    default:
                        return RotationEnum.deg0;
                }
        }

        public void Assign(StructBlock sbNew)
        {
            Angle = sbNew.angle;
            Type  = sbNew.type;
        }
    }

    class TetrisClass : BaseClass
    {
        static int FieldWidth;
        static int FieldHeight;
        static DifficultyEnum m_Difficulty;

        public event TetrisHandler ProcessEvent;

        public BlockClass Block = new BlockClass();
        
        public DifficultyEnum Difficulty
        {
            get 
            {
                return m_Difficulty;
            }
            set
            {
                m_Difficulty = value;
            }
        }

        public TetrisClass(WindowRect wrField)
        {
            TetrisField = new WindowRect(0, 0, wrField.width / Block.Width, 
                                               wrField.height / Block.Height);
            FieldWidth  = wrField.width;
            FieldHeight = wrField.height;

            BuildField();
        }

        public void Reset()
        {
            for (var i = 0; i < arrField.Length; i++)
                arrField[i] = new StructBlockStyle(Color.Transparent, false);
        }

        public void BuildField()
        {
            arrField = new StructBlockStyle[TetrisField.width *
                                            TetrisField.height];
        }

        public static Image DrawField(Graphics e, WindowRect wrBlockAdj)
        {
            System.Drawing.Image    tmpBitmap   = new Bitmap(FieldWidth, FieldHeight);
            System.Drawing.Graphics tmpGraphics = System.Drawing.Graphics.FromImage(tmpBitmap);

            int w = TetrisField.width;
            int h = TetrisField.height;

            BlockClass Block = new BlockClass();

            for (int row = 0; row < h; row++)
                for (int col = 0; col < w; col++)
                    if (((StructBlockStyle)arrField[col + row * w]).isBlock)
                        Block.imlBlockImage.Draw(tmpGraphics, new System.Drawing.Point(col * Block.Width, row * Block.Height), 
                                                 Block.GetImageByColor(((StructBlockStyle)arrField[col + row * w]).color));
            tmpGraphics.Dispose();

            return tmpBitmap;
        }

        public bool IsCollided(Point pt, WindowRect wrBlockAdj)
        {
            int sw = TetrisField.width;

            int blockIndex;
            int fieldIndex;

            for (int row = 0; row < wrBlockAdj.height; row++)
                for (int col = 0; col < wrBlockAdj.width; col++)
                {
                    blockIndex = (wrBlockAdj.left + col) + ((wrBlockAdj.top + row) * BLOCK_SIZE);
                    fieldIndex = ((pt.x + pt.y * sw) + col) + row * sw;

                    if ((fieldIndex>=0) && (fieldIndex < arrField.Length))
                    {
                        if (arrBlock[blockIndex] && ((StructBlockStyle)arrField[fieldIndex]).isBlock)
                            return true;
                    }
                    else
                        return true;
                }
            
            return false;
        }

        public void SendToField(Point pt, WindowRect wrBlockAdj)
        {
            // This function sends the block data to the field.
            int blockIndex;
            int fieldIndex;

            for (int row = 0; row < wrBlockAdj.height; row++)
                for (int col = 0; col < wrBlockAdj.width; col++)
                {
                    blockIndex = (wrBlockAdj.left + col) +
                                 (wrBlockAdj.top + row) *
                                    Block.Size;
                    fieldIndex = (pt.x - TetrisField.left + col) +
                                 (pt.y - TetrisField.top + row) *
                                    TetrisField.width;

                    if (arrBlock[blockIndex])
                        arrField[fieldIndex] = new StructBlockStyle(Block.Color(Block.Type), true);
                }

            ProcessRows();
        }

        public void ProcessRows()
        {
            // This function will check to see if rows were completed.
            int w = TetrisField.width;
            int h = TetrisField.height;
            int rowCounter  = h - 1;
            int rowTotal    = 0;
            bool isFullLine = true;

            // Store rows that are not completed.
            StructBlockStyle[] arrData = new StructBlockStyle[TetrisField.width *
                                                              TetrisField.height];
            
            for (int row = h - 1; row >= 0; row--)
            {
                for (int col = w - 1; (col >= 0) && isFullLine; col--)
                    if (!((StructBlockStyle)arrField[col + row * w]).isBlock)
                        isFullLine = false;

                if (!isFullLine)
                {
                    // copy the row
                    for (int col = w - 1; col >= 0; col--)
                        arrData[col + rowCounter * w] = arrField[col + row * w];

                    rowCounter--;
                    isFullLine = true;
                }
                else
                    // exclude rows that are completed.
                    rowTotal++;
            }

            // get all the rows that are not completed.
            arrField = arrData;

            EventArgs e = new EventArgs(rowTotal);
            RaiseEvent((object)this, e);
        }

        private void RaiseEvent(object o, EventArgs e)
        {
            if (ProcessEvent != null)
                ProcessEvent(o, e);
        }
    }
}

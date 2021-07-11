using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Game
{
    public partial class frmMain : Form
    {
        TetrisClass Tetris;

        Point       ptBlock    = new Point();      // The x and y position of the block.
        WindowRect  wrBlockAdj = new WindowRect(); // Block adjustment.
        Sound       sndEffect  = new Sound();      // Sound effect.

        StructBlock currBlock = new StructBlock();
        StructBlock nextBlock = new StructBlock();

        int  Speed  = 1;     // Speed of the game.
        int  Level  = 1;     // Level of the game.
        int  Score  = 0;     // Total score.
        int  Lines  = 0;     // Total lines completed.

        Boolean isProcessRow  = false;
        Boolean isGameStart   = false;
        Boolean isGameOver    = false;
        Boolean isGameSuccess = false;
        Boolean isCanRotate   = false;  

        Bitmap saveImage; //    instead of redrawing each block in the field everytime the block collided or rows were completed, 
                          // simply save the image, then redraw only the image, and the block everytime it moves or rotated, 
                          // this will greatly increase in performance. 

        public frmMain()
        {
            InitializeComponent();
        }

        private void picField_Paint(object sender, PaintEventArgs e)
        {
            if (isGameStart)
            {
                if (isProcessRow)
                {
                    saveImage = new Bitmap(Tetris.Block.Draw(e.Graphics, ptBlock, wrBlockAdj, true),
                                new Size(picField.Width, picField.Height));
                    e.Graphics.DrawImage(saveImage, new PointF(0, 0));
                   
                    if(mnuGameSettingsSound.Checked)
                        sndEffect.Play(global::Game.SoundResource.S103);

                    ptBlock.y    = -1;
                    isProcessRow = false;
                    return;
                }

                e.Graphics.DrawImage(saveImage, 0, 0, saveImage.Width, saveImage.Height);
                Tetris.Block.Draw(e.Graphics, ptBlock, wrBlockAdj, false);        
            }

            if (isGameSuccess || isGameOver)
            {
                e.Graphics.DrawImage(saveImage, 0, 0, saveImage.Width, saveImage.Height);
                Tetris.Block.Draw(e.Graphics, ptBlock, wrBlockAdj, false);

                if (isGameSuccess)
                {
                    Size tsz = e.Graphics.MeasureString("Congratulations!!!", new Font("Arial Black", 16)).ToSize();

                    e.Graphics.DrawString("Congratulations!!!", new Font("Arial Black", 16),
                                                                new SolidBrush(Color.White),
                                                                new PointF((picField.Width - tsz.Width) / 2, (picField.Height - tsz.Height) / 4));
                }
                else
                {
                    Size tsz = e.Graphics.MeasureString("Game Over!!!", new Font("Arial Black", 20)).ToSize();

                    e.Graphics.DrawString("Game Over!!!", new Font("Arial Black", 20),
                                                          new SolidBrush(Color.White),
                                                          new PointF((picField.Width - tsz.Width) / 2, (picField.Height - tsz.Height) / 4));
                }

                GameStart(false);
            }
        }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {  
            if(isGameStart)
                // show the next block.
                Tetris.Block.Preview(e.Graphics, new WindowRect(0, 0, picPreview.Width,
                                                                      picPreview.Height), nextBlock);
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            // initalize the width and height of the stage.
            Tetris = new TetrisClass(new WindowRect(0, 0, picField.Width, picField.Height));
            //// Hook up the ProcessEvent.
            Tetris.ProcessEvent += new Game.TetrisHandler(Tetris_Process);
            // initialize the size of the block.
            Tetris.Block.Width  = 24;
            Tetris.Block.Height = 24;

            saveImage = new Bitmap(picField.Width, picField.Height);
        }

        private void PlayBlock(Game.StructBlock sbBlock, Boolean isNew)
        {
            if (isNew)
                // create a new block
                sbBlock = Tetris.Block.Generate(Tetris.Difficulty);
            else
                Tetris.SendToField(ptBlock, wrBlockAdj);

            Tetris.Block.Assign(sbBlock);
            Tetris.Block.Build();
            Tetris.Block.Adjustment(ref wrBlockAdj);
            // draw the block in center-x, top-y
            ptBlock.x =  (picField.Width/Tetris.Block.Width - wrBlockAdj.width) / 2;
            ptBlock.y = 0;
            
            ShowNextBlock(isNew);
            picField.Invalidate();

            if (Tetris.IsCollided(ptBlock, wrBlockAdj))
            {
                isGameOver = true;
                if (mnuGameSettingsSound.Checked)
                    sndEffect.Play(global::Game.SoundResource.S102);
            }
            else
                if (mnuGameSettingsSound.Checked)
                    sndEffect.Play(global::Game.SoundResource.S100);
        }

        private void ShowNextBlock(Boolean isNew)
        {
            if(isNew)
                //    I really don't why I need to add this code everytime the game starts. This will produce 
                // two different blocks for current and next block. Without this, 
                // the current and next block will always be the same everytime the game start...
                System.Threading.Thread.Sleep(200);
            
            nextBlock = Tetris.Block.Generate(Tetris.Difficulty); 
            picPreview.Invalidate();
        }

        private void tmrGame_Tick(object sender, System.EventArgs e)
        { 
            if (picField.Height > (Tetris.Block.Width * (ptBlock.y + wrBlockAdj.height)))
                if (Tetris.IsCollided(new Point(ptBlock.x, ptBlock.y + 1), wrBlockAdj))
                {
                    copyImage();
                    // The block has collided, set the next block.
                    PlayBlock(nextBlock, false);
                }
                else
                {   
                    picField.Invalidate();
                    ptBlock.y++;
                }
            else
            {
                copyImage();
                // where at the bottom, set the next block.
                PlayBlock(nextBlock, false);
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameStart) return;

            // check if key input is valid (up, down, left, right, space, escape)
            if (!(e.KeyCode.Equals(Keys.Up) || e.KeyCode.Equals(Keys.Down) ||
                  e.KeyCode.Equals(Keys.Left) || e.KeyCode.Equals(Keys.Right) ||
                  e.KeyCode.Equals(Keys.Space) || e.KeyCode.Equals(Keys.Escape)))
                    return;

            Point newPos        = ptBlock; // get block current position
            Boolean isValidMove = false;

            switch (e.KeyCode)
            {
                case Keys.Right: // Right
                    // could go right?
                    if((newPos.x+wrBlockAdj.width)*Tetris.Block.Width<picField.Width)
                        newPos.x++;

                    if (newPos.x.Equals(ptBlock.x))
                        return;
                    break;
                case Keys.Left: // Left
                    // could go left?
                    if (newPos.x > 0)
                        newPos.x--;

                    if (newPos.x.Equals(ptBlock.x))
                        return;
                    break;
                case Keys.Down: // Down
                    // could go down?
                    if((newPos.y+wrBlockAdj.height)*Tetris.Block.Height<picField.Height)
                        newPos.y++;

                    if (newPos.y.Equals(ptBlock.y))
                        return;
                    break;
                case Keys.Up:    // Up       (rotate)
                case Keys.Space: // Spacebar (rotate)
                    WindowRect newBlockAdj = new WindowRect();

                    // Save old angle.
                    Game.RotationEnum saveAngle = Tetris.Block.Angle;
                    
                    // try clockwise
                    newBlockAdj = Tetris.Block.Rotate(Tetris.Block.getNextAngle(0));

                     if ((newPos.x + newBlockAdj.width) * Tetris.Block.Width > picField.Width)
                        newPos.x = picField.Width/Tetris.Block.Width - newBlockAdj.width;
                     if ((newPos.y + newBlockAdj.height) * Tetris.Block.Height > picField.Height)
                         return;

                    if (Tetris.IsCollided(new Point(newPos.x, newPos.y), newBlockAdj))
                    {
                        // try counter-clockwise
                        newBlockAdj = Tetris.Block.Rotate(Tetris.Block.getNextAngle(1));

                        if ((newPos.x + newBlockAdj.width) * Tetris.Block.Width > picField.Width)
                            newPos.x = picField.Width / Tetris.Block.Width - newBlockAdj.width;
                        if ((newPos.y + newBlockAdj.height) * Tetris.Block.Height > picField.Height)
                            return;

                        if (Tetris.IsCollided(new Point(newPos.x, newPos.y), newBlockAdj))
                            isCanRotate = false;
                        else
                            isCanRotate = true;
                    }
                    else
                        isCanRotate = true;

                    if (isCanRotate)
                    {  
                        if (wrBlockAdj.left.Equals(newBlockAdj.left) &&
                            wrBlockAdj.top.Equals(newBlockAdj.top) &&
                            wrBlockAdj.width.Equals(newBlockAdj.width) &&
                            wrBlockAdj.height.Equals(newBlockAdj.height))
                                // nothing has changed, just leave it.
                                return;

                        // can rotate, apply the new settings.
                        wrBlockAdj = newBlockAdj;
                        ptBlock     = newPos;
                        isValidMove = true;
                    }
                    else
                    {
                        // can't rotate, restore old angle.
                        Tetris.Block.Rotate(saveAngle);
                        return;
                    }
                    break;
            }
            
            if (!(e.KeyCode.Equals(Keys.Space) || e.KeyCode.Equals(Keys.Up)))
                if (!Tetris.IsCollided(new Point(newPos.x, newPos.y), wrBlockAdj))
                {
                    ptBlock     = newPos;
                    isValidMove = true;
                }

            if (isValidMove)
                picField.Invalidate();
        }

        private void Tetris_Process(object o, Game.EventArgs e)
        {
            if (e.RowsCompleted > 0)
            {
                isProcessRow = true;
                Score += e.RowsCompleted * (e.RowsCompleted > 1 ? 15 : 10);
                Lines += e.RowsCompleted;

                // Increase the speed according to the number of lines completed.
                if ((Lines >= 11) && (Lines <= 20))
                    Speed = 2;
                else if ((Lines >= 21) && (Lines <= 30))
                    Speed = 3;
                else if ((Lines >= 31) && (Lines <= 40))
                    Speed = 4;
                else if ((Lines >= 41) && (Lines <= 50))
                    Speed = 5;
                else if ((Lines >= 51) && (Lines <= 60))
                    Speed = 6;
                else if ((Lines >= 61) && (Lines <= 70))
                    Speed = 7;
                else if ((Lines >= 71) && (Lines <= 80))
                    Speed = 8;
                else if ((Lines >= 81) && (Lines <= 90))
                    Speed = 9;

                // level is equal to speed.
                Level = Speed; 
                // calculate new speed
                tmrGame.Interval = 1000 - (Speed * 100);

                picField.Invalidate();
                ShowStatus();
            }
        }

        private void ShowStatus()
        {
            lblScore.Text = String.Format("Score : {0:D6}", Score);
            lblLines.Text = String.Format("Lines : {0:D6}", Lines);
            lblSpeed.Text = String.Format("Speed : {0:D2}", Speed);
            lblLevel.Text = String.Format("Level : {0:D2}", Level);

            if (Lines >= 100)
            {
                isGameSuccess = true;
                if (mnuGameSettingsSound.Checked)
                    sndEffect.Play(global::Game.SoundResource.S104);
            }
        }
        
        private void copyImage()
        {
            picField.DrawToBitmap(saveImage, new Rectangle(0, 0, picField.Width, picField.Height));
        }

        private void mnuGameSettingsEasy_Click(object sender, System.EventArgs e)
        {
            mnuGameSettingsEasy.Checked = true;
            mnuGameSettingsHard.Checked = false;
        }

        private void mnuGameSettingsHard_Click(object sender, System.EventArgs e)
        {
            mnuGameSettingsEasy.Checked = false;
            mnuGameSettingsHard.Checked = true;
        }

        private void mnuGameSettingsSound_Click(object sender, System.EventArgs e)
        {
            mnuGameSettingsSound.Checked = !mnuGameSettingsSound.Checked;
        }

        private void mnuGamePlay_Click(object sender, System.EventArgs e)
        {
            if (mnuGamePlay.Text.ToUpper().Equals("&START"))
                GameStart(true);
            else
                GameStart(false);
        }

        public void GameStart(Boolean isStart)
        {
            if (isStart)
            {
                if (mnuGameSettingsEasy.Checked)
                    Tetris.Difficulty = DifficultyEnum.Easy;
                else
                    Tetris.Difficulty = DifficultyEnum.Hard;

                // reset tetris
                Tetris.Reset();
                // show background image
                picField.CreateGraphics().DrawImage(global::Game.Properties.Resources.background, new PointF(0, 0));
                // copy the background image and place it to saveImage.
                copyImage();
                // reset menus
                mnuGameSettingsEasy.Enabled = false;
                mnuGameSettingsHard.Enabled = false;
                // reset status
                Speed = 1;    
                Level = 1;    
                Score = 0;     
                Lines = 0;
                // show status
                ShowStatus();
                // reset variables
                isGameSuccess = false;
                isGameOver    = false;
                isGameStart   = true;
                // set current block
                PlayBlock(currBlock, true);
                // initialize the timer.
                tmrGame.Interval = 1000;
                // start the game.
                tmrGame.Enabled  = true;

                mnuGamePlay.Text = "&Stop";
            }
            else
            {
                // reset menus
                mnuGameSettingsEasy.Enabled = true;
                mnuGameSettingsHard.Enabled = true;
                // reset variables
                isGameSuccess = false;
                isGameOver    = false;
                isGameStart   = false;
                // stop the game.
                isGameStart     = false;
                tmrGame.Enabled = false;

                mnuGamePlay.Text = "&Start";
            }
        }

        private void mnuAbout_Click(object sender, System.EventArgs e)
        {
            new frmAbout().ShowDialog(this);
        }

        private void mnuGameExit_Click(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

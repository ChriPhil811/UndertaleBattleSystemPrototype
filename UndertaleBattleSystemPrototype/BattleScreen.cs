using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using UndertaleBattleSystemPrototype.Classes;

namespace UndertaleBattleSystemPrototype
{
    public partial class BattleScreen : UserControl
    {
        #region variables and lists

        //player key press variables
        Boolean wDown, aDown, sDown, dDown, spaceDown, shiftDown;

        //boolean for checking if player is in the fighting area or not
        Boolean isFighting = false;

        //booleans for checking what menu the player is in
        Boolean fightMenuSelected = false, actMenuSelected = false, itemMenuSelected = false, mercyMenuSelected = false;

        //brush for walls, hp bar, and projectiles
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        //create the player
        Player player = new Player();

        //create a new image for the player sprite and button sprites
        Image playerSprite, fightSprite, actSprite, itemSprite, mercySprite;

        //create rectangles for buttons
        Rectangle fightRec, actRec, itemRec, mercyRec;

        //health bar rectangles
        Rectangle maxHPRec = new Rectangle(410, 550, 80, 20);
        Rectangle remainingHPRec = new Rectangle(410, 550, 80, 20);

        //new list for battle area walls
        List<Rectangle> arenaWalls = new List<Rectangle>();

        //lists for act string variables
        List<string> actNames = new List<string>() {" ", " ", " ", " "};
        List<string> actText = new List<string>() {" ", " ", " ", " "};

        #endregion variables and lists

        #region battle system brought up
        public BattleScreen()
        {
            InitializeComponent();

            //screen setup
            OnStart();

            //hide the cursor
            Cursor.Hide();

            //set images
            playerSprite = Properties.Resources.heart;
            fightSprite = Properties.Resources.fightButton;
            actSprite = Properties.Resources.actButton;
            itemSprite = Properties.Resources.itemButton;
            mercySprite = Properties.Resources.mercyButton;
        }
        #endregion battle system brought up

        #region setup
        public void OnStart()
        {
            //set button positions and sizes
            fightRec = new Rectangle(236 - 190, this.Height - 100, 140, 50);
            actRec = new Rectangle(472 - 190, this.Height - 100, 140, 50);
            itemRec = new Rectangle(708 - 190, this.Height - 100, 140, 50);
            mercyRec = new Rectangle(944 - 190, this.Height - 100, 140, 50);

            //create rectangles for the arena walls
            Rectangle leftWall = new Rectangle(fightRec.X, fightRec.Y - 250, 5, 200);
            Rectangle rightWall = new Rectangle(mercyRec.X + 135, mercyRec.Y - 250, 5, 200);
            Rectangle topWall = new Rectangle(fightRec.X, fightRec.Y - 250, 848, 5);
            Rectangle bottomWall = new Rectangle(fightRec.X, fightRec.Y - 50, 848, 5);

            //add the walls to the arena walls list
            arenaWalls.Add(leftWall);
            arenaWalls.Add(rightWall);
            arenaWalls.Add(topWall);
            arenaWalls.Add(bottomWall);

            //set the text of the act labels
            actLabel1.Text = "  " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];

            //set player initial position (on fight button)
            player = new Player(fightRec.X + 15, fightRec.Y + 15, 20);
        }
        #endregion setup

        #region key down and up
        private void BattleScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.ShiftKey:
                    shiftDown = true;
                    break;
            }
        }

        private void BattleScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.ShiftKey:
                    shiftDown = false;
                    break;
            }
        }

        #endregion key down and up

        #region movement, collisions, and menus (gameloop)
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //update the position of the player collision
            Rectangle playerRec = new Rectangle(player.x, player.y, player.size, player.size);

            #region fighting area code

            //if player is in the fighting area...
            if (isFighting == true)
            {
                //player movement
                if (dDown == true && player.x < this.Width - player.size)
                {
                    player.MoveLeftRight(5);
                }
                if (aDown == true && player.x > 0)
                {
                    player.MoveLeftRight(-5);
                }
                if (wDown == true && player.y > 0)
                {
                    player.MoveUpDown(-5);
                }
                if (sDown == true && player.y < this.Height - player.size)
                {
                    player.MoveUpDown(5);
                }
            }

            #endregion fighting area code

            #region buttons code

            //if player is in the buttons and menus...
            else
            {
                if (actMenuSelected == true) { ActMenu(); }
                if (mercyMenuSelected == true) { MercyMenu(); }

                //check which button the player is currently on and set it to the blank version of the button's sprite
                //if player moves to a different button, change button sprite back and set player position to the new button
                #region fight
                if (playerRec.IntersectsWith(fightRec))
                {
                    fightSprite = Properties.Resources.fightButtonBlank;

                    if(dDown == true)
                    {
                        fightSprite = Properties.Resources.fightButton;
                        player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                }
                #endregion fight

                #region act
                if (playerRec.IntersectsWith(actRec))
                {
                    actSprite = Properties.Resources.actButtonBlank;
                    actMenuSelected = false;

                    //go into the act menu
                    if (spaceDown == true)
                    {
                        //make the text output not visible
                        textOutput.Visible = false;

                        ///make the act labels visible
                        actLabel1.Visible = true;
                        actLabel2.Visible = true;
                        actLabel3.Visible = true;
                        actLabel4.Visible = true;

                        //set player position to the act1 label
                        player = new Player(actLabel1.Location.X, actLabel1.Location.Y + 5, 20);

                        //setup the act menu for the current enemy
                        ActMenuText();

                        //set boolean for act menu check to true
                        actMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        actSprite = Properties.Resources.actButton;
                        player = new Player(fightRec.X + 15, fightRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        actSprite = Properties.Resources.actButton;
                        player = new Player(itemRec.X + 15, itemRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                }

                #endregion act

                #region item
                if (playerRec.IntersectsWith(itemRec))
                {
                    itemSprite = Properties.Resources.itemButtonBlank;

                    if (aDown == true)
                    {
                        itemSprite = Properties.Resources.itemButton;
                        player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        itemSprite = Properties.Resources.itemButton;
                        player = new Player(mercyRec.X + 15, mercyRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                }
                #endregion item

                #region mercy
                if (playerRec.IntersectsWith(mercyRec))
                {
                    mercySprite = Properties.Resources.mercyButtonBlank;
                    mercyMenuSelected = false;

                    //go into the mercy menu
                    if (spaceDown == true)
                    {
                        //make the text output not visible
                        textOutput.Visible = false;

                        ///make the act labels visible
                        actLabel1.Visible = true;
                        actLabel2.Visible = true;

                        //set player position to the act1 label
                        player = new Player(actLabel1.Location.X, actLabel1.Location.Y + 5, 20);

                        //setup the act menu for the current enemy
                        MercyMenuText();

                        //set boolean for mercy menu check to true
                        mercyMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        mercySprite = Properties.Resources.mercyButton;
                        player = new Player(itemRec.X + 15, itemRec.Y + 15, 20);

                        Thread.Sleep(150);
                    }
                }
                #endregion mercy
            }

            #endregion buttons code

            Refresh();
        }
        #endregion movement, collisions, and menus (gameloop)

        #region paint graphics
        private void BattleScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw the text box/arena walls
            foreach(Rectangle r in arenaWalls)
            {
                e.Graphics.FillRectangle(whiteBrush, r);
            }

            //draw the buttons
            e.Graphics.DrawImage(fightSprite, fightRec);
            e.Graphics.DrawImage(actSprite, actRec);
            e.Graphics.DrawImage(itemSprite, itemRec);
            e.Graphics.DrawImage(mercySprite, mercyRec);

            //draw the health bar
            e.Graphics.FillRectangle(redBrush, maxHPRec);
            e.Graphics.FillRectangle(yellowBrush, remainingHPRec);

            //draw the player
            e.Graphics.DrawImage(playerSprite, player.x, player.y);
        }
        #endregion paint graphics

        #region menu methods

        #region act menu
        private void ActMenu()
        {
            //if for player exiting the act menu
            if (shiftDown == true)
            {
                //show the main text output
                textOutput.Visible = true;

                //hide the act labels
                actLabel1.Visible = false;
                actLabel2.Visible = false;
                actLabel3.Visible = false;
                actLabel4.Visible = false;

                //set player back to the act button
                player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                Thread.Sleep(150);
            }

            //check which act option the player is on and do things accordingly
            #region option selection
            if (player.x == actLabel1.Location.X && player.y == actLabel1.Location.Y + 5)
            {
                actLabel1.Text = "  " + actNames[0];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[0];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;
                    actLabel3.Visible = false;
                    actLabel4.Visible = false;

                    player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 3
                if (sDown == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player = new Player(actLabel3.Location.X, actLabel3.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
                //move to option 2
                if (dDown == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player = new Player(actLabel2.Location.X, actLabel2.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel2.Location.X && player.y == actLabel2.Location.Y + 5)
            {
                actLabel2.Text = "  " + actNames[1];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[1];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;
                    actLabel3.Visible = false;
                    actLabel4.Visible = false;

                    player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 1
                if (aDown == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player = new Player(actLabel1.Location.X, actLabel1.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
                //move to option 4
                if (sDown == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player = new Player(actLabel4.Location.X, actLabel4.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel3.Location.X && player.y == actLabel3.Location.Y + 5)
            {
                actLabel3.Text = "  " + actNames[2];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[2];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;
                    actLabel3.Visible = false;
                    actLabel4.Visible = false;

                    player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 1
                if (wDown == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player = new Player(actLabel1.Location.X, actLabel1.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
                //move to option 4
                if (dDown == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player = new Player(actLabel4.Location.X, actLabel4.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel4.Location.X && player.y == actLabel4.Location.Y + 5)
            {
                actLabel4.Text = "  " + actNames[3];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[3];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;
                    actLabel3.Visible = false;
                    actLabel4.Visible = false;

                    player = new Player(actRec.X + 15, actRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 2
                if (wDown == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player = new Player(actLabel2.Location.X, actLabel2.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
                //move to option 3
                if (aDown == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player = new Player(actLabel3.Location.X, actLabel3.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            #endregion option selection
        }
        #endregion act menu
        #region act menu text
        private void ActMenuText()
        {
            //create a counter
            int i = 0;

            //create an xml reader
            XmlReader reader = XmlReader.Create("file:///C:/Users/chriphil811/Source/Repos/ChriPhil811/UndertaleBattleSystemPrototype/UndertaleBattleSystemPrototype/Resources/TestEnemy.xml");

            while (reader.Read() && i < 4)
            {
                //read to the next action in the enemy xml file
                reader.ReadToFollowing("Act");

                //fill out the proper details for each act option
                actNames[i] = Convert.ToString(reader.GetAttribute("actName"));
                actText[i] = "* " + Convert.ToString(reader.GetAttribute("actLine1")) + "\n\n* " + Convert.ToString(reader.GetAttribute("actLine2")) + "\n\n* " + Convert.ToString(reader.GetAttribute("actLine3"));

                //add 1 to the counter
                i++;
            }

            //these lines of code are nessecary for the initial display of each label
            actLabel1.Text = "* " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];
        }
        #endregion act menu text

        #region mercy menu
        private void MercyMenu()
        {
            //if for player exiting the act menu
            if (shiftDown == true)
            {
                //show the main text output
                textOutput.Visible = true;

                //hide the act labels
                actLabel1.Visible = false;
                actLabel2.Visible = false;

                //set player back to the act button
                player = new Player(mercyRec.X + 15, mercyRec.Y + 15, 20);

                Thread.Sleep(150);
            }

            //check which act option the player is on and do things accordingly
            #region option selection
            if (player.x == actLabel1.Location.X && player.y == actLabel1.Location.Y + 5)
            {
                actLabel1.Text = "  " + actNames[0];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[0];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;

                    player = new Player(mercyRec.X + 15, mercyRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 2
                if (dDown == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player = new Player(actLabel2.Location.X, actLabel2.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel2.Location.X && player.y == actLabel2.Location.Y + 5)
            {
                actLabel2.Text = "  " + actNames[1];

                //show the output text and hide the act options
                //set output text to the appropriate text and go back to the act button
                if (spaceDown == true)
                {
                    textOutput.Text = actText[1];
                    textOutput.Visible = true;

                    actLabel1.Visible = false;
                    actLabel2.Visible = false;

                    player = new Player(mercyRec.X + 15, mercyRec.Y + 15, 20);

                    Thread.Sleep(150);
                }
                //move to option 1
                if (aDown == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player = new Player(actLabel1.Location.X, actLabel1.Location.Y + 5, 20);

                    Thread.Sleep(150);
                }
            }
            #endregion option selection
        }
        #endregion mercy menu
        #region act menu text
        private void MercyMenuText()
        {
            //set the action names and display them
            actNames[0] = "Spare";
            actNames[1] = "Flee";
            actLabel1.Text = "* Spare";
            actLabel2.Text = "* Flee";

            //set the action results
            actText[0] = "* ...";
            actText[1] = "* You ran away...";
        }
        #endregion act menu text

        #endregion menu methods
    }
}

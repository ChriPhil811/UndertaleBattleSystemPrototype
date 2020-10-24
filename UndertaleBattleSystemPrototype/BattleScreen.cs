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
using UndertaleBattleSystemPrototype.Properties;
using System.IO;

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

        //create an xml reader for the enemy file and player file
        XmlReader eReader, pReader;

        //brush for walls, hp bar, and projectiles
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        //create the player
        Player player = new Player();

        //create a new image for the player sprite and button sprites
        Image playerSprite, fightSprite, actSprite, itemSprite, mercySprite, fightUISprite;

        //create rectangles for buttons
        Rectangle fightRec, actRec, itemRec, mercyRec;

        //rectangle for attacking UI
        Rectangle attackRec;

        //health bar rectangles
        Rectangle maxHPRec = new Rectangle(410, 550, 80, 20);
        Rectangle remainingHPRec = new Rectangle(410, 550, 80, 20);

        //new list for battle area walls
        List<Rectangle> arenaWalls = new List<Rectangle>();

        //lists for act string variables
        List<string> actNames = new List<string>() {" ", " ", " ", " "};
        List<string> actText = new List<string>() {" ", " ", " ", " "};
        List<int> itemHeals = new List<int>() {0, 0, 0, 0};

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
            playerSprite = Resources.heart;
            fightSprite = Resources.fightButton;
            actSprite = Resources.actButton;
            itemSprite = Resources.itemButton;
            mercySprite = Resources.mercyButton;
            fightUISprite = Resources.fightUISprite;
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

            //fill in player details for battle use
            pReader = XmlReader.Create("Resources/Player.xml");

            pReader.ReadToFollowing("General");
            player.name = pReader.GetAttribute("name");
            pReader.ReadToFollowing("Battle");
            player.hp = Convert.ToInt16(pReader.GetAttribute("currentHP"));
            player.atk = Convert.ToInt16(pReader.GetAttribute("atk"));
            player.def = Convert.ToInt16(pReader.GetAttribute("def"));

            pReader.Close();

            //fill in player start position
            player.x = fightRec.X + 15;
            player.y = fightRec.Y + 15;
            player.size = 20;


            //set the name label to the correct name
            nameLabel.Text = player.name;
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
                #region player movement
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
                #endregion player movement
            }

            #endregion fighting area code

            #region buttons code

            //if player is in the buttons and menus...
            else
            {
                if (fightMenuSelected == true) { FightUI(); }
                if (actMenuSelected == true) { ActMenu(); }
                if (itemMenuSelected == true) { ItemMenu(); }
                if (mercyMenuSelected == true) { MercyMenu(); }

                //check which button the player is currently on and set it to the blank version of the button's sprite
                //if player moves to a different button, change button sprite back and set player position to the new button
                #region fight
                if (playerRec.IntersectsWith(fightRec))
                {
                    fightSprite = Resources.fightButtonBlank;

                    //go into the fight menu
                    if (spaceDown == true)
                    {
                        //hide the text output box
                        textOutput.Visible = false;

                        //set boolean for fight menu check to true
                        fightMenuSelected = true;

                        //set the attackRec position for fighting
                        attackRec = new Rectangle(50, 338, 15, 190);

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        fightSprite = Resources.fightButton;
                        player.x = actRec.X + 15;
                        player.y = actRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion fight

                #region act
                if (playerRec.IntersectsWith(actRec))
                {
                    actSprite = Resources.actButtonBlank;
                    actMenuSelected = false;

                    //go into the act menu
                    if (spaceDown == true)
                    {
                        //set actions to be visible and put player in the action menu
                        MenuDisplay();

                        //setup the act menu for the current enemy
                        ActMenuText();

                        //set boolean for act menu check to true
                        actMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        actSprite = Resources.actButton;
                        player.x = fightRec.X + 15;
                        player.y = fightRec.Y + 15;

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        actSprite = Resources.actButton;
                        player.x = itemRec.X + 15;
                        player.y = itemRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }

                #endregion act

                #region item
                if (playerRec.IntersectsWith(itemRec))
                {
                    itemSprite = Resources.itemButtonBlank;

                    //go into the item menu
                    if (spaceDown == true)
                    {
                        //set actions to be visible and put player in the action menu
                        MenuDisplay();

                        //setup the item menu for the current enemy
                        ItemMenuText();

                        //set boolean for item menu check to true
                        itemMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        itemSprite = Resources.itemButton;
                        player.x = actRec.X + 15;
                        player.y = actRec.Y + 15;

                        Thread.Sleep(150);
                    }
                    if (dDown == true)
                    {
                        itemSprite = Resources.itemButton;
                        player.x = mercyRec.X + 15;
                        player.y = mercyRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion item

                #region mercy
                if (playerRec.IntersectsWith(mercyRec))
                {
                    mercySprite = Resources.mercyButtonBlank;
                    mercyMenuSelected = false;

                    //go into the mercy menu
                    if (spaceDown == true)
                    {
                        //set actions to be visible and put player in the action menu
                        MenuDisplay();

                        //The mercy menu should only have act labels 1 and 2 displaying, so these ensure that happens
                        actLabel3.Visible = false;
                        actLabel4.Visible = false;

                        //setup the act menu for the current enemy
                        MercyMenuText();

                        //set boolean for mercy menu check to true
                        mercyMenuSelected = true;

                        Thread.Sleep(150);
                    }
                    if (aDown == true)
                    {
                        mercySprite = Resources.mercyButton;
                        player.x = itemRec.X + 15;
                        player.y = itemRec.Y + 15;

                        Thread.Sleep(150);
                    }
                }
                #endregion mercy
            }

            #endregion buttons code

            //update the health bar and hp label depending on the player's hp
            remainingHPRec.Width = player.hp * 2;
            hpValueLabel.Text = player.hp + " / 40";

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

            //draw the fight UI if the player is in the fight menu
            if (fightMenuSelected == true)
            {
                e.Graphics.DrawImage(fightUISprite, arenaWalls[0].X + 5, arenaWalls[2].Y + 5, 838, 195);
                e.Graphics.FillRectangle(grayBrush, attackRec);
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

        #region fight UI
        private void FightUI()
        {

        }
        #endregion fight UI

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
                player.x = actRec.X + 15;
                player.y = actRec.Y + 15;

                //stop ActMenu() from being called when act menu is exited
                actMenuSelected = false;

                Thread.Sleep(150);
            }

            //call the menus method
            Menus();
        }
        #endregion act menu
        #region act menu text
        private void ActMenuText()
        {
            //create a counter
            int i = 0;

            //set reader to beginning of enemy file
            eReader = XmlReader.Create("Resources/TestEnemy.xml");

            while (eReader.Read() && i < 4)
            {
                //read to the next action in the enemy xml file
                eReader.ReadToFollowing("Act");

                //fill out the proper details for each act option
                actNames[i] = Convert.ToString(eReader.GetAttribute("actName"));
                actText[i] = "* " + Convert.ToString(eReader.GetAttribute("actLine1")) + "\n\n* " + Convert.ToString(eReader.GetAttribute("actLine2")) + "\n\n* " + Convert.ToString(eReader.GetAttribute("actLine3"));

                //add 1 to the counter
                i++;
            }

            eReader.Close();

            //these lines of code are nessecary for the initial display of each label
            actLabel1.Text = "* " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];
        }
        #endregion act menu text

        #region item menu
        private void ItemMenu()
        {
            //if for player exiting the item menu
            if (shiftDown == true)
            {
                //show the main text output
                textOutput.Visible = true;

                //hide the act labels
                actLabel1.Visible = false;
                actLabel2.Visible = false;
                actLabel3.Visible = false;
                actLabel4.Visible = false;

                //set player back to the item button
                player.x = itemRec.X + 15;
                player.y = itemRec.Y + 15;

                //stop ItemMenu() from being called when item menu is exited
                itemMenuSelected = false;

                Thread.Sleep(150);
            }

            //call the menus method
            Menus();
        }
        #endregion item menu
        #region item menu text
        private void ItemMenuText()
        {
            //create a counter
            int i = 0;

            //set reader to the Items section of the player xml file
            pReader = XmlReader.Create("Resources/Player.xml");

            while (pReader.Read() && i < 4)
            {
                //gather and set item info for each item
                pReader.ReadToFollowing("Item");
                actNames[i] = pReader.GetAttribute("name");
                itemHeals[i] = Convert.ToInt16(pReader.GetAttribute("heal"));

                actText[i] = "* You ate the " + actNames[i] + "\n\n* ..." + "\n\n* You recovered " + itemHeals[i] + " HP!";

                i++;
            }

            pReader.Close();

            //display items
            actLabel1.Text = "* " + actNames[0];
            actLabel2.Text = "* " + actNames[1];
            actLabel3.Text = "* " + actNames[2];
            actLabel4.Text = "* " + actNames[3];
        }
        #endregion item menu text

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
                player.x = mercyRec.X + 15;
                player.y = mercyRec.Y + 15;

                //stop MercyMenu() from being called when mercy menu is exited
                mercyMenuSelected = false;

                Thread.Sleep(150);
            }

            //call the menus method
            Menus();
        }
        #endregion mercy menu
        #region marcy menu text
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
        #endregion mercy menu text

        #region general menu code
        private void Menus()
        {
            //disable act options if they are blank
            if (actLabel2.Text == "*  ") { actLabel2.Visible = false; }
            if (actLabel3.Text == "*  ") { actLabel3.Visible = false; }
            if (actLabel4.Text == "*  ") { actLabel4.Visible = false; }

            //check which act option the player is on and do things accordingly
            #region option selection
            if (player.x == actLabel1.Location.X && player.y == actLabel1.Location.Y + 5)
            {
                actLabel1.Text = "  " + actNames[0];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(0);
                    Thread.Sleep(150);
                }
                //move to option 3
                if (sDown == true && actLabel3.Visible == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player.x = actLabel3.Location.X;
                    player.y = actLabel3.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 2
                if (dDown == true && actLabel2.Visible == true)
                {
                    actLabel1.Text = "* " + actNames[0];
                    player.x = actLabel2.Location.X;
                    player.y = actLabel2.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel2.Location.X && player.y == actLabel2.Location.Y + 5)
            {
                actLabel2.Text = "  " + actNames[1];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(1);
                    Thread.Sleep(150);
                }
                //move to option 1
                if (aDown == true && actLabel1.Visible == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player.x = actLabel1.Location.X;
                    player.y = actLabel1.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 4
                if (sDown == true && actLabel4.Visible == true)
                {
                    actLabel2.Text = "* " + actNames[1];
                    player.x = actLabel4.Location.X;
                    player.y = actLabel4.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel3.Location.X && player.y == actLabel3.Location.Y + 5)
            {
                actLabel3.Text = "  " + actNames[2];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(2);
                    Thread.Sleep(150);
                }
                //move to option 1
                if (wDown == true && actLabel1.Visible == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player.x = actLabel1.Location.X;
                    player.y = actLabel1.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 4
                if (dDown == true && actLabel4.Visible == true)
                {
                    actLabel3.Text = "* " + actNames[2];
                    player.x = actLabel4.Location.X;
                    player.y = actLabel4.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            if (player.x == actLabel4.Location.X && player.y == actLabel4.Location.Y + 5)
            {
                actLabel4.Text = "  " + actNames[3];

                //call the menu disappear method and go back to the fight button
                if (spaceDown == true)
                {
                    MenuDisappear(3);
                    Thread.Sleep(150);
                }
                //move to option 2
                if (wDown == true && actLabel2.Visible == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player.x = actLabel2.Location.X;
                    player.y = actLabel2.Location.Y + 5;

                    Thread.Sleep(150);
                }
                //move to option 3
                if (aDown == true && actLabel3.Visible == true)
                {
                    actLabel4.Text = "* " + actNames[3];
                    player.x = actLabel3.Location.X;
                    player.y = actLabel3.Location.Y + 5;

                    Thread.Sleep(150);
                }
            }
            #endregion option selection
        }
        #endregion general menu code
        #region menu disappear code
        private void MenuDisappear(int i)
        {
            //set output text to the appropraite message and make it visible
            textOutput.Text = actText[i];
            textOutput.Visible = true;

            //make all act options invisible
            actLabel1.Visible = false;
            actLabel2.Visible = false;
            actLabel3.Visible = false;
            actLabel4.Visible = false;

            //add hp to player if item was used and remove the item
            if (itemMenuSelected == true)
            {
                player.hp += itemHeals[i];
                if(player.hp > 40) { player.hp = 40; }

                PlayerXmlUpdate(i);
            }

            //make sure menu methods don't continue to be called when not in a menu
            fightMenuSelected = false;
            actMenuSelected = false;
            itemMenuSelected = false;
            mercyMenuSelected = false;

            //set all buttons to their non-active state
            fightSprite = Resources.fightButton;
            actSprite = Resources.actButton;
            itemSprite = Resources.itemButton;
            mercySprite = Resources.mercyButton;

            //set player back to the fight button
            player.x = fightRec.X + 15;
            player.y = fightRec.Y + 15;
        }
        #endregion menu disappear code
        #region menu display code
        private void MenuDisplay()
        {
            //make the text output not visible
            textOutput.Visible = false;

            ///make the act labels visible
            actLabel1.Visible = true;
            actLabel2.Visible = true;
            actLabel3.Visible = true;
            actLabel4.Visible = true;

            //set player position to the act1 label
            player.x = actLabel1.Location.X;
            player.y = actLabel1.Location.Y + 5;
        }
        #endregion menu display code

        #endregion menu methods

        #region player xml update method
        private void PlayerXmlUpdate(int i)
        {
            //open the player xml file adn place it in doc
            XmlDocument doc = new XmlDocument();
            doc.Load("Resources/Player.xml");

            //create a list of all nodes called "Item"
            XmlNodeList itemList = doc.GetElementsByTagName("Item");

            //search each Item node in the list until the text matches the item used
            //then change it to nothing to get rid of the item
            foreach (XmlNode n in itemList)
            {
                if (n.Attributes[0].InnerText == actNames[i])
                {
                    n.Attributes[0].InnerText = " ";
                    n.Attributes[1].InnerText = "0";
                }
            }

            //int for counting
            i = 0;

            //search each Item node in the list until the text is empty (aka item is empty)
            //then change it to the next item's info and change the next item's info to be empty
            //this should move all items back one item in the xml file
            foreach (XmlNode n in itemList)
            {
                if(n.Attributes[0].InnerText == " ")
                {
                    if (i < 3)
                    {
                        n.Attributes[0].InnerText = itemList[i + 1].Attributes[0].InnerText;
                        n.Attributes[1].InnerText = itemList[i + 1].Attributes[1].InnerText;

                        itemList[i + 1].Attributes[0].InnerText = " ";
                        itemList[i + 1].Attributes[1].InnerText = "0";
                    }
                    else
                    {
                        n.Attributes[0].InnerText = " ";
                        n.Attributes[1].InnerText = "0";
                    }
                }

                i++;
            }

            //save and close the player xml
            doc.Save("Resources/Player.xml");
        }
        #endregion player xml update method
    }
}

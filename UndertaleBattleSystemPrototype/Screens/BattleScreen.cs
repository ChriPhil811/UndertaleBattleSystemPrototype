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

        #region enemy attack variables (sorry these are in a section of their own, it made it easier at the time to find them)
        //enemy attack variables
        Boolean hornLeft = true;
        Boolean hornSpaceChange = false;
        int attackSpeed = 5;
        int spaceBetweenAttacks = 40;

        #endregion enemy attack variables (sorry these are in a section of their own, it made it easier at the time to find them)

        #region xml readers, brushes, images, and other
        //create an xml reader for the enemy file and player file
        XmlReader eReader, pReader;

        //brush for walls, player attacks, and hp bars
        SolidBrush wallBrush = new SolidBrush(Color.White);
        SolidBrush attackBrush = new SolidBrush(Color.Indigo);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        //create a new image for the player sprite and button sprites
        Image playerSprite, fightSprite, actSprite, itemSprite, mercySprite, fightUISprite;

        //create the player
        Player player = new Player();

        //create the enemy
        Enemy enemy = new Enemy();
        #endregion xml readers, brushes, images, and other

        #region ints and strings
        //int for after turn counting and enemy turn counting
        int afterTurnCounter = 0;
        int enemyTurnCounter = 500;

        //int for sparing
        int spareNum = -1;

        //string for damage number drawing
        string playerDamageNum;

        //string for enemy attack name
        string attackName;
        #endregion ints and strings

        #region booleans
        //make a globally useable spare variable
        public static Boolean canSpare = false;

        //player key press variables
        Boolean wDown, aDown, sDown, dDown, spaceDown, shiftDown;

        //boolean for checking if it's the enemy's turn or not 
        Boolean enemyTurn = false;

        //booleans for checking what menu the player is in
        Boolean fightMenuSelected = false, actMenuSelected = false, itemMenuSelected = false;

        //boolean for checking if the player has made an attack
        Boolean playerAttack = false;
        #endregion booleans

        #region rectangles
        //create rectangles for buttons
        Rectangle fightRec, actRec, itemRec, mercyRec;

        //rectangle for attacking UI
        Rectangle attackRec;

        //create a rectangle for player collision
        Rectangle playerRec;

        //health bar rectangles
        Rectangle maxHPRec = new Rectangle(410, 550, 80, 20);
        Rectangle remainingHPRec = new Rectangle(410, 550, 80, 20);
        Rectangle enemyMaxHPRec = new Rectangle(372, 50, 200, 20);
        Rectangle enemyRemainingHPRec = new Rectangle(372, 50, 200, 20);
        #endregion rectangles

        #region lists
        //new list for battle area walls
        List<Rectangle> arenaWalls = new List<Rectangle>();

        //lists for acting
        List<string> actNames = new List<string>() {" ", " ", " ", " "};
        List<string> actText = new List<string>() {" ", " ", " ", " "};
        List<int> spareValues = new List<int>() {0, 0, 0, 0};
        List<int> itemHeals = new List<int>() {0, 0, 0, 0};

        //lists for enemy turn
        List<string> enemyAttacks = new List<string>();
        List<int> enemyAttackValues = new List<int>();
        List<Rectangle> attackRecs = new List<Rectangle>();
        List<Projectile> attacks = new List<Projectile>();
        #endregion lists

        //random number generator
        Random randNum = new Random();

        #endregion variables and lists

        #region battle system brought up
        public BattleScreen()
        {
            InitializeComponent();

            //screen setup
            OnStart();

            //call the attack type method
            AttackType();

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

            //fill in enemy details for battle use
            eReader = XmlReader.Create("Resources/TestEnemy.xml");

            eReader.ReadToFollowing("Stats");
            enemy.hp = Convert.ToInt16(eReader.GetAttribute("hp"));
            enemy.atk = Convert.ToInt16(eReader.GetAttribute("atk"));
            enemy.def = Convert.ToInt16(eReader.GetAttribute("def"));

            eReader.Close();

            //fill in player details for battle use
            pReader = XmlReader.Create("Resources/Player.xml");

            pReader.ReadToFollowing("Battle");
            player.hp = Convert.ToInt16(pReader.GetAttribute("currentHP"));
            player.atk = Convert.ToInt16(pReader.GetAttribute("atk"));
            player.def = Convert.ToInt16(pReader.GetAttribute("def"));

            pReader.Close();

            //fill in player start position
            player.x = fightRec.X + 15;
            player.y = fightRec.Y + 15;
            player.size = 20;
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
            playerRec = new Rectangle(player.x, player.y, player.size, player.size);

            #region fighting area code

            //if it is the enemy's turn...
            if (enemyTurn == true && enemyTurnCounter > 0)
            {
                enemyTurnCounter--;

                EnemyAttack(enemyTurnCounter);

                #region player movement
                //player movement
                if (dDown == true && player.x < arenaWalls[1].X - arenaWalls[1].Width - player.size)
                {
                    player.MoveLeftRight(5);
                }
                if (aDown == true && player.x > arenaWalls[0].X + arenaWalls[0].Width)
                {
                    player.MoveLeftRight(-5);
                }
                if (wDown == true && player.y > arenaWalls[2].Y + arenaWalls[2].Height)
                {
                    player.MoveUpDown(-5);
                }
                if (sDown == true && player.y < arenaWalls[3].Y - arenaWalls[3].Height - player.size)
                {
                    player.MoveUpDown(5);
                }
                #endregion player movement
            }
            
            //if the enemy turn is over
            #region enemy turn over

            else if (enemyTurn == true && enemyTurnCounter <= 0)
            {
                //reset the enemy turn counter
                enemyTurnCounter = 500;
                enemyTurn = false;

                //reset the arena walls
                Rectangle leftWall = new Rectangle(fightRec.X, fightRec.Y - 250, 5, 200);
                Rectangle rightWall = new Rectangle(mercyRec.X + 135, mercyRec.Y - 250, 5, 200);
                Rectangle topWall = new Rectangle(fightRec.X, fightRec.Y - 250, 848, 5);
                Rectangle bottomWall = new Rectangle(fightRec.X, fightRec.Y - 50, 848, 5);

                //add the walls to the arena walls list
                arenaWalls.Clear();
                arenaWalls.Add(leftWall);
                arenaWalls.Add(rightWall);
                arenaWalls.Add(topWall);
                arenaWalls.Add(bottomWall);

                //set the player on the fight button
                player.x = fightRec.X + 15;
                player.y = fightRec.Y + 15;
            }

            #endregion enemy turn over
            #endregion fighting area code

            #region buttons code

            //if player is in the buttons and menus...
            else
            {
                if (fightMenuSelected == true) { FightUI(); }
                if (actMenuSelected == true) { ActMenu(); }
                if (itemMenuSelected == true) { ItemMenu(); }

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

                        //move the player off-screen during player attack
                        player.x = -20;
                        player.y = -20;

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
                    itemMenuSelected = false;

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
                    //check if the enemy is spare-able and set the button sprite accordingly
                    if (canSpare == true) { mercySprite = Resources.mercyButtonSpareBlank; }
                    else { mercySprite = Resources.mercyButtonBlank; }

                    //go into the mercy menu
                    if (spaceDown == true)
                    {
                        //check if the enemy is spare-able or not
                        if (canSpare == false)
                        {
                            //set the action result
                            actText[0] = "* You showed mercy." + "\n\n* ...";

                            //call the menu disappear method
                            MenuDisappear(-1);
                        }
                        else
                        {
                            //set the action result to show that the player won
                            actText[0] = "* You Won!";

                            //call the menu disappear method
                            MenuDisappear(0);

                            //go back to the town screen
                            TownScreen ts = new TownScreen();
                            Form form = this.FindForm();
                            form.Controls.Add(ts);
                            form.Controls.Remove(this);
                            ts.Focus();
                        }
                    }
                    if (aDown == true)
                    {
                        if (canSpare == true) { mercySprite = Resources.mercyButtonSpare; }
                        else { mercySprite = Resources.mercyButton; }

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

            //update the enemy's health bar depending on the enemey's hp
            enemyRemainingHPRec.Width = enemy.hp * 2;

            Refresh();
        }
        #endregion movement, collisions, and menus (gameloop)

        #region paint graphics
        private void BattleScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw the text box/arena walls
            foreach(Rectangle r in arenaWalls)
            {
                e.Graphics.FillRectangle(wallBrush, r);
            }

            //draw the fight UI if the player is in the fight menu
            if (fightMenuSelected == true)
            {
                e.Graphics.DrawImage(fightUISprite, arenaWalls[0].X + 5, arenaWalls[2].Y + 5, 838, 195);
                e.Graphics.FillRectangle(attackBrush, attackRec);
            }

            #region enemy attacks

            foreach (Projectile p in attacks)
            {
                e.Graphics.DrawImage(p.image, p.x, p.y, p.width, p.height);
            }

            #endregion enemy attacks

            #region enemy health bar

            //draw the enemy health bar and damage if the player has attacked, then pause before going to enemy turn
            if (playerAttack == true)
            {
                e.Graphics.FillRectangle(redBrush, enemyMaxHPRec);
                e.Graphics.FillRectangle(yellowBrush, enemyRemainingHPRec);

                damageLabel.Visible = true;
                damageLabel.Text = playerDamageNum;

                afterTurnCounter++;

                //if 2 seconds have passed then hide the enemy health bar and reset the after turn counter
                if (afterTurnCounter >= 100)
                {
                    playerAttack = false;
                    damageLabel.Visible = false;

                    //call the turn made method
                    TurnMade();

                    afterTurnCounter = 0;
                }
            }

            #endregion enemy health bar

            #region player UI

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

            #endregion player UI
        }
        #endregion paint graphics

        #region menu methods

        #region fight UI
        private void FightUI()
        {
            //if the player presses space, check where the attack rec is and do a damage calulation accordingly
            if (spaceDown == true)
            {
                //int for the center of the attack rec and the damage number for doing damage to the enemy
                int atkCenter = attackRec.X + attackRec.Width / 2;
                int damageNum = 0;

                //if attack is in the red areas of the fight UI do damage accordingly
                if (atkCenter > 50 && atkCenter < 300 || atkCenter > 638 && atkCenter < 888) 
                {
                    damageNum = randNum.Next(player.atk, player.atk * 2);
                }
                //if attack is in the yellow areas of the fight UI do damage accordingly
                if (atkCenter > 300 && atkCenter < 450 || atkCenter > 488 && atkCenter < 638) 
                {
                    damageNum = 2 * (randNum.Next(player.atk, player.atk * 2));
                }
                //if attack is in the green area of the fight UI do damage accordingly
                if (atkCenter > 450 && atkCenter < 488) 
                {
                    damageNum = 3 * (randNum.Next(player.atk, player.atk * 2));
                }

                //subtract the damage number from the enemy's hp
                enemy.hp -= damageNum;

                //set the player damage number string to the damage dealt for drawing
                playerDamageNum = Convert.ToString(damageNum);

                //set player attack boolean to true for drawing the enemy health bar and damage
                //also set the fightMenuSelected and spaceDown boolean to false so no fight UI reappears
                playerAttack = true;
                fightMenuSelected = false;
                spaceDown = false;
            }
            else if (attackRec.X > 888)
            {
                //set the player damage number string to "miss" for drawing
                playerDamageNum = "MISS";

                //set player attack boolean to true for drawing the enemy health bar and damage
                //also set the fightMenuSelected and false so no fight UI reappears
                playerAttack = true;
                fightMenuSelected = false;
            }
            else
            {
                attackRec.X += 20;
            }
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
                //check what step of sparing the player is on 
                //(0 is spare-able, -1 is a negative action, 1 to infinity is a step forward/neutral action)
                if (spareNum == 0 || spareNum == -1) { eReader.ReadToFollowing("Act"); }
                if (spareNum == 1) { eReader.ReadToFollowing("Act1"); }

                //fill out the proper details for each act option
                spareValues[i] = Convert.ToInt16(eReader.GetAttribute("spareValue"));
                actNames[i] = eReader.GetAttribute("actName");
                actText[i] = "* " + eReader.GetAttribute("actLine1") + "\n\n* " + eReader.GetAttribute("actLine2") + "\n\n* " + eReader.GetAttribute("actLine3");

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
            //set the spare number depending on the action selected
            spareNum = spareValues[i];

            //check if the player has made the correct choices for the enemy to be spared
            if (spareNum == 0) { canSpare = true; }

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
                if (player.hp > 40) { player.hp = 40; }

                PlayerXmlUpdate(i);
            }

            //set all buttons to their non-active state
            fightSprite = Resources.fightButton;
            actSprite = Resources.actButton;
            itemSprite = Resources.itemButton;
            if (canSpare == true) { mercySprite = Resources.mercyButtonSpare; }
            else { mercySprite = Resources.mercyButton; }

            //call the turn made method
            TurnMade();
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
        #region turn made code (going into the enemy turn)
        private void TurnMade()
        {
            //make the player disappear
            player.x = -20;
            player.y = -20;

            //wait for 3 seconds before starting the enemy turn if an act was made (so that the player can read lol)
            if (textOutput.Visible == true)
            {
                Refresh();
                Thread.Sleep(3000);
            }

            //make the main output label invisible
            textOutput.Visible = false;

            //set enemy turn boolean to true
            enemyTurn = true;

            //set the player in the middle of the battle area
            player.x = 462;
            player.y = 400;

            //resize the arena area
            Rectangle leftWall = new Rectangle(actRec.X, actRec.Y - 250, 5, 200);
            Rectangle rightWall = new Rectangle(itemRec.X + 135, itemRec.Y - 250, 5, 200);
            Rectangle topWall = new Rectangle(actRec.X, actRec.Y - 250, 376, 5);
            Rectangle bottomWall = new Rectangle(actRec.X, actRec.Y - 50, 376, 5);

            //add the new walls to the arena walls list
            arenaWalls.Clear();
            arenaWalls.Add(leftWall);
            arenaWalls.Add(rightWall);
            arenaWalls.Add(topWall);
            arenaWalls.Add(bottomWall);

            //randomly choose an attack to do from the available attacks
            int attackValue = randNum.Next(enemyAttackValues.Count());

            //set the attack name correctly
            attackName = enemyAttacks[attackValue];
        }
        #endregion turn made code (going into the enemy turn)

        #endregion menu methods

        #region enemy turn methods

        #region attack type method (filling in the possible attacks)
        private void AttackType()
        {
            //read from the enemy xml file
            eReader = XmlReader.Create("Resources/TestEnemy.xml");

            //fill in the enemy attacks lists from the enemy xml
            while (eReader.Read())
            {
                eReader.ReadToFollowing("Attack");
                enemyAttacks.Add(eReader.GetAttribute("name"));
                enemyAttackValues.Add(Convert.ToInt16(eReader.GetAttribute("value")));
            }

            //remove the last thing in both lists
            enemyAttacks.RemoveAt(enemyAttacks.Count() - 1);
            enemyAttackValues.RemoveAt(enemyAttackValues.Count() - 1);

            //stop the reader
            eReader.Close();
        }
        #endregion attack type method (filling in the possible attacks)
        #region enemy attack
        private void EnemyAttack(int timer)
        {
            //check which attack was randomly selected and do it
            if(attackName == "HornAttack") 
            { 
                //if it's been 1 second, and the enemy turn isn't over, spawn a new horn attack
                if (timer % spaceBetweenAttacks == 0  && timer != 0)
                {
                    //alternate between left and right attacks
                    if (hornLeft == true)
                    {
                        Projectile hornProjL = new Projectile(arenaWalls[0].X + 5, arenaWalls[3].Y, 200, 100, Resources.attackHornO);
                        attacks.Add(hornProjL);
                        hornLeft = false;
                    }
                    else
                    {
                        Projectile hornProjR = new Projectile(arenaWalls[1].X - 200, arenaWalls[3].Y, 200, 100, Resources.attackHorn);
                        attacks.Add(hornProjR);
                        hornLeft = true;
                        
                        //make the attack get more difficult as time goes on
                        if (hornSpaceChange == true && spaceBetweenAttacks >= 30)
                        {
                            attackSpeed += 1;
                            spaceBetweenAttacks -= 10;
                            hornSpaceChange = false;
                        }
                        else
                        {
                            hornSpaceChange = true;
                        }
                    }

                    //clear the attack rec list
                    attackRecs.Clear();

                    //for each projectile, create a rec for collisions
                    foreach (Projectile p in attacks)
                    {
                        Rectangle horn = new Rectangle(p.x, p.y, p.width, p.height);
                        attackRecs.Add(horn);
                    }
                }
                //reset the attack speed and spacing
                else if (timer == 0)
                {
                    attackSpeed = 4;
                    spaceBetweenAttacks = 50;
                }

                //move the projectiles according to the attack and get rid of the first one if it goes out of the arena box
                foreach (Projectile p in attacks) 
                { 
                    p.HornAttack(attackSpeed);

                    if (p.y <= arenaWalls[2].Y - 50)
                    {
                        attacks.Remove(p);
                        break;
                    }
                }
            }
        }
        #endregion enemy attack

        #endregion enemy turn methods

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
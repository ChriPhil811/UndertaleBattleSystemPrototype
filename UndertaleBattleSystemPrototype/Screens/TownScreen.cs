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


namespace UndertaleBattleSystemPrototype
{
    public partial class TownScreen : UserControl
    {
        #region global variables
        Boolean aDown, dDown, wDown, sDown, spaceDown, escapeDown;

        //nori
        Player nori;
        Rectangle noriRec;
        const int HEROSPEED = 5;

        Image noriSprite;
        List<Image> noriAnimation = new List<Image>();
        int animationCounterH = 0;
        int frameCounter = 1;

        //objects
        List<Object> objects = new List<Object>();
        List<Rectangle> objectRecs = new List<Rectangle>();

        //Text box
        Boolean displayText = false, displayTextBox = false;
        Rectangle textBox;
        List<string> textList = new List<string>();
        string text = "";
        int textNum = 0;
        XmlReader reader;

        //dialogue changes (counters)
        int talkingS = 0, talkingC = 0, talkingF = 0;
        #endregion

        //fighting variables
        Boolean fightCal, spareCal;
        public static string enemyName;

        //TODO HP/attack/defense ints that pull from hero xml

        public TownScreen()
        {
            InitializeComponent();
            OnStart();

            #region initializing nori animation images
            noriSprite = Properties.Resources.noriRR;
            noriAnimation.Add(Properties.Resources.noriBR);
            noriAnimation.Add(Properties.Resources.noriB1);
            noriAnimation.Add(Properties.Resources.noriB2);

            noriAnimation.Add(Properties.Resources.noriFR);
            noriAnimation.Add(Properties.Resources.noriF1);
            noriAnimation.Add(Properties.Resources.noriF2);

            noriAnimation.Add(Properties.Resources.noriRR);
            noriAnimation.Add(Properties.Resources.noriR1);
            noriAnimation.Add(Properties.Resources.noriR2);

            noriAnimation.Add(Properties.Resources.noriLR);
            noriAnimation.Add(Properties.Resources.noriL1);
            noriAnimation.Add(Properties.Resources.noriL2);
            #endregion

            #region initializing objects+object recs
            objects.Add(new Object(1750, 0, 290, 850, Properties.Resources.library));
            objects.Add(new Object(2700, 0, 300, 550, Properties.Resources.arlos));

            objects.Add(new Object(0, 0, 290, 780, Properties.Resources.appartement1));
            objects.Add(new Object(780, 0, 290, 815, Properties.Resources.appartement2));

            objects.Add(new Object(1900, 210, 90, 70, Properties.Resources.Sharol_WS));
            objects.Add(new Object(2610, 145, 155, 80, Properties.Resources.Calum_WS));
            objects.Add(new Object(3350, 150, 150, 100, Properties.Resources.Franky_WS));

            //just putting osmething in here so that the list isn't empty
            objectRecs.Add(new Rectangle(0, 0, 0, 0));

            #endregion

            textBox = new Rectangle(100, this.Height / 2 + 100, this.Width - 200, this.Height / 2 - 150);

        }

        public void OnStart()
        {
            nori = new Player(this.Width / 2 - 100, this.Height / 2 - 100, 150, 20, 20, 20);

            //filling the text list with all of the dialogue that happens on this screen
            reader = XmlReader.Create("Resources/text.xml");
            while (reader.Read())
            {
                reader.ReadToFollowing("text");

                textList.Add(reader.GetAttribute("value"));
            }

        }
        private void TownScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.Escape:
                    escapeDown = true;
                    break;

            }
        }

        private void TownScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.Escape:
                    escapeDown = false;
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region updating object recs
            objectRecs.Clear();
            objectRecs.Add(new Rectangle(objects[0].x + 265, objects[0].y + 290, 120, 20));
            objectRecs.Add(new Rectangle(objects[1].x + 85, objects[1].y + 290, 110, 20));

            //appartments(3-6)
            objectRecs.Add(new Rectangle(objects[2].x + 50, objects[2].y + 290, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + 650, objects[2].y + 290, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + 850, objects[2].y + 290, 100, 20));
            objectRecs.Add(new Rectangle(objects[2].x + 1450, objects[2].y + 290, 100, 20));

            //characters (7-9)
            objectRecs.Add(new Rectangle(objects[3].x, objects[3].y, objects[3].width, objects[3].height));
            objectRecs.Add(new Rectangle(objects[4].x, objects[4].y, objects[4].width, objects[4].height));
            objectRecs.Add(new Rectangle(objects[5].x, objects[5].y, objects[5].width, objects[5].height));

            #endregion

            #region update nori/object movement

            //setting the rectangles to the updated x,y
            noriRec = new Rectangle(nori.x + 40, nori.y + 130, 70, 20);

            if (wDown == true && sDown == false && nori.y >= 165)
            {
                nori.MoveUpDown(-HEROSPEED);
                animationCounterH++;
                if (animationCounterH == 4)
                {

                    if (frameCounter == 1)
                    {
                        noriSprite = noriAnimation[1];
                        animationCounterH = 0;
                        frameCounter = 2;
                    }
                    else if (frameCounter == 2)
                    {
                        noriSprite = noriAnimation[0];
                        animationCounterH = 0;
                        frameCounter = 3;
                    }
                    else if (frameCounter == 3)
                    {
                        noriSprite = noriAnimation[2];
                        animationCounterH = 0;
                        frameCounter = 4;
                    }
                    else if (frameCounter == 4)
                    {
                        noriSprite = noriAnimation[0];
                        animationCounterH = 0;
                        frameCounter = 1;
                    }
                }
            }
            if (sDown == true && wDown == false && nori.y <= this.Height)
            {
                nori.MoveUpDown(HEROSPEED);
                animationCounterH++;
                if (animationCounterH == 4)
                {

                    if (frameCounter == 1)
                    {
                        noriSprite = noriAnimation[4];
                        animationCounterH = 0;
                        frameCounter = 2;
                    }
                    else if (frameCounter == 2)
                    {
                        noriSprite = noriAnimation[3];
                        animationCounterH = 0;
                        frameCounter = 3;
                    }
                    else if (frameCounter == 3)
                    {
                        noriSprite = noriAnimation[5];
                        animationCounterH = 0;
                        frameCounter = 4;
                    }
                    else if (frameCounter == 4)
                    {
                        noriSprite = noriAnimation[3];
                        animationCounterH = 0;
                        frameCounter = 1;
                    }
                }
            }
            if (dDown == true && aDown == false)
            {
                foreach (Object o in objects)
                {
                    o.MoveLeftRight(-HEROSPEED);
                }

                animationCounterH++;
                if (animationCounterH == 4)
                {

                    if (frameCounter == 1)
                    {
                        noriSprite = noriAnimation[7];
                        animationCounterH = 0;
                        frameCounter = 2;
                    }
                    else if (frameCounter == 2)
                    {
                        noriSprite = noriAnimation[6];
                        animationCounterH = 0;
                        frameCounter = 3;
                    }
                    else if (frameCounter == 3)
                    {
                        noriSprite = noriAnimation[8];
                        animationCounterH = 0;
                        frameCounter = 4;
                    }
                    else if (frameCounter == 4)
                    {
                        noriSprite = noriAnimation[6];
                        animationCounterH = 0;
                        frameCounter = 1;
                    }
                }
            }
            if (aDown == true && dDown == false)
            {
                foreach (Object o in objects)
                {
                    o.MoveLeftRight(HEROSPEED);
                }
                animationCounterH++;
                if (animationCounterH == 4)
                {

                    if (frameCounter == 1)
                    {
                        noriSprite = noriAnimation[10];
                        animationCounterH = 0;
                        frameCounter = 2;
                    }
                    else if (frameCounter == 2)
                    {
                        noriSprite = noriAnimation[9];
                        animationCounterH = 0;
                        frameCounter = 3;
                    }
                    else if (frameCounter == 3)
                    {
                        noriSprite = noriAnimation[11];
                        animationCounterH = 0;
                        frameCounter = 4;
                    }
                    else if (frameCounter == 4)
                    {
                        noriSprite = noriAnimation[9];
                        animationCounterH = 0;
                        frameCounter = 1;
                    }
                }
            }
            #endregion

            #region collision checks

            #region screen changing collisions
            //library door
            if (noriRec.IntersectsWith(objectRecs[0]) && spaceDown == true)
            {
                LibraryScreen ls = new LibraryScreen();
                this.Controls.Add(ls);
                ls.Focus();
            }

            //arlo's door
            if (noriRec.IntersectsWith(objectRecs[1]) && spaceDown == true)
            {
                ShopScreen ss = new ShopScreen();
                this.Controls.Add(ss);
                ss.Focus();
            }
            #endregion

            #region text displaying collisions
            if (displayText == false)
            {
                #region appartement doors
                for (int i = 2; i < 6; i++)
                {
                    if (noriRec.IntersectsWith(objectRecs[i]) && spaceDown == true)
                    {
                        textNum = i - 2;
                        displayText = true;
                        Thread.Sleep(200);
                    }
                }
                #endregion

                #region Sharol
                if (noriRec.IntersectsWith(objectRecs[7]) && spaceDown == true)
                {
                    switch (talkingS)
                    {
                        case 0:
                            textNum = 4;
                            displayText = true;
                            spaceDown = false;
                            talkingS++;
                            break;
                        case 1:
                            textNum = 5;
                            displayText = true;
                            spaceDown = false;
                            talkingS++;
                            break;
                        case 2:
                            textNum = 6;
                            displayText = true;
                            spaceDown = false;
                            break;
                    }
                }
                #endregion

                #region Callum
                if (noriRec.IntersectsWith(objectRecs[8]) && spaceDown == true)
                {
                    if (talkingC == 2)
                    {
                        //flash and make a noise?
                        enemyName = "Callum";
                        talkingC++;
                        BattleScreen bs = new BattleScreen();
                        Form form = this.FindForm();
                        form.Controls.Add(bs);
                        form.Controls.Remove(this);
                        bs.Focus();

                    }
                    //positive outcome
                    if (talkingC == 1)
                    {
                        textNum = 7;
                        displayText = true;
                        spaceDown = false;
                    }

                    //negative outcome
                    if (talkingC == 0)
                    {
                        textNum = 8;
                        displayText = true;
                        spaceDown = false;
                    }
                    
                }
                #endregion

                #region Frankie

                #endregion
            }
            else
            {
                DisplayTextCollisions();

            }


            #endregion


            #endregion

            if (escapeDown == true)
            {
                Pausing();
            }

            Refresh();
        }

        private void TownScreen_Paint(object sender, PaintEventArgs e)
        {
            //drawing all images on the screen
            foreach (Object o in objects)
            {
                e.Graphics.DrawImage(o.sprite, o.x, o.y, o.width, o.height);
            }

            // for testing rectangle collisions
            Pen test = new Pen(Color.Red);
            foreach (Rectangle r in objectRecs)
            {
                e.Graphics.DrawRectangle(test, r);
            }

            //drawing nori
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);

            if (displayTextBox == true)
            {
                e.Graphics.DrawRectangle(test, textBox);
            }
        }
        private void DisplayTextCollisions()
        {

            text = textList[textNum];

            displayTextBox = true;
            textLabel.Visible = true;
            textLabel.Text = text;
            if (spaceDown == true)
            {
                displayTextBox = false;
                displayText = false;
                textLabel.Visible = false;
                textLabel.Text = "";
                Thread.Sleep(200);
            }

        }

        private void Pausing()
        {
            
            if (gameTimer.Enabled == true)
            {

                gameTimer.Enabled = false;
                Pause pauseForm = new Pause();

                DialogResult dr = Pause.Show();

                if (dr == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (dr == DialogResult.Abort)
                {
                    Form form = this.FindForm();
                    MenuScreen ms = new MenuScreen();

                    ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);

                    form.Controls.Add(ms);
                    form.Controls.Remove(this);
                }
            }

        }
    }
}

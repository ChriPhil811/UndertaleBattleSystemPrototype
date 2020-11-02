using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class TownScreen : UserControl
    {
        Boolean aDown, dDown, wDown, sDown, spaceDown;
        Player nori;
        const int HEROSPEED = 5;

        Image noriSprite;
        List<Image> noriAnimation = new List<Image>();
        int animationCounterH = 0;
        int frameCounter = 1;
        string direction;

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
        }

        public void OnStart()
        {
            nori = new Player(20, 170, 150, 20, 20, 20);

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
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region update nori movement
            if (wDown == true)
            {
                nori.MoveUpDown(-HEROSPEED);
                direction = "up";
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
            if (sDown == true)
            {
                nori.MoveUpDown(HEROSPEED);
                direction = "down";
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
            if (dDown == true)
            {
                nori.MoveLeftRight(HEROSPEED);
                direction = "right";
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
            if (aDown == true)
            {
                nori.MoveLeftRight(-HEROSPEED);
                direction = "left";
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
            Refresh();
        }

        private void TownScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(noriSprite, nori.x, nori.y, nori.size, nori.size);
        }
    }
}

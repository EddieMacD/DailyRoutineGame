using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Daily_Routine_Game
{
    public partial class Form1 : Form
    {
        private int[] playerPos = new int[] { 1, 1, 1, 1 };
        private int currentPlyr = 1;
        private BoardSqu[] boardSquares = new BoardSqu[30];

        private void initGame()
        {
            playerPos = new int[] { 1, 1, 1, 1 };
            currentPlyr = 1;
            lstEvents.Items.Clear();            
            playerNumGo.Text = "Nächste: Spieler 1";

            PlayerMove(player1, squ1);
            PlayerMove(player2, squ1);
            PlayerMove(player3, squ1);
            PlayerMove(player4, squ1);
        }

        public Form1()
        {
            InitializeComponent();
            InitBoardArray();
        }

        private void PlayerMove(int player, int targetSqu)
        {
            Panel playerPnl = (Panel)Controls["player" + player.ToString()];
            Panel targetPnl = (Panel)Controls["squ" + targetSqu.ToString()];

            PlayerMove(playerPnl, targetPnl);
        }

        private void AnimateToTarget(Panel player, int newLeft, int newTop)
        {
            player.BringToFront(); //don't want players to move behind other players

            int animSteps = 10; //set this to 1 for no animation

            int xStep = (int)((newLeft - player.Left) / animSteps);
            int yStep = (int)((newTop - player.Top) / animSteps);
            int oldLeft = player.Left;
            int oldTop = player.Top;

            for (int n = 0; n < animSteps; n++)
            {
                player.Left = oldLeft + (xStep * n);
                player.Top = oldTop + (yStep * n);

                System.Threading.Thread.Sleep(10); //wait a little so it's not too quick
                this.Refresh(); //cause the form to redraw
            }

            //deal with any odd pixels caused by int rounding
            player.Left = newLeft;
            player.Top = newTop;
        }

        private void PlayerMove(Panel player, Panel targetSqu)
        {
            if (player == player1)
            {
                AnimateToTarget(player, targetSqu.Left, targetSqu.Top);
            }
            else if (player == player2)
            {
                AnimateToTarget(player, targetSqu.Right - player.Width, targetSqu.Top);
            }
            else if (player == player3)
            {
                AnimateToTarget(player, targetSqu.Left, targetSqu.Bottom - player.Height);
            }
            else if (player == player4)
            {
                AnimateToTarget(player, targetSqu.Right - player.Width, targetSqu.Bottom - player.Height);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initGame();
        }

        private void UpdateDiceLabel(int roll)
        {
            for (int n=0;n<20;n++)
            {
                Random r = new Random();
                int dummyRoll = r.Next(1, 7);
                diceNum.Text = dummyRoll.ToString();
                System.Threading.Thread.Sleep(3); //wait a moment
                this.Refresh(); //cause the form to redraw
            }

            diceNum.Text = roll.ToString();
        }

        private void rollBtn_Click(object sender, EventArgs e)
        {
            //roll num
            Random r = new Random();
            int roll = r.Next(1, 7);
            UpdateDiceLabel(roll);            

            //make move
            int newPos = MakeMove(roll);

            //event check
            if (boardSquares[newPos - 1].IsEvent)
            {
                int posNeg = r.Next(2);

                if (posNeg == 1)
                {
                    BoardSqu boardAction = boardSquares[newPos - 1];
                    string msgText = string.Format(boardAction.PosActText + "\n\nVorwärts {1} platzen gehen.", currentPlyr, boardAction.EventAction);
                    lstEvents.Items.Insert(0, string.Format(boardAction.PosActText, currentPlyr));
                    MessageBox.Show(msgText, "Gut Nachrichten!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    newPos = MakeMove(boardAction.EventAction);
                }
                else
                {
                    BoardSqu boardAction = boardSquares[newPos - 1];
                    string msgText = string.Format(boardAction.NegActText + "\n\n Rückwärts {1} platzen gehen.", currentPlyr, boardAction.EventAction);
                    lstEvents.Items.Insert(0, string.Format(boardAction.NegActText, currentPlyr));
                    MessageBox.Show(msgText, "Schlecht Nachrichten!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    newPos = MakeMove(-boardAction.EventAction);
                }
            }

            //win check
            if (newPos == 30)
            {
                string msgText = string.Format("Spieler {0} hat Gewonnen!", currentPlyr);
                lstEvents.Items.Insert(0, msgText);
                MessageBox.Show(msgText, "Glückwunsch!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initGame();
                return;
            }

            //next player
            currentPlyr++;
            if (currentPlyr > 4)
            {
                currentPlyr = 1;
            }
            playerNumGo.Text = "Nächste: Spieler " + currentPlyr.ToString();
        }

        private int MakeMove(int roll)
        {
            int newPos = playerPos[currentPlyr - 1] + roll;
            if (newPos > 30)
            {
                PlayerMove(currentPlyr, 30);
                System.Threading.Thread.Sleep(100);
                newPos = 30 - (newPos - 30);
            }
            playerPos[currentPlyr - 1] = newPos;
            PlayerMove(currentPlyr, newPos);

            return newPos;
        }

        private void InitBoardArray()
        {
            boardSquares[0] = new BoardSqu(false, 0, "", "");
            boardSquares[1] = new BoardSqu(false, 0, "", "");
            boardSquares[2] = new BoardSqu(false, 0, "", "");
            boardSquares[3] = new BoardSqu(true, 3, "Der Spiele {0} wascht selbst.", "Der Spiele {0} wascht selbst nicht.");
            boardSquares[4] = new BoardSqu(false, 0, "", "");
            boardSquares[5] = new BoardSqu(false, 0, "", "");
            boardSquares[6] = new BoardSqu(false, 0, "", "");
            boardSquares[7] = new BoardSqu(false, 0, "", "");
            boardSquares[8] = new BoardSqu(false, 0, "", "");
            boardSquares[9] = new BoardSqu(true, 2, "Der Spiele {0} duscht selbst.", "Der Spiele {0} duscht selbst nicht.");

            boardSquares[10] = new BoardSqu(false, 0, "", "");
            boardSquares[11] = new BoardSqu(false, 0, "", "");
            boardSquares[12] = new BoardSqu(false, 0, "", "");
            boardSquares[13] = new BoardSqu(false, 0, "", "");
            boardSquares[14] = new BoardSqu(true, 4, "Der Spiele {0} zieht selbst an.", "Der Spiele {0} zieht selbst nicht an.");
            boardSquares[15] = new BoardSqu(false, 0, "", "");
            boardSquares[16] = new BoardSqu(false, 0, "", "");
            boardSquares[17] = new BoardSqu(true, 4, "Der Spiele {0} früchstückt", "Der Spiele {0} früchstückt nicht");
            boardSquares[18] = new BoardSqu(false, 0, "", "");
            boardSquares[19] = new BoardSqu(true, 2, "Der Spiele {0} gehe aus.", "Der Spiele {0} gehe vergessen aus.");

            boardSquares[20] = new BoardSqu(false, 0, "", "");
            boardSquares[21] = new BoardSqu(false, 0, "", "");
            boardSquares[22] = new BoardSqu(false, 0, "", "");
            boardSquares[23] = new BoardSqu(true, 3, "Der Spiele {0} komme zurück.", "Der Spiele {0} komme vergessen zurück.");
            boardSquares[24] = new BoardSqu(false, 0, "", "");
            boardSquares[25] = new BoardSqu(false, 0, "", "");
            boardSquares[26] = new BoardSqu(false, 0, "", "");
            boardSquares[27] = new BoardSqu(false, 0, "", "");
            boardSquares[28] = new BoardSqu(true, 4, "Der Spiele {0} isst zu abend.", "Der Spiele {0} isst zu abend nicht.");
            boardSquares[29] = new BoardSqu(false, 0, "", "");
        }
    }
}

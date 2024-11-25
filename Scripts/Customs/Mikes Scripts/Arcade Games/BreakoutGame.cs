#region Header
//               _,-'/-'/
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2023  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #                                       #
#endregion

#region References
using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;
using Server.Mobiles;
#endregion

namespace Server.Games
{
    public static class BreakoutGame
    {
        public static void Initialize()
        {
            CommandSystem.Register("breakout", AccessLevel.Player, Breakout_OnCommand);
        }

        [Usage("breakout")]
        [Description("Starts a game of Breakout.")]
        private static void Breakout_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted)
                return;

            new BreakoutGump(from).Send();
        }
    }

    public class BreakoutGump : SuperGump
    {
        private int PaddleX;
        private int PaddleWidth = 500;
        private int PaddleHeight = 20;

        private int BallX;
        private int BallY;
        private int BallSize = 10;
        private int BallDirX;
        private int BallDirY;

        private bool GameOver;
        private Timer GameTimer;
        private List<Rectangle2D> Bricks;

        private const int GumpWidth = 600;
        private const int GumpHeight = 400;

        public BreakoutGump(Mobile user)
            : base(user, null, 50, 50)
        {
            PaddleX = (GumpWidth - PaddleWidth) / 2;
            BallX = GumpWidth / 2;
            BallY = GumpHeight - 60;
            BallDirX = -4;
            BallDirY = -4;
            GameOver = false;
            Bricks = new List<Rectangle2D>();

            int brickWidth = 50;
            int brickHeight = 20;
            for (int y = 50; y <= 150; y += brickHeight + 5)
            {
                for (int x = 50; x <= GumpWidth - 50 - brickWidth; x += brickWidth + 5)
                {
                    Bricks.Add(new Rectangle2D(x, y, brickWidth, brickHeight));
                }
            }

            CanClose = true;
            CanMove = false;
            ForceRecompile = true;

            GameTimer = new GameUpdateTimer(this);
            GameTimer.Start();
        }

        protected override void CompileLayout(SuperGumpLayout layout)
        {
            base.CompileLayout(layout);

            layout.Add("background", () =>
            {
                AddBackground(0, 0, GumpWidth, GumpHeight, 9270);
            });

            layout.Add("paddle", () =>
            {
                AddImageTiled(PaddleX, GumpHeight - 40, PaddleWidth, PaddleHeight, 2624);
            });

            layout.Add("ball", () =>
            {
                AddImageTiled(BallX, BallY, BallSize, BallSize, 2648);
            });

            int index = 0;
            foreach (var brick in Bricks)
            {
                int x = brick.X;
                int y = brick.Y;
                layout.Add("brick" + index++, () =>
                {
                    AddImageTiled(x, y, brick.Width, brick.Height, 30073 + (index % 10));
                });
            }

            layout.Add("button_left", () =>
            {
                AddButton(10, GumpHeight - 30, 4014, 4016, b =>
                {
                    MovePaddle(-20);
                });
            });

            layout.Add("button_right", () =>
            {
                AddButton(GumpWidth - 40, GumpHeight - 30, 4005, 4007, b =>
                {
                    MovePaddle(20);
                });
            });
        }

        private void MovePaddle(int delta)
        {
            PaddleX += delta;

            // Log to confirm button click
            User.SendMessage($"Paddle moved by {delta} units. New position: {PaddleX}");

            // Keep paddle within gump bounds
            if (PaddleX < 0)
                PaddleX = 0;
            if (PaddleX > GumpWidth - PaddleWidth)
                PaddleX = GumpWidth - PaddleWidth;

            ForceRecompile = true;  // Forces a complete recompile to reflect the paddle position
            Refresh();  // Trigger a full redraw
        }

        public override void Close(bool all)
        {
            base.Close(all);
            StopGame();
        }

        private void StopGame()
        {
            GameTimer?.Stop();
            GameTimer = null;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            StopGame();
        }

        private class GameUpdateTimer : Timer
        {
            private BreakoutGump _gump;

            public GameUpdateTimer(BreakoutGump gump)
                : base(TimeSpan.Zero, TimeSpan.FromMilliseconds(50))
            {
                _gump = gump;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (_gump == null || _gump.User == null || _gump.User.Deleted || _gump.GameOver)
                {
                    Stop();
                    return;
                }

                _gump.UpdateGame();
            }
        }

        private void UpdateGame()
        {
            BallX += BallDirX;
            BallY += BallDirY;

            if (BallX <= 0 || BallX >= GumpWidth - BallSize)
                BallDirX *= -1;

            if (BallY <= 0)
                BallDirY *= -1;

            if (BallY + BallSize >= GumpHeight - 40 && BallY + BallSize <= GumpHeight - 20)
            {
                if (BallX + BallSize >= PaddleX && BallX <= PaddleX + PaddleWidth)
                {
                    BallDirY *= -1;
                    BallY = GumpHeight - 40 - BallSize;
                }
            }

            Rectangle2D ballRect = new Rectangle2D(BallX, BallY, BallSize, BallSize);
            for (int i = 0; i < Bricks.Count; i++)
            {
                if (Bricks[i].Intersects(ballRect))
                {
                    Bricks.RemoveAt(i);
                    BallDirY *= -1;
                    break;
                }
            }

            if (BallY >= GumpHeight)
            {
                GameOver = true;
                User.SendMessage(38, "Game Over!");
                Close(true);
                return;
            }

            if (Bricks.Count == 0)
            {
                GameOver = true;
                User.SendMessage(68, "You Win!");
                Close(true);
                return;
            }

            Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;

namespace Custom.Commands
{
    public class PlayerStatsCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("mycaps", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("MyStats")]
        [Description("Opens a gump displaying your current skill and stat information.")]
        public static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from is PlayerMobile player)
            {
                player.CloseGump(typeof(PlayerStatsGump));
                player.SendGump(new PlayerStatsGump(player));
            }
        }
    }

    public class PlayerStatsGump : Gump
    {
        private const int _pageCount = 6;
        private readonly PlayerMobile _player;

        public PlayerStatsGump(PlayerMobile player) : base(50, 50)
        {
            _player = player;

            AddPage(0);
            AddBackground(0, 0, 400, 400, 9200); // Solid black background

            AddHtml(20, 20, 360, 20, "<center><basefont color=#FFFFFF><b>Player Stats</b></basefont></center>", false, false);

            for (int i = 1; i <= _pageCount; i++)
            {
                AddButton(20 + ((i - 1) * 60), 360, 4005, 4006, i, GumpButtonType.Page, i);
                AddHtml(25 + ((i - 1) * 60), 360, 50, 20, $"<basefont color=#FFFFFF>{i}</basefont>", false, false);
            }

            AddPage(1);
            AddPageOne();

            AddPage(2);
            AddPageTwo();

            AddPage(3);
            AddPageThree();

            AddPage(4);
            AddPageFour();

            AddPage(5);
            AddPageFive();

            AddPage(6);
            AddPageSix();
        }

        private void AddPageOne()
        {
            AddHtml(20, 50, 360, 20, $"<basefont color=#FFFFFF>Total Skill Cap: {_player.SkillsCap}</basefont>", false, false);
            AddHtml(20, 80, 360, 20, $"<basefont color=#FFFFFF>Total Stat Cap: {_player.StatCap}</basefont>", false, false);
            AddHtml(20, 110, 360, 20, $"<basefont color=#FFFFFF>Maximum Followers: {_player.FollowersMax}</basefont>", false, false);
        }

        private void AddPageTwo()
        {
            AddHtml(20, 50, 360, 20, "<basefont color=#FFFFFF><u>Individual Stat Caps:</u></basefont>", false, false);
            AddHtml(30, 80, 340, 20, $"<basefont color=#FFFFFF>Strength: {_player.StrCap}</basefont>", false, false);
            AddHtml(30, 110, 340, 20, $"<basefont color=#FFFFFF>Dexterity: {_player.DexCap}</basefont>", false, false);
            AddHtml(30, 140, 340, 20, $"<basefont color=#FFFFFF>Intelligence: {_player.IntCap}</basefont>", false, false);
        }

        private void AddSkillPage(int startIndex, int endIndex)
        {
            AddHtml(20, 50, 360, 20, "<basefont color=#FFFFFF><u>Individual Skill Caps:</u></basefont>", false, false);
            int y = 80;
            List<Skill> skills = new List<Skill>(_player.Skills);
            skills.Sort((a, b) => b.Base.CompareTo(a.Base));

            for (int i = startIndex; i < Math.Min(endIndex, skills.Count); i++)
            {
                Skill skill = skills[i];
                if (skill.Base > 0)
                {
                    AddHtml(30, y, 340, 20, $"<basefont color=#FFFFFF>{skill.Name}: {skill.Cap}</basefont>", false, false);
                    y += 25;
                }
            }
        }

        private void AddPageThree() => AddSkillPage(0, 10);
        private void AddPageFour() => AddSkillPage(10, 20);
        private void AddPageFive() => AddSkillPage(20, 30);
        private void AddPageSix() => AddSkillPage(30, 40);
    }
}
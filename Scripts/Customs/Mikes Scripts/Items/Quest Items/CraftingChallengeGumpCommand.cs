using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Commands
{
    public class CraftingChallengeGumpCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CraftingChallenges", AccessLevel.Player, new CommandEventHandler(CraftingChallenges_OnCommand));
        }

        [Usage("CraftingChallenges")]
        [Description("Brings up a mockup of the crafting challenges gump.")]
        public static void CraftingChallenges_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new CraftingChallengeGump(e.Mobile));
        }
    }

    public class CraftingChallengeGump : Gump
    {
        public CraftingChallengeGump(Mobile mobile) : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 350, 450, 5054);
            AddLabel(115, 20, 1152, "Crafting Challenges");

            AddLabel(50, 60, 1152, "Lifetime Points: 0");
            AddLabel(50, 80, 1152, "Points to Spend: 0");

            AddButton(50, 110, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(85, 113, 1152, "Craft 10 Exceptional Swords");

            AddButton(50, 140, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddLabel(85, 143, 1152, "Craft 12 Exceptional Studded Skirts");

            AddButton(50, 170, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddLabel(85, 173, 1152, "Craft 15 Leather Chests");

            // ... Add more buttons and labels for additional challenges

            AddLabel(50, 400, 1152, "Jobs will reset in 7m");
            AddLabel(50, 420, 1152, "Remaining Contracts Available for Account: 10");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            // Here you would add the logic for each button click
        }
    }
}

using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class AnimalTamingQuestDealerGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("AnimalTamingQuestDealerGump", AccessLevel.GameMaster, new CommandEventHandler(AnimalTamingQuestDealerGump_OnCommand));
        }

        private static void AnimalTamingQuestDealerGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new AnimalTamingQuestDealerGump(e.Mobile));
        }

        public AnimalTamingQuestDealerGump(Mobile owner) : base(50, 50)
        {
            AddPage(0);

            AddImageTiled(54, 33, 369, 400, 2624);
            AddAlphaRegion(54, 33, 369, 400);

            AddImageTiled(416, 39, 44, 389, 203);
            AddImage(97, 49, 9005);
            AddImageTiled(58, 39, 29, 390, 10460);
            AddImageTiled(412, 37, 31, 389, 10460);
            AddLabel(140, 60, 0x34, "AnimalTaming Contracts");

            AddHtml(107, 140, 300, 230, "<BODY>" +
            "<BASEFONT COLOR=YELLOW>Hello there! I am in need of some assistance with Taming various creatures around the land.<BR><BR>" +
            "If you help me tame these creatures, I will provide you with a reward for your efforts. The creatures required and their respective rewards will be detailed in the contract.<BR><BR>" +
            "Would you like to take on a AnimalTaming contract and help me out?" +
            "</BODY>", false, true);

            AddImage(430, 9, 10441);
            AddImageTiled(40, 38, 17, 391, 9263);
            AddImage(6, 25, 10421);
            AddImage(34, 12, 10420);
            AddImageTiled(94, 25, 342, 15, 10304);
            AddImageTiled(40, 427, 415, 16, 10304);
            AddImage(-10, 314, 10402);
            AddImage(56, 150, 10411);
            AddImage(155, 120, 2103);
            AddImage(136, 84, 96);

            AddButton(125, 370, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 1: // Accept AnimalTaming Contract
                    {
                        from.SendMessage("The AnimalTaming contract has been placed in your backpack. Good luck!");
                        from.AddToBackpack(new AnimalTamingContract((PlayerMobile)from));
                        break;
                    }
                default:
                    {
                        from.SendMessage("Maybe next time. Stay safe out there!");
                        break;
                    }
            }
        }
    }
}

using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class UltimateCookingMasterGump : Gump
    {
        public UltimateCookingMasterGump(Mobile owner) : base(50, 50)
        {
            AddPage(0);
            AddImageTiled(54, 33, 369, 400, 2624);
            AddAlphaRegion(54, 33, 369, 400);

            AddImageTiled(416, 39, 44, 389, 203);
            AddImage(97, 49, 9005);
            AddImageTiled(58, 39, 29, 390, 10460);
            AddImageTiled(412, 37, 31, 389, 10460);
            AddLabel(140, 60, 0x34, "Ultimate Cooking Challenges");

            AddHtml(107, 140, 300, 230, "<BODY><BASEFONT COLOR=YELLOW>Greetings, aspiring chef! I have a challenge for you. Gather the materials listed in the contract I've provided, and I shall reward you beyond measure. Your skill in Cooking shall rise, and you will receive a special item from my private stores. The reward includes an Cooking Powerscroll that will raise your skill cap by 10 (up to 150), and a special item.<BR><BR>Check your pack for the contract and good luck!</BODY>", false, true);

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

            AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            state.Mobile.SendMessage("Your contract has been placed in your backpack. Complete it for your skill reward!");
        }
    }
}

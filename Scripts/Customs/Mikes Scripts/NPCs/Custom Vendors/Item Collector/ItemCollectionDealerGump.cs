using System;
using Server;
using Server.Gumps;
using Server.Network; // Add this for NetState class
using Server.Mobiles;

namespace Server.Gumps
{
    public class ItemCollectionDealerGump : Gump
    {
        public ItemCollectionDealerGump(Mobile owner) : base(50, 50)
        {
            AddPage(0);
            AddImageTiled(54, 33, 369, 400, 2624);
            AddAlphaRegion(54, 33, 369, 400);

            AddImageTiled(416, 39, 44, 389, 203);
            AddImage(97, 49, 9005);
            AddImageTiled(58, 39, 29, 390, 10460);
            AddImageTiled(412, 37, 31, 389, 10460);
            AddLabel(140, 60, 0x34, "Item Collection Contracts");

            AddHtml(107, 140, 300, 230, "<BODY><BASEFONT COLOR=YELLOW>Greetings! I need your help collecting certain items for me. If you succeed, there will be a reward waiting for you.  The items required and their respective rewards will be detailed in the contract.<<BR><BR>Good luck!</BODY>", false, true);

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

        public override void OnResponse(NetState state, RelayInfo info) // Ensure correct signature and using directive
        {
            Mobile from = state.Mobile;
            from.SendMessage("The contract was placed in your bag. Happy collecting!");
        }
    }
}

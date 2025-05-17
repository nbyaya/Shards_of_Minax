using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Bittiez.CustomSystems
{
    public class TensionAdminGump : Gump
    {
        public TensionAdminGump() : base(50, 50)
        {
            Closable = true;
            Dragable = true;
            AddPage(0);
            AddBackground(0, 0, 300, 200, 9270); // Background box
            AddLabel(90, 20, 1152, "Tension Admin Panel"); // Title in white

            // Display the current tension value in white
            AddLabel(50, 50, 1152, $"Current Tension: {TensionManager.Tension}");

            // Text entry box
            AddLabel(50, 80, 1152, "Adjust Tension By:");
            AddBackground(180, 75, 50, 20, 3000);
            AddTextEntry(180, 77, 46, 16, 1152, 0, "0"); // Default value "0"

            // Apply Button
            AddButton(50, 110, 4023, 4025, 1, GumpButtonType.Reply, 0);
            AddLabel(90, 110, 1152, "Apply Change");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1) // Apply button clicked
            {
                TextRelay entry = info.GetTextEntry(0);
                if (entry != null && int.TryParse(entry.Text, out int amount))
                {
                    TensionManager.IncreaseTension(amount);
                    from.SendMessage($"Tension adjusted by {amount}. New Tension: {TensionManager.Tension}");
                }
                else
                {
                    from.SendMessage("Invalid number entered. Please enter a valid integer.");
                }

                // Refresh gump to show updated value
                from.SendGump(new TensionAdminGump());
            }
        }
    }
}

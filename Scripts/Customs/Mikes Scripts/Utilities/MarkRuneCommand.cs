using System;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Items;

namespace Server.Commands
{
    public static class MarkRuneCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("MarkRune", AccessLevel.Player, new CommandEventHandler(MarkRune_OnCommand));
        }

        [Usage("MarkRune")]
        [Description("Marks a rune with the player's current location.")]
        public static void MarkRune_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || from.Backpack == null)
            {
                from.SendMessage("You need a backpack to mark a rune.");
                return;
            }

            if (!from.Alive)
            {
                from.SendMessage("You cannot mark a rune while dead.");
                return;
            }

            // Check for blank runes in the player's backpack
            RecallRune blankRune = from.Backpack.FindItemByType(typeof(RecallRune)) as RecallRune;

            if (blankRune == null)
            {
                from.SendMessage("You need a blank rune to mark.");
                return;
            }

            // Consume the blank rune
            blankRune.Delete();

            // Create a marked rune
            RecallRune markedRune = new RecallRune();
            markedRune.Marked = true;
            markedRune.Target = from.Location;
            markedRune.TargetMap = from.Map;
            markedRune.Description = "A marked rune";

            // Add the marked rune to the player's backpack
            if (!from.Backpack.TryDropItem(from, markedRune, false))
            {
                markedRune.Delete();
                from.SendMessage("You do not have enough space in your backpack for the marked rune.");
            }
            else
            {
                from.SendMessage("You successfully mark the rune.");
            }
        }
    }
}

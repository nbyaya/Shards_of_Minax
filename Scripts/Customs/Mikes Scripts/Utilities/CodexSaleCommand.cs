using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Scripts.Commands
{
    public class CodexSaleCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("codex", AccessLevel.Player, new CommandEventHandler(CodexSale_OnCommand));
        }

        [Usage("Codex")]
        [Description("Opens the Codex Sale gump to purchase spellbooks.")]
        public static void CodexSale_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || from.NetState == null)
                return;

            from.CloseGump(typeof(CodexSaleGump));
            from.SendGump(new CodexSaleGump());
        }
    }
}
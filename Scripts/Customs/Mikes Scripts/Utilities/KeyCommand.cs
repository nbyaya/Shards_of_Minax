using System;
using Server;
using Server.ACC.CM;
using Server.ACC.CSS.Modules;
using Server.ACC.CSS;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC
{
    public class KeyCommand
    {
        public static void Initialize()
        {
            // Register the Key command for all players
            CommandSystem.Register("Key", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("Key <hotkey>")]
        [Description("Executes the spell or action associated with the specified hotkey.")]
        private static void OnCommand(CommandEventArgs e)
        {
            var player = e.Mobile as PlayerMobile;

            if (player == null)
            {
                e.Mobile.SendMessage("This command can only be used by players.");
                return;
            }

            if (e.Arguments.Length == 0)
            {
                e.Mobile.SendMessage("You must specify a hotkey.");
                return;
            }

            string hotkey = e.Arguments[0].ToLower();

            // Retrieve the CastCommandsModule for the player
            var castModule = CentralMemory.GetModule(player.Serial, typeof(CastCommandsModule)) as CastCommandsModule;

            if (castModule == null)
            {
                e.Mobile.SendMessage("No hotkey module found for you.");
                return;
            }

            // Retrieve the spell or action associated with the hotkey
            var castInfo = castModule.Get(hotkey);

            if (castInfo == null)
            {
                e.Mobile.SendMessage($"No action or spell is mapped to the hotkey '{hotkey}'.");
                return;
            }

            // Attempt to cast the spell
            Spell spell = SpellInfoRegistry.NewSpell(castInfo.SpellType, castInfo.School, player, null); // Auto-target player
            if (spell != null)
            {
                spell.Cast();
                e.Mobile.SendMessage($"Hotkey '{hotkey}' triggered spell '{castInfo.SpellType}'.");
            }
            else
            {
                e.Mobile.SendMessage($"Failed to cast spell '{castInfo.SpellType}' for hotkey '{hotkey}'.");
            }
        }
    }
}

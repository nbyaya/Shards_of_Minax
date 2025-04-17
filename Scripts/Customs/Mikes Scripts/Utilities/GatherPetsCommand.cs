using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Commands
{
    public class GatherPetsCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("gatherpets", AccessLevel.Player, new CommandEventHandler(GatherPets_OnCommand));
        }

        [Usage("gatherpets")]
        [Description("Summons all of your controlled and summoned pets to your current location.")]
        public static void GatherPets_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            // Gather the player's pets (controlled or summoned).
            List<BaseCreature> pets = new List<BaseCreature>();
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseCreature bc)
                {
                    if ((bc.Controlled && bc.ControlMaster == from) ||
                        (bc.Summoned  && bc.SummonMaster  == from))
                    {
                        pets.Add(bc);
                    }
                }
            }

            if (pets.Count > 0)
            {
                foreach (BaseCreature pet in pets)
                {
                    // If the pet is mounted, dismount it first.
                    if (pet is IMount mount && mount.Rider != null)
                    {
                        mount.Rider = null; // forcibly dismount
                    }

                    // Teleport the pet to the player's current location.
                    pet.MoveToWorld(from.Location, from.Map);

                    // Optional: Visual effect for pet arrival.
                    Effects.SendLocationEffect(from.Location, from.Map, 0x3728, 10, 30); // Replace EffectItem
                    Effects.PlaySound(from.Location, from.Map, 0x201);
                }

                // Inform the player.
                from.SendMessage("Your pets have been summoned to your location.");
            }
            else
            {
                // Inform the player that no pets were found.
                from.SendMessage("You do not have any pets to summon.");
            }
        }
    }
}

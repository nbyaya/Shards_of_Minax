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
            BaseCreature currentMount = from.Mount as BaseCreature;

            List<BaseCreature> pets = new List<BaseCreature>();
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseCreature bc && bc != currentMount)
                {
                    if ((bc.Controlled && bc.ControlMaster == from) ||
                        (bc.Summoned && bc.SummonMaster == from))
                    {
                        pets.Add(bc);
                    }
                }
            }

            if (pets.Count > 0)
            {
                foreach (BaseCreature pet in pets)
                {
                    // Only move pets that aren't currently being ridden
                    if (pet is IMount mount && mount.Rider != null)
                    {
                        if (mount.Rider != from) // Only dismount other riders
                            mount.Rider = null;
                    }
                    
                    pet.MoveToWorld(from.Location, from.Map);
                    Effects.SendLocationEffect(from.Location, from.Map, 0x3728, 10, 30);
                    Effects.PlaySound(from.Location, from.Map, 0x201);
                }

                from.SendMessage("Your pets have been summoned to your location.");
            }
            else
            {
                from.SendMessage("You do not have any pets to summon.");
            }
        }
    }
}
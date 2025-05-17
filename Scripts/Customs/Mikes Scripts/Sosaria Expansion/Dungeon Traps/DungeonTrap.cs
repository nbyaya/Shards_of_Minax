using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;  // For Poison definitions

namespace Server.Items
{
    public class InvisibleTrap : Item
    {
        [Constructable]
        public InvisibleTrap() : base(0x1) // Using an arbitrary graphic id; the trap is invisible.
        {
            Movable = false;
            Visible = false; // The trap will not appear in the world.
            Name = "invisible trap";
        }

        // Override OnMoveOver to return a boolean.
        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile)
            {
                // Calculate detection chance using RemoveTrap skill: 200 equals 100% detection, 0 equals 0%.
                double removalChance = m.Skills[SkillName.RemoveTrap].Value / 200.0;

                // If the player's RemoveTrap roll succeeds, disable the trap.
                if (Utility.RandomDouble() < removalChance)
                {
                    m.SendMessage("You quickly detect and disable the trap.");
                    this.Delete();
                    return base.OnMoveOver(m);
                }

                // The trap activates; roll for a random trap effect (0-10).
                int effect = Utility.Random(11);
                switch (effect)
                {
                    case 0: // Stun Rune: freezes the player for 10 seconds.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x0E62, 16, 1, 0, 0);
                        m.Paralyze(TimeSpan.FromSeconds(10));
                        m.SendMessage("A stun rune has frozen you in place!");
                        break;

                    case 1: // Teleport Trap: teleports the player to a nearby location.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x0F6C, 16, 1, 0, 0);
                        m.SendMessage("You are suddenly teleported by a mysterious force!");
                        // Determine a random nearby location.
                        int offsetX = Utility.RandomMinMax(-5, 5);
                        int offsetY = Utility.RandomMinMax(-5, 5);
                        Point3D newLocation = new Point3D(m.X + offsetX, m.Y + offsetY, m.Z);
                        m.Location = newLocation;
                        break;

                    case 2: // Web Trap: immobilizes for 4 seconds and inflicts deadly poison.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x10DA, 16, 1, 0, 0);
                        m.Paralyze(TimeSpan.FromSeconds(4));
                        m.SendMessage("Sticky webbing immobilizes you while a toxic substance burns through your veins!");
                        m.ApplyPoison(m, Poison.Deadly);
                        break;

                    case 3: // Fire Trap: inflicts 50 damage.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x10F7, 16, 1, 0, 0);
                        m.Damage(50, m);
                        m.SendMessage("Flames burst forth, scorching you!");
                        break;

                    case 4: // Saw Trap: inflicts 60 damage.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x11AD, 16, 1, 0, 0);
                        m.Damage(60, m);
                        m.SendMessage("A vicious saw cuts into your flesh!");
                        break;

                    case 5: // Spike Trap: inflicts 40 damage.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x119A, 16, 1, 0, 0);
                        m.Damage(40, m);
                        m.SendMessage("Spikes impale you!");
                        break;

                    case 6: // Mushroom Trap: drains all stamina.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x1126, 16, 1, 0, 0);
                        m.SendMessage("A strange fungus saps all your strength!");
                        m.Stam = 0;
                        break;

                    case 7: // Gas Trap: inflicts deadly poison.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x1126, 16, 1, 0, 0);
                        m.SendMessage("A cloud of poisonous gas envelops you!");
                        m.ApplyPoison(m, Poison.Deadly);
                        break;

                    case 8: // Magic Trap: drains all mana.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x1126, 16, 1, 0, 0);
                        m.SendMessage("Mystical forces drain your magical energy!");
                        m.Mana = 0;
                        break;

                    case 9: // Axe Trap: sets the player's health to 1.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x1193, 16, 1, 0, 0);
                        m.Hits = 1;
                        m.SendMessage("A sharp blade cuts you down, leaving you barely alive!");
                        break;

                    case 10: // Guillotine Trap: instant death.
                        Effects.SendLocationEffect(m.Location, m.Map, 0x1245, 16, 1, 0, 0);
                        m.SendMessage("A deadly guillotine trap has been triggered!");
                        m.Kill();
                        break;
                }
                // Remove the trap after it has been activated.
                this.Delete();
            }
            return base.OnMoveOver(m);
        }

        public InvisibleTrap(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

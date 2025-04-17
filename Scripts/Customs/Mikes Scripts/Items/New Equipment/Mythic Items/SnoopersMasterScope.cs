using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Regions;

namespace Server.Items
{
    public class SnoopersMasterScope : Item
    {
        [Constructable]
        public SnoopersMasterScope() : base(0x14F5)
        {
            Weight = 1.0;
            Name = "Snooper's Master Scope";
            Hue = 1153; // Change hue if needed
        }

        public SnoopersMasterScope(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            double snoopSkill = from.Skills[SkillName.Snooping].Value;
            int snoopRange = (int)(snoopSkill / 10); // 150 skill = 15 tile range, 100 skill = 10 tile range, etc.

            if (snoopRange < 1)
            {
                from.SendMessage("You lack the skill to use this properly.");
                return;
            }

            from.SendMessage($"Select a container to snoop (Range: {snoopRange} tiles).");
            from.Target = new SnoopTarget(snoopRange);
        }

        private class SnoopTarget : Target
        {
            private readonly int _snoopRange;

            public SnoopTarget(int snoopRange) : base(snoopRange, false, TargetFlags.None)
            {
                _snoopRange = snoopRange;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from.Deleted || targeted == null)
                    return;

                if (targeted is Container container)
                {
                    AttemptSnoop(from, container, null);
                }
                else if (targeted is Mobile mob)
                {
                    if (mob.Backpack != null)
                        AttemptSnoop(from, mob.Backpack, mob);
                    else
                        from.SendMessage("That NPC has no backpack.");
                }
                else
                {
                    from.SendMessage("That is not a valid container.");
                }
            }

            private void AttemptSnoop(Mobile from, Container container, Mobile owner)
            {
                double snoopSkill = from.Skills[SkillName.Snooping].Value;
                double failChance = 1.0 - (snoopSkill / 150.0); // At 150 skill, fail chance is 0%. At 75 skill, fail chance is 50%.

                bool success = Utility.RandomDouble() >= failChance; // Determine if snoop is successful

                // **Skill Gain Attempt**
                from.CheckSkill(SkillName.Snooping, 0.0, 200.0); // Allows skill to increase with use

                if (success) // Successful snoop
                {
                    container.DisplayTo(from);
                    from.SendMessage("You successfully snoop the container.");
                }
                else // Failed snoop
                {
                    from.SendMessage("You fail to snoop discreetly!");

                    // If NPC owner, they turn hostile
                    if (owner is BaseCreature npc && npc.Controlled == false)
                    {
                        npc.Combatant = from;
                        npc.AggressiveAction(from);
                        from.SendMessage($"{owner.Name} catches you snooping and becomes hostile!");
                    }

                    HandleCriminalFlag(from);
                }
            }

            private void HandleCriminalFlag(Mobile from)
            {
                Region reg = from.Region;
                bool isInTown = reg != null && reg.IsPartOf<GuardedRegion>();

                if (isInTown)
                {
                    from.CriminalAction(true); // Guards attack the player
                    from.SendMessage("The guards have been alerted to your criminal activity!");
                }
                else
                {
                    from.Criminal = true;
                    from.SendMessage("You have been flagged as a criminal for your actions!");
                }
            }
        }
    }
}

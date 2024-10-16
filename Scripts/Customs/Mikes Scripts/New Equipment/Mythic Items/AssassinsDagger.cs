using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class AssassinsDagger : Dagger
    {
        [Constructable]
        public AssassinsDagger() : base()
        {
            Name = "Assassin's Dagger";
        }

        public AssassinsDagger(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            // Check: The item must be in the player's pack or hands to be used
            if (!IsChildOf(from.Backpack) && !IsChildOf(from))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("Who will you assassinate?");
            from.Target = new AssassinationTarget(this);
        }

        private class AssassinationTarget : Target
        {
            private AssassinsDagger m_Dagger;

            public AssassinationTarget(AssassinsDagger dagger) : base(1, false, TargetFlags.None)
            {
                m_Dagger = dagger;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Dagger.Deleted)
                    return;

                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.InRange(target, 1))
                    {
                        target.BoltEffect(0); // Visual effect of the bolt
                        target.PlaySound(0x1E4); // Play a death sound
                        int damage = target.Hits + 10; // Plus a little extra to ensure it's a kill.
						target.Damage(damage, from);

                        from.SendMessage("You have assassinated your target!");
                        m_Dagger.Delete(); // Delete the dagger after use
                    }
                    else
                    {
                        from.SendMessage("Your target is too far away!");
                    }
                }
                else
                {
                    from.SendMessage("This is not a valid target.");
                }
            }
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
    }
}

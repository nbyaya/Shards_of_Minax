using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class NetTrap : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Net Trap", "Deploy a magical fishing net that ensnares and immobilizes enemies within its area.",
            //SpellCircle.Fifth,
            266,
            0x1C
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public NetTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private NetTrap m_Owner;

            public InternalTarget(NetTrap owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D point)
                {
                    m_Owner.DeployNet(new Point3D(point), from);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private void DeployNet(Point3D location, Mobile caster)
        {
            if (CheckSequence())
            {
                // Play sound effect
                Effects.PlaySound(location, caster.Map, 0x1F5);

                // Deploy net item (custom net item)
                NetTrapItem net = new NetTrapItem();
                net.MoveToWorld(location, caster.Map);

                // Visual effect for the net deployment
                Effects.SendLocationEffect(location, caster.Map, 0x1C3, 30, 10);

                // Create a timer to check for creatures in the net area
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    foreach (Mobile mobile in caster.GetMobilesInRange(4))
                    {
                        if (mobile.Alive && !mobile.IsDeadBondedPet)
                        {
                            double distance = mobile.GetDistanceToSqrt(location);
                            if (distance <= 4)
                            {
                                // Apply the immobilization effect
                                mobile.Frozen = true;
                                mobile.SendMessage("You are ensnared by a magical net!");
                                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                                {
                                    if (mobile != null && mobile.Alive)
                                        mobile.Frozen = false;
                                });
                            }
                        }
                    }
                });
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }

    public class NetTrapItem : Item
    {
        [Constructable]
        public NetTrapItem() : base(0x1F4C) // Using a placeholder item ID; you might want to use a custom item ID or create a new item
        {
            Movable = false;
            Visible = false;
        }

        public NetTrapItem(Serial serial) : base(serial)
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
    }
}

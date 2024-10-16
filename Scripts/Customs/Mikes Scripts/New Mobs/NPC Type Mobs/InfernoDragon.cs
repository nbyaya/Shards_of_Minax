using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class InfernoDragon : BaseCreature
    {
        private DateTime m_NextSpecialAttackTime;
        private int attackRadius = 5; // Default radius of the circular attack
        private int attackThickness = 1; // Default thickness of the attack

        [Constructable]
        public InfernoDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "InfernoDragon";
            Body = 49; // Adjust to appropriate dragon model
            BaseSoundID = 362;

            // Your stats, skills, and resistances configuration here

            m_NextSpecialAttackTime = DateTime.UtcNow; // Initialize the cooldown
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextSpecialAttackTime)
            {
                CircleFireSpecialAttack(attackRadius, attackThickness);
                m_NextSpecialAttackTime = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Adjust cooldown as needed
            }
        }

        public void CircleFireSpecialAttack(int radius, int thickness)
        {
            if (Map == null) return;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (i * i + j * j <= radius * radius && i * i + j * j >= (radius - thickness) * (radius - thickness))
                    {
                        Point3D p = new Point3D(X + i, Y + j, Z);
                        if (Map.CanFit(p.X, p.Y, Z, 16, false, false))
                        {
                            Effects.SendLocationEffect(p, Map, 0x36BD, 16, 10, 0, 0); // Flamestrike animation with default hue
                            DealDamageAtPoint(p);
                        }
                    }
                }
            }
        }

        private void DealDamageAtPoint(Point3D p)
        {
            IPooledEnumerable eable = Map.GetMobilesInRange(p, 0);
            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile || m is BaseCreature)
                {
                    m.Damage(20, this); // Adjust damage as needed
                }
            }
            eable.Free();
        }

        public InfernoDragon(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

using System;
using System.Collections;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public class UltraPoisonBomb : Item
    {
        private const int ExplosionRange = 8;
        private Timer m_Timer;
        private ArrayList m_Users;
        private bool m_Exploded = false;

        [Constructable]
        public UltraPoisonBomb() : base(0xF0D)
        {
            Hue = 2473;
            Weight = 1.0;
			Name = "Ultra Poison Bomb";
        }

        public UltraPoisonBomb(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber { get { return 1072095; } } // Poison Bomb
        public virtual int MinDamage { get { return 15; } }
        public virtual int MaxDamage { get { return 30; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.
                return;
            }
			
			if (from.Skills[SkillName.Poisoning].Base < 100.0)
            {
                from.SendMessage("You need at least 100 Poisoning skill to use this bomb.");
                return;
            }

            from.RevealingAction();

            if (m_Users == null)
                m_Users = new ArrayList();

            if (!m_Users.Contains(from))
                m_Users.Add(from);

            from.Target = new ThrowTarget(this);

            if (m_Timer == null)
            {
                m_Timer = Timer.DelayCall(
                    TimeSpan.FromSeconds(0.75),
                    TimeSpan.FromSeconds(1.0),
                    4,
                    new TimerStateCallback(Detonate_OnTick),
                    new object[] { from, 3 }); // 2.6 seconds explosion delay
            }
        }

        public void Explode(Mobile from, Point3D loc, Map map)
        {
            if (Deleted || map == null || m_Exploded)
                return;

            m_Exploded = true;
            Consume();

            Effects.PlaySound(loc, map, 0x307);
			Effects.PlaySound(loc, map, 0x231);

            Effects.SendLocationEffect(loc, map, 0x11A6, 50, 10, 2473, 0);

            IPooledEnumerable eable = map.GetMobilesInRange(loc, ExplosionRange);
            foreach (Mobile m in eable)
            {
                if (from == null || SpellHelper.ValidIndirectTarget(from, m))
                {
                    if (from != null)
                    {
                        from.DoHarmful(m);
                    }

                    int damage = Utility.RandomMinMax(MinDamage, MaxDamage);
                    AOS.Damage(m, from, damage, 0, 0, 0, 100, 0);

                    m.ApplyPoison(from, Poison.Lethal);

                    m.SendLocalizedMessage(1070820); // You have been poisoned!
                }
            }
            eable.Free();

            TriggerPoisonCloudWave(loc, map);
        }

        private void TriggerPoisonCloudWave(Point3D center, Map map)
        {
            for (int radius = 0; radius <= ExplosionRange; radius++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10 * radius), () =>
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        for (int y = -radius; y <= radius; y++)
                        {
                            if (x * x + y * y <= radius * radius)
                            {
                                Point3D loc = new Point3D(center.X + x, center.Y + y, center.Z);
                                Effects.SendLocationEffect(loc, map, 0x11A6, 30, 10, 2511, 0);
                            }
                        }
                    }
                });
            }
        }

        private void Detonate_OnTick(object state)
        {
            if (Deleted)
                return;

            object[] states = (object[])state;
            Mobile from = (Mobile)states[0];
            int timer = (int)states[1];

            if (timer == 0)
            {
                Explode(from, GetWorldLocation(), Map);
                m_Timer = null;
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
                states[1] = timer - 1;
            }
        }

        private class ThrowTarget : Target
        {
            private readonly UltraPoisonBomb m_UltraPoisonBomb;

            public ThrowTarget(UltraPoisonBomb UltraPoisonBomb) : base(12, true, TargetFlags.None)
            {
                m_UltraPoisonBomb = UltraPoisonBomb;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_UltraPoisonBomb.Deleted || m_UltraPoisonBomb.Map == Map.Internal)
                    return;

                IPoint3D p = targeted as IPoint3D;
                if (p == null)
                    return;

                Map map = from.Map;
                if (map == null)
                    return;

                SpellHelper.GetSurfaceTop(ref p);

                from.RevealingAction();

                IEntity to = new Entity(Serial.Zero, new Point3D(p), map);

                Effects.SendMovingEffect(from, to, m_UltraPoisonBomb.ItemID, 7, 0, false, false, m_UltraPoisonBomb.Hue, 0);

                // Remove the bomb from the player's inventory
                m_UltraPoisonBomb.MoveToWorld(new Point3D(p), map);

                // Cancel any existing timer
                if (m_UltraPoisonBomb.m_Timer != null)
                {
                    m_UltraPoisonBomb.m_Timer.Stop();
                    m_UltraPoisonBomb.m_Timer = null;
                }

                // Set a new timer for the explosion at the target location
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => m_UltraPoisonBomb.Explode(from, new Point3D(p), map));
            }
        }
    }
}
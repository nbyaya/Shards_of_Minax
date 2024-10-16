using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlNuke : XmlAttachment
    {
        private static readonly TimeSpan NukeCooldown = TimeSpan.FromSeconds(25);
        private DateTime m_NextNukeAllowed;
        private int m_BaseDamage = 50;
        private int m_Range = 8;

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseDamage { get { return m_BaseDamage; } set { m_BaseDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [Attachable]
        public XmlNuke() { }

        [Attachable]
        public XmlNuke(int baseDamage, int range)
        {
            BaseDamage = baseDamage;
            Range = range;
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextNukeAllowed)
            {
                DoNuke(attacker);
                m_NextNukeAllowed = DateTime.UtcNow + NukeCooldown;
            }
        }

        private void DoNuke(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            Map targetMap = owner.Map; // Renamed variable here
            owner.Say("Prepare to be incinerated!");

            Effects.PlaySound(owner.Location, targetMap, 0x349);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (owner.Alive)
                {
                    Effects.PlaySound(owner.Location, targetMap, 0x44B);

                    for (int i = 0; i < Range; i++)
                    {
                        Misc.Geometry.Circle2D(owner.Location, targetMap, i, (pnt, map) =>
                        {
                            Effects.SendLocationEffect(pnt, map, 14000, 14, 10, Utility.RandomMinMax(2497, 2499), 2);
                        });
                    }
                }
            });

            IPooledEnumerable nearbyMobiles = targetMap.GetMobilesInRange(owner.Location, Range);

            foreach (Mobile m in nearbyMobiles)
            {
                if (m != null && m.Alive && m != owner && !m.IsDeadBondedPet)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1.75), () =>
                    {
                        int fireResist = m.GetResistance(ResistanceType.Fire);
                        int finalDamage = (int)(BaseDamage * (1.0 - fireResist / 100.0));
                        m.Damage(finalDamage, owner);
                    });
                }
            }

            nearbyMobiles.Free();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_BaseDamage);
            writer.Write(m_Range);
            writer.WriteDeltaTime(m_NextNukeAllowed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_BaseDamage = reader.ReadInt();
            m_Range = reader.ReadInt();
            m_NextNukeAllowed = reader.ReadDeltaTime();
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFossilBurst : XmlAttachment
    {
        private int m_MinDamage = 20; // Minimum damage dealt
        private int m_MaxDamage = 30; // Maximum damage dealt
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown time
        private DateTime m_NextFossilBurst;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlFossilBurst(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFossilBurst() { }

        [Attachable]
        public XmlFossilBurst(int minDamage, int maxDamage, double cooldown)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextFossilBurst);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextFossilBurst = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Fossil Burst: Deals {0}-{1} damage to nearby targets.", m_MinDamage, m_MaxDamage);
        }

        public void ExecuteFossilBurst(Mobile owner)
        {
            if (DateTime.UtcNow < m_NextFossilBurst || owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes ancient energy *");
            owner.PlaySound(0x665);
            owner.FixedEffect(0x36BD, 20, 10);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = owner.GetMobilesInRange(5);

            foreach (Mobile m in eable)
            {
                if (m != owner && owner.CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
                AOS.Damage(m, owner, damage, 100, 0, 0, 0, 0);
                m.SendLocalizedMessage(1070844); // The creature's aura of energy decreases your resistance to physical attacks.

                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -20);
                m.AddResistanceMod(mod);

                Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
                {
                    m.RemoveResistanceMod(mod);
                    m.SendLocalizedMessage(1070845); // Your resistance to physical attacks has returned.
                });
            }

            m_NextFossilBurst = DateTime.UtcNow + m_Cooldown; // Reset cooldown
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            ExecuteFossilBurst(attacker); // Execute the ability on weapon hit
        }
    }
}

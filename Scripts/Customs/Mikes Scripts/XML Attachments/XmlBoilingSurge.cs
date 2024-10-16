using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBoilingSurge : XmlAttachment
    {
        private int m_DamageIncrease = 10; // Damage increase for boiling aura
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown for Boiling Surge
        private DateTime m_NextBoilingSurge;

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageIncrease { get { return m_DamageIncrease; } set { m_DamageIncrease = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlBoilingSurge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBoilingSurge() { }

        [Attachable]
        public XmlBoilingSurge(int damageIncrease, double cooldown)
        {
            DamageIncrease = damageIncrease;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_DamageIncrease);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextBoilingSurge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_DamageIncrease = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextBoilingSurge = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextBoilingSurge)
            {
                PerformBoilingSurge(attacker);
                m_NextBoilingSurge = DateTime.UtcNow + m_Cooldown;
            }
        }

		public void PerformBoilingSurge(Mobile owner)
		{
			if (owner == null) return;

			if (owner is BaseCreature creature)
			{
				creature.SetDamage(creature.DamageMin + m_DamageIncrease, creature.DamageMax + m_DamageIncrease);
				owner.SendMessage("A boiling aura intensifies your power!");
				Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 9917);

				Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(() =>
				{
					creature.SetDamage(creature.DamageMin - m_DamageIncrease, creature.DamageMax - m_DamageIncrease);
				}));
			}
			else
			{
				owner.SendMessage("You cannot use this ability.");
			}
		}

    }
}

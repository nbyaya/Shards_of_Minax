using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBlazingCharge : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(60); // Cooldown for the ability
        private DateTime m_NextBlazingCharge;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlBlazingCharge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBlazingCharge() { }

        [Attachable]
        public XmlBlazingCharge(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextBlazingCharge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextBlazingCharge = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Blazing Charge Ability: Charges at the target, dealing damage and setting them ablaze.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextBlazingCharge && defender != null && defender.Alive)
            {
                PerformBlazingCharge(attacker, defender);
                m_NextBlazingCharge = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformBlazingCharge(Mobile attacker, Mobile target)
        {
            if (target == null || target.Deleted || attacker == null || attacker.Deleted)
                return;

            attacker.MoveToWorld(target.Location, target.Map);
            target.SendMessage("You are charged at, engulfed in flames!");
            target.Damage(Utility.RandomMinMax(20, 30), attacker);
            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);
            target.SendMessage("You are set ablaze!");

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() =>
            {
                if (target != null && !target.Deleted)
                    target.SendMessage("The flames fade away.");
            }));
        }
    }
}

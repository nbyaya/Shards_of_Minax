using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDreamDust : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(20); // Cooldown for DreamDust
        private DateTime m_NextDreamDust;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlDreamDust(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDreamDust() { }

        [Attachable]
        public XmlDreamDust(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextDreamDust);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextDreamDust = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Dreamy Dust Attack: Confuses and freezes nearby targets.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextDreamDust)
            {
                PerformDreamDustAttack(attacker);
                m_NextDreamDust = DateTime.UtcNow + m_Cooldown;
            }
        }

        public void PerformDreamDustAttack(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A cloud of dreamy dust scatters around! *");
            Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 1154);

            List<Mobile> mobilesList = new List<Mobile>();
            foreach (Mobile target in owner.GetMobilesInRange(3))
            {
                if (target != owner && target.Alive)
                {
                    mobilesList.Add(target);
                }
            }

            foreach (Mobile target in mobilesList)
            {
                target.SendMessage("You feel drowsy and confused as the dreamy dust settles over you.");
                target.Freeze(TimeSpan.FromSeconds(5));
                if (Utility.RandomBool())
                {
                    target.SendMessage("You attack your allies in your confusion!");
                    target.Hits -= Utility.RandomMinMax(10, 20);

                    Mobile randomMobile = mobilesList[Utility.Random(mobilesList.Count)];
                    if (randomMobile != owner && randomMobile != target.Combatant)
                    {
                        target.Combatant = randomMobile;
                    }
                }
            }
        }
    }
}

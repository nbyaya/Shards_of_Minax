using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDisguise : XmlAttachment
    {
        private Timer m_DisguiseTimer;

        [Attachable]
        public XmlDisguise() { }

        // ✅ Correct constructor for XmlAttachment
        public XmlDisguise(ASerial serial) : base(serial)
        {
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (attacker != null && m_DisguiseTimer == null)
            {
                m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(15), () => Disguise(attacker));
            }
        }

        private void Disguise(Mobile owner)
        {
            if (owner == null || owner.Deleted)
                return;

            owner.BodyMod = 0x191;
            owner.Name = NameList.RandomName("female");
            owner.Hue = Utility.RandomSkinHue();

            m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(300), () => RemoveDisguise(owner));
        }

        private void RemoveDisguise(Mobile owner)
        {
            if (owner == null || owner.Deleted)
                return;

            owner.BodyMod = 0;
            owner.Name = null;
            m_DisguiseTimer = null;
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

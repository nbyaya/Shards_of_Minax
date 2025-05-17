using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone priest's remains")]
    public class AnointedBonePriest : SkeletalMage
    {
        [Constructable]
        public AnointedBonePriest() : base()
        {
            Name = "an Anointed Bone Priest";
            Hue = 1157; // Pale blue hue for ghostly divine vibe

            SetStr(110, 125);
            SetDex(75, 90);
            SetInt(220, 250);

            SetHits(130, 160);
            SetDamage(8, 14);

            SetSkill(SkillName.Necromancy, 100.0, 110.0);
            SetSkill(SkillName.SpiritSpeak, 100.0, 110.0);
            SetSkill(SkillName.Magery, 80.0, 95.0);
            SetSkill(SkillName.MagicResist, 75.0, 90.0);

            VirtualArmor = 45;

            AddItem(new CeremonialAnkh());

            Fame = 5000;
            Karma = -5000;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Utility.RandomDouble() < 0.05)
            {
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, false, "*chants in a forgotten tongue*");
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5022);
            }
        }

        public AnointedBonePriest(Serial serial) : base(serial)
        {
        }

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

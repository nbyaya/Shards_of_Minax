using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a spectral sentry corpse")]
    public class SpectralSentry : ShadowDweller
    {
        [Constructable]
        public SpectralSentry() : base()
        {
            Name = "a spectral sentry";
            Hue = 1150; // Unique ghostly hue
            BaseSoundID = 0x482;

            SetStr(220, 240);
            SetDex(140, 160);
            SetInt(300, 320);

            SetHits(160, 180);
            SetDamage(28, 34);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.Magery, 85.0);
            SetSkill(SkillName.MagicResist, 95.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 85.0);

            PackItem(new NecromancerSpellbook());


        }

        public SpectralSentry(Serial serial) : base(serial) { }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 2);
            if (Utility.RandomDouble() < 0.02) // 2% drop chance
                PackItem(new FracturedOathstone());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

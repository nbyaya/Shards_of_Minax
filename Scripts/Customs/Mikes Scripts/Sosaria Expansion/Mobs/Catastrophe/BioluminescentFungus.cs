using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a glowing fungus corpse")]
    public class BioluminescentFungus : PlagueBeast
    {
        [Constructable]
        public BioluminescentFungus() : base()
        {
            Name = "a bioluminescent fungus";
            Hue = 1278; // Glowing cyan-green
            Fame = 6000;
            Karma = -6000;

            SetHits(400, 500);
            SetDamage(18, 24);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 95.0);

            if (Utility.RandomDouble() < 0.75)
                PackItem(new GlowsporeClump());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override Poison PoisonImmune => Poison.Deadly;

        public BioluminescentFungus(Serial serial) : base(serial) { }

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

using System;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the wave overlord")]
    public class SurferSummonerBoss : SurferSummoner
    {
        [Constructable]
        public SurferSummonerBoss() : base()
        {
            Name = "Wave Overlord";
            Title = "the Supreme Surfer";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Matching Barracoon's upper dexterity
            SetInt(250); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the XmlRandomAbility
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can go here, such as special behaviors or spells
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public SurferSummonerBoss(Serial serial) : base(serial)
        {
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

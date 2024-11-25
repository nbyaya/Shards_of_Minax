using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the necro overlord")]
    public class NecroSummonerBoss : NecroSummoner
    {
        [Constructable]
        public NecroSummonerBoss() : base()
        {
            Name = "Necro Overlord";
            Title = "the Supreme Summoner";

            // Enhance stats to match or exceed Barracoon's stats where possible
            SetStr(700, 900); // Increase Strength
            SetDex(150, 200); // Increase Dexterity
            SetInt(400, 600); // Increase Intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhance skills
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 70.0, 80.0);
            SetSkill(SkillName.MagicResist, 95.0, 115.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Negative karma for a dark boss

            VirtualArmor = 70; // Increased armor for the boss

            // Attach the XmlRandomAbility for additional dynamic behavior
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            
            // Boss-specific behavior can be added here, such as special summon logic or more frequent attacks
        }

        public NecroSummonerBoss(Serial serial) : base(serial)
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

using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sawmill overlord")]
    public class SawmillWorkerBoss : SawmillWorker
    {
        [Constructable]
        public SawmillWorkerBoss() : base()
        {
            Name = "Sawmill Overlord";
            Title = "the Supreme Lumberjack";

            // Update stats to match or exceed Barracoon's values where appropriate
            SetStr(900, 1200); // Increase strength
            SetDex(250, 300); // Increase dexterity
            SetInt(150, 200); // Increase intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 80); // Improved resistances
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 100.0); // High skill values
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Lumberjacking, 120.0);

            Fame = 22500; // Higher fame for boss status
            Karma = -22500; // Boss has negative karma

            VirtualArmor = 70; // Increase virtual armor

            // Attach a random ability to this boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
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

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here
        }

        public SawmillWorkerBoss(Serial serial) : base(serial)
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

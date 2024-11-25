using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an elder tendril corpse")]
    public class ElderTendrilBoss : ElderTendril
    {
        [Constructable]
        public ElderTendrilBoss() : base("Elder Tendril Overlord")
        {
            // Boss-tier stats based on Barracoon
            SetStr(425); // Upper bound of Barracoon's strength
            SetDex(150); // Upper bound of Barracoon's dexterity
            SetInt(750); // Upper bound of Barracoon's intelligence

            SetHits(12000); // Higher health to make it more formidable
            SetDamage(29, 38); // Consistent with Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0); // Solid resist for magic
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for boss level
            SetSkill(SkillName.Wrestling, 120.0); // Boss-level wrestling skill

            Fame = 22500; // Higher fame value
            Karma = -22500; // Boss enemies should have negative karma

            VirtualArmor = 90; // Increase virtual armor for tanking damage

            // Attach the random ability that will apply additional dynamic abilities to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            PackItem(new BossTreasureBox());
            base.GenerateLoot();
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Optionally, you could add more advanced logic here for the boss's AI
        }

        public ElderTendrilBoss(Serial serial) : base(serial)
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

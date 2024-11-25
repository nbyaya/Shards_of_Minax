using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a jester's corpse")]
    public class JesterBoss : Jester
    {
        [Constructable]
        public JesterBoss() : base()
        {
            Name = "Jester Overlord";
            Title = "the Mad Jester";

            // Update stats to match or exceed Barracoon or a boss-tier level
            SetStr(1200); // Exceeds the original stats, making the jester more powerful
            SetDex(255); // Maxed out dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Increased health to match a boss-tier entity
            SetDamage(30, 40); // Higher damage output

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for combat strategy
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skills for melee

            Fame = 22500; // Higher fame to match a boss-tier NPC
            Karma = -22500; // Boss-tier reputation as a villain

            VirtualArmor = 80; // Enhanced virtual armor for more protection

            // Attach the XmlRandomAbility for additional dynamic abilities
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
            // Additional boss logic could be added here
        }

        public JesterBoss(Serial serial) : base(serial)
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

using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the knight of mercy overlord")]
    public class KnightOfMercyBoss : KnightOfMercy
    {
        [Constructable]
        public KnightOfMercyBoss() : base()
        {
            Name = "Knight of Mercy Overlord";
            Title = "the Eternal Healer";

            // Update stats to match or exceed Barracoon (if not already superior)
            SetStr(1200); // Improved strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health for a boss-level encounter

            SetDamage(25, 35); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000; // Increase fame to reflect the power of a boss
            Karma = 25000; // Keep karma high (or set it negative for a dark twist)

            VirtualArmor = 80; // Increased virtual armor for more resilience

            // Attach a random ability for more unpredictability
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

        public KnightOfMercyBoss(Serial serial) : base(serial)
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

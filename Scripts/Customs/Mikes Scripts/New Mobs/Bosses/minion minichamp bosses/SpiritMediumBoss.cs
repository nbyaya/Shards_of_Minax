using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the spirit overlord")]
    public class SpiritMediumBoss : SpiritMedium
    {
        [Constructable]
        public SpiritMediumBoss() : base()
        {
            Name = "Spirit Overlord";
            Title = "the Eternal Medium";

            // Update stats to match or exceed Barracoon-like boss
            SetStr(400, 600); // Higher strength than original
            SetDex(200, 250); // Enhanced dexterity
            SetInt(400, 600); // Enhanced intelligence

            SetHits(5000, 7000); // Much higher health to match a boss-level enemy

            SetDamage(15, 30); // Stronger damage range

            // Update resistances for a stronger encounter
            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.MagicResist, 120.0, 150.0); // Stronger magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Stronger combat tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Stronger wrestling skill
            SetSkill(SkillName.Magery, 120.0, 150.0); // Increased magery for greater spells

            Fame = 22500;
            Karma = -22500; // Evil boss-tier karma

            VirtualArmor = 60; // More virtual armor to tank hits

            // Attach a random ability (like in the previous example)
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
            // Additional boss-specific logic can be added here if needed
        }

        public SpiritMediumBoss(Serial serial) : base(serial)
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

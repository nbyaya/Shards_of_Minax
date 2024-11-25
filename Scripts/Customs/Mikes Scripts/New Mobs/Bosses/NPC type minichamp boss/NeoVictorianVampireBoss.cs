using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a corpse of the Neo-Victorian Overlord")]
    public class NeoVictorianVampireBoss : NeoVictorianVampire
    {
        [Constructable]
        public NeoVictorianVampireBoss() : base()
        {
            Name = "Neo-Victorian Overlord";
            Title = "the Supreme Machine-Feeder";

            // Update stats to match or exceed Barracoon's stats or superior if possible
            SetStr(1200); // Higher than original NeoVictorianVampire
            SetDex(255); // Max dexterity, boss-tier
            SetInt(250); // High intelligence for a powerful mage

            SetHits(12000); // High health matching Barracoon
            SetDamage(29, 38); // Matching Barracoon's damage range for a tough fight

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Same as original
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // High virtual armor to make it harder to hit

            // Attach a random ability
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
            // Additional boss logic could be added here (e.g., special abilities, random speech, etc.)
        }

        public NeoVictorianVampireBoss(Serial serial) : base(serial)
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

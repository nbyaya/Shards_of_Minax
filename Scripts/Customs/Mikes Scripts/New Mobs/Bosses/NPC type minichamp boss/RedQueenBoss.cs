using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Red Queen, Supreme")]
    public class RedQueenBoss : RedQueen
    {
        [Constructable]
        public RedQueenBoss() : base()
        {
            Name = "Red Queen, Supreme";
            Title = "the Overlord of the Abyss";

            // Update stats to match or exceed Barracoon's or superior values
            SetStr(1200); // Max strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(30, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Maximum magic resist skill
            SetSkill(SkillName.Tactics, 120.0); // Boss-tier tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 22500; // Increased fame for a boss-level encounter
            Karma = -22500; // Negative karma

            VirtualArmor = 70; // Enhanced armor

            // Attach the XmlRandomAbility for dynamic behavior
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

            // Implement boss-specific behavior here (e.g., summoning succubus)
        }

        public RedQueenBoss(Serial serial) : base(serial)
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

using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the wasteland biker overlord")]
    public class WastelandBikerBoss : WastelandBiker
    {
        [Constructable]
        public WastelandBikerBoss() : base()
        {
            Name = "Wasteland Biker Overlord";
            Title = "the Wasteland King";

            // Update stats to match or exceed the boss level
            SetStr(1200); // Enhanced Strength
            SetDex(255);  // Max Dexterity for high speed
            SetInt(250);  // Enhanced Intelligence

            SetHits(12000); // Enhanced Health to match the boss theme

            SetDamage(30, 45); // Higher damage output to reflect the boss's strength

            SetResistance(ResistanceType.Physical, 75, 85); // Stronger resistance
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 12000; // Increased Fame for boss-tier recognition
            Karma = -12000; // Negative Karma to reflect its villainous nature

            VirtualArmor = 75; // Enhanced armor for better survivability

            // Attach random ability
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

            // Add special loot phrases
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The wasteland... will consume you!"); break;
                case 1: this.Say(true, "You're no match for the king of the wasteland!"); break;
            }
        }

        public WastelandBikerBoss(Serial serial) : base(serial)
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

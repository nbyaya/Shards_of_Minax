using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shadowed archer")]
    public class ScoutArcherBoss : ScoutArcher
    {
        [Constructable]
        public ScoutArcherBoss() : base()
        {
            Name = "Shadowed Archer";
            Title = "the Silent Striker";

            // Update stats to match or exceed Barracoon (or enhanced stats)
            SetStr(600); // Higher strength for the boss
            SetDex(400); // Increased dexterity for the boss
            SetInt(200); // Slightly better intelligence

            SetHits(5000); // Boss health
            SetDamage(40, 60); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Higher resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced skill range
            SetSkill(SkillName.Archery, 120.0, 130.0); // Enhanced Archery skill
            SetSkill(SkillName.Hiding, 100.0, 120.0); // Enhanced Stealth/Hiding skills
            SetSkill(SkillName.Stealth, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0); // Enhanced Tactics skill

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60; // More virtual armor for better defense

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

            // Add some additional flavor for the boss' speech
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The shadows... they consume me..."); break;
                case 1: this.Say(true, "You won't find peace..."); break;
            }

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss behavior could be implemented here
        }

        public ScoutArcherBoss(Serial serial) : base(serial)
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

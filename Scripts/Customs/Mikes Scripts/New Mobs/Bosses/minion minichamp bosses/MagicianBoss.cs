using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the grand magician")]
    public class MagicianBoss : Magician
    {
        [Constructable]
        public MagicianBoss() : base()
        {
            Name = "Grand Magician";
            Title = "the Supreme Sorcerer";

            // Update stats to match or exceed Barracoon's or better
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(200); // Enhanced dexterity for faster spellcasting and mobility
            SetInt(750); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(29, 38); // Matching Barracoon's damage range

            // Update resistances
            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 75, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resist
            SetSkill(SkillName.EvalInt, 120.0); // Enhanced magic evaluation
            SetSkill(SkillName.Magery, 120.0); // Enhanced magery skill
            SetSkill(SkillName.Meditation, 100.0); // High meditation skill for regeneration
            SetSkill(SkillName.Tactics, 80.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 70.0); // Basic wrestling skill

            Fame = 22500; // High fame for a boss-tier NPC
            Karma = -22500; // Negative karma, fitting for a boss

            VirtualArmor = 60; // Better armor for boss-tier durability

            // Attach the XmlRandomAbility to add an extra random ability
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

            // Add extra custom dialogue for the boss
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My magic will shatter you!"); break;
                case 1: this.Say(true, "You are but an insect before my power!"); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, such as special abilities or phases
        }

        public MagicianBoss(Serial serial) : base(serial)
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

using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the infiltrator lord")]
    public class InfiltratorBoss : Infiltrator
    {
        [Constructable]
        public InfiltratorBoss() : base()
        {
            Name = "Infiltrator Lord";
            Title = "the Shadowmaster";

            // Update stats to match or exceed Barracoon
            SetStr(700); // Higher strength for the boss version
            SetDex(400); // Higher dexterity for better speed and evasion
            SetInt(200); // Higher intelligence for more magic potential

            SetHits(12000); // Matching Barracoon's health for a boss-tier NPC
            SetDamage(29, 38); // Matching Barracoon's damage for balance

            // Resistances enhanced for boss-tier difficulty
            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Skills enhanced for difficulty
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 70.0, 90.0);
            SetSkill(SkillName.Hiding, 120.0, 140.0);
            SetSkill(SkillName.Stealth, 120.0, 140.0);
            SetSkill(SkillName.Snooping, 120.0, 140.0);
            SetSkill(SkillName.Fencing, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 70;

            // Attach a random ability for added unpredictability
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

            // Additional loot logic for the boss
            PackGold(500, 700); // Increased gold drop
            AddLoot(LootPack.Rich);

            // Say something dramatic upon defeat
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "You cannot win against the shadows!"); break;
                case 1: this.Say(true, "I am the darkness..."); break;
            }

            // Drop extra items
            PackItem(new Nightshade(Utility.RandomMinMax(10, 30))); // More nightshade for theme
        }

        public InfiltratorBoss(Serial serial) : base(serial)
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

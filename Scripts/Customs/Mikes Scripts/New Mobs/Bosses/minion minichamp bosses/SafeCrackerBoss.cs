using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master safe cracker")]
    public class SafeCrackerBoss : SafeCracker
    {
        [Constructable]
        public SafeCrackerBoss() : base()
        {
            Name = "Master Safe Cracker";
            Title = "the Grandmaster";

            // Update stats to match or exceed a boss-tier level like Barracoon
            SetStr(600, 850); // Enhanced strength for boss level
            SetDex(300, 400); // Enhanced dexterity
            SetInt(150, 250); // Enhanced intelligence

            SetHits(1500, 2500); // Much higher hit points
            SetStam(500); // Higher stamina
            SetMana(250); // Enhanced mana

            SetDamage(20, 35); // Enhanced damage range

            // Enhance resistances to match or exceed Barracoon
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Lockpicking, 100.1, 120.0); // Increase lockpicking skill for boss
            SetSkill(SkillName.Stealing, 100.1, 120.0);
            SetSkill(SkillName.Stealth, 100.1, 120.0);
            SetSkill(SkillName.Hiding, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 100.1, 120.0); // Increase magic resist for a stronger fight
            SetSkill(SkillName.Tactics, 120.1, 130.0); // Increase tactics for better combat efficiency
            SetSkill(SkillName.Wrestling, 100.1, 120.0); // Increase wrestling skill

            Fame = 12000; // Increase fame for boss level
            Karma = -12000; // Higher negative karma, typical for a boss

            VirtualArmor = 80; // Increased virtual armor

            // Attach a random ability to the boss
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

            // Add additional boss loot here if needed
            PackGold(500, 1000);
            AddLoot(LootPack.Rich); // Rich loot for a boss encounter
            PackItem(new Lockpick(Utility.RandomMinMax(20, 30))); // More lockpicks for the theme
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss behavior could be enhanced further here (e.g., special speech or combat actions)
        }

        public SafeCrackerBoss(Serial serial) : base(serial)
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

using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master pickpocket")]
    public class PickpocketBoss : Pickpocket
    {
        [Constructable]
        public PickpocketBoss() : base()
        {
            Name = "Master Pickpocket";
            Title = "the Unseen Thief";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(425); // Increased strength to match boss-tier NPC
            SetDex(500); // Increased dexterity for agility and speed
            SetInt(250); // Increased intelligence for better abilities

            SetHits(8000); // Much higher health than normal pickpocket
            SetDamage(20, 40); // Increased damage for a tougher fight

            // Increase resistance to match a boss-level difficulty
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Stealing, 100.0); // Perfect Stealing skill
            SetSkill(SkillName.Snooping, 100.0); // Perfect Snooping skill
            SetSkill(SkillName.Hiding, 100.0); // Maxed Hiding skill
            SetSkill(SkillName.Stealth, 100.0); // Maxed Stealth skill
            SetSkill(SkillName.Anatomy, 100.0); // For high damage
            SetSkill(SkillName.Tactics, 100.0); // Tactics for combat
            SetSkill(SkillName.Fencing, 100.0); // Fencing for melee damage

            Fame = 15000; // Increased fame value for a boss-tier creature
            Karma = -15000; // Negative karma, as it's a villain

            VirtualArmor = 50; // Increased armor to make it more resistant

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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add custom behavior or extra logic here if needed
        }

        public PickpocketBoss(Serial serial) : base(serial)
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

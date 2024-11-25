using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the spear fisher lord")]
    public class SpearFisherBoss : SpearFisher
    {
        [Constructable]
        public SpearFisherBoss() : base()
        {
            Name = "Spear Fisher Lord";
            Title = "the Supreme Hunter";

            // Enhanced stats for the boss
            SetStr(900, 1200); // Higher strength for boss tier
            SetDex(200, 250); // Higher dexterity for faster movement
            SetInt(150, 200); // Higher intelligence for magic resistance

            SetHits(12000); // Higher health for boss tier
            SetDamage(30, 45); // Higher damage for boss tier

            SetResistance(ResistanceType.Physical, 70, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 50, 70); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 50, 70); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 60, 80); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 40, 60); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 80.0, 100.0); // Enhanced anatomy skill
            SetSkill(SkillName.Archery, 100.0, 120.0); // Enhanced archery skill
            SetSkill(SkillName.MagicResist, 90.0, 110.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Enhanced tactics skill

            Fame = 22500; // Increased fame for boss tier
            Karma = -22500; // Negative karma for a villainous boss

            VirtualArmor = 70; // Enhanced armor for boss tier

            // Attach random ability to the boss NPC
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
            // Additional boss logic could be added here if needed
        }

        public SpearFisherBoss(Serial serial) : base(serial)
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

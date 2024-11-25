using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the scout ninja overlord")]
    public class ScoutNinjaBoss : ScoutNinja
    {
        [Constructable]
        public ScoutNinjaBoss() : base()
        {
            Name = "Scout Ninja Overlord";
            Title = "the Master of Shadows";

            // Update stats to match or exceed Barracoon and increase overall power
            SetStr(425); // Stronger than the original Scout Ninja
            SetDex(350); // Enhanced dexterity for better speed and skill
            SetInt(200); // Slight increase to intelligence for improved tactics and magic resist

            SetHits(12000); // Increased health for the boss-tier experience

            SetDamage(29, 38); // Higher damage range for more impactful attacks

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 70, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 80.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 90.0, 110.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Ninjitsu, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 16000; // Enhanced fame for a boss
            Karma = -16000; // Boss-tier karma

            VirtualArmor = 60; // Higher armor to make the fight more challenging

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
            // Additional boss logic could be added here, such as more aggressive behavior or summoning allies
        }

        public ScoutNinjaBoss(Serial serial) : base(serial)
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

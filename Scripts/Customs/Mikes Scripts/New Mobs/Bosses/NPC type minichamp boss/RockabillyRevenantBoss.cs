using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the rockabilly overlord")]
    public class RockabillyRevenantBoss : RockabillyRevenant
    {
        [Constructable]
        public RockabillyRevenantBoss() : base()
        {
            Name = "Rockabilly Overlord";
            Title = "the Haunted Idol";

            // Update stats to match or exceed Barracoon (or superior where possible)
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(10000); // Increased health to make it more boss-like

            SetDamage(20, 35); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Unchanged, as it was already high
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 120.0); // Enhanced skills for tougher fights
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 60.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 15000; // Increased fame for boss status
            Karma = -15000; // Negative karma, fitting for a boss

            VirtualArmor = 75; // Higher virtual armor for increased survivability

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
            // Additional boss logic can be added here if needed
        }

        public RockabillyRevenantBoss(Serial serial) : base(serial)
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

using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sword overlord")]
    public class SwordDefenderBoss : SwordDefender
    {
        [Constructable]
        public SwordDefenderBoss() : base()
        {
            Name = "Sword Overlord";
            Title = "the Supreme Defender";

            // Update stats to match or exceed Barracoon (if not already better)
            SetStr(1000); // Matching or surpassing Barracoon's upper strength
            SetDex(200); // Matching Barracoon's upper dexterity
            SetInt(150); // Keeping a reasonable intelligence for balance

            SetHits(8000); // Increase health for a boss-tier creature
            SetDamage(25, 35); // Slightly higher damage

            SetResistance(ResistanceType.Physical, 75, 85); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 60, 70); // Increase fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Increase cold resistance
            SetResistance(ResistanceType.Poison, 50, 60); // Increase poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Increase energy resistance

            SetSkill(SkillName.Swords, 120.0, 150.0); // Higher sword skill for a boss
            SetSkill(SkillName.Parry, 120.0, 150.0); // Higher parry skill for a boss
            SetSkill(SkillName.Tactics, 100.0, 130.0); // Higher tactics skill
            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increased anatomy skill for better damage output

            Fame = 22500; // Increase fame to match a boss
            Karma = -22500; // Negative karma for a boss

            VirtualArmor = 75; // Higher armor value to make it tankier

            // Attach the random ability
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

        public SwordDefenderBoss(Serial serial) : base(serial)
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

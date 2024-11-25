using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an Ish'Kar the Forgotten corpse")]
    public class IshKarTheForgottenBoss : IshKarTheForgotten
    {
        [Constructable]
        public IshKarTheForgottenBoss() : base()
        {
            // Update to a stronger version with Barracoon-like stats
            Name = "Ish'Kar the Forgotten";
            Title = "the Supreme Sorcerer";
            Hue = 0x482; // Unique hue for Ish'Kar
            BaseSoundID = 377;

            // Enhanced Stats
            SetStr(1200); // Higher strength to make it more powerful
            SetDex(255);  // Maxed dexterity to improve agility
            SetInt(250);  // Maxed intelligence for magic power

            SetHits(12000); // High health like Barracoon
            SetDamage(35, 50); // Increased damage range

            // Damage Types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Increased resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            // Skills
            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 24000;
            Karma = -24000;
            VirtualArmor = 90;

            PackItem(new BossTreasureBox());
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
            // Any additional logic for the boss can go here
        }

        public IshKarTheForgottenBoss(Serial serial) : base(serial)
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

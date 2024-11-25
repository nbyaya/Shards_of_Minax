using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an abyssal warden corpse")]
    public class AbyssalWardenBoss : AbyssalWarden
    {
        [Constructable]
        public AbyssalWardenBoss() : base()
        {
            Name = "Abyssal Warden";
            Title = "the Supreme Warden";
            Body = 9; // Daemon body
            Hue = 1473; // Unique hue for Abyssal Warden
            BaseSoundID = 357;

            // Enhance stats to match or exceed Barracoon or other boss-tier stats
            SetStr(1200); // Upper end of the original strength
            SetDex(255); // Upper end of the original dexterity
            SetInt(250); // Upper end of the original intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(35, 45); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30); // Increased fire damage
            SetDamageType(ResistanceType.Energy, 30); // Increased energy damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false; // Bosses are not tamable
            ControlSlots = 3; // Not needed for non-tamable bosses, but added for consistency
            MinTameSkill = 100.0; // Not applicable but still useful for reference

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
            // Additional boss-specific logic could be added here if needed
        }

        public AbyssalWardenBoss(Serial serial) : base(serial)
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

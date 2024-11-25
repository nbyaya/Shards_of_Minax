using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the vaudeville valkyrie overlord")]
    public class VaudevilleValkyrieBoss : VaudevilleValkyrie
    {
        [Constructable]
        public VaudevilleValkyrieBoss() : base()
        {
            Name = "Vaudeville Valkyrie Overlord";
            Title = "the Supreme Performer";

            // Enhanced stats to match or exceed Barracoon
            SetStr(1200); // Maximum strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Increased health to match a boss
            SetDamage(29, 38); // Enhanced damage range

            // Improved resistances for a boss
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Enhanced skill levels for the boss
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.Archery, 100.0);
            SetSkill(SkillName.Swords, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 75;

            // Attach random abilities for additional complexity
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
            // You can add additional boss-specific logic if necessary
        }

        public VaudevilleValkyrieBoss(Serial serial) : base(serial)
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

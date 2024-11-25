using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the retro android overlord")]
    public class RetroAndroidBoss : RetroAndroid
    {
        [Constructable]
        public RetroAndroidBoss() : base()
        {
            Name = "Retro Android Overlord";
            Title = "the Supreme Overlord";

            // Update stats to match or exceed Barracoon's level
            SetStr(1200); // Much higher strength for a boss
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Much higher health for a boss

            SetDamage(25, 40); // Stronger damage range

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0); // Stronger tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Stronger wrestling skill
            SetSkill(SkillName.Archery, 120.0); // Boss-level archery

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the XmlRandomAbility
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
            // Boss-specific behaviors (additional logic could be added here)
        }

        public RetroAndroidBoss(Serial serial) : base(serial)
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

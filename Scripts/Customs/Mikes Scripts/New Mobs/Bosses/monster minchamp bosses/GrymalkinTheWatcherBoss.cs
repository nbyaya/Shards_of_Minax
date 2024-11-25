using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a gryfalkin corpse")]
    public class GrymalkinTheWatcherBoss : GrymalkinTheWatcher
    {
        [Constructable]
        public GrymalkinTheWatcherBoss()
            : base()
        {
            Name = "Grymalkin the Supreme Watcher";
            Title = "the Omniscient One";
            Hue = 1670; // Slightly adjusted hue to make it visually distinct as a boss

            // Update stats to match or exceed Barracoon-like levels
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Higher health
            SetDamage(35, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame for the boss tier
            Karma = -30000; // Increased karma for the boss tier

            VirtualArmor = 120; // Increased virtual armor for better survivability

            Tamable = false; // Not tamable as this is a boss
            ControlSlots = 0; // Boss NPC should not be controlled

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

            // Optionally add other special boss loot or enhancements here
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss-specific abilities (like advanced ability cooldowns or extra mechanics) can go here
        }

        public GrymalkinTheWatcherBoss(Serial serial)
            : base(serial)
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

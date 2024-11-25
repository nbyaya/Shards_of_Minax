using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a poisonous crab corpse")]
    public class PoisonousCrabBoss : PoisonousCrab
    {
        [Constructable]
        public PoisonousCrabBoss() : base()
        {
            Name = "Toxic Overlord";
            Title = "the Venomous Monarch";
            Hue = 1400; // Green poisonous hue, can adjust if needed
            BaseSoundID = 0x4F2; // Original sound ID for poison effects

            // Update stats to match or exceed boss standards
            SetStr(1200); // Increased strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // High intelligence for magic resistance

            SetHits(12000);  // High health for a boss
            SetDamage(35, 45);  // Higher damage range

            // Keep resistances for a tough fight
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;  // Higher fame for the boss
            Karma = -30000;  // Negative karma for the boss

            VirtualArmor = 100;  // Tougher defense

            // Attach random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            Tamable = false;
            ControlSlots = 3; // Not tamable, similar to other bosses
            MinTameSkill = 0;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional rich loot for the boss
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 10);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public PoisonousCrabBoss(Serial serial) : base(serial)
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

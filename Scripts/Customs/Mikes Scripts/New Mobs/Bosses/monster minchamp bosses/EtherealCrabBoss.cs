using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an ethereal crab boss corpse")]
    public class EtherealCrabBoss : EtherealCrab
    {
        [Constructable]
        public EtherealCrabBoss() : base()
        {
            Name = "Ethereal Crab Boss";
            Title = "the Phantom Overlord";

            // Upgrade stats to match or exceed boss-level creature stats (e.g., Barracoon's stats)
            SetStr(1200); // Set strength higher for boss
            SetDex(255); // Max dexterity
            SetInt(250); // Set intelligence

            SetHits(12000); // High health
            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false; // Bosses are typically untamable
            ControlSlots = 0;

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

            // Add additional loot if necessary (could include rare items or increased gold)
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific behavior can go here
        }

        public EtherealCrabBoss(Serial serial) : base(serial)
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

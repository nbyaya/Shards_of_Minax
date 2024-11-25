using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the soulful boss mermaid")]
    public class MotownMermaidBoss : MotownMermaid
    {
        [Constructable]
        public MotownMermaidBoss() : base()
        {
            Name = "Soulful Boss Mermaid";
            Title = "the Supreme Siren";

            // Enhance stats to match or exceed Barracoon's levels
            SetStr(1200); // Stronger than original
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // High health for a boss-level NPC
            SetDamage(29, 38); // Match Barracoon's damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 75);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 22500; // Boss fame
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 70; // Stronger armor

            // Attach random ability
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

            // Add additional loot for the boss
            PackGold(500, 1000);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);
        }

        public MotownMermaidBoss(Serial serial) : base(serial)
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

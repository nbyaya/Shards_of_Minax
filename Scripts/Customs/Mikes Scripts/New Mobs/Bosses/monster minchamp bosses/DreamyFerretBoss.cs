using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the dreamy ferret overlord")]
    public class DreamyFerretBoss : DreamyFerret
    {
        [Constructable]
        public DreamyFerretBoss() : base()
        {
            Name = "Dreamy Ferret Overlord";
            Title = "the Dream Weaver";

            // Update stats to match or exceed the original Dreamy Ferret
            SetStr(1200); // Higher strength than the original
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Much higher hit points
            SetDamage(35, 45); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 75, 85); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 120.0); // Increased skill
            SetSkill(SkillName.Magery, 120.0);  // Increased skill
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 35000; // Increased fame
            Karma = -35000; // Negative karma

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // No longer tamable as a boss
            ControlSlots = 0; // No control slots


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

            // Add additional high-tier loot
            this.AddLoot(LootPack.FilthyRich, 3);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public DreamyFerretBoss(Serial serial) : base(serial)
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

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss dall sheep corpse")]
    public class BossDallSheep : DallSheep
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BossDallSheep() : base()
        {
            Name = "Dall Sheep";
            Title = "the Frost Titan";
            Hue = 0x497; // Unique hue for the boss
            Body = 0xD1; // Keep the same body type
            BaseSoundID = 0x99; // Keep the same sound

            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(300); // Increased intelligence

            SetHits(15000); // Increased hit points for boss
            SetDamage(40, 55); // Higher damage range for a boss

            SetResistance(ResistanceType.Physical, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Increased skill for resisting magic
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill
            SetSkill(SkillName.Magery, 100.0); // Magery skill for extra abilities

            Fame = 25000; // Boss-level fame
            Karma = -25000; // Boss-level karma

            VirtualArmor = 100; // Increased virtual armor for the boss

            Tamable = false;
            ControlSlots = 3;


            // Attach the XmlRandomAbility for added randomness
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drops 5 MaxxiaScroll items
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities

            // Custom abilities for the boss go here (same as original)
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Base loot is included
            this.Say("You will be buried beneath the snow!");
            PackGold(1500, 2500); // Increased gold drops for boss
            AddLoot(LootPack.FilthyRich, 3); // More loot for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // Increased ingot drop for a boss fight
        }

        public BossDallSheep(Serial serial) : base(serial)
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

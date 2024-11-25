using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("boss eclipse reindeer corpse")]
    public class BossEclipseReindeer : EclipseReindeer
    {
        [Constructable]
        public BossEclipseReindeer()
        {
            Name = "Eclipse Reindeer";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for the boss variant

            SetStr(1200); // Higher strength than the original
            SetDex(255); // Max dexterity for speed
            SetInt(250); // Increased intelligence

            SetHits(15000); // Higher health for the boss
            SetDamage(35, 45); // Higher damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics
            SetSkill(SkillName.Wrestling, 120.0); // Improved wrestling
            SetSkill(SkillName.Magery, 110.0); // Higher magery for the boss

            Fame = 24000; // Same fame level for consistency
            Karma = -24000; // Same karma level for consistency

            VirtualArmor = 90; // Consistent with the original

            Tamable = false; // Boss is not tamable
            ControlSlots = 0; // Not controllable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls in addition to other loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("I will eclipse you!");
            PackGold(2000, 3000); // Increased gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities
        }

        public BossEclipseReindeer(Serial serial) : base(serial)
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

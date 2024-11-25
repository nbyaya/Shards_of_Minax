using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss mandrill shaman corpse")]
    public class BossMandrillShaman : MandrillShaman
    {
        [Constructable]
        public BossMandrillShaman()
        {
            Name = "Mandrill Shaman";
            Title = "the Mystic Overlord";
            Hue = 0x497; // Unique hue for a boss
            Body = 0x1D; // Default body for the Mandrill Shaman

            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300);   // Enhanced dexterity
            SetInt(250, 350);   // Enhanced intelligence

            SetHits(15000);     // Increased health for the boss
            SetDamage(40, 55);  // Enhanced damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // Enhanced magery
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Negative karma for boss
            VirtualArmor = 100; // Increased virtual armor

            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0; // Boss is untamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add 5 MaxxiaScroll drops
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You face the fury of the mystic forces!");
            PackGold(2000, 3000); // Increased gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // Increased ingot drops
            AddLoot(LootPack.FilthyRich, 2); // Rich loot for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossMandrillShaman(Serial serial) : base(serial)
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

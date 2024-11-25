using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss ibex corpse")]
    public class BossIbex : Ibex
    {
        [Constructable]
        public BossIbex()
        {
            Name = "Ibex";
            Title = "the Mighty";
            Hue = 0x497; // Unique hue for the boss
            Body = 0xD1; // Same body as Ibex, can be changed if needed
            BaseSoundID = 0x99;

            // Enhanced stats for the boss version
            SetStr(1200, 1500);
            SetDex(255, 300);
            SetInt(250, 350);

            SetHits(12000, 15000); // Increased health for the boss

            SetDamage(40, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 32000; // Increased fame for boss
            Karma = -32000;

            VirtualArmor = 90; // Boss-level virtual armor

            Tamable = false; // Bosses are not tamable
            ControlSlots = 0;
            MinTameSkill = 0;

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain base loot
            this.Say("You dare challenge me, mortal!");
            PackGold(1000, 1500); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities like horn charge and evasions
        }

        public BossIbex(Serial serial) : base(serial)
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

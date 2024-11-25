using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a gibbon mystic corpse")]
    public class BossGibbonMystic : GibbonMystic
    {
        [Constructable]
        public BossGibbonMystic()
            : base() // Inherit everything from GibbonMystic
        {
            Name = "Boss Gibbon Mystic";
            Title = "the Ancient Mystic";
            Hue = 1964; // Unique hue for boss
            Body = 0x1D; // Gorilla body

            // Enhance stats
            SetStr(1200, 1500);
            SetDex(255);
            SetInt(250);

            SetHits(15000);
            SetDamage(40, 50);

            // Damage types and resistances
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 40000;
            Karma = -40000;

            VirtualArmor = 120;

            // Attach the XmlRandomAbility for extra random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Tamable but not recommended for bosses
            Tamable = false;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Keep base loot
            this.Say("You will fall before my mystic power!");
            PackGold(1000, 1500); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for a boss

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for ability calls
        }

        public BossGibbonMystic(Serial serial)
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

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a venomous roe corpse")]
    public class BossVenomousRoe : VenomousRoe
    {
        [Constructable]
        public BossVenomousRoe() : base()
        {
            Name = "The Venomous Roe";
            Title = "the Toxic Colossus";
            Hue = 0x497; // Unique boss hue

            SetStr(1200); // Increased strength for boss
            SetDex(255); // Maxed dexterity for boss
            SetInt(250); // Increased intelligence for boss

            SetHits(15000); // Increased health for boss

            SetDamage(40, 50); // Increased damage for boss

            SetResistance(ResistanceType.Physical, 75); // Increased physical resistance for boss
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance for boss
            SetResistance(ResistanceType.Cold, 60); // Increased cold resistance for boss
            SetResistance(ResistanceType.Poison, 80); // Increased poison resistance for boss
            SetResistance(ResistanceType.Energy, 60); // Increased energy resistance for boss

            SetSkill(SkillName.Anatomy, 60.0, 100.0); // Improved skill levels for boss
            SetSkill(SkillName.EvalInt, 100.0, 120.0); 
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0); 
            SetSkill(SkillName.Tactics, 100.0, 120.0); 
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 35000; // Increased fame for boss
            Karma = -35000; // Increased karma for boss

            VirtualArmor = 120; // Increased armor for boss

            Tamable = false;
            ControlSlots = 0; // Make sure it's untamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // 5 MaxxiaScrolls drop
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You cannot escape my venom!");
            PackGold(2000, 3000); // Enhanced gold drops for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // Boss-level item drop
            this.AddLoot(LootPack.FilthyRich, 2); // More loot for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for toxic aura, enrage, and summoning minions
        }

        public BossVenomousRoe(Serial serial) : base(serial)
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

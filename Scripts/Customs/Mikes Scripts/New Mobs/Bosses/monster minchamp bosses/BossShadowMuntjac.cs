using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow muntjac corpse")]
    public class BossShadowMuntjac : ShadowMuntjac
    {
        private DateTime m_NextShadowMeld;
        private DateTime m_NextSilentStrike;
        private DateTime m_NextShadowClone;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BossShadowMuntjac()
            : base()
        {
            Name = "Boss Shadow Muntjac";
            Title = "the Shadow Master";
            Hue = 1989; // Dark hue to blend into shadows, can be customized for a boss
            Body = 0xEA; // GreatHart body, can be randomized if needed

            SetStr(1200); // Boss-level strength
            SetDex(255);  // Maxed dexterity for fast actions
            SetInt(250);  // Higher intelligence for tougher abilities

            SetHits(15000); // Boss-level health
            SetDamage(35, 45); // Higher damage output

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 75);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000;

            VirtualArmor = 120; // Boss-level virtual armor

            Tamable = false; // Boss-level creature, non-tamable
            ControlSlots = 0; // Not tamable or controllable

            m_AbilitiesInitialized = false; // Initialize abilities flag

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Base loot for the creature
            this.Say("You have dared to challenge me?");
            PackGold(1500, 2000); // Enhanced gold loot for a boss
            this.AddLoot(LootPack.FilthyRich, 3); // More rare loot
            this.AddLoot(LootPack.Rich, 2);
            this.AddLoot(LootPack.Gems, 10); // More gems for additional reward

            // Dropping the 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is an item in the server
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the original thinking logic, includes abilities
        }

        public BossShadowMuntjac(Serial serial)
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}

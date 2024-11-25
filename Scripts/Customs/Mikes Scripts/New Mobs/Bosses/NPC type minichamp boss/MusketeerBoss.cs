using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dashing musketeer lord")]
    public class MusketeerBoss : Musketeer
    {
        [Constructable]
        public MusketeerBoss() : base()
        {
            Name = "Musketeer Lord";
            Title = "the Grand Musketeer";

            // Enhance stats to make it a boss-tier creature
            SetStr(1200); // Higher strength
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Matching Barracoon's health range
            SetDamage(25, 40); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Maxed resist skills
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0); // Maxed wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased armor for more survivability

            // Attach the XmlRandomAbility for random boss abilities
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

            // Other loot from the original NPC (already handled in base method)
            // This will add gold, rich loot, and translocation powder
        }

        public MusketeerBoss(Serial serial) : base(serial)
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

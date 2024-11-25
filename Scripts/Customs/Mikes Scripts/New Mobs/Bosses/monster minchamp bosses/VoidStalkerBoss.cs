using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a void stalker boss corpse")]
    public class VoidStalkerBoss : VoidStalker
    {
        [Constructable]
        public VoidStalkerBoss() : base()
        {
            Name = "Void Stalker";
            Title = "the Eternal Shadow";

            // Update stats to match or exceed Barracoon (based on original stats and improvements)
            SetStr(1200); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(250); // Increased intelligence
            
            SetHits(12000); // Enhanced health
            SetDamage(35, 50); // Increased damage range

            // Adjust resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            // Enhanced skills to match a boss-tier creature
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            // Attach the XmlRandomAbility for dynamic abilities
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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss-specific behavior could be added here if needed
        }

        public VoidStalkerBoss(Serial serial) : base(serial)
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

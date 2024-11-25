using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a quor'zael corpse")]
    public class QuorZaelBoss : QuorZael
    {
        [Constructable]
        public QuorZaelBoss() : base()
        {
            Name = "Quor'Zael the Harbinger";
            Hue = 1768; // Unique hue for Quor'Zael
            Body = 22; // ElderGazer body
            BaseSoundID = 377;

            // Enhanced Stats
            SetStr(1200); // Increase strength for higher survivability
            SetDex(255); // Max dexterity for better speed and evasion
            SetInt(250); // Max intelligence

            SetHits(12000); // Increase health significantly for a boss fight
            SetDamage(35, 45); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 80); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 50.0); // Enhancing skills for boss-like performance
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000; // High fame to reflect the boss status
            Karma = -24000; // Negative karma for the harbinger

            VirtualArmor = 100; // Enhanced virtual armor for extra protection

            Tamable = false; // Boss should not be tamable
            ControlSlots = 0; // Not controllable

            // Attach the XmlRandomAbility for additional random abilities
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
            // You can add additional logic for the boss AI behavior if needed
        }

        public QuorZaelBoss(Serial serial) : base(serial)
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

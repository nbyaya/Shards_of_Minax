using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a voidcaller overlord corpse")]
    public class VorgathBoss : Vorgath
    {
        private DateTime m_NextVoidRift;
        private DateTime m_NextDarkPulse;
        private DateTime m_NextVoidShift;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VorgathBoss() : base()
        {
            Name = "Vorgath the Voidcaller";
            Title = "the Void Overlord";
            Body = 22; // ElderGazer body
            Hue = 1763; // Dark purple hue for a void-like appearance
            BaseSoundID = 377;

            // Enhance stats to match or exceed Barracoon's level
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Maximized dexterity for boss level agility
            SetInt(750); // Increased intelligence for spellcasting prowess

            SetHits(12000); // High health to make it a challenging boss

            SetDamage(35, 45); // Increased damage to make it more dangerous

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Increased negative karma for a villain

            VirtualArmor = 100; // Increased virtual armor for tanking hits

            Tamable = false; // Boss version is not tamable
            ControlSlots = 0; // Cannot be controlled

            m_AbilitiesInitialized = false; // Initialize the flag for ability setup

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // This is the missing serialization constructor
        public VorgathBoss(Serial serial) : base(serial)
        {
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
            // Additional boss logic or mechanics can be added here
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}

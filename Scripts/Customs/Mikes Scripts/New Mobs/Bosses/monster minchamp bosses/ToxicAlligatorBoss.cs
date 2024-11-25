using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a toxic alligator corpse")]
    public class ToxicAlligatorBoss : ToxicAlligator
    {
        private DateTime m_NextToxicBreath;
        private DateTime m_NextToxicAura;
        private bool m_AbilitiesActivated; // Flag to track initial ability activation

        [Constructable]
        public ToxicAlligatorBoss()
            : base()
        {
            Name = "Toxic Alligator";
            Title = "the Supreme Warden";

            // Update stats to match or exceed boss-level standards
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Max health
            SetDamage(35, 50); // Higher damage

            SetResistance(ResistanceType.Physical, 80); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 70); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maximum poison resistance
            SetResistance(ResistanceType.Energy, 60); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 50.0); // Slightly better skill range
            SetSkill(SkillName.EvalInt, 100.0); // High magic evaluation
            SetSkill(SkillName.Magery, 100.0); // High magery skill
            SetSkill(SkillName.Meditation, 50.0); // Enhanced meditation skill
            SetSkill(SkillName.MagicResist, 150.0); // High magic resistance
            SetSkill(SkillName.Tactics, 100.0); // High tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // High wrestling skill

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased negative karma

            VirtualArmor = 100; // Enhanced virtual armor

            Tamable = false; // Bosses aren't tamable
            ControlSlots = 3;
            MinTameSkill = 0; // Not tamable

            m_AbilitiesActivated = false; // Initialize ability flag

            // Attach a random ability (using the XmlRandomAbility class)
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

            // Additional loot can be added here if desired
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public ToxicAlligatorBoss(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return !Controlled;
            }
        }

        public override bool AutoDispel
        {
            get
            {
                return !Controlled;
            }
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 6; // Higher level treasure map for bosses
            }
        }

        public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }

        private void UseToxicBreath()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a cloud of toxic gas!");
                    m.Damage(20, this); // Increased damage
                    m.SendMessage("You feel poisoned by the toxic breath!");
                    m.SendMessage("Toxic Alligator breathes a cloud of poison!");
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x379F, 10, 30, 1109);

            m_NextToxicBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void UseToxicAura()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are surrounded by a noxious aura!");
                    m.Damage(10, this); // Increased damage
                    m.SendMessage("You feel sickened by the aura!");
                    m.SendMessage("Toxic Alligator's aura spreads poison!");
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 1109);

            m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

            m_AbilitiesActivated = false; // Reset flag to ensure proper initialization
        }
    }
}

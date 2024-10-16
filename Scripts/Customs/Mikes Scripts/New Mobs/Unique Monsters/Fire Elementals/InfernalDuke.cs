using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an infernal duke corpse")]
    public class InfernalDuke : BaseCreature
    {
        private DateTime m_NextHellfireStorm;
        private DateTime m_NextDemonsRoar;
        private DateTime m_NextInfernalConduit;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernalDuke()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an infernal duke";
            this.Body = 15; // Fire Elemental body
            this.Hue = 1657; // Unique hue for Infernal Duke
            this.BaseSoundID = 838;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            this.PackItem(new SulfurousAsh(5));

            m_AbilitiesInitialized = false; // Set initialization flag to false
        }

        public InfernalDuke(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override double DispelDifficulty
        {
            get { return 117.5; }
        }

        public override double DispelFocus
        {
            get { return 45.0; }
        }

        public override bool BleedImmune
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextHellfireStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDemonsRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextInfernalConduit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHellfireStorm)
                {
                    CastHellfireStorm();
                }

                if (DateTime.UtcNow >= m_NextDemonsRoar)
                {
                    PerformDemonsRoar();
                }

                if (DateTime.UtcNow >= m_NextInfernalConduit)
                {
                    ActivateInfernalConduit();
                }
            }
        }

        private void CastHellfireStorm()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a storm of hellfire!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0); // Fire damage
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Duke calls down a storm of hellfire! *");
            m_NextHellfireStorm = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown
        }

        private void PerformDemonsRoar()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are struck with fear by the Infernal Duke's roar!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Simple fear effect
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Duke lets out a fearsome roar! *");
            m_NextDemonsRoar = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown
        }

        private void ActivateInfernalConduit()
        {
            this.SetDamageType(ResistanceType.Fire, 100); // Increase fire damage
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Duke channels infernal energy, empowering its attacks! *");

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.SetDamageType(ResistanceType.Fire, 75); // Reset fire damage
            });

            m_NextInfernalConduit = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set cooldown
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

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_NextHellfireStorm = DateTime.UtcNow; // Reset timers to ensure they are reinitialized
            m_NextDemonsRoar = DateTime.UtcNow;
            m_NextInfernalConduit = DateTime.UtcNow;
        }
    }
}

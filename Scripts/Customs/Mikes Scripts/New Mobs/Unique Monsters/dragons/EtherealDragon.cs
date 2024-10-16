using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an ethereal dragon corpse")]
    public class EtherealDragon : BaseCreature
    {
        private DateTime m_NextEtherealBreath;
        private DateTime m_NextPhasing;
        private DateTime m_NextSpectralRoar;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public EtherealDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ethereal dragon";
            Body = 12; // Dragon body
            Hue = 1482; // Ghostly hue
            BaseSoundID = 362;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public EtherealDragon(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEtherealBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPhasing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpectralRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEtherealBreath)
                {
                    PerformEtherealBreath();
                }

                if (DateTime.UtcNow >= m_NextPhasing)
                {
                    PerformPhasing();
                }

                if (DateTime.UtcNow >= m_NextSpectralRoar)
                {
                    PerformSpectralRoar();
                }
            }
        }

        private void PerformEtherealBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ethereal dragon exhales a breath of spectral energy! *");

                    AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                    target.SendMessage("You are struck by a ghostly breath that chills you to the bone!");

                    // Reduce accuracy (example implementation, needs to be replaced with actual accuracy reduction code)
                    target.SendMessage("Your attacks seem to miss more often!");

                    m_NextEtherealBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void PerformPhasing()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ethereal dragon phases out of reality, becoming intangible! *");

            this.Frozen = true; // Temporarily disable movement and attacks

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                if (!this.Deleted)
                    this.Frozen = false;
            });

            m_NextPhasing = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void PerformSpectralRoar()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ethereal dragon emits a fearsome spectral roar! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 0);
                    m.SendMessage("You are struck by a fearsome roar that rattles your nerves!");
                }
            }

            m_NextSpectralRoar = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

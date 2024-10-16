using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a poisonous crab corpse")]
    public class PoisonousCrab : BaseCreature
    {
        private DateTime m_NextVenomousGrasp;
        private DateTime m_NextToxicBurst;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PoisonousCrab()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Poisonous Crab";
            Body = 1510; // Coconut Crab body
            Hue = 1400; // Green poisonous hue
			BaseSoundID = 0x4F2;

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
            SetResistance(ResistanceType.Poison, 100);
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

        public PoisonousCrab(Serial serial)
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
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextVenomousGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomousGrasp)
                {
                    VenomousGrasp();
                }

                if (DateTime.UtcNow >= m_NextToxicBurst)
                {
                    ToxicBurst();
                }
            }
        }

        private void VenomousGrasp()
        {
            if (Combatant == null)
                return;

            // Cast Combatant to Mobile
            Mobile mobileCombatant = Combatant as Mobile;
            if (mobileCombatant == null)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Poisonous Crab lunges and grasps its foe! *");
            PlaySound(0x4F2); // Custom sound for venomous grasp effect
            Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x36BD, 10, 10); // Green poisonous smoke

            // Apply poison effect
            mobileCombatant.SendMessage("You are poisoned by the Poisonous Crab's grasp!");
            AOS.Damage(mobileCombatant, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            mobileCombatant.ApplyPoison(this, Poison.Lethal);

            m_NextVenomousGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // 30 seconds cooldown
        }

        private void ToxicBurst()
        {
            if (Combatant == null)
                return;

            // Cast Combatant to Mobile
            Mobile mobileCombatant = Combatant as Mobile;
            if (mobileCombatant == null || !mobileCombatant.Poisoned)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Poisonous Crab unleashes a toxic burst! *");
            PlaySound(0x4F2); // Custom sound for toxic burst effect
            Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x36BD, 10, 10); // Green poisonous smoke

            // Apply additional poison damage
            mobileCombatant.SendMessage("You suffer from a toxic burst due to the poison!");
            AOS.Damage(mobileCombatant, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);

            m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45); // 45 seconds cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextVenomousGrasp);
            writer.Write(m_NextToxicBurst);
            writer.Write(m_AbilitiesInitialized); // Serialize the initialization flag
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextVenomousGrasp = reader.ReadDateTime();
            m_NextToxicBurst = reader.ReadDateTime();
            m_AbilitiesInitialized = reader.ReadBool(); // Deserialize the initialization flag
        }
    }
}

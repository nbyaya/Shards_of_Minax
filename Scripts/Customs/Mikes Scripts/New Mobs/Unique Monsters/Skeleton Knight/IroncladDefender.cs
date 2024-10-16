using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ironclad defender corpse")]
    public class IroncladDefender : BaseCreature
    {
        private DateTime m_NextShieldBash;
        private DateTime m_NextFortifiedStance;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IroncladDefender()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ironclad defender";
            Body = 57; // BoneKnight body
            Hue = 2369; // Unique hue for Ironclad Defender
			BaseSoundID = 451;

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

            this.PackItem(new WoodenShield()); // Adding a shield
            this.PackItem(new Scimitar()); // Adding a weapon

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public IroncladDefender(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextShieldBash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFortifiedStance = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShieldBash)
                {
                    ShieldBash();
                }

                if (DateTime.UtcNow >= m_NextFortifiedStance)
                {
                    FortifiedStance();
                }
            }
        }

        private void ShieldBash()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shield Bash! *");
                PlaySound(0x1F5); // Shield Bash sound

                // Stun and reduce target's defense
                target.FixedEffect(0x376A, 10, 16);
                target.Damage(Utility.RandomMinMax(10, 15), this);
                target.SendMessage("You are stunned by the Ironclad Defender's shield bash!");

                m_NextShieldBash = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
            }
        }

        private void FortifiedStance()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Fortified Stance! *");
            PlaySound(0x1F5); // Fortified Stance sound
            FixedEffect(0x37C4, 10, 36); // Visual effect

            // Increase own defense and resistances
            VirtualArmor += 20;
            SetResistance(ResistanceType.Physical, 50, 60);

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                VirtualArmor -= 20;
                SetResistance(ResistanceType.Physical, 40, 50);
            });

            m_NextFortifiedStance = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown
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

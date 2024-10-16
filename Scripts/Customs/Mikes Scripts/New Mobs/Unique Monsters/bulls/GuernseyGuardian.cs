using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Guernsey Guardian corpse")]
    public class GuernseyGuardian : BaseCreature
    {
        private DateTime m_NextGuardianBlessing;
        private DateTime m_NextShieldSlam;
        private DateTime m_NextHealingAura;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GuernseyGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Guernsey Guardian";
            Body = 0xE8; // Bull body
            BaseSoundID = 0x64;
            Hue = 1286; // Unique hue (you can adjust this)

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

            // Add a shield to the creature
            AddItem(new OrderShield());
        }

        public GuernseyGuardian(Serial serial)
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

        public override int Meat { get { return 15; } }
        public override int Hides { get { return 20; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bull; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Initialize abilities with random intervals
                    Random rand = new Random();
                    m_NextGuardianBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextShieldSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextHealingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGuardianBlessing)
                {
                    DoGuardiansBlessing();
                }

                if (DateTime.UtcNow >= m_NextShieldSlam)
                {
                    DoShieldSlam();
                }
            }

            if (DateTime.UtcNow >= m_NextHealingAura)
            {
                DoHealingAura();
            }
        }

        private void DoGuardiansBlessing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Guernsey Guardian invokes a protective blessing! *");
            PlaySound(0x1ED);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Controlled)))
                {
                    m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    m.SendLocalizedMessage(1062320); // You have been blessed with temporary invulnerability!
                    Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate { RemoveBlessing(m); }));
                }
            }

            m_NextGuardianBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Fixed cooldown
        }

        private void RemoveBlessing(Mobile m)
        {
            m.SendLocalizedMessage(1062321); // Your temporary invulnerability has worn off.
        }

        private void DoShieldSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Guernsey Guardian slams its shield! *");
            PlaySound(0x239);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    m.ApplyStun(TimeSpan.FromSeconds(3)); // Apply custom stun
                    m.FixedParticles(0x377A, 10, 15, 5032, EffectLayer.Head);
                }
            }

            m_NextShieldSlam = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        private void DoHealingAura()
        {
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Controlled)))
                {
                    int healAmount = Utility.RandomMinMax(5, 10);
                    m.Heal(healAmount);
                    m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                }
            }

            m_NextHealingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public static class MobileExtensions
    {
        public static void ApplyStun(this Mobile mobile, TimeSpan duration)
        {
            if (mobile == null || !mobile.Alive)
                return;

            mobile.Frozen = true; // Freeze the mobile to prevent actions

            // Set up a timer to unfreeze the mobile after the stun duration
            Timer.DelayCall(duration, () => mobile.Frozen = false);
        }
    }
}

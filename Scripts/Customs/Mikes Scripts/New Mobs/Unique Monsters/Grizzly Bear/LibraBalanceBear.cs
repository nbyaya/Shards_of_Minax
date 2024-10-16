using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a libra balancebear corpse")]
    public class LibraBalanceBear : BaseCreature
    {
        private static readonly int LibraBalanceBearHue = 1999; // Unique hue
        private DateTime m_NextScaleOfJusticeTime;
        private DateTime m_NextEquilibriumStrikeTime;
        private DateTime m_NextAuraOfBalanceTime;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LibraBalanceBear()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Libra BalanceBear";
            Body = 212; // GrizzlyBear body
            Hue = LibraBalanceBearHue;
			BaseSoundID = 0xA3;

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

        public LibraBalanceBear(Serial serial)
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
                    m_NextScaleOfJusticeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEquilibriumStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAuraOfBalanceTime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextScaleOfJusticeTime)
                {
                    UseScaleOfJustice();
                    m_NextScaleOfJusticeTime = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
                }

                if (DateTime.UtcNow >= m_NextEquilibriumStrikeTime)
                {
                    UseEquilibriumStrike();
                    m_NextEquilibriumStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
                }

                if (DateTime.UtcNow >= m_NextAuraOfBalanceTime)
                {
                    UseAuraOfBalance();
                    m_NextAuraOfBalanceTime = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown
                }
            }
        }

        private void UseScaleOfJustice()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Libra BalanceBear equalizes the fight! *");
            Effects.PlaySound(Location, Map, 0x1F5); // Sound effect

            int healthDiff = Math.Abs(Combatant.Hits - Hits);
            if (Combatant.Hits > Hits)
            {
                Hits += healthDiff / 2;
                Combatant.Hits -= healthDiff / 2;
            }
            else
            {
                Hits -= healthDiff / 2;
                Combatant.Hits += healthDiff / 2;
            }
        }

        private void UseEquilibriumStrike()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Libra BalanceBear strikes with equilibrium! *");
            Effects.PlaySound(Location, Map, 0x5C8); // Sound effect

            Mobile mobileCombatant = Combatant as Mobile;
            if (mobileCombatant != null)
            {
                int damage = (int)(mobileCombatant.Str / 10);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
            }
        }

        private void UseAuraOfBalance()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Libra BalanceBear radiates an aura of balance! *");
            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 16); // Visual effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    int damage = Utility.RandomMinMax(5, 15);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("The aura of the Libra BalanceBear harms you!");
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Libra BalanceBear's claws glow with celestial energy! *");
                defender.PlaySound(0x20A);
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(5, 10);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
            }
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a chocolate truffle corpse")]
    public class ChocolateTruffle : BaseCreature
    {
        private DateTime m_NextTruffleTrap;
        private DateTime m_NextVelvetyShield;
        private DateTime m_NextChocolateBurst;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized
        private DateTime m_ShieldEnd;

        [Constructable]
        public ChocolateTruffle()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Chocolate Truffle";
            Body = 0xCF; // Sheep body
            Hue = 2353; // Unique hue for the Chocolate Truffle
			BaseSoundID = 0xD6;

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

        public ChocolateTruffle(Serial serial)
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
                    m_NextTruffleTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextVelvetyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextChocolateBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTruffleTrap)
                {
                    SetTruffleTrap();
                }

                if (DateTime.UtcNow >= m_NextVelvetyShield && DateTime.UtcNow >= m_ShieldEnd)
                {
                    ActivateVelvetyShield();
                }

                if (DateTime.UtcNow >= m_NextChocolateBurst)
                {
                    ChocolateBurst();
                }
            }

            if (DateTime.UtcNow >= m_ShieldEnd && m_ShieldEnd != DateTime.MinValue)
            {
                DeactivateVelvetyShield();
            }
        }

        private void SetTruffleTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chocolate Truffle sets a trap with a sweet, deadly twist! *");

            // Create and place the trap
            Point3D trapLocation = Location;
            PoisonTile trap = new PoisonTile();
            trap.MoveToWorld(trapLocation, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => ExplodeTruffleTrap(trap));

            m_NextTruffleTrap = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for TruffleTrap
        }

        private void ExplodeTruffleTrap(PoisonTile trap)
        {
            if (trap.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chocolate Truffle's trap explodes with a sweet, devastating burst!*");
            PlaySound(0x1F2); // Explosion sound

            Effects.SendLocationEffect(trap.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Full damage

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are hit by the exploding chocolate trap!");
                    }

                    m.SendLocalizedMessage(1114727); // The trap knocks you back!

                    // Apply slow effect
                    m.SendMessage("You are covered in sticky chocolate and slowed down!");
                    m.Damage(5); // Minor damage from the sticky chocolate
                    m.SendMessage("You are slowed by the sticky chocolate!");
                }
            }

            trap.Delete();
        }

        private void ActivateVelvetyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chocolate Truffle creates a velvety shield of chocolate! *");
            PlaySound(0x1E3); // Shield sound

            FixedParticles(0x36D4, 9, 32, 5030, EffectLayer.Waist);

            m_ShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextVelvetyShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DeactivateVelvetyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The velvety shield of chocolate fades away. *");
            m_ShieldEnd = DateTime.MinValue;
        }

        private void ChocolateBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chocolate Truffle releases a burst of chocolatey goodness! *");
            PlaySound(0x1E3); // Chocolate burst sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Full damage

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are hit by a burst of chocolate!");
                    }

                    m.SendMessage("You feel sticky and debuffed!");
                    m.SendMessage("Your attack speed is reduced by the chocolate burst!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Temporary debuff
                }
            }

            m_NextChocolateBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ChocolateBurst
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_ShieldEnd > DateTime.UtcNow)
            {
                damage = Math.Max(0, damage - 50); // Absorb up to 50 damage
                from.SendLocalizedMessage(1114728); // Your attack is deflected by the chocolate shield!

                // Chocolate spikes damage
                int spikeDamage = Utility.RandomMinMax(5, 10);
                AOS.Damage(from, this, spikeDamage, 0, 0, 0, 0, 0); // Minor damage from spikes
                from.SendMessage("You are pricked by chocolate spikes!");
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}

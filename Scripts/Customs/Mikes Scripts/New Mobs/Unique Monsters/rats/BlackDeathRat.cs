using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black death rat corpse")]
    public class BlackDeathRat : BaseCreature
    {
        private static readonly int BubonicDamage = 10; // Base damage for Bubonic Spread
        private static readonly TimeSpan BubonicDamageInterval = TimeSpan.FromSeconds(5); // Damage interval
        private static readonly TimeSpan BubonicSpreadDelay = TimeSpan.FromSeconds(30); // Delay between spreads

        private DateTime m_NextBlindingBite;
        private DateTime m_NextDeathFrenzy;
        private DateTime BubonicSpreadEnd;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BlackDeathRat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a black death rat";
            Body = 2270; // Same body as GiantRat
            Hue = 1153; // Unique hue for the Black Death Rat (dark and ominous)
            this.BaseSoundID = 0xCC;
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

        public BlackDeathRat(Serial serial)
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

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Display the message and release the swarm
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Fleas leap from the rat's corpse, carrying death with them!*");
            ReleaseFleas();
        }

        private void ReleaseFleas()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && !m.Hidden)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (m != null && !m.Deleted && m.Alive)
                        {
                            m.SendMessage("You are bitten by a swarm of infected fleas!");
                            ApplyPoison(m);
                        }
                    });
                }
            }
        }

        private void ApplyPoison(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                // Apply poison with increasing damage over time
                Poison poison = Poison.Lethal; // Change to a suitable poison level if needed
                target.ApplyPoison(this, poison);
            }
        }

        private void BlindingBite()
        {
            if (Combatant != null && DateTime.UtcNow >= m_NextBlindingBite)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("The Black Death Rat blinds you with a vicious bite!");
                    target.SendMessage("Your vision is clouded, and you struggle to see!");
                    target.SendMessage("Your hit chance is reduced for a short time!");
                    target.PlaySound(0x1F5); // Sound of a painful bite

                    // Apply temporary blindness effect (e.g., reduce hit chance)
                    target.SendMessage("You are blinded!");
                    target.SendMessage("You can barely see!");
                    target.FixedEffect(0x376A, 10, 16);

                    m_NextBlindingBite = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reuse the blinding bite after a while
                }
            }
        }

        private void ActivateDeathFrenzy()
        {
            if (DateTime.UtcNow >= m_NextDeathFrenzy)
            {
                if (Hits < HitsMax / 2)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Black Death Rat goes into a frenzy!*");
                    PlaySound(0x1F5); // Frenzy sound effect
                    FixedEffect(0x37C4, 10, 36);

                    SetDamage(DamageMin + 4, DamageMax + 4); // Increase damage output

                    m_NextDeathFrenzy = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reuse the frenzy after some time
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBlindingBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random start for Blinding Bite
                    m_NextDeathFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90)); // Random start for Death Frenzy
                    BubonicSpreadEnd = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40)); // Random start for Bubonic Spread
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                BlindingBite(); // Try to perform a blinding bite periodically
                ActivateDeathFrenzy(); // Check if the rat should enter a frenzy state
            }

            if (DateTime.UtcNow >= BubonicSpreadEnd)
            {
                BubonicSpreadEnd = DateTime.UtcNow + BubonicSpreadDelay;
                ReleaseFleas(); // Trigger the spread of fleas periodically
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

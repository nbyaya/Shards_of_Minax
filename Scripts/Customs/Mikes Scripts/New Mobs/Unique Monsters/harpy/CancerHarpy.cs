using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cancer harpy corpse")]
    public class CancerHarpy : Harpy
    {
        private DateTime m_NextMoonbeam;
        private DateTime m_NextCrabClawSwipe;
        private DateTime m_NextLunarBurst;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CancerHarpy()
            : base()
        {
            Name = "a Cancer Harpy";
            Body = 30; // Using Harpy body
            Hue = 2078; // Soft moonlit blue hue
			BaseSoundID = 402; // Harpy sound

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

        public CancerHarpy(Serial serial)
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
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextMoonbeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCrabClawSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextLunarBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));

                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextMoonbeam)
                {
                    Moonbeam();
                }

                if (DateTime.UtcNow >= m_NextCrabClawSwipe)
                {
                    CrabClawSwipe();
                }

                if (DateTime.UtcNow >= m_NextLunarBurst)
                {
                    LunarBurst();
                }
            }
        }

        private void Moonbeam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Calls down a moonbeam! *");
            FixedEffect(0x376A, 10, 16, 1153, 0); // Moonbeam effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    // Heal self or allies in vicinity
                    if (m == this || (m is BaseCreature creature && creature.IsFriend(this))) // Custom check for allies
                    {
                        m.Heal(20); // Increased heal amount
                    }
                }
            }

            m_NextMoonbeam = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Moonbeam
        }

        private void CrabClawSwipe()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Swipes with crab claws! *");
            FixedEffect(0x373A, 10, 15, 1153, 0); // Swipe effect

            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(15, 20);
                Combatant.Damage(damage, this);

                if (Combatant is Mobile mob)
                {
                    mob.SendMessage("You are poisoned and slowed by the Cancer Harpy's swipe!");
                    mob.SendMessage("You feel a lingering poison seeping into your body!");

                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        if (Combatant != null && Combatant.Alive && Combatant is Mobile delayedMob)
                        {
                            delayedMob.SendMessage("The poison begins to wane.");
                        }
                    });

                    mob.ApplyPoison(this, Poison.Lethal); // Applying poison
                    mob.Freeze(TimeSpan.FromSeconds(5)); // Slowing effect
                }
            }

            m_NextCrabClawSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for CrabClawSwipe
        }

        private void LunarBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a burst of lunar energy! *");
            FixedEffect(0x3779, 10, 15, 1153, 0); // Lunar burst effect

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (m is Mobile mob)
                    {
                        mob.SendMessage("The Cancer Harpy's burst of lunar energy disorients you!");
                        mob.Paralyze(TimeSpan.FromSeconds(3)); // Disorient effect
                    }
                }
            }

            m_NextLunarBurst = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for LunarBurst
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

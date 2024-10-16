using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a scorpion spider corpse")]
    public class ScorpionSpider : BaseCreature
    {
        private DateTime m_NextScorpionSting;
        private DateTime m_NextDefensiveWeb;
        private DateTime m_NextTailSwipe;
        private DateTime m_NextSummonSpiderlingMites;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ScorpionSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a scorpion spider";
            Body = 28;
            Hue = 1786; // Unique hue for Scorpion Spider
            BaseSoundID = 0x388;

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

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public ScorpionSpider(Serial serial)
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
                    m_NextScorpionSting = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDefensiveWeb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTailSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextSummonSpiderlingMites = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 2));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextScorpionSting)
                {
                    ScorpionSting();
                }

                if (DateTime.UtcNow >= m_NextDefensiveWeb)
                {
                    DefensiveWeb();
                }

                if (DateTime.UtcNow >= m_NextTailSwipe)
                {
                    TailSwipe();
                }

                if (DateTime.UtcNow >= m_NextSummonSpiderlingMites)
                {
                    SummonSpiderlingMites();
                }
            }
        }

        private void ScorpionSting()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpion Spider's sting strikes with venomous force! *");
            m_NextScorpionSting = DateTime.UtcNow + TimeSpan.FromSeconds(20);

            if (Combatant != null)
            {
                Combatant.PlaySound(0x229); // Sting sound
                Combatant.FixedParticles(0x373A, 10, 30, 5052, EffectLayer.LeftFoot);

                // Cast Combatant to Mobile to access Freeze and ApplyPoison
                Mobile target = Combatant as Mobile;

                if (target != null)
                {
                    // Apply poison and paralysis
                    target.Freeze(TimeSpan.FromSeconds(3)); // Increased paralysis time
                    AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    target.ApplyPoison(this, Poison.Lethal);
                }
            }
        }

        private void DefensiveWeb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpion Spider spins a defensive web! *");
            m_NextDefensiveWeb = DateTime.UtcNow + TimeSpan.FromSeconds(30);

            // Reduces incoming damage
            VirtualArmor += 15; // Increased armor
            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(ResetVirtualArmor));
        }

        private void ResetVirtualArmor()
        {
            VirtualArmor -= 15;
        }

        private void TailSwipe()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpion Spider's tail sweeps through the air! *");
            m_NextTailSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(25);

            if (Combatant != null)
            {
                Combatant.PlaySound(0x229); // Tail swipe sound
                Combatant.FixedParticles(0x373A, 10, 30, 5052, EffectLayer.LeftFoot);

                // Cast Combatant to Mobile to access MoveToWorld and Freeze
                Mobile target = Combatant as Mobile;

                if (target != null)
                {
                    // Knockback and stun
                    target.MoveToWorld(new Point3D(target.X + Utility.RandomMinMax(-4, 4), target.Y + Utility.RandomMinMax(-4, 4), target.Z), Map);
                    target.Freeze(TimeSpan.FromSeconds(3)); // Increased stun time
                }
            }
        }

        private void SummonSpiderlingMites()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Scorpion Spider summons spiderlings to assist in battle! *");
            m_NextSummonSpiderlingMites = DateTime.UtcNow + TimeSpan.FromMinutes(1);

            for (int i = 0; i < 2; i++)
            {
                SpiderlingMite spiderling = new SpiderlingMite();
                spiderling.MoveToWorld(new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z), Map);
                spiderling.Combatant = Combatant;
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

    public class SpiderlingMite : BaseCreature
    {
        [Constructable]
        public SpiderlingMite()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spiderling";
            Body = 28;
            Hue = 1152; // Slightly different hue for spiderlings
            BaseSoundID = 0x388;

            SetStr(40, 60);
            SetDex(30, 50);
            SetInt(10, 20);

            SetHits(30, 50);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Poison, 30, 40);

            SetSkill(SkillName.Poisoning, 30.1, 50.0);
            SetSkill(SkillName.MagicResist, 10.1, 30.0);
            SetSkill(SkillName.Tactics, 30.1, 50.0);
            SetSkill(SkillName.Wrestling, 30.1, 50.0);

            Fame = 200;
            Karma = -200;

            VirtualArmor = 10;
        }

        public SpiderlingMite(Serial serial)
            : base(serial)
        {
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
        }
    }
}

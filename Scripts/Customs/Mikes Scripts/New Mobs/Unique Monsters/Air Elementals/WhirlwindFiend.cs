using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a whirlwind fiend corpse")]
    public class WhirlwindFiend : BaseCreature
    {
        private DateTime m_NextWhirlwindFury;
        private DateTime m_NextStormDash;
        private DateTime m_NextTurbulentAura;
        private bool m_AbilitiesInitialized; // Flag to check if abilities have been initialized

        [Constructable]
        public WhirlwindFiend()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a whirlwind fiend";
            Body = 13; // Air elemental body
            Hue = 1155; // Darker hue for chaos
            BaseSoundID = 655;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public WhirlwindFiend(Serial serial)
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
                    // Initialize ability timers with random intervals
                    Random rand = new Random();
                    m_NextWhirlwindFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStormDash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextTurbulentAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));

                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextWhirlwindFury)
                {
                    WhirlwindFury();
                }

                if (DateTime.UtcNow >= m_NextStormDash)
                {
                    StormDash();
                }

                if (DateTime.UtcNow >= m_NextTurbulentAura)
                {
                    TurbulentAura();
                }
            }
        }

        private void WhirlwindFury()
        {
            int range = 5;

            foreach (Mobile m in GetMobilesInRange(range))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are caught in the whirlwind fury of the Whirlwind Fiend!");
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Confusion effect
                }
            }

            Effects.PlaySound(this.Location, this.Map, 0x307); // Whirlwind sound
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A whirlwind surrounds the fiend, causing havoc! *");

            m_NextWhirlwindFury = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void StormDash()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    this.MoveToWorld(target.Location, target.Map);
                    Effects.PlaySound(this.Location, this.Map, 0x1F2); // Dash sound
                    target.Damage(Utility.RandomMinMax(20, 30), this);
                    target.Freeze(TimeSpan.FromSeconds(1)); // Stun effect

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Whirlwind Fiend dashes through you with immense force! *");

                    m_NextStormDash = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }
            }
        }

        private void TurbulentAura()
        {
            int range = 8;

            foreach (Mobile m in GetMobilesInRange(range))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The turbulent aura of the Whirlwind Fiend makes it hard to focus!");
                    m.SendMessage("You feel your attacks becoming less accurate and your defenses weaker!");

                    // Use a debuff item or create a custom effect here if available
                    // For example, you could reduce the combat effectiveness by a fixed amount
                    // This is just a placeholder for such effects
                    m.SendMessage("You feel weaker from the chaotic winds!");
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The air around the fiend becomes turbulent and chaotic! *");

            m_NextTurbulentAura = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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

            m_AbilitiesInitialized = false; // Reset flag for initialization
        }
    }
}

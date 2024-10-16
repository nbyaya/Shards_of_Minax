using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a lollipop lord corpse")]
    public class LollipopLord : BaseCreature
    {
        private DateTime m_NextLollipopSlam;
        private DateTime m_NextSweetShield;
        private DateTime m_NextSugarRush;
        private DateTime m_SweetShieldEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public LollipopLord()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Lollipop Lord";
            Body = 0xCF; // Sheep body
            Hue = 2345; // Unique hue (light pink)
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

            m_AbilitiesInitialized = false;
        }

        public LollipopLord(Serial serial)
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
                    m_NextLollipopSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSweetShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSugarRush = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLollipopSlam)
                {
                    LollipopSlam();
                }

                if (DateTime.UtcNow >= m_NextSweetShield && DateTime.UtcNow >= m_SweetShieldEnd)
                {
                    SweetShield();
                }

                if (DateTime.UtcNow >= m_NextSugarRush)
                {
                    SugarRush();
                }
            }

            if (DateTime.UtcNow >= m_SweetShieldEnd && m_SweetShieldEnd != DateTime.MinValue)
            {
                DeactivateSweetShield();
            }
        }

		private void LollipopSlam()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Lollipop Lord slams down with a sugary force! *");
			PlaySound(0x208); // Candy sound effect

			if (Combatant != null)
			{
				int damage = Utility.RandomMinMax(20, 30);
				AOS.Damage(Combatant, this, damage, 0, 0, 100, 0, 0);

				// Chance to knock back the target
				if (Utility.RandomDouble() < 0.5) // 50% chance
				{
					// Cast Combatant to Mobile
					Mobile mobileCombatant = Combatant as Mobile;

					if (mobileCombatant != null)
					{
						mobileCombatant.SendMessage("You are knocked back by the force of the lollipop!");
						// Move the mobileCombatant to a new location
						mobileCombatant.MoveToWorld(new Point3D(mobileCombatant.X + Utility.RandomMinMax(-2, 2), mobileCombatant.Y + Utility.RandomMinMax(-2, 2), mobileCombatant.Z), mobileCombatant.Map);
					}
				}
			}

			m_NextLollipopSlam = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Lollipop Slam
		}


        private void SweetShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Lollipop Lord creates a protective barrier of swirling lollipops! *");
            PlaySound(0x1E3); // Protective barrier sound effect

            // Activate shield
            m_SweetShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextSweetShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Sweet Shield
        }

        private void DeactivateSweetShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The protective barrier of swirling lollipops fades away. *");
            m_SweetShieldEnd = DateTime.MinValue;
        }

        private void SugarRush()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Lollipop Lord experiences a sugar rush and moves with incredible speed! *");
            PlaySound(0x1F5); // Speed effect sound

            // Increase speed
            this.Dex += 50;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.Dex -= 50; // Revert speed after 10 seconds
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The sugar rush wears off and the Lollipop Lord slows down. *");
            });

            m_NextSugarRush = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for Sugar Rush
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_SweetShieldEnd > DateTime.UtcNow)
            {
                damage = (int)(damage * 0.75); // Reduce damage by 25%
                from.SendMessage("Your attack is partially deflected by the sweet shield!");
            }
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

            m_AbilitiesInitialized = false;
        }
    }
}

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a celestial satyr's corpse")]
    public class CelestialSatyr : BaseCreature
    {
        private DateTime m_NextHeavenlyHymn;
        private DateTime m_NextLuminousChime;
        private DateTime m_NextSeraphicBallad;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CelestialSatyr()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a celestial satyr";
            Body = 271; // Using satyr body
            Hue = 2333; // Celestial hue
			this.BaseSoundID = 0x586;

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

        public CelestialSatyr(Serial serial)
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
                    m_NextHeavenlyHymn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLuminousChime = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextSeraphicBallad = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHeavenlyHymn)
                {
                    HeavenlyHymn();
                }

                if (DateTime.UtcNow >= m_NextLuminousChime)
                {
                    LuminousChime();
                }

                if (DateTime.UtcNow >= m_NextSeraphicBallad)
                {
                    SeraphicBallad();
                }
            }
        }

        private void HeavenlyHymn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a celestial hymn *");
            PlaySound(0x2D6);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).IsFriend(this))
                {
                    m.Heal(20);
                    m.SendMessage("You feel rejuvenated by the celestial hymn!");
                }
            }

            m_NextHeavenlyHymn = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Heavenly Hymn
        }

		private void LuminousChime()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emits a burst of light *");
			PlaySound(0x2D7);
			FixedEffect(0x373A, 10, 16);

			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m == this)
					continue;

				// only consider it "friendly" if it's a BaseCreature that IsFriend
				bool isFriend = (m is BaseCreature bc && bc.IsFriend(this));

				if (!isFriend)
				{
					m.SendMessage("You are blinded by the celestial burst of light!");
					m.SendMessage("You feel your skills diminished by the celestial light!");

					// we can debuff _any_ mobile with Skills—even players
					Timer.DelayCall(TimeSpan.Zero, () => ApplySkillDebuff(m));
				}
			}

			m_NextLuminousChime = DateTime.UtcNow + TimeSpan.FromSeconds(45);
		}

		private void ApplySkillDebuff(Mobile m)
		{
			// grab the Tactics skill on whatever Mobile this is
			var tactics = m.Skills[SkillName.Tactics];
			double original = tactics.Base;

			tactics.Base *= 0.7; // −30%

			Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
			{
				if (!m.Deleted)
				{
					tactics.Base = original;
					m.SendMessage("The celestial light's effects fade, and you feel your skills return to normal.");
				}
			});
		}


        private void SeraphicBallad()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons angelic spirits *");
            PlaySound(0x2D8);
            FixedEffect(0x373A, 10, 16);

            Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
            if (Map.CanSpawnMobile(loc))
            {
                AngelicSpirit spirit = new AngelicSpirit();
                spirit.MoveToWorld(loc, Map);
            }

            m_NextSeraphicBallad = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Seraphic Ballad
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

    public class AngelicSpirit : BaseCreature
    {
        [Constructable]
        public AngelicSpirit()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an angelic spirit";
            Body = 0x1C; // Use a fitting body or create a new one
            Hue = 1155; // Celestial hue

            SetStr(150);
            SetDex(100);
            SetInt(100);

            SetHits(150, 200);
            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 50.0, 60.0);
            SetSkill(SkillName.Tactics, 60.0, 70.0);
            SetSkill(SkillName.Wrestling, 60.0, 70.0);

            Fame = 3000;
            Karma = 1000;

            VirtualArmor = 30;
        }

        public AngelicSpirit(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add any additional behavior if needed
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

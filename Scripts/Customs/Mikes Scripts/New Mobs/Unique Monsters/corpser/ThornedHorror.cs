using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a thorned horror corpse")]
    public class ThornedHorror : BaseCreature
    {
        private DateTime m_NextThornBarrage;
        private DateTime m_NextThornyEmbrace;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ThornedHorror()
            : this("Thorned Horror")
        {
        }

        [Constructable]
        public ThornedHorror(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1383; // Dark green hue
            this.Body = 8;
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public ThornedHorror(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 5; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextThornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextThornyEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextThornBarrage)
                {
                    ThornBarrage(Combatant as Mobile);
                }

                if (DateTime.UtcNow >= m_NextThornyEmbrace && InRange(Combatant, 1))
                {
                    ThornyEmbrace(Combatant as Mobile);
                }
            }
        }

        public void ThornBarrage(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 8))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Thorned Horror lashes out with its thorns!*");
            PlaySound(0x1E5);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    Direction = GetDirectionTo(target);
                    MovingEffect(target, 0xF7A, 10, 1, false, false, 0x496, 0);
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                });
            }

            m_NextThornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Cooldown for ThornBarrage
        }

        public void ThornyEmbrace(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 1))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Thorned Horror ensnares you with its thorny vines!*");
            PlaySound(0x20B);

            target.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
            int damage = Utility.RandomMinMax(15, 25);
            AOS.Damage(target, this, damage, 80, 0, 0, 20, 0);

            target.SendLocalizedMessage(1075091); // *The thorns dig into your flesh, slowing your movement*
            target.AddSkillMod(new TimedSkillMod(SkillName.Wrestling, true, -20, TimeSpan.FromSeconds(5)));
            target.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, -20, TimeSpan.FromSeconds(5)));

            m_NextThornyEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for ThornyEmbrace
        }

        public override int GetIdleSound()
        {
            return 443;
        }

        public override int GetAngerSound()
        {
            return 442;
        }

        public override int GetHurtSound()
        {
            return 445;
        }

        public override int GetDeathSound()
        {
            return 447;
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

using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a toxic alligator corpse")]
    public class ToxicAlligator : BaseCreature
    {
        private DateTime m_NextToxicBreath;
        private DateTime m_NextToxicAura;
        private bool m_AbilitiesActivated; // Flag to track initial ability activation

        [Constructable]
        public ToxicAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic alligator";
            Body = 0xCA; // Alligator body
            Hue = 1160; // Unique hue for the toxic effect
            BaseSoundID = 660;

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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public ToxicAlligator(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    Random rand = new Random();
                    m_NextToxicBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextToxicBreath)
                {
                    UseToxicBreath();
                }

                if (DateTime.UtcNow >= m_NextToxicAura)
                {
                    UseToxicAura();
                }
            }
        }

        private void UseToxicBreath()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in a cloud of toxic gas!");
                    m.Damage(10, this);
                    m.SendMessage("You feel poisoned by the toxic breath!");
                    m.SendMessage("Toxic Alligator breathes a cloud of poison!");
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x379F, 10, 30, 1109);

            m_NextToxicBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void UseToxicAura()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are surrounded by a noxious aura!");
                    m.Damage(5, this);
                    m.SendMessage("You feel sickened by the aura!");
                    m.SendMessage("Toxic Alligator's aura spreads poison!");
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 1109);

            m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesActivated = false; // Reset flag to ensure proper initialization
        }
    }
}

using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cosmic stalker corpse")]
    public class CosmicStalker : BaseCreature
    {
        private DateTime m_NextDimensionalShift;
        private DateTime m_NextStellarClaws;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CosmicStalker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cosmic Stalker";
            Body = 0xD6; // Panther body
            Hue = 2184; // Iridescent blue hue
            BaseSoundID = 0x462; // Panther sound

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

            // Initialize the abilities
            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public CosmicStalker(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextDimensionalShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 30)); // Random initial time between 5 and 30 seconds
                    m_NextStellarClaws = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 45)); // Random initial time between 10 and 45 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDimensionalShift)
                {
                    DimensionalShift();
                }

                if (DateTime.UtcNow >= m_NextStellarClaws)
                {
                    StellarClaws();
                }
            }
        }

        private void DimensionalShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shifts through dimensions *");
            PlaySound(0x1FE);

            Map map = Map;

            if (map != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    Point3D to = GetSpawnPosition(5);

                    if (to != Point3D.Zero)
                    {
                        Point3D from = Location;
                        Location = to;

                        Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        PlaySound(0x1FE);

                        break;
                    }
                }
            }

            m_NextDimensionalShift = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown for next ability
        }

        private void StellarClaws()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Charges stellar claws *");
            PlaySound(0x5C3);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                    m.PlaySound(0x213);

                    m.SendLocalizedMessage(1070825); // The creature's star-shaped claws tear into your flesh!

                    int damage = Utility.RandomMinMax(20, 30);
                    Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 5, new TimerStateCallback(DoCosmicBurn), new object[] { m, damage });
                }
            }

            m_NextStellarClaws = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Fixed cooldown for next ability
        }

        private void DoCosmicBurn(object state)
        {
            object[] states = (object[])state;
            Mobile m = (Mobile)states[0];
            int damage = (int)states[1];

            if (m.Alive)
            {
                m.Damage(damage, this);
                m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p) && !Server.Spells.SpellHelper.CheckMulti(p, Map))
                    return p;
            }

            return Point3D.Zero;
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

            // Reset the ability initialization flag
            m_AbilitiesInitialized = false;
        }
    }
}

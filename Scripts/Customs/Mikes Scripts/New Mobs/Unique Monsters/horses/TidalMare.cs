using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a tidal mare corpse")]
    public class TidalMare : BaseMount
    {
        private DateTime m_NextTidalWave;
        private DateTime m_NextWaterCloak;
        private DateTime m_NextAquaSurge;
        private bool m_WaterCloakActive;
        private Mobile m_LastTidalWaveTarget;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TidalMare()
            : base("Tidal Mare", 0xE2, 0x3EA0, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2;
            ItemID = 0x3EA0;
            Hue = 2087; // Deep blue hue
            BaseSoundID = 0xA8;

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

        public TidalMare(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTidalWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextWaterCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextAquaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTidalWave)
                {
                    TidalWave();
                }

                if (DateTime.UtcNow >= m_NextWaterCloak && !m_WaterCloakActive)
                {
                    WaterCloak();
                }

                if (DateTime.UtcNow >= m_NextAquaSurge)
                {
                    AquaSurge();
                }
            }
        }

        private void TidalWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A mighty wave crashes over you! *");
            PlaySound(0x11);
            FixedEffect(0x376A, 10, 15);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    m.PlaySound(0x026);

                    // Knock back effect
                    Direction d = GetDirectionTo(m);
                    int offsetX = 0;
                    int offsetY = 0;

                    // Determine offsets based on direction
                    switch (d)
                    {
                        case Direction.North:
                            offsetY = -1;
                            break;
                        case Direction.South:
                            offsetY = 1;
                            break;
                        case Direction.East:
                            offsetX = 1;
                            break;
                        case Direction.West:
                            offsetX = -1;
                            break;
                    }

                    int x = m.X + Utility.RandomMinMax(1, 3) * offsetX;
                    int y = m.Y + Utility.RandomMinMax(1, 3) * offsetY;
                    int z = m.Map.GetAverageZ(x, y);
                    Point3D to = new Point3D(x, y, z);

                    if (m.Map.CanFit(to, 16, false, false))
                    {
                        m.MoveToWorld(to, m.Map);
                    }

                    m_LastTidalWaveTarget = m;
                }
            }

            m_NextTidalWave = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void WaterCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tidal Mare is cloaked in water! *");
            PlaySound(0x026);
            FixedEffect(0x376A, 10, 15);

            ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, 25);
            AddResistanceMod(mod);

            m_WaterCloakActive = true;

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                RemoveResistanceMod(mod);
                m_WaterCloakActive = false;
            });

            m_NextWaterCloak = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void AquaSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tidal Mare unleashes a powerful Aqua Surge! *");
            PlaySound(0x11);
            FixedEffect(0x376A, 10, 15);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(25, 35);
                    
                    if (m == m_LastTidalWaveTarget)
                    {
                        damage += Utility.RandomMinMax(15, 25);
                        m.SendLocalizedMessage(1072374); // The aqua surge is particularly effective against you!
                    }

                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    m.PlaySound(0x026);
                }
            }

            m_NextAquaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(25);
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

            m_NextTidalWave = DateTime.UtcNow;
            m_NextWaterCloak = DateTime.UtcNow;
            m_NextAquaSurge = DateTime.UtcNow;
            m_WaterCloakActive = false;
            m_LastTidalWaveTarget = null;
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}

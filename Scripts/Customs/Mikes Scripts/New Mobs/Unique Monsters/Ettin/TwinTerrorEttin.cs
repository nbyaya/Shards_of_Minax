using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an Twin Terror Ettin corpse")]
    public class TwinTerrorEttin : BaseCreature
    {
        private DateTime m_NextEarthquakeStomp;
        private DateTime m_NextRockArmor;
        private DateTime m_NextStoneSpire;
        private DateTime m_RockArmorEnd;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public TwinTerrorEttin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Twin Terror Ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1563; // Earthy brown hue

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

        public TwinTerrorEttin(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRockArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextStoneSpire = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEarthquakeStomp)
                {
                    EarthquakeStomp();
                }

                if (DateTime.UtcNow >= m_NextRockArmor && m_RockArmorEnd == DateTime.MinValue)
                {
                    RockArmor();
                }

                if (DateTime.UtcNow >= m_NextStoneSpire)
                {
                    StoneSpire();
                }
            }

            if (DateTime.UtcNow >= m_RockArmorEnd && m_RockArmorEnd != DateTime.MinValue)
            {
                EndRockArmor();
            }
        }

        private void EarthquakeStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Twin Terror Ettin stomps the ground, causing violent tremors! *");
            PlaySound(0x2F3);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                    m.SendLocalizedMessage(1060024); // The ground beneath you shakes!

                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.PlaySound(0x491);

                    // Knock down effect
                    m.Animate(20, 7, 1, true, false, 0);
                }
            }

            // Create earthquake visual effect
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-5, 5);
                int y = Y + Utility.RandomMinMax(-5, 5);
                int z = Z;

                Effects.SendLocationEffect(new Point3D(x, y, z), Map, 0x37CC, 15, 10);
            }

            m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void RockArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Twin Terror Ettin encases itself in rock-like armor! *");
            PlaySound(0x1BC);
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            SetResistance(ResistanceType.Physical, 65, 75);
            m_RockArmorEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextRockArmor = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void EndRockArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The rock armor crumbles away *");
            SetResistance(ResistanceType.Physical, 45, 55);
            m_RockArmorEnd = DateTime.MinValue;
        }

        private void StoneSpire()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Twin Terror Ettin summons stone spires from the ground! *");
            PlaySound(0x216);

            for (int i = 0; i < 5; i++)
            {
                int range = 6;
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D spireLocation = new Point3D(x, y, z);

                if (Map.CanFit(spireLocation, 16, false, false))
                {
                    Effects.SendLocationEffect(spireLocation, Map, 0x3837, 40);
                    PlaySound(0x216);

                    foreach (Mobile m in GetMobilesInRange(1))
                    {
                        if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                        {
                            DoHarmful(m);

                            int damage = Utility.RandomMinMax(20, 40);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                            m.SendLocalizedMessage(1111696); // You've been impaled by a stone spire!
                        }
                    }
                }
            }

            m_NextStoneSpire = DateTime.UtcNow + TimeSpan.FromSeconds(25);
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

            // Reset initialization flag and randomize the next ability times
            m_AbilitiesInitialized = false;
            m_NextEarthquakeStomp = DateTime.UtcNow;
            m_NextRockArmor = DateTime.UtcNow;
            m_NextStoneSpire = DateTime.UtcNow;
            m_RockArmorEnd = DateTime.MinValue;
        }
    }
}

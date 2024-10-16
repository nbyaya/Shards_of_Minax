using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Magnetic Crab corpse")]
    public class MagneticCrab : BaseMount
    {
        private DateTime m_NextMagneticGrasp;
        private DateTime m_NextElectromagneticBurst;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MagneticCrab()
            : base("Magnetic Crab", 1510, 16081, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F2;

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

            Hue = 1455; // Unique blue-ish hue

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MagneticCrab(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Combatant is Mobile)
            {
                Mobile target = (Mobile)Combatant;

                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMagneticGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextElectromagneticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMagneticGrasp)
                {
                    MagneticGrasp(target);
                }

                if (DateTime.UtcNow >= m_NextElectromagneticBurst && InRange(target.Location, 2))
                {
                    ElectromagneticBurst(target);
                }
            }
        }

        public void MagneticGrasp(Mobile target)
        {
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Magnetic Grasp *");

                target.MoveToWorld(GetSpawnPosition(1), Map);
                target.Freeze(TimeSpan.FromSeconds(3));
                
                // Reduce target's dexterity
                target.AddStatMod(new StatMod(StatType.Dex, "MagneticGrasp", -(target.Dex / 5), TimeSpan.FromSeconds(10)));

                Effects.SendTargetParticles(target, 0x3818, 10, 10, 0x13B5, EffectLayer.Head);
                target.PlaySound(0x1FE);

                m_NextMagneticGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        public void ElectromagneticBurst(Mobile target)
        {
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Electromagnetic Burst *");

                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                Effects.SendTargetParticles(target, 0x3818, 10, 10, 0x13B5, EffectLayer.Head);
                target.PlaySound(0x29);

                m_NextElectromagneticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && !m.Deleted && m != this)
            {
                if (DateTime.UtcNow >= m_NextMagneticGrasp && InRange(m.Location, 8) && !InRange(oldLocation, 8) && CanBeHarmful(m))
                {
                    MagneticGrasp(m);
                }
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

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Location;
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

            m_AbilitiesInitialized = false; // Reset the initialization flag

            // Reinitialize random intervals for abilities
            Random rand = new Random();
            m_NextMagneticGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
            m_NextElectromagneticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
        }
    }
}

using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a stone rooster corpse")]
    public class StoneRooster : BaseCreature
    {
        private DateTime m_NextStonePeck;
        private DateTime m_NextQuakeEgg;

        [Constructable]
        public StoneRooster()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Stone Rooster";
            Body = 0xD0; // Chicken body
            BaseSoundID = 0x6E;
            Hue = 1365; // Rocky grey hue

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

            m_NextStonePeck = DateTime.UtcNow;
            m_NextQuakeEgg = DateTime.UtcNow;
        }

        public StoneRooster(Serial serial)
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
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override bool CanFly { get { return true; } }
        public override int Feathers { get { return 25; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextStonePeck)
                {
                    StonePeck();
                }

                if (DateTime.UtcNow >= m_NextQuakeEgg)
                {
                    QuakeEgg();
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Stone Peck! *");
                defender.PlaySound(0x208);
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(10, 15);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
            }
        }

        private void StonePeck()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Stone Peck! *");
            PlaySound(0x208);

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by the Stone Rooster's peck!");
                }
            }

            m_NextStonePeck = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void QuakeEgg()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lays a Quake Egg! *");
            PlaySound(0x208);

            QuakeEgg egg = new QuakeEgg(this);
            egg.MoveToWorld(Location, Map);

            m_NextQuakeEgg = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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

            m_NextStonePeck = DateTime.UtcNow;
            m_NextQuakeEgg = DateTime.UtcNow;
        }
    }

    public class QuakeEgg : Item
    {
        private Mobile m_Owner;
        private DateTime m_ExplosionTime;

        public QuakeEgg(Mobile owner) : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 1153; // Rocky grey hue
            Name = "Quake Egg";

            m_ExplosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Explode));
        }

        public QuakeEgg(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this.GetWorldLocation(), 0))
            {
                Explode();
            }
        }

        public void Explode()
        {
            if (Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The egg explodes with a quake! *");

            Effects.PlaySound(GetWorldLocation(), Map, 0x307);

            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != m_Owner && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, m_Owner, damage, 0, 100, 0, 0, 0);

                    m.PlaySound(0x1DD);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Owner);
            writer.Write(m_ExplosionTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_ExplosionTime = reader.ReadDateTime();

            if (DateTime.UtcNow < m_ExplosionTime)
            {
                TimeSpan delay = m_ExplosionTime - DateTime.UtcNow;
                Timer.DelayCall(delay, new TimerCallback(Explode));
            }
            else
            {
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Explode));
            }
        }
    }
}

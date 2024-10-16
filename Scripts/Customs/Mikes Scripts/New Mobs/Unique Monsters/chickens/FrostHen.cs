using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost hen corpse")]
    public class FrostHen : BaseCreature
    {
        private DateTime m_NextFrostPeck;
        private DateTime m_NextIceEgg;

        [Constructable]
        public FrostHen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frost Hen";
            Body = 0xD0; // Chicken body
            BaseSoundID = 0x6E;
            Hue = 1378; // Icy blue hue

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

            m_NextFrostPeck = DateTime.UtcNow;
            m_NextIceEgg = DateTime.UtcNow;
        }

        public FrostHen(Serial serial)
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
                if (DateTime.UtcNow >= m_NextFrostPeck)
                {
                    FrostPeck();
                }

                if (DateTime.UtcNow >= m_NextIceEgg)
                {
                    IceEgg();
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frost Peck! *");
                defender.PlaySound(0x208);
                defender.FixedParticles(0x376A, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(4, 8);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);

                defender.SendMessage("You feel the icy chill of the Frost Peck!");
            }
        }

        private void FrostPeck()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frost Peck! *");
            m_NextFrostPeck = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void IceEgg()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lays an Ice Egg! *");
            PlaySound(0x665);

            IceEgg egg = new IceEgg(this);
            egg.MoveToWorld(Location, Map);

            m_NextIceEgg = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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

            m_NextFrostPeck = DateTime.UtcNow;
            m_NextIceEgg = DateTime.UtcNow;
        }
    }

    public class IceEgg : Item
    {
        private Mobile m_Owner;
        private DateTime m_ExplosionTime;

        public IceEgg(Mobile owner) : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 1152; // Icy blue hue
            Name = "Ice Egg";

            m_ExplosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Explode));
        }

        public IceEgg(Serial serial) : base(serial)
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

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The egg explodes into a burst of ice! *");

            Effects.PlaySound(GetWorldLocation(), Map, 0x307);

            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != m_Owner && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(m, m_Owner, damage, 0, 100, 0, 0, 0);

                    m.SendMessage("You slip on the icy ground and take cold damage!");
                    m.Freeze(TimeSpan.FromSeconds(2));
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

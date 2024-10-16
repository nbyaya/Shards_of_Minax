using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic fowl corpse")]
    public class MysticFowl : BaseCreature
    {
        private DateTime m_NextMysticEgg;

        [Constructable]
        public MysticFowl()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Mystic Fowl";
            Body = 0xD0; // Chicken body
            BaseSoundID = 0x6E;
            Hue = 1372; // Iridescent hue

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

            m_NextMysticEgg = DateTime.UtcNow;
        }

        public MysticFowl(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override bool CanFly { get { return true; } }
        public override int Feathers { get { return 25; } }

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
                if (DateTime.UtcNow >= m_NextMysticEgg)
                {
                    LayMysticEgg();
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                ArcanePeck(defender);
            }
        }

        private void ArcanePeck(Mobile defender)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Arcane Peck! *");
            defender.PlaySound(0x213);
            defender.FixedParticles(0x376A, 10, 15, 5052, EffectLayer.Head);

            int damage = Utility.RandomMinMax(5, 10);
            int elementType = Utility.Random(5);

            switch (elementType)
            {
                case 0: // Fire
                    AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);
                    defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    break;
                case 1: // Cold
                    AOS.Damage(defender, this, damage, 0, 0, 100, 0, 0);
                    defender.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.LeftFoot);
                    break;
                case 2: // Poison
                    AOS.Damage(defender, this, damage, 0, 0, 0, 100, 0);
                    defender.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.LeftFoot);
                    break;
                case 3: // Energy
                    AOS.Damage(defender, this, damage, 0, 0, 0, 0, 100);
                    defender.FixedParticles(0x3818, 10, 30, 5052, EffectLayer.LeftFoot);
                    break;
                default: // Physical
                    AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                    defender.FixedParticles(0x37CC, 10, 30, 5052, EffectLayer.LeftFoot);
                    break;
            }
        }

        private void LayMysticEgg()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lays a Mystic Egg! *");
            PlaySound(0x42);

            MysticEgg egg = new MysticEgg(this);
            egg.MoveToWorld(Location, Map);

            m_NextMysticEgg = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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

            m_NextMysticEgg = DateTime.UtcNow;
        }
    }

    public class MysticEgg : Item
    {
        private Mobile m_Owner;
        private DateTime m_ActivationTime;

        public MysticEgg(Mobile owner) : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 1153; // Iridescent hue
            Name = "Mystic Egg";

            m_ActivationTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Activate));
        }

        public MysticEgg(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this.GetWorldLocation(), 0))
            {
                Activate();
            }
        }

        public void Activate()
        {
            if (Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The egg activates! *");

            Effects.PlaySound(GetWorldLocation(), Map, 0x1E0);

            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x3728, 20, 10, 2023, 0);

            int effect = Utility.Random(3);

            switch (effect)
            {
                case 0:
                    Teleport();
                    break;
                case 1:
                    HealingBurst();
                    break;
                case 2:
                    MagicalBarrier();
                    break;
            }

            this.Delete();
        }

        private void Teleport()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != m_Owner && m.Alive && !m.IsDeadBondedPet)
                {
                    Point3D loc = new Point3D(
                        m.X + Utility.RandomMinMax(-5, 5),
                        m.Y + Utility.RandomMinMax(-5, 5),
                        m.Z);

                    m.MoveToWorld(loc, m.Map);
                    m.PlaySound(0x1FE);
                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                }
            }
        }

        private void HealingBurst()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m.Alive && !m.IsDeadBondedPet)
                {
                    int heal = Utility.RandomMinMax(15, 25);
                    m.Heal(heal);
                    m.PlaySound(0x202);
                    m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                }
            }
        }

        private void MagicalBarrier()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m.Alive && !m.IsDeadBondedPet)
                {
                    ResistanceMod[] mods = new ResistanceMod[]
                    {
                        new ResistanceMod(ResistanceType.Physical, 10),
                        new ResistanceMod(ResistanceType.Fire, 10),
                        new ResistanceMod(ResistanceType.Cold, 10),
                        new ResistanceMod(ResistanceType.Poison, 10),
                        new ResistanceMod(ResistanceType.Energy, 10)
                    };

                    foreach (ResistanceMod mod in mods)
                        m.AddResistanceMod(mod);

                    m.PlaySound(0x1ED);
                    m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                    {
                        foreach (ResistanceMod mod in mods)
                            m.RemoveResistanceMod(mod);
                    }));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Owner);
            writer.Write(m_ActivationTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_ActivationTime = reader.ReadDateTime();

            if (DateTime.UtcNow < m_ActivationTime)
            {
                TimeSpan delay = m_ActivationTime - DateTime.UtcNow;
                Timer.DelayCall(delay, new TimerCallback(Activate));
            }
            else
            {
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Activate));
            }
        }
    }
}
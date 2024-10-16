using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a red-tailed squirrel corpse")]
    public class RedTailedSquirrel : BaseCreature
    {
        private DateTime m_NextTailWhip;
        private DateTime m_NextFlameAcorns;
        private DateTime m_NextAcornBarrage;
        private DateTime m_NextFieryRetreat;
        private bool m_AbilitiesInitialized;
        private bool m_IsRetreating;

        [Constructable]
        public RedTailedSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a red-tailed squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2430; // Red hue

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
            m_IsRetreating = false;
        }

        public RedTailedSquirrel(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTailWhip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 10));
                    m_NextFlameAcorns = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextAcornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 30));
                    m_NextFieryRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(60);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextTailWhip)
                {
                    TailWhip();
                }

                if (DateTime.UtcNow >= m_NextFlameAcorns)
                {
                    FlameAcorns();
                }

                if (DateTime.UtcNow >= m_NextAcornBarrage)
                {
                    AcornBarrage();
                }

                if (Hits < HitsMax * 0.3 && !m_IsRetreating)
                {
                    FieryRetreat();
                }
            }
        }

        private void TailWhip()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Red-Tailed Squirrel lashes out with its fiery tail! *");
            PlaySound(0x208);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    m.SendMessage("You are struck by a fiery tail whip!");
                    Effects.SendLocationEffect(m.Location, Map, 0x36BD, 20, 10);
                    m.PlaySound(0x1F4);

                    // Apply a Burning debuff
                    if (Utility.RandomDouble() < 0.5) // 50% chance
                    {
                        m.SendMessage("You are burned by the tail whip!");
                        Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyBurningDebuff(m));
                    }
                }
            }

            m_NextTailWhip = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void ApplyBurningDebuff(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                int damage = Utility.RandomMinMax(5, 10);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.SendMessage("You are burned by the tail whip!");
            }
        }

        private void FlameAcorns()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Red-Tailed Squirrel hurls a burning acorn! *");
            PlaySound(0x208);

            Point3D loc = Location;
            FlameAcornItem acorn = new FlameAcornItem();
            acorn.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeFlameAcorn(acorn));

            m_NextFlameAcorns = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ExplodeFlameAcorn(FlameAcornItem acorn)
        {
            if (acorn.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The flame acorn explodes in a burst of fire! *");
            PlaySound(0x307);

            Effects.SendLocationEffect(acorn.Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are burned by the exploding flame acorn!");
                    m.PlaySound(0x1F4);

                    // Set the target on fire with a chance
                    if (Utility.RandomDouble() < 0.5) // 50% chance
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyBurningDebuff(m));
                    }
                }
            }

            acorn.Delete();
        }

        private void AcornBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Red-Tailed Squirrel launches a barrage of burning acorns! *");
            PlaySound(0x208);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    Point3D loc = Location;
                    FlameAcornItem acorn = new FlameAcornItem();
                    acorn.MoveToWorld(loc, Map);
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeFlameAcorn(acorn));
                });
            }

            m_NextAcornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void FieryRetreat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Red-Tailed Squirrel retreats with fiery speed! *");
            PlaySound(0x1F4);

            // Increase defense
            this.VirtualArmor = 70;

            m_IsRetreating = true;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor = 50; // Reset defense
                m_IsRetreating = false;
            });
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
            m_AbilitiesInitialized = false;
            m_IsRetreating = false;
        }
    }

    public class FlameAcornItem : Item
    {
        public FlameAcornItem() : base(0x171D)
        {
            Movable = false;
        }

        public FlameAcornItem(Serial serial) : base(serial)
        {
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
        }
    }
}

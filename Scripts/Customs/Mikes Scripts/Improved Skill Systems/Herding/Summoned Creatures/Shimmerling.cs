using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shimmerling corpse")]
    public class Shimmerling : BaseCreature
    {
        private static readonly string[] m_Vocabulary = new string[]
        {
            "twinkle",
            "shimmer shimmer",
            "sparkle sparkle!"
        };

        private bool m_CanTalk;
        private DateTime m_NextShimmerEffect;
        private DateTime m_NextMirageClone;

        [Constructable]
        public Shimmerling()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a shimmerling";
            Body = 0x117; // Using ferret body as base
            Hue = 1153; // Iridescent hue

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_CanTalk = true;
            m_NextShimmerEffect = DateTime.UtcNow;
            m_NextMirageClone = DateTime.UtcNow;
        }

        public Shimmerling(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies; } }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m is Shimmerling && m.InRange(this, 3) && m.Alive)
                Talk((Shimmerling)m);
        }

        public void Talk()
        {
            Talk(null);
        }

        public void Talk(Shimmerling to)
        {
            if (m_CanTalk)
            {
                Say(m_Vocabulary[Utility.Random(m_Vocabulary.Length)]);

                if (to != null && Utility.RandomBool())
                    Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8)), new TimerCallback(delegate() { to.Talk(); }));

                m_CanTalk = false;

                Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30)), new TimerCallback(delegate() { m_CanTalk = true; }));
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextShimmerEffect)
                {
                    ShimmerEffect();
                }

                if (DateTime.UtcNow >= m_NextMirageClone)
                {
                    CreateMirageClone();
                }
            }
        }

        private void ShimmerEffect()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shimmers brightly *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Shimmerling's bright light dazzles you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextShimmerEffect = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CreateMirageClone()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                MirageClone clone = new MirageClone(this);
                clone.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate() 
                {
                    if (!clone.Deleted)
                        clone.Delete(); 
                }));

                m_NextMirageClone = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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

            return Point3D.Zero;
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

            m_CanTalk = true;
            m_NextShimmerEffect = DateTime.UtcNow;
            m_NextMirageClone = DateTime.UtcNow;
        }
    }

    public class MirageClone : BaseCreature
    {
        private Mobile m_Master;

        public MirageClone(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public MirageClone(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
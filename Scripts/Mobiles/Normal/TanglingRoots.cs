using System;
using System.Collections.Generic;

using Server.Items;
using Server.Spells;
using Server.Spells.Spellweaving;

namespace Server.Mobiles
{
    [CorpseName("a tangling root corpse")]
    public class TanglingRoots : BaseCreature
    {
        [Constructable]
        public TanglingRoots()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tangling root";
            Body = 8;
            BaseSoundID = 684;

            SetStr(157, 189);
            SetDex(51, 64);
            SetInt(26, 39);

            SetHits(231, 246);
            SetMana(0);

            SetDamage(10, 23);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.Wrestling, 45.1, 60.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 18;

            if (0.25 > Utility.RandomDouble())
                PackItem(new Board(10));
            else
                PackItem(new Log(10));

            PackItem(new MandrakeRoot(3));
        }

        public TanglingRoots(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune { get { return Poison.Lesser; } }
        public override bool DisallowAllMoves { get { return true; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }

        private static List<Mobile> m_TangleCooldown = new List<Mobile>();
        private Dictionary<Mobile, Timer> m_DamageTable = new Dictionary<Mobile, Timer>();

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Alive && !m.IsDeadBondedPet && m.AccessLevel == AccessLevel.Player && !m.Hidden && !TransformationSpellHelper.UnderTransformation(m, typeof(EtherealVoyageSpell)))
            {
                if (0.2 > Utility.RandomDouble() && !m_TangleCooldown.Contains(m) && InRange(m, 6) && !FountainOfFortune.UnderProtection(m))
                {
                    m.Frozen = true;
                    m.MoveToWorld(Location, Map);

                    m.PlaySound(0x1FE);
                    m.SendLocalizedMessage(1111641); // You become entangled in the acid drenched roots.

                    m_TangleCooldown.Add(m);

                    Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)), new TimerStateCallback<Mobile>(Untangle), m);
                    Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerStateCallback<Mobile>(RemoveCooldown), m);
                }

                if (m.InRange(this, 1) && !m_DamageTable.ContainsKey(m))
                {
                    // Should start the timer
                    m_DamageTable[m] = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), new TimerStateCallback<Mobile>(DoDamage), m);
                }
            }
        }

        protected void Untangle(Mobile m)
        {
            m.Frozen = false;
            m.SendLocalizedMessage(1111642); // You manage to untangle yourself.
        }

        protected void RemoveCooldown(Mobile m)
        {
            if (m_TangleCooldown.Contains(m))
                m_TangleCooldown.Remove(m);
        }

        protected void DoDamage(Mobile m)
        {
            if (m.Alive && !m.IsDeadBondedPet && !Deleted && m.InRange(this, 1))
            {
                m.Damage(4, this);
                m.SendLocalizedMessage(1111643); // The acid is damaging you!
            }
            else
            {
                Timer t = m_DamageTable[m];
                t.Stop();

                m_DamageTable.Remove(m);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.02)
                c.DropItem(new LuckyCoin());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new SwamplardsSash());
            }				
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

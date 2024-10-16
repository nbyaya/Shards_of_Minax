using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using System.Collections.Generic;
using Server.Network;
using System.Linq;

namespace Server.Mobiles
{
    [CorpseName("a Primeval Dragon corpse")]
    public class PrimevalDragon : BaseChampion
    {
        private DateTime m_NextBreathTime;
        private DateTime m_NextAbilityTime;
        private Timer m_Timer;

        [Constructable]
        public PrimevalDragon()
            : base(AIType.AI_Mage)
        {
            this.Name = "Primeval Dragon";
            this.Body = 12; // Use the appropriate body ID for a dragon
            this.BaseSoundID = 362;

            this.SetStr(1000);
            this.SetDex(200);
            this.SetInt(1000);

            this.SetHits(50000);
            this.SetMana(5000);

            this.SetDamage(25, 30);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Fire, 20);
            this.SetDamageType(ResistanceType.Cold, 20);
            this.SetDamageType(ResistanceType.Energy, 20);
            this.SetDamageType(ResistanceType.Poison, 20);

            this.SetResistance(ResistanceType.Physical, 80);
            this.SetResistance(ResistanceType.Fire, 80);
            this.SetResistance(ResistanceType.Cold, 80);
            this.SetResistance(ResistanceType.Poison, 80);
            this.SetResistance(ResistanceType.Energy, 80);

            this.SetSkill(SkillName.EvalInt, 120.0);
            this.SetSkill(SkillName.Magery, 120.0);
            this.SetSkill(SkillName.Meditation, 120.0);
            this.SetSkill(SkillName.MagicResist, 150.0);
            this.SetSkill(SkillName.Tactics, 120.0);
            this.SetSkill(SkillName.Wrestling, 120.0);

            this.Fame = 50000;
            this.Karma = -50000;

            this.VirtualArmor = 100;

            m_Timer = new TeleportTimer(this);
            m_Timer.Start();
        }

        public PrimevalDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return false; } }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.None; } }

        public override Type[] UniqueList
        {
            get
            {
                return new Type[] { typeof(MaxxiaScroll), typeof(MaxxiaScroll), typeof(MaxxiaScroll), typeof(MaxxiaScroll) };
            }
        }
        public override Type[] SharedList
        {
            get
            {
                return new Type[] { typeof(MaxxiaScroll), typeof(MaxxiaScroll), typeof(MaxxiaScroll), typeof(MaxxiaScroll) };
            }
        }
        public override Type[] DecorativeList
        {
            get
            {
                return new Type[] { typeof(MaxxiaScroll) };
            }
        }
        public override MonsterStatuetteType[] StatueTypes
        {
            get
            {
                return new MonsterStatuetteType[] { MonsterStatuetteType.Dragon };
            }
        } 

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.SuperBoss, 3);
            this.AddLoot(LootPack.FilthyRich);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new MaxxiaScroll());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.UtcNow > m_NextAbilityTime && 0.2 > Utility.RandomDouble())
            {
                switch (Utility.Random(2))
                {
                    case 0: TailSwipe(); break;
                    case 1: WingBuffet(); break;
                }

                m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_NextBreathTime <= DateTime.UtcNow)
            {
                PrismaticBreath();
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
            }
        }

        #region Prismatic Breath
        private void PrismaticBreath()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.InRange(this, 10))
            {
                this.Frozen = true;
                this.Direction = this.GetDirectionTo(target);

                Effects.PlaySound(this.Location, this.Map, 0x16A);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(BreathEffect_Callback));
            }
        }

        private void BreathEffect_Callback()
        {
            this.Frozen = false;

            if (Combatant is Mobile)
            {
                Mobile m = (Mobile)Combatant;

                if (m.InRange(this, 10))
                {
                    this.Direction = this.GetDirectionTo(m);
                    this.MovingParticles(m, 0x36D4, 5, 0, false, true, 0x3F, 0, 0x249, 0x32D, 0x2A7, 0);
                    AOS.Damage(m, this, Utility.RandomMinMax(100, 150), 0, 25, 25, 25, 25);
                }
            }
        }
        #endregion

        #region Tail Swipe
        private void TailSwipe()
        {
            ArrayList list = new ArrayList();

            IPooledEnumerable eable = GetMobilesInRange(3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m) && m.InLOS(this))
                    list.Add(m);
            }
            eable.Free();

            foreach (Mobile m in list)
            {
                AOS.Damage(m, this, Utility.RandomMinMax(50, 80), 100, 0, 0, 0, 0);
                m.SendLocalizedMessage(1063361); // You're knocked back by the dragon's tail!
                m.MoveToWorld(GetSpawnPosition(2), Map);
            }
        }
        #endregion

        #region Wing Buffet
        private void WingBuffet()
        {
            ArrayList list = new ArrayList();

            IPooledEnumerable eable = GetMobilesInRange(5);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m) && m.InLOS(this))
                    list.Add(m);
            }
            eable.Free();

            foreach (Mobile m in list)
            {
                AOS.Damage(m, this, Utility.RandomMinMax(30, 60), 100, 0, 0, 0, 0);
                m.SendLocalizedMessage(1075081); // The dragon's wings buffet you with a powerful gust of wind!
                m.Freeze(TimeSpan.FromSeconds(2));
            }
        }
        #endregion

        #region Teleport
        private class TeleportTimer : Timer
        {
            private Mobile m_Owner;

            public TeleportTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if (m_Owner.Deleted)
                {
                    Stop();
                    return;
                }

                Map map = m_Owner.Map;

                if (map == null)
                    return;

                if (0.25 < Utility.RandomDouble())
                    return;

                Mobile toTeleport = null;

                foreach (Mobile m in m_Owner.GetMobilesInRange(10))
                {
                    if (m != m_Owner && m.IsPlayer() && m_Owner.CanBeHarmful(m) && m_Owner.CanSee(m))
                    {
                        toTeleport = m;
                        break;
                    }
                }

                if (toTeleport != null)
                {
                    Point3D from = toTeleport.Location;
                    Point3D to = m_Owner.Location;

                    toTeleport.Location = to;
                    m_Owner.Location = from;

                    toTeleport.ProcessDelta();
                    m_Owner.ProcessDelta();

                    Effects.SendLocationParticles(EffectItem.Create(from, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.SendLocationParticles(EffectItem.Create(to, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                    toTeleport.PlaySound(0x1FE);

                    m_Owner.Combatant = toTeleport;
                }
            }
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Timer = new TeleportTimer(this);
            m_Timer.Start();
        }
    }
}
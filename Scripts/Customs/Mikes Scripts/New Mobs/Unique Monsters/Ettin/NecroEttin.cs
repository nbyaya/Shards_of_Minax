using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a necro ettin corpse")]
    public class NecroEttin : BaseCreature
    {
        private DateTime m_NextLifeDrain;
        private DateTime m_NextNecroticWave;
        private DateTime m_NextUndyingRage;
        private DateTime m_UndyingRageEnd;
        private List<Mobile> m_Minions;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public NecroEttin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a necro ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1560; // Dark purple hue

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

            m_Minions = new List<Mobile>();
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public NecroEttin(Serial serial)
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
                    m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextUndyingRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLifeDrain)
                {
                    LifeDrain();
                }

                if (DateTime.UtcNow >= m_NextNecroticWave)
                {
                    NecroticWave();
                }

                if (DateTime.UtcNow >= m_NextUndyingRage && Hits < (HitsMax / 2))
                {
                    UndyingRage();
                }
            }

            if (DateTime.UtcNow >= m_UndyingRageEnd && m_UndyingRageEnd != DateTime.MinValue)
            {
                EndUndyingRage();
            }
        }

        private void LifeDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Necro Ettin drains life and summons minions! *");
            PlaySound(0x1FB);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    Hits = Math.Min(Hits + damage, HitsMax);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                    m.PlaySound(0x231);
                }
            }

            SummonMinion();

            m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SummonMinion()
        {
            Map map = this.Map;

            if (map == null)
                return;

            int newMinions = Utility.RandomMinMax(1, 3);

            for (int i = 0; i < newMinions; ++i)
            {
                BaseCreature minion = new Skeleton();

                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    minion.MoveToWorld(loc, map);
                    minion.Combatant = Combatant;
                    minion.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m_Minions.Add(minion);
                }
                else
                {
                    minion.Delete();
                }
            }
        }

        private void NecroticWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Necro Ettin releases a wave of dark energy! *");
            PlaySound(0x64E);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(30, 40);
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x213);

                    StatMod mod = new StatMod(StatType.Str, "NecroEttin_Str_Curse", -20, TimeSpan.FromSeconds(60));
                    m.AddStatMod(mod);
                }
            }

            m_NextNecroticWave = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void UndyingRage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Necro Ettin enters an undying rage! *");
            PlaySound(0x165);
            FixedParticles(0x375A, 10, 15, 5017, 0x496, 0, EffectLayer.Waist);

            SetStr(Str + 100);
            SetDex(Dex + 50);
            SetResistance(ResistanceType.Physical, GetResistance(ResistanceType.Physical) + 20);

            m_UndyingRageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextUndyingRage = DateTime.UtcNow + TimeSpan.FromMinutes(5);
        }

        private void EndUndyingRage()
        {
            SetStr(Str - 100);
            SetDex(Dex - 50);
            SetResistance(ResistanceType.Physical, GetResistance(ResistanceType.Physical) - 20);

            m_UndyingRageEnd = DateTime.MinValue;
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

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            foreach (BaseCreature minion in m_Minions)
            {
                if (!minion.Deleted)
                    minion.Kill();
            }

            m_Minions.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write(m_Minions.Count);
            foreach (BaseCreature minion in m_Minions)
                writer.Write(minion);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int count = reader.ReadInt();
            m_Minions = new List<Mobile>();
            for (int i = 0; i < count; i++)
            {
                BaseCreature minion = reader.ReadMobile() as BaseCreature;
                if (minion != null)
                    m_Minions.Add(minion);
            }

            if (version >= 0)
            {
                m_AbilitiesInitialized = false;
                m_UndyingRageEnd = DateTime.MinValue;
            }
        }
    }
}

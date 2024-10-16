using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of a necrotic general")]
    public class NecroticGeneral : BaseCreature
    {
        private DateTime m_NextSummonUndead;
        private DateTime m_NextDeathlyCommand;
        private List<Mobile> m_Minions;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NecroticGeneral()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a necrotic general";
            Body = 57; // BoneKnight body
            BaseSoundID = 451;
            Hue = 2368; // Dark red hue

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
            m_AbilitiesInitialized = false; // Initialize the flag

            PackItem(new PowerScroll(SkillName.Necromancy, 115.0));
        }

        public NecroticGeneral(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDeathlyCommand = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSummonUndead)
                {
                    SummonUndead();
                }

                if (DateTime.UtcNow >= m_NextDeathlyCommand)
                {
                    DeathlyCommand();
                }
            }
        }

        private void SummonUndead()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summon Undead *");
            PlaySound(0x1FB);

            Map map = this.Map;

            if (map == null)
                return;

            int newMinions = Utility.RandomMinMax(2, 4);

            for (int i = 0; i < newMinions; ++i)
            {
                BaseCreature minion = new SkeletalKnight();

                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    minion.MoveToWorld(loc, map);
                    minion.Combatant = Combatant;
                    m_Minions.Add(minion);

                    Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    minion.PlaySound(0x1FB);
                }
                else
                {
                    minion.Delete();
                }
            }

            m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set the cooldown for next summon
        }

        private void DeathlyCommand()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Deathly Command *");
            PlaySound(0x1FB);

            foreach (BaseCreature minion in m_Minions)
            {
                if (!minion.Deleted && minion.Alive)
                {
                    minion.FixedEffect(0x376A, 10, 16);
                    minion.PlaySound(0x1FB);

                    minion.SetSkill(SkillName.Tactics, minion.Skills[SkillName.Tactics].Base + 20);
                    minion.SetSkill(SkillName.Wrestling, minion.Skills[SkillName.Wrestling].Base + 20);
                    minion.RawStr += 50;
                    minion.RawDex += 50;

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate()
                    {
                        if (!minion.Deleted && minion.Alive)
                        {
                            minion.SetSkill(SkillName.Tactics, minion.Skills[SkillName.Tactics].Base - 20);
                            minion.SetSkill(SkillName.Wrestling, minion.Skills[SkillName.Wrestling].Base - 20);
                            minion.RawStr -= 50;
                            minion.RawDex -= 50;
                        }
                    }));
                }
            }

            m_NextDeathlyCommand = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Set the cooldown for next command
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
                if (!minion.Deleted && minion.Alive)
                    minion.Kill();
            }

            m_Minions.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

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
                if (minion != null && !minion.Deleted)
                    m_Minions.Add(minion);
            }

            // Reset initialization flag and cooldowns
            m_AbilitiesInitialized = false;
            m_NextSummonUndead = DateTime.UtcNow;
            m_NextDeathlyCommand = DateTime.UtcNow;
        }
    }
}

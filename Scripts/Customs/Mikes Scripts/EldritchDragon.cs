using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("an eldritch dragon corpse")]
    public class EldritchDragon : BaseCreature
    {
        private bool m_PhaseTwo;
        private bool m_PhaseThree;

        [Constructable]
        public EldritchDragon() : base(AIType.AI_Mage, FightMode.Closest, 18, 1, 0.2, 0.4)
        {
            Name = "Eldritch Dragon";
            Body = 46; // Dragon body
            BaseSoundID = 362;

            SetStr(1500, 1700);
            SetDex(200, 250);
            SetInt(800, 900);

            SetHits(20000);
            SetStam(250, 300);
            SetMana(5000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Meditation, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;

            m_PhaseTwo = false;
            m_PhaseThree = false;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.HighScrolls, 2);
            // Add custom loot here
        }

        public override bool AutoDispel { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool Uncalmable { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_PhaseTwo && Hits < HitsMax * 0.66)
            {
                EnterPhaseTwo();
            }
            else if (!m_PhaseThree && Hits < HitsMax * 0.33)
            {
                EnterPhaseThree();
            }
        }

        private void EnterPhaseTwo()
        {
            m_PhaseTwo = true;
            Say("You dare wound me? Feel the wrath of the void!");

            // Increase stats or abilities
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 85, 95);

            // Summon minions
            SummonVoidMinions(3);
        }

        private void EnterPhaseThree()
        {
            m_PhaseThree = true;
            Say("I shall consume your very essence!");

            // Further increase stats or abilities
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 90, 100);

            // Special attack or ability
            BeginVoidBreath();
        }

        private void SummonVoidMinions(int count)
        {
            for (int i = 0; i < count; i++)
            {
                VoidMinion minion = new VoidMinion();
                minion.Team = this.Team;
                minion.Map = this.Map;
                minion.Location = GetSpawnPosition(2);

                Say("Arise, my servants!");

                Effects.SendLocationParticles(
                    EffectItem.Create(minion.Location, minion.Map, EffectItem.DefaultDuration),
                    0x3728, 10, 10, 2023);

                PlaySound(0x216);
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            Map map = this.Map;

            if (map == null)
                return this.Location;

            // Try to find a valid location within range
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = map.GetAverageZ(x, y);

                if (map.CanSpawnMobile(new Point3D(x, y, z)))
                    return new Point3D(x, y, z);
            }

            return this.Location;
        }

        private void BeginVoidBreath()
        {
            // Start a timer for the breath attack
            Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(VoidBreath));
        }

        private void VoidBreath()
        {
            Say("Feel the emptiness!");

            List<Mobile> targets = new List<Mobile>();

            IPooledEnumerable eable = this.GetMobilesInRange(10);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && ((BaseCreature)m).Controlled)
                    targets.Add(m);
                else if (m.Player)
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                m.PlaySound(0x1FB);

                // Apply a debuff or damage over time
                m.SendMessage("You feel your life force draining away!");
                m.AddStatMod(new StatMod(StatType.Str, "VoidBreathStr", -10, TimeSpan.FromSeconds(30.0)));
                m.AddStatMod(new StatMod(StatType.Dex, "VoidBreathDex", -10, TimeSpan.FromSeconds(30.0)));
                m.AddStatMod(new StatMod(StatType.Int, "VoidBreathInt", -10, TimeSpan.FromSeconds(30.0)));
            }
        }

        public EldritchDragon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version

            writer.Write(m_PhaseTwo);
            writer.Write(m_PhaseThree);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_PhaseTwo = reader.ReadBool();
            m_PhaseThree = reader.ReadBool();
        }
    }

    public class VoidMinion : BaseCreature
    {
        [Constructable]
        public VoidMinion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Void Minion";
            Body = 58; // Wraith body
            BaseSoundID = 0x482;

            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(3000);
            SetStam(250, 300);
            SetMana(0);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;
        }

        public override bool AutoDispel { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public VoidMinion(Serial serial) : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            // Optional: Add loot or special items
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

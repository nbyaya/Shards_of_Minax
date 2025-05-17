using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the blood of the pharaoh corpse")]
    public class BloodOfThePharaoh : BaseCreature, IBloodCreature
    {
        // Cooldown timers
        private DateTime m_NextSiphonTime;
        private DateTime m_NextCurseTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextJudgmentTime;

        // Unique crimson hue
        private const int UniqueHue = 1175;

        [Constructable]
        public BloodOfThePharaoh()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "the Blood of the Pharaoh";
            Body = 159;
            BaseSoundID = 278;
            Hue = UniqueHue;

            // Stats
            SetStr(650, 750);
            SetDex(100, 125);
            SetInt(400, 500);

            SetHits(2000, 2400);
            SetStam(150, 200);
            SetMana(800, 900);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 110.0);
            SetSkill(SkillName.Wrestling, 90.1, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSiphonTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurseTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_NextJudgmentTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Loot: heavy gold & chance for Pharaoh's Scepter
            PackGold(2000, 4000);
            PackItem(new BlackPearl(15));
            PackItem(new Bloodmoss(15));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new KabutoOfTheBloomingSteel()); // your unique artifact
        }
		
		public BloodOfThePharaoh(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && Alive && Map != null && Map != Map.Internal)
            {
                var now = DateTime.UtcNow;
                double dist = GetDistanceToSqrt(target);

                // 1) Blood Siphon: steal life & heal self (close range)
                if (now >= m_NextSiphonTime && dist <= 8)
                {
                    m_NextSiphonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                    BloodSiphon(target);
                }
                // 2) Curse of the Sands: poison‐hazard ground
                else if (now >= m_NextCurseTime && dist <= 12)
                {
                    m_NextCurseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                    CurseOfTheSands(target);
                }
                // 3) Summon Mummy Legion
                else if (now >= m_NextSummonTime)
                {
                    m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
                    SummonUndeadLegion();
                }
                // 4) Pharaoh's Judgment: targeted necromantic fire strike
                else if (now >= m_NextJudgmentTime && dist <= 16)
                {
                    m_NextJudgmentTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 30));
                    PharaohsJudgment(target);
                }
            }
        }

        // --- Ability 1: Blood Siphon ---
        private void BloodSiphon(Mobile target)
        {
            Say("*By royal blood, I feast!*");
            PlaySound(0x4F1);
            target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

            int drain = Utility.RandomMinMax(40, 60);
            if (target.Hits > drain)
            {
                target.Hits -= drain;
                Hits += drain; // self‐heal
            }
        }

        // --- Ability 2: Curse of the Sands (PoisonTile AoE) ---
        private void CurseOfTheSands(Mobile target)
        {
            Say("*Taste the sands of decay!*");
            PlaySound(0x228);
            Point3D loc = target.Location;

            for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                var p = new Point3D(loc.X + i, loc.Y + j, loc.Z);
                if (Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                {
                    PoisonTile tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(p, Map);
                }
            }
        }

        // --- Ability 3: Summon Undead Legion ---
        private void SummonUndeadLegion()
        {
            Say("*Arise, my eternal guards!*");
            PlaySound(0x2E3);

            for (int i = 0; i < 3; i++)
            {
                var offset = Utility.RandomList(
                    new Point3D( 1,  1, 0),
                    new Point3D(-1,  1, 0),
                    new Point3D( 1, -1, 0),
                    new Point3D(-1, -1, 0));

                Point3D spawn = new Point3D(X + offset.X, Y + offset.Y, Z);
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                // MummyWarrior must be a custom creature defined elsewhere
                Mobile mummy = new Mummy();
                mummy.MoveToWorld(spawn, Map);
                mummy.Combatant = Combatant;
            }
        }

        // --- Ability 4: Pharaoh's Judgment (FlamestrikeHazardTile) ---
        private void PharaohsJudgment(Mobile target)
        {
            Say("*Feel the sun’s wrath!*");
            PlaySound(0x208);
            Point3D p = target.Location;

            // impact effect
            Effects.SendLocationParticles(
                EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                0x3709, 8, 20, UniqueHue, 0, 5052, 0);

            // place hazardous flames
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                FlamestrikeHazardTile flame = new FlamestrikeHazardTile();
                flame.Hue = UniqueHue;
                flame.MoveToWorld(p, Map);
            });
        }

        // --- Death Explosion & Hazard Spill ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (Map == null)
				return;

            Say("*My cycle... begins anew...*");
            PlaySound(0x4F2);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 40, UniqueHue, 0, 5052, 0);

            // Spawn 4–6 PoisonTiles around corpse
            int count = Utility.RandomMinMax(4, 6);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                Point3D p = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                PoisonTile tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);
            }
        }

        // Standard overrides
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 7; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus    { get { return 75.0;  } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on load
            var now = DateTime.UtcNow;
            m_NextSiphonTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurseTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_NextJudgmentTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Vault Sentinel X-8 corpse")]
    public class VaultSentinelX8 : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextEMPTime;
        private DateTime m_NextCascadeTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique Hue for this sentinel (a cold, metallic teal)
        private const int UniqueHue = 1365;

        [Constructable]
        public VaultSentinelX8()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "Vault Sentinel X-8";
            Body = 0x9d;               // same body as SentinelSpider
            BaseSoundID = 0x388;       // same sound
            Hue = UniqueHue;

            // ——— High-End Stats ———
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(1200, 1500);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 30);

            // Damage profile: mostly energy
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 110.0, 125.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextEMPTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Starter loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
        }

        // ——— Aura: Electromagnetic Discharge on Movement ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || m.Map != Map || !CanBeHarmful(m, false))
                return;

            if (m.InRange(this.Location, 2) && Utility.RandomDouble() < 0.15) // 15% chance per move
            {
                DoHarmful(m);
                // Drain both mana and stamina
                int drainAmt = Utility.RandomMinMax(10, 20);
                if (m.Mana >= drainAmt)
                {
                    m.Mana -= drainAmt;
                    m.SendMessage(0x22, "Your magical reserves are sapped by the sentinel's aura!");
                    m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    m.PlaySound(0x1F8);
                }

                if (m.Stam >= drainAmt)
                {
                    m.Stam -= drainAmt;
                }

                // Minor energy damage
                AOS.Damage(m, this, Utility.RandomMinMax(5, 12), 0, 0, 0, 0, 100);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Refresh last location if no tile drop
            if (this.Location != m_LastLocation)
                m_LastLocation = this.Location;

            // EMP Pulse: AoE mana/stam/energy burst
            if (DateTime.UtcNow >= m_NextEMPTime && this.InRange(Combatant.Location, 8))
            {
                EMPPulse();
                m_NextEMPTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Arc Flux Cascade: chain lightning between targets
            else if (DateTime.UtcNow >= m_NextCascadeTime && this.InRange(Combatant.Location, 12))
            {
                ArcFluxCascade();
                m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Summon Micro-Sentinels around itself
            else if (DateTime.UtcNow >= m_NextSummonTime && this.Hits < this.HitsMax * 0.75)
            {
                SummonMicroSentinels();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        // ——— EMP Pulse: drains and damages all mobiles in radius ———
        private void EMPPulse()
        {
            PlaySound(0x1F4);
            FixedParticles(0x3709, 15, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                int drain = Utility.RandomMinMax(30, 50);
                if (m.Mana >= drain) { m.Mana -= drain; }
                if (m.Stam >= drain) { m.Stam -= drain; }

                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
                m.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // ——— Arc Flux Cascade: chain energy bolts ———
        private void ArcFluxCascade()
        {
            Say("*System override: Arc flux engaged.*");
            PlaySound(0x20A);

            var hitList = new List<Mobile>();
            if (Combatant is Mobile initial && CanBeHarmful(initial, false) && SpellHelper.ValidIndirectTarget(this, initial))
                hitList.Add(initial);

            int maxBounces = 4, range = 8;
            for (int i = 0; i < maxBounces && hitList.Count > 0; i++)
            {
                var src = hitList[i];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(src.Location, range))
                {
                    if (m != this && m != src && !hitList.Contains(m)
                        && CanBeHarmful(m, false)                    // ← fixed: first arg is target, second is bool
                        && SpellHelper.ValidIndirectTarget(this, m)
                        && src.InLOS(m))
                    {
                        double d = src.GetDistanceToSqrt(m);
                        if (d < bestDist) { bestDist = d; next = m; }
                    }
                }

                if (next == null) break;
                hitList.Add(next);
            }

            for (int i = 0; i < hitList.Count; i++)
            {
                Mobile tgt = hitList[i];
                Mobile src = (i == 0 ? this : hitList[i - 1]);

                // Visual bolt
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100
                );

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (tgt != null && CanBeHarmful(tgt, false))   // ← fixed here too
                    {
                        DoHarmful(tgt);
                        AOS.Damage(tgt, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                        tgt.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // ——— Summon Micro-Sentinels: temporary friendly spiders ———
        private void SummonMicroSentinels()
        {
            Say("*Deploying micro-sentinels.*");
            for (int i = 0; i < 3; i++)
            {
                var loc = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z
                );
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mini = new SentinelSpider();
                mini.Hue = UniqueHue;
                mini.MoveToWorld(loc, Map);

                // schedule deletion in 20s instead of calling non-existent DeleteAfterDelay
                Timer.DelayCall(TimeSpan.FromSeconds(20.0), () =>
                {
                    if (!mini.Deleted)
                        mini.Delete();
                });
            }
        }

        // ——— Death Effect: minefield generation ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;
            PlaySound(0x211);
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            // Scatter landmines around the corpse
            for (int i = 0; i < Utility.RandomMinMax(5, 8); i++)
            {
                var dx = Utility.RandomMinMax(-3, 3);
                var dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(this.X + dx, this.Y + dy, this.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        // ——— Loot & Serialization ———
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.025) // 2.5% for unique core
                PackItem(new ManaDrainTile()); // as a placeholder for a “Void Core”
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus    => 80.0;

        public VaultSentinelX8(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init cooldowns
            m_NextEMPTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}

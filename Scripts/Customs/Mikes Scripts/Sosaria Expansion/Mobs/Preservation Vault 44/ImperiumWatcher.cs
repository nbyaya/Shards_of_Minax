using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // for spell effects
using Server.Spells.Seventh; // for Chain Lightning style


namespace Server.Mobiles
{
    [CorpseName("an Imperium Watcher corpse")]
    public class ImperiumWatcher : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextTemporalWave;
        private DateTime m_NextNanoSwarm;
        private DateTime m_NextVaultBreach;
        private DateTime m_NextBeamBarrage;
        private Point3D m_LastLocation;

        // Unique metallic‑silver hue
        private const int UniqueHue = 1153;

        [Constructable]
        public ImperiumWatcher() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name           = "an Imperium Watcher";
            Body           = 4;     // Gargoyle body
            BaseSoundID    = 372;   // Gargoyle sound
            Hue            = UniqueHue;

            // —— Enhanced Stats ——
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1200, 1500);
            SetStam(300, 350);
            SetMana(800, 1000);

            SetDamage(20, 30);

            // —— Damage Types ——
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Fire,      10);
            SetDamageType(ResistanceType.Cold,     15);
            SetDamageType(ResistanceType.Poison,   10);
            SetDamageType(ResistanceType.Energy,   50);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   40, 50);
            SetResistance(ResistanceType.Energy,   90, 100);

            // —— Skills ——
            SetSkill(SkillName.EvalInt,      120.0, 135.0);
            SetSkill(SkillName.Magery,       120.0, 135.0);
            SetSkill(SkillName.MagicResist,  130.0, 145.0);
            SetSkill(SkillName.Meditation,   110.0, 120.0);
            SetSkill(SkillName.Tactics,       95.0, 105.0);
            SetSkill(SkillName.Wrestling,     95.0, 105.0);

            Fame        = 25000;
            Karma       = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize ability timers
            m_NextTemporalWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextNanoSwarm    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextVaultBreach  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextBeamBarrage  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Loot: advanced reagents & vault tech scraps
            PackItem(new BlackPearl( Utility.RandomMinMax(15, 20) ));
            PackItem(new SulfurousAsh( Utility.RandomMinMax(15, 20) ));
            PackItem(new SpidersSilk( Utility.RandomMinMax(15, 20) ));
            PackItem(new VoidEssence(   Utility.RandomMinMax(5, 10) )); // custom reagent
        }

        // —— Aura: Temporal Drain on approach ——
        public override void OnMovement( Mobile m, Point3D oldLoc )
        {
            base.OnMovement(m, oldLoc);

            if (m != null && m != this && m.Map == Map && Alive && m.InRange(Location, 3) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Drain stamina & mana
                    int drain = Utility.RandomMinMax(15, 25);
                    if (target.Stam >= drain) target.Stam -= drain;
                    if (target.Mana >= drain) target.Mana -= drain;

                    target.SendMessage(0x22, "You feel your life essence warped by the Imperium Watcher!");
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    PlaySound(0x1F8);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Leave a Nano‑Tile trail behind
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                Point3D old = m_LastLocation;
                m_LastLocation = Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    PoisonTile tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }

            // Ability priorities
            DateTime now = DateTime.UtcNow;
            double dist = (Combatant as Mobile)?.GetDistanceToSqrt(this) ?? double.MaxValue;

            if (now >= m_NextBeamBarrage && dist <= 12)
            {
                EnergyBeamBarrage();
                m_NextBeamBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (now >= m_NextVaultBreach && dist <= 14)
            {
                VaultBreach();
                m_NextVaultBreach = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextTemporalWave && dist <= 8)
            {
                TemporalWave();
                m_NextTemporalWave = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextNanoSwarm && dist <= 6)
            {
                NanoSwarm();
                m_NextNanoSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            }
        }

        // —— Temporal Wave: slows & damages in AoE ——
        private void TemporalWave()
        {
            if (Map == null) return;
            Say("*Time fractures!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 12, 64, UniqueHue, 0, 5039, 0
            );

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);
                AOS.Damage(t, this, Utility.RandomMinMax(40, 60), 20, 0, 0, 0, 80);
                t.SendMessage(0x35, "Your movements slow as time itself warps!");
                t.Paralyze(TimeSpan.FromSeconds(2));
            }
        }

        // —— Nano Swarm: drops bio‑hazard tiles around target ——
        private void NanoSwarm()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Deploying nano‑shells!*");
            PlaySound(0x22F);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    Point3D loc = target.Location;
                    int dx = Utility.RandomMinMax(-1, 1), dy = Utility.RandomMinMax(-1, 1);
                    Point3D drop = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                    if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                        drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(drop, Map);
                });
            }
        }

        // —— Vault Breach: opens a temporary portal hazard ——
        private void VaultBreach()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Vault integrity compromised!*");
            PlaySound(0x20A);
            Point3D loc = target.Location;

            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 8, 20, UniqueHue, 0, 5039, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (Map == null) return;

                // Create a chaotic teleport tile
                ChaoticTeleportTile tile = new ChaoticTeleportTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x1F6);
            });
        }

        // —— Energy Beam Barrage: chain‑lightning style ——
        private void EnergyBeamBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Imperium energy unleashed!*");
            PlaySound(0x20A);

            var chain = new List<Mobile> { initial };
            int max = 5, range = 6;

            for (int i = 0; i < max; i++)
            {
                Mobile last = chain[chain.Count - 1], next = null;
                double best = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !chain.Contains(m) && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < best) { best = d; next = m; }
                    }
                }

                if (next != null) chain.Add(next);
                else break;
            }

            for (int i = 0; i < chain.Count; i++)
            {
                Mobile src = (i == 0) ? this : chain[i - 1];
                Mobile tgt = chain[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100
                );

                var dmgTarget = tgt;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(dmgTarget, false))
                    {
                        DoHarmful(dmgTarget);
                        AOS.Damage(dmgTarget, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                        dmgTarget.FixedParticles(0x374A, 5, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // —— Death Effect: massive vault detonation & lingering hazards ——
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*System failure... purge initiated!*");
                Effects.PlaySound(Location, Map, 0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 80, UniqueHue, 0, 5052, 0
                );

                // Spawn random hazard tiles
                int count = Utility.RandomMinMax(5, 8);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                    Point3D drop = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                        drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                    FlamestrikeHazardTile lava = new FlamestrikeHazardTile();
                    lava.Hue = UniqueHue;
                    lava.MoveToWorld(drop, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(drop, Map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, UniqueHue, 0, 5039, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        // —— Standard overrides & loot ——
        public override bool BleedImmune    { get { return true; } }
        public override int  TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus      { get { return 75.0;  } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03) // 3% unique vault artifact
                PackItem(new MaxxiaScroll()); // placeholder for an artifact
        }

        public ImperiumWatcher(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            m_NextTemporalWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10,15));
            m_NextNanoSwarm    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8,12));
            m_NextVaultBreach  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20,30));
            m_NextBeamBarrage  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25,35));
            m_LastLocation     = this.Location;
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a rockbound spectral corpse")]
    public class RockboundSpectral : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextTremorTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextQuakeTime;
        private Point3D m_LastLocation;

        // Unique Hue – a spectral earthen glow
        private const int UniqueHue = 1157;

        [Constructable]
        public RockboundSpectral()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Rockbound Spectral";
            Body = 26;                 // Same as Spectre
            BaseSoundID = 0x482;       // Same ghostly sound
            Hue = UniqueHue;

            // ——— Attributes ———
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(150, 200);

            SetHits(1200, 1400);
            SetStam(250, 300);
            SetMana(300, 400);

            SetDamage(25, 35);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            // ——— Skills ———
            SetSkill(SkillName.Wrestling, 95.1, 110.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 100.2, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            // Leave a trail of hazardous earth occasionally
            m_LastLocation = this.Location;

            // Basic loot: iron ore & gems
            PackItem(new IronOre(Utility.RandomMinMax(20, 40)));
            PackGem();
            PackGem();
        }

        // ——— Movement: Leave spectral landmines behind ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                Point3D drop = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                    drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(drop, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Tremor Stomp: close-range AoE
            if (DateTime.UtcNow >= m_NextTremorTime && InRange(Combatant.Location, 4))
            {
                TremorStomp();
                m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Spectral Shard Burst: medium-range burst
            else if (DateTime.UtcNow >= m_NextShardTime && InRange(Combatant.Location, 8))
            {
                ShardBurst();
                m_NextShardTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            }
            // Unstable Quake: targeted ground hazard
            else if (DateTime.UtcNow >= m_NextQuakeTime && InRange(Combatant.Location, 12))
            {
                UnstableQuake();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // ——— Ability: Tremor Stomp ———
        private void TremorStomp()
        {
            this.Say("*The earth trembles!*");
            PlaySound(0x2F3);

            // Use CenterFeet instead of non-existent 'Feet'
            // Correct parameter order: (itemID, speed, duration, hue, renderMode, effectID, layer)
            FixedParticles(
                0x376A,      // earth shock art
                20,          // speed
                30,          // duration
                UniqueHue,   // hue
                0,           // render mode
                5032,        // effect tile
                EffectLayer.CenterFeet
            );

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 4))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                // Brief stun
                m.Freeze(TimeSpan.FromSeconds(1.0));

                // Stun effect above head
                m.FixedParticles(
                    0x3728,      // stun art
                    10,
                    15,
                    UniqueHue,   // hue
                    0,           // render mode
                    5032,        // effect tile
                    EffectLayer.Head
                );
            }
        }

        // ——— Ability: Spectral Shard Burst ———
        private void ShardBurst()
        {
            this.Say("*Shatter beneath you!*");
            PlaySound(0x20A);

            var shardTargets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    shardTargets.Add(m);
            }

            foreach (var m in shardTargets)
            {
                // Visual projectile from self to target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, m.Location, Map),
                    0x36BD,       // shard art
                    5, 0, false, false,
                    UniqueHue,    // hue
                    0,            // render mode
                    9502,         // effect ID on arrival
                    1,            // explode effect
                    0,            // explode sound
                    EffectLayer.Waist,  // layer
                    0             // unknown
                );

                Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
                {
                    if (CanBeHarmful(m, false))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 70, 0, 0, 0, 30);

                        m.FixedParticles(
                            0x36BD,      // hit effect
                            10,
                            15,
                            UniqueHue,   // hue
                            0,           // render mode
                            9502,        // effect tile
                            EffectLayer.Head
                        );
                    }
                });
            }
        }

        // ——— Ability: Unstable Quake ———
        private void UnstableQuake()
        {
            if (Combatant is Mobile target)
            {
                this.Say("*A fissure opens!*");
                PlaySound(0x22F);

                var loc = target.Location;
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3779, 10, 30, UniqueHue, 0, 5039, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    Point3D spawn = loc;
                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                    var quake = new EarthquakeTile();
                    quake.Hue = UniqueHue;
                    quake.MoveToWorld(spawn, Map);

                    Effects.PlaySound(spawn, Map, 0x29D);
                });
            }
        }

        // ——— Death Effect: Shattering Rupture ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Fragments... of stone and soul...*");
                Effects.PlaySound(Location, Map, 0x2F3);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 20, 60, UniqueHue, 0, 5052, 0);

                // Spawn several landmine hazards around corpse
                for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;

                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }

            base.OnDeath(c);
        }

        // ——— Loot & Properties ———
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override TribeType Tribe     => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override int TreasureMapLevel       => 6;
        public override double DispelDifficulty    => 145.0;
        public override double DispelFocus         => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // 3% chance for a unique core
            if (Utility.RandomDouble() < 0.03)
                PackItem(new NightstepThreads());
        }

        // ——— Serialization ———
        public RockboundSpectral(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init cooldowns after load
            m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation   = this.Location;
        }
    }
}

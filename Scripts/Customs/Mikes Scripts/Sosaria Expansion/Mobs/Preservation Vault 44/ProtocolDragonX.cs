using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a protocol dragon X corpse")]
    public class ProtocolDragonX : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextProtocolBreath;
        private DateTime m_NextTemporalDischarge;
        private DateTime m_NextQuantumRift;
        private DateTime m_NextElectroStorm;
        private Point3D m_LastLocation;

        // Unique obsidian‑red hue
        private const int UniqueHue = 1266;

        [Constructable]
        public ProtocolDragonX() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Protocol Dragon X";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // ——— Core Stats ———
            SetStr(1200, 1600);
            SetDex(200, 300);
            SetInt(1200, 1500);

            SetHits(2000, 3000);
            SetStam(400, 500);
            SetMana(1000, 1200);

            SetDamage(30, 40);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire,     30);
            SetDamageType(ResistanceType.Energy,   30);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire,     80,100);
            SetResistance(ResistanceType.Cold,     60, 80);
            SetResistance(ResistanceType.Poison,   50, 70);
            SetResistance(ResistanceType.Energy,   85, 95);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,     120.0, 130.0);
            SetSkill(SkillName.Magery,      120.0, 130.0);
            SetSkill(SkillName.MagicResist, 125.0, 135.0);
            SetSkill(SkillName.Meditation,  110.0, 120.0);
            SetSkill(SkillName.Tactics,     110.0, 120.0);
            SetSkill(SkillName.Wrestling,   110.0, 120.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextProtocolBreath    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextTemporalDischarge = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuantumRift       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextElectroStorm      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // Standard loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 25)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 25)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
        }

        // ——— Aura on Movement ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (Alive && m.Map == this.Map && m.InRange(Location, 3) && CanBeHarmful(m, false) && m is Mobile target)
            {
                DoHarmful(target);

                int stamDrain = Utility.RandomMinMax(5, 15);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.SendMessage("The dragon's protocol aura saps your strength!");
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x212);
                }
            }

            // Leave an EarthquakeTile behind ~20% of the time when moving
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var oldLoc = m_LastLocation;
                m_LastLocation = Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    var quake = new EarthquakeTile();
                    quake.Hue = UniqueHue;
                    quake.MoveToWorld(oldLoc, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }
        }

        // ——— Brain: decide when to use each special ———
        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            var now = DateTime.UtcNow;
            var range = Combatant.Location;

            if (now >= m_NextProtocolBreath && InRange(range, 8))
            {
                ProtocolFlameBreath();
                m_NextProtocolBreath = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }

            if (now >= m_NextTemporalDischarge && InRange(range, 6))
            {
                TemporalDischargeAttack();
                m_NextTemporalDischarge = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }

            if (now >= m_NextQuantumRift && InRange(range, 12))
            {
                QuantumRiftAttack();
                m_NextQuantumRift = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }

            if (now >= m_NextElectroStorm)
            {
                ElectroStormAttack();
                m_NextElectroStorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
        }

        // ——— 1) Protocol Flame Breath — cone AoE, mixed damage ———
        public void ProtocolFlameBreath()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Protocol Overload!*");
                PlaySound(0x5D1);
                FixedParticles(0x373A, 1, 30, 9504, UniqueHue, 0, EffectLayer.Head);

                var victims = new List<Mobile>();
                foreach (var m in Map.GetMobilesInRange(Location, 3))
                    if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        victims.Add(m);

                foreach (var v in victims)
                {
                    DoHarmful(v);
                    // 20% Physical, 30% Fire, 50% Energy
                    AOS.Damage(v, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 0, 100);
                }
            }
        }

        // ——— 2) Temporal Discharge — AoE + brief stun ———
        public void TemporalDischargeAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Temporal rupture!*");
            PlaySound(0x2D6);
            FixedParticles(0x376A, 10, 30, 5032, UniqueHue, 0, EffectLayer.CenterFeet);

            var hits = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 5))
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    hits.Add(m);

            foreach (var m in hits)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 100, 0, 0);
                if (m is Mobile mob) mob.Paralyze(TimeSpan.FromSeconds(1.5));
                m.SendMessage("Time itself seems to halt around you!");
            }
        }

        // ——— 3) Quantum Rift — spawns a damaging VortexTile at the target location ———
        public void QuantumRiftAttack()
        {
            if (!(Combatant is Mobile) || Map == null) return;

            Say("*Rift the protocol!*");
            PlaySound(0x22F);

            Point3D loc = Combatant.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;
                var spawn = loc;
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var vortex = new VortexTile();
                vortex.Hue = UniqueHue;
                vortex.MoveToWorld(spawn, Map);
                Effects.PlaySound(spawn, Map, 0x1F6);
            });
        }

        // ——— 4) Electro Storm — rains down LightningStormTiles around foe ———
        public void ElectroStormAttack()
        {
            if (!(Combatant is Mobile target) || Map == null) return;

            Say("*Initiating storm protocol!*");
            PlaySound(0x226);

            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var drop = new Point3D(target.X + dx, target.Y + dy, target.Z);

                if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                    drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                var storm = new LightningStormTile();
                storm.Hue = UniqueHue;
                storm.MoveToWorld(drop, Map);
            }
        }

        // ——— Death Event — carpet‑bomb of hazards ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*Protocol... reset...*");
            Effects.PlaySound(Location, Map, 0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            var hazards = new Type[]
            {
                typeof(ManaDrainTile), typeof(EarthquakeTile),
                typeof(PoisonTile), typeof(FlamestrikeHazardTile)
            };

            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var p = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                var type = hazards[Utility.Random(hazards.Length)];
                var tile = (Item)Activator.CreateInstance(type);
                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        // ——— Immunities & Loot ———
        public override bool BleedImmune      => true;
        public override int TreasureMapLevel  => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      =>  80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03) // 3% chance for unique artifact
                PackItem(new MaxxiaScroll()); // replace with your real high‑end artifact
        }

        // ——— Boilerplate Serialization ———
        public ProtocolDragonX(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            var now = DateTime.UtcNow;
            m_NextProtocolBreath    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextTemporalDischarge = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuantumRift       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextElectroStorm      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}

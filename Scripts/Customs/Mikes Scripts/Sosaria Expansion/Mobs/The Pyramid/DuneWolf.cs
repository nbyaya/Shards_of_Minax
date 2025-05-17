using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // for ValidIndirectTarget
using Server.Spells.Seventh; // if you want to borrow effects
using Server.Regions;     // for map checks, if needed

namespace Server.Mobiles
{
    [CorpseName("a dune wolf corpse")]
    public class DuneWolf : BaseCreature
    {
        private DateTime m_NextSiroccoTime;
        private DateTime m_NextQuicksandTime;
        private DateTime m_NextShardTime;
        private Point3D m_LastLocation;

        // A sandy‑gold hue
        private const int UniqueHue = 2214;

        [Constructable]
        public DuneWolf() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a dune wolf";
            Body = 739;
            BaseSoundID = 0xE5;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(150, 180);
            SetInt(120, 150);

            SetHits(100, 150);
            SetStam(200, 250);
            SetMana(200, 250);

            SetDamage(20, 30);

            // Damage types: mix of Physical & Fire
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Meditation, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 120;
            ControlSlots = 5;

            // Set up ability timers
            m_NextSiroccoTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuicksandTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // Thematic loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
        }

        // Heat Aura: drains stamina + does minor fire damage to anyone who moves within 3 tiles
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    int stamDrain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x22, "The desert’s heat sears your strength!");
                        target.FixedParticles(0x3728, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x208);
                    }

                    // Minor fire damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Sirocco Blast: AoE fire/physical around self
            if (DateTime.UtcNow >= m_NextSiroccoTime && this.InRange(Combatant.Location, 10))
            {
                SiroccoBlast();
                m_NextSiroccoTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Quicksand Rift: places a QuicksandTile under the target
            else if (DateTime.UtcNow >= m_NextQuicksandTime)
            {
                QuicksandRift();
                m_NextQuicksandTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Sand Shard Barrage: chain‑bounce shard bolts
            else if (DateTime.UtcNow >= m_NextShardTime && this.InRange(Combatant.Location, 12))
            {
                SandShardBarrage();
                m_NextShardTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        private void SiroccoBlast()
        {
            if (Map == null) return;

            this.Say("*Feel the desert winds!*");
            PlaySound(0x228);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 30, 0, 0, 0, 70); 
                    m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
            eable.Free();
        }

        private void QuicksandRift()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Point3D loc = target.Location;
                this.Say("*The sands claim you!*");
                PlaySound(0x22F);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                    0x3728, 8, 20, UniqueHue, 0, 5035, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;

                    Point3D spawn = loc;
                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    {
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);
                        if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                            return;
                    }

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(spawn, this.Map);

                    Effects.PlaySound(spawn, this.Map, 0x1F6);
                });
            }
        }

        private void SandShardBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false) || Map == null)
                return;

            this.Say("*Shards of the dunes!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            int maxBounces = 5, range = 5;

            // Gather bounce targets
            for (int i = 0; i < maxBounces; i++)
            {
                Mobile last = targets[targets.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                var inRange = Map.GetMobilesInRange(last.Location, range);
                foreach (Mobile m in inRange)
                {
                    if (m != this && m != last && !targets.Contains(m)
                     && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }
                inRange.Free();

                if (next == null) break;
                targets.Add(next);
            }

            // Launch shard bolts
            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36E4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                var dmgTarget = dst;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(dmgTarget, false))
                    {
                        DoHarmful(dmgTarget);
                        AOS.Damage(dmgTarget, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
                        dmgTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // Death rifts: scatter quicksand hazards
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Let the sands rise!*");
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, UniqueHue, 0, 5052, 0);

                int count = Utility.RandomMinMax(4, 8);
                for (int i = 0; i < count; i++)
                {
                    int xo = Utility.RandomMinMax(-4, 4), yo = Utility.RandomMinMax(-4, 4);
                    var spawn = new Point3D(X + xo, Y + yo, Z);

                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    {
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);
                        if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                            continue;
                    }

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(spawn, this.Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(spawn, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // Standard overrides
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
        }

        // Serialization
        public DuneWolf(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑initialize timers
            m_NextSiroccoTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuicksandTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mineral water elemental corpse")]
    public class MineralWaterElemental : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextAcidMistTime;
        private DateTime m_NextSpikeTime;
        private DateTime m_NextSurgeTime;

        // Track last movement for ground effect
        private Point3D m_LastLocation;

        // Unique shimmering turquoise hue
        private const int UniqueHue = 1321;

        [Constructable]
        public MineralWaterElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "a mineral water elemental";
            Body = 16;               // same as basic Water Elemental
            BaseSoundID = 278;       // same as basic Water Elemental
            Hue = UniqueHue;

            // — Stats —
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetStam(150, 200);
            SetMana(400, 500);

            SetDamage(12, 18);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,    20, 30);
            SetResistance(ResistanceType.Cold,    70, 80);
            SetResistance(ResistanceType.Poison,  50, 60);
            SetResistance(ResistanceType.Energy,  40, 50);

            // — Skills —
            SetSkill(SkillName.EvalInt,     90.1, 100.0);
            SetSkill(SkillName.Magery,      90.1, 100.0);
            SetSkill(SkillName.MagicResist,100.2, 115.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);
            SetSkill(SkillName.Tactics,     80.1,  90.0);
            SetSkill(SkillName.Wrestling,   70.1,  80.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextAcidMistTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpikeTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSurgeTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Initial last location
            m_LastLocation = this.Location;

            // Loot: gems and average treasure
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus     { get { return 60.0; } }

        public override int TreasureMapLevel { get { return 6; } }

        public MineralWaterElemental(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers after load
            var now = DateTime.UtcNow;
            m_NextAcidMistTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpikeTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSurgeTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;
        }

        // Leave behind corrosive mineral runoff
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null) return;

            // Only when moving to a new tile
            if (this.Location != m_LastLocation)
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                // Try placing a PoisonTile (acid puddle)
                if (this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                }
                else
                {
                    int z = this.Map.GetAverageZ(loc.X, loc.Y);
                    if (this.Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    {
                        var tile = new PoisonTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (this.Combatant == null || this.Map == null || this.Map == Map.Internal || !this.Alive)
                return;

            var now = DateTime.UtcNow;

            // Acid Mist AoE
            if (now >= m_NextAcidMistTime && this.InRange(this.Combatant.Location, 6))
            {
                AcidMistAttack();
                m_NextAcidMistTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Crystal Spike Barrage
            else if (now >= m_NextSpikeTime && this.InRange(this.Combatant.Location, 12))
            {
                SpikeBarrageAttack();
                m_NextSpikeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 28));
            }
            // Mineral Healing Surge
            else if (now >= m_NextSurgeTime)
            {
                HealingSurge();
                m_NextSurgeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // 1) Corrosive Acid Mist — AoE poison damage + spawn PoisonTiles
        private void AcidMistAttack()
        {
            if (!(this.Combatant is Mobile primaryTarget) || !CanBeHarmful(primaryTarget, false))
                return;

            this.Say("*The waters churn with deadly acid!*");
            this.PlaySound(0x654);

            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5032, 0);

            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(this.Location, 5);
            foreach (var m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile)
                    targets.Add(m);
            }
            eable.Free();

            foreach (var m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(20, 40);
                AOS.Damage(m, this, damage, 0, 0, 0, damage, 0); // 100% poison

                // Spawn acid puddle
                var loc = m.Location;
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                }

                m.FixedParticles(0x374A, 8, 20, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // 2) Crystal Spike Barrage — chain-cold damage
        private void SpikeBarrageAttack()
        {
            if (!(this.Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Shards of mineral ice!*");
            this.PlaySound(0x1F0);

            var hits = new List<Mobile> { target };
            int maxBounces = 4, range = 6;

            // find bouncing targets
            for (int i = 0; i < maxBounces; i++)
            {
                var last = hits[hits.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                var neigh = Map.GetMobilesInRange(last.Location, range);
                foreach (var m in neigh)
                {
                    if (m != this && !hits.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }
                neigh.Free();

                if (next != null)
                    hits.Add(next);
                else
                    break;
            }

            // apply damage and effects
            for (int i = 0; i < hits.Count; i++)
            {
                var src = (i == 0 ? this : hits[i - 1]);
                var dst = hits[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                int dmg = Utility.RandomMinMax(25, 35);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, dmg, 0, dmg, dmg, 0, 0); // 50% cold / 50% physical
                        dst.FixedParticles(0x37B9, 5, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // 3) Mineral Healing Surge — self-heal + HealingPulseTile
        private void HealingSurge()
        {
            this.Say("*Waters of renewal!*");
            this.PlaySound(0x58A);

            int heal = Utility.RandomMinMax(100, 200);
            this.Hits += heal;
            this.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

            // spawn HealingPulseTile beneath self
            var loc = this.Location;
            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                var pulse = new HealingPulseTile();
                pulse.Hue = UniqueHue;
                pulse.MoveToWorld(loc, this.Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(2, 4));

            // 3% chance to drop a rare Mineral Core (placeholder item)
            if (Utility.RandomDouble() < 0.03)
                PackItem(new BeastwardShoulders());
        }
    }
}

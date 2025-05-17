using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a gloom knight corpse")]
    public class GloomKnight : BaseCreature
    {
        private DateTime m_NextAuraTime;
        private DateTime m_NextNecroticSlashTime;
        private DateTime m_NextTileRainTime;
        private Point3D m_LastLocation;

        // A deep shadowy hue
        private const int GloomHue = 1175;

        [Constructable]
        public GloomKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Gloom Knight";
            Body = 318;               // same body as DemonKnight
            BaseSoundID = 0x165;      // same base sound
            Hue = GloomHue;

            // Stats
            SetStr(600, 700);
            SetDex(120, 140);
            SetInt(300, 350);

            SetHits(40000);
            SetStam(200);
            SetMana(2000);

            SetDamage(20, 25);

            // Damage distribution
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Energy, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 85, 95);

            // Skills
            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Tactics,   110.0, 130.0);
            SetSkill(SkillName.MagicResist,125.0, 140.0);
            SetSkill(SkillName.DetectHidden,100.0);
            SetSkill(SkillName.Magery,     100.0, 120.0);
            SetSkill(SkillName.EvalInt,    100.0, 120.0);
            SetSkill(SkillName.Necromancy, 120.0, 140.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 70;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextAuraTime          = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextNecroticSlashTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextTileRainTime      = DateTime.UtcNow + TimeSpan.FromSeconds(20);

            m_LastLocation = this.Location;
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanFlee { get { return false; } }
        public override int TreasureMapLevel { get { return 6; } }

        public GloomKnight(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // 1) Shadow Aura: periodic mana & stam drain in close range
            if (DateTime.UtcNow >= m_NextAuraTime)
            {
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                ShadowAura();
            }

            // 2) Necrotic Slash: powerful melee ranged line attack
            if (DateTime.UtcNow >= m_NextNecroticSlashTime && InRange(Combatant.Location, 4))
            {
                m_NextNecroticSlashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                NecroticSlash();
            }

            // 3) Falling Gloom Tiles: drop hazardous ground effects as it moves
            if (this.Location != m_LastLocation)
            {
                m_LastLocation = this.Location;
                if (DateTime.UtcNow >= m_NextTileRainTime)
                {
                    m_NextTileRainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
                    TileRain();
                }
            }
        }

        // --- Shadow Aura: drains mana & stamina of all nearby targets ---
        private void ShadowAura()
        {
            this.Say("*The darkness within you weakens!*");
            this.PlaySound(0x482);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m, false) || !Alive) continue;
                if (SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);

                    int drainAmount = Utility.RandomMinMax(15, 30);
                    if (m.Mana >= drainAmount)
                    {
                        m.Mana -= drainAmount;
                        m.SendMessage(0x23, "You feel your magical vigor sapped by the Gloom Knight!");
                    }

                    if (m.Stam >= drainAmount)
                    {
                        m.Stam -= drainAmount;
                        m.SendMessage(0x23, "Your strength wanes under the knight's oppressive aura!");
                    }

                    // Visual effect
                    m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                }
            }
            eable.Free();
        }

        // --- Necrotic Slash: line attack that hits the Combatant and pierces through ---
        private void NecroticSlash()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Feel the bite of shadow!*");
            this.PlaySound(0x23D);

            // Determine line from self to target and hit everyone in line
            List<Mobile> toHit = new List<Mobile>();
            Point3D src = this.Location;
            Point3D dst = target.Location;

            foreach (Mobile m in Map.GetMobilesInRange(src, 8))
            {
                if (m == this || !CanBeHarmful(m, false)) continue;
                if (LineOfSightTest(src, dst, m.Location))
                {
                    toHit.Add(m);
                }
            }

            foreach (Mobile m in toHit)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, damage, 0, 0, 0, 50, 50); // 50% cold, 50% energy
                m.FixedParticles(0x3779, 20, 25, 1161, EffectLayer.Waist); // dark slash effect
            }
        }

        // Helper for line-of-fire check
        private bool LineOfSightTest(Point3D from, Point3D to, Point3D test)
        {
            return from.X == to.X && test.X == from.X && Math.Min(from.Y, to.Y) <= test.Y && test.Y <= Math.Max(from.Y, to.Y)
                || from.Y == to.Y && test.Y == from.Y && Math.Min(from.X, to.X) <= test.X && test.X <= Math.Max(from.X, to.X);
        }

        // --- Tile Rain: spawns a mix of hazardous tiles beneath random nearby locations ---
        private void TileRain()
        {
            int count = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < count; i++)
            {
                int rx = X + Utility.RandomMinMax(-5, 5);
                int ry = Y + Utility.RandomMinMax(-5, 5);
                int rz = Z;
                Point3D loc = new Point3D(rx, ry, rz);

                if (!Map.CanFit(rx, ry, rz, 16, false, false))
                    loc.Z = Map.GetAverageZ(rx, ry);

                // Randomly pick a ground hazard tile
                Item tile;
                switch (Utility.Random(4))
                {
                    case 0: tile = new PoisonTile();         break;
                    case 1: tile = new QuicksandTile();      break;
                    case 2: tile = new VortexTile();         break;
                    default: tile = new NecromanticFlamestrikeTile(); break;
                }

                tile.Hue = GloomHue;
                tile.MoveToWorld(loc, Map);
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Chance to retaliate with a burst of shadow spikes
            if (!willKill && Utility.RandomDouble() < 0.10 && from != null && from != this)
            {
                if (from is Mobile target && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int spikeDmg = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, spikeDmg, 0, 0, 0, 60, 40); // mostly energy
                    target.FixedParticles(0x3709, 10, 15, 5032, EffectLayer.Head);
                    PlaySound(0x1F9);
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("*My vigil... ends...*");
            this.PlaySound(0x2F2);

            // On-death gloom explosion
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 20, 30, GloomHue, 0, 5032, 0);
            
            // Scatter a few dark runes (tiles) around the corpse
            for (int i = 0; i < 4; i++)
            {
                int rx = X + Utility.RandomMinMax(-3, 3);
                int ry = Y + Utility.RandomMinMax(-3, 3);
                Point3D loc = new Point3D(rx, ry, Z);
                if (!Map.CanFit(rx, ry, Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(rx, ry);

                var rune = new ManaDrainTile();
                rune.Hue = GloomHue;
                rune.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.03) // 3% for unique weapon
                PackItem(new LegguardsOfTheCrashingLine()); // example unique drop
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

            // Reinitialize timers
            m_NextAuraTime          = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextNecroticSlashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextTileRainTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;
        }
    }
}

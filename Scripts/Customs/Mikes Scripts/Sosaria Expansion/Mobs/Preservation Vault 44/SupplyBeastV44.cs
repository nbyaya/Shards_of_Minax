using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;           // if you want future spell effects
using Server.ContextMenus;    // for context entries (optional)

namespace Server.Mobiles
{
    [CorpseName("a supply beast V-44 carcass")]
    public class SupplyBeastV44 : BaseCreature
    {
        // cooldown timers for special abilities
        private DateTime m_NextCrateBarrage;
        private DateTime m_NextOverclock;
        private DateTime m_NextChainShock;
        private Point3D m_LastLocation;

        // a vivid, factory‑industrial green
        private const int UniqueHue = 1154;

        [Constructable]
        public SupplyBeastV44() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name           = "Supply Beast V-44";
            Body           = 292;
            BaseSoundID    = 0x3F3;
            Hue            = UniqueHue;

            // —— Stats —— 
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(2000, 2300);
            SetStam(300, 350);
            SetMana(400, 450);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   60, 70);

            // —— Skills ——
            SetSkill(SkillName.Magery,       100.0, 110.0);
            SetSkill(SkillName.MagicResist,  120.0, 130.0);
            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,    100.0, 110.0);
            SetSkill(SkillName.EvalInt,      100.0, 110.0);
            SetSkill(SkillName.Meditation,   100.0, 110.0);

            Fame         = 25000;
            Karma        = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // initial cooldowns (staggered)
            m_NextCrateBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextOverclock     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChainShock    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // Replace the default backpack with a locked StrongBackpack
            Container pack = Backpack;
            if (pack != null)
                pack.Delete();
            pack = new StrongBackpack { Movable = false };
            AddItem(pack);

            // fill with random supply loot
            for (int i = 0; i < 10; i++)
                pack.DropItem(new HealingPotion());
            for (int i = 0; i < 5; i++)
                pack.DropItem(new GreaterExplosionPotion());
            pack.DropItem(new BagOfReagents(Utility.RandomMinMax(20, 30)));
        }

        // —— Aura: Supply Jam ——
        // Any foe within 2 tiles suffers slowed attacks & minor energy damage.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == this || m.Map != this.Map || !m.Alive)
                return;

            if (m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // slow effect
                    if (!target.Paralyzed)
                        target.Paralyze(TimeSpan.FromSeconds(1.5));

                    // Energy spark
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                    target.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            // leave supply crates behind randomly
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var crate = new LandmineTile();
                    crate.Hue = UniqueHue;
                    crate.MoveToWorld(loc, Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // cooldown‑driven specials
            if (DateTime.UtcNow >= m_NextCrateBarrage && Combatant is Mobile)
            {
                CrateBarrage();
                m_NextCrateBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextOverclock && Combatant is Mobile)
            {
                Overclock();
                m_NextOverclock = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (DateTime.UtcNow >= m_NextChainShock && Combatant is Mobile)
            {
                ChainShock();
                m_NextChainShock = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // —— Special #1: Crate Barrage ——
        // Spawns a spread of random hazard tiles around the target
        public void CrateBarrage()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Deploying supply barrage!*");
            PlaySound(0x1FE);

            // drop 6–10 random hazard tiles in a 4‑tile radius
            var tiles = new Func<Item>[] {
                () => new PoisonTile(),
                () => new LandmineTile(),
                () => new VortexTile(),
                () => new IceShardTile(),
                () => new FlamestrikeHazardTile()
            };

            for (int i = 0; i < Utility.RandomMinMax(6, 10); i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(target.X + dx, target.Y + dy, target.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = tiles[Utility.Random(tiles.Length)]();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
            }
        }

        // —— Special #2: Overclock ——
        // Buffs self: heals and temporarily increases speed
        public void Overclock()
        {
            Say("*Systems overclocking!*");
            PlaySound(0x2F3);

            // heal a percentage of hits
            int heal = (int)(HitsMax * 0.20);
            Hits = Math.Min(Hits + heal, HitsMax);
            FixedParticles(0x374A, 20, 30, 9502, UniqueHue, 0, EffectLayer.Waist);

            // speed boost
            this.ActiveSpeed = 0.1;
            Timer.DelayCall(TimeSpan.FromSeconds(8.0), () => { if (Alive) this.ActiveSpeed = 0.2; });
        }

        // —— Special #3: Chain Shock ——
        // Bounces a lightning bolt between up to 4 nearby foes
        public void ChainShock()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Electrical chain link!*");
            PlaySound(0x1A3);

            var targets = new List<Mobile> { initial };
            int maxBounces = 4;
            int bounceRange = 5;

            for (int i = 0; i < maxBounces; i++)
            {
                var prev = targets[targets.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(prev.Location, bounceRange))
                {
                    if (m != this && !targets.Contains(m) && m.Alive && CanBeHarmful(m, false) && prev.InLOS(m))
                    {
                        double d = prev.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }

                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var from = (i == 0 ? this : targets[i - 1]);
                var to   = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, from.Location, from.Map),
                    new Entity(Serial.Zero, to.Location, to.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                int dmg = Utility.RandomMinMax(30, 45);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (to.Alive && CanBeHarmful(to, false))
                    {
                        DoHarmful(to);
                        AOS.Damage(to, this, dmg, 0, 0, 0, 0, 100);
                        to.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // —— On‑death supply cache explosion ——
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;
            Say("*Supplies... released...*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 30, UniqueHue, 0, 5052, 0);

            // drop 3–5 healing‑pulse tiles
            int count = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var pulse = new HealingPulseTile();
                pulse.Hue = UniqueHue;
                pulse.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));
        }

        public override bool BleedImmune    => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 145.0;
        public override double DispelFocus      => 75.0;

        public SupplyBeastV44(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // re‑initialize cooldowns on reload
            m_NextCrateBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextOverclock     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChainShock    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation = this.Location;
        }
    }
}

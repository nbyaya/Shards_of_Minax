using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an entropy elemental corpse")]
    public class EntropyElemental : BaseCreature
    {
        // Cooldowns for special attacks
        private DateTime m_NextAuraTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextCascadeTime;
        private Point3D  m_LastLocation;

        // Corrupted, flickering gray-green
        private const int EntropyHue = 1271;

        [Constructable]
        public EntropyElemental() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "an entropy elemental";
            Body = 112;
            BaseSoundID = 268;
            Hue = EntropyHue;

            // ——— Stats ———
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1600, 1800);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 25);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Cold,     25);
            SetDamageType(ResistanceType.Fire,     25);
            SetDamageType(ResistanceType.Poison,   15);
            SetDamageType(ResistanceType.Energy,   20);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   40, 50);
            SetResistance(ResistanceType.Energy,   80, 90);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,    120.0, 130.0);
            SetSkill(SkillName.Magery,     120.0, 130.0);
            SetSkill(SkillName.MagicResist,120.0, 140.0);
            SetSkill(SkillName.Meditation,110.0, 120.0);
            SetSkill(SkillName.Tactics,     95.0, 105.0);
            SetSkill(SkillName.Wrestling,   95.0, 105.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 85;
            ControlSlots = 6;

            // ——— Initialize cooldowns ———
            m_NextAuraTime    = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextRiftTime    = DateTime.UtcNow + TimeSpan.FromSeconds(12);
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(18);

            m_LastLocation = this.Location;

            // ——— Default loot ———
            PackItem(new Apple(Utility.RandomMinMax(5, 10))); // Entropy Fragments
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
        }

        // ——— Aura: corrodes armor & drains stamina of anyone moving near ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.UtcNow < m_NextAuraTime || m == this || !Alive || m.Map != this.Map || !m.InRange(this.Location, 2))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Drain Stamina
                int stamDrain = Utility.RandomMinMax(10, 20);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.SendMessage(0x22, "You feel your vigor decaying!");
                    target.FixedParticles(0x373A, 10, 15, 5025, EffectLayer.Head);
                    target.PlaySound(0x1E2);
                }

                // Minor corrosive damage
                AOS.Damage(target, this, Utility.RandomMinMax(8, 15), 100, 0, 0, 0, 0);

                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(6);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // leave a drifting EntropyMist behind
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                m_LastLocation = this.Location;
                var mist = new ToxicGasTile();
                mist.Hue = EntropyHue;
                mist.MoveToWorld(this.Location, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // ——— Corruption Rift ———
            if (DateTime.UtcNow >= m_NextRiftTime && this.InRange(Combatant.Location, 12))
            {
                CastCorruptionRift();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 20));
            }
            // ——— Chaos Cascade ———
            else if (DateTime.UtcNow >= m_NextCascadeTime && this.InRange(Combatant.Location, 10))
            {
                CastChaosCascade();
                m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // ——— Ability: tear reality & spawn random hazard tile under target ———
        private void CastCorruptionRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Reality frays!*");
            PlaySound(0x22F);

            var loc = target.Location;


            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (Map == null) return;

                // pick one of several hazard tiles at random
                Type[] tiles = new Type[] {
                    typeof(QuicksandTile),
                    typeof(PoisonTile),
                    typeof(ToxicGasTile),
                    typeof(LandmineTile),
                    typeof(FlamestrikeHazardTile)
                };

                Type tileType = tiles[Utility.Random(tiles.Length)];
                var tile = (Item)Activator.CreateInstance(tileType);
                tile.Hue = EntropyHue;

                // ensure valid placement
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                tile.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x208);
            });
        }

        // ——— Ability: bounce random elemental bolts among up to 6 targets ———
        private void CastChaosCascade()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false)) return;

            Say("*Chaos reigns!*");
            PlaySound(0x1FE);

            List<Mobile> hits = new List<Mobile> { initial };
            int maxBounces = 6;

            for (int i = 0; i < maxBounces; i++)
            {
                var last = hits[hits.Count - 1];
                Mobile next = null; double best = -1;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, 8))
                {
                    if (m != this && !hits.Contains(m) && m.Alive && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (best < 0 || d < best) { best = d; next = m; }
                    }
                }
                if (next == null) break;
                hits.Add(next);
            }

            for (int i = 0; i < hits.Count; i++)
            {
                var src = (i == 0 ? this : hits[i - 1]);
                var dst = hits[i];

                // pick a random damage type
                var dt = (ResistanceType)Utility.RandomList(
                    ResistanceType.Fire,
                    ResistanceType.Cold,
                    ResistanceType.Poison,
                    ResistanceType.Energy
                );

                int hueEffect = EntropyHue;
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, Map),
                    new Entity(Serial.Zero, dst.Location, Map),
                    0x36D4, 7, 0, false, false, hueEffect, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
                );

                int damage = Utility.RandomMinMax(30, 45);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, damage, dt == ResistanceType.Physical ? 100 : 0,
                                                 dt == ResistanceType.Fire     ? 100 : 0,
                                                 dt == ResistanceType.Cold     ? 100 : 0,
                                                 dt == ResistanceType.Poison   ? 100 : 0,
                                                 dt == ResistanceType.Energy   ? 100 : 0);
                        dst.FixedParticles(0x3779, 10, 15, 5032, EffectLayer.Head);
                    }
                });
            }
        }

        // ——— On death: massive entropic burst & random hazards ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*All returns to dust…*");
                Effects.PlaySound(Location, Map, 0x211);


                int count = Utility.RandomMinMax(5, 8);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    // spawn a random hazard tile
                    Type[] deathTiles = new Type[] {
                        typeof(EarthquakeTile),
                        typeof(ThunderstormTile),
                        typeof(NecromanticFlamestrikeTile),
                        typeof(ToxicGasTile),
                        typeof(QuicksandTile)
                    };
                    var tile = (Item)Activator.CreateInstance(
                        deathTiles[Utility.Random(deathTiles.Length)]
                    );
                    tile.Hue = EntropyHue;
                    tile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // ——— Properties ———
        public override bool BleedImmune      { get { return true; } }
        public override int  TreasureMapLevel { get { return 7;    } }
        public override double DispelDifficulty{ get { return 145.0;} }
        public override double DispelFocus     { get { return 75.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            // 3% chance at the legendary Shard of Unmaking
            if (Utility.RandomDouble() < 0.03)
                PackItem(new Trailpiercer());
        }

        public EntropyElemental(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // reset cooldowns
            m_NextAuraTime    = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextRiftTime    = DateTime.UtcNow + TimeSpan.FromSeconds(12);
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(18);
            m_LastLocation    = this.Location;
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;   // For Effects
using Server.Spells;    // For SpellHelper if needed

namespace Server.Mobiles
{
    [CorpseName("a one‑eyed servant corpse")]
    public class OneEyedServant : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextShadowBeam;
        private DateTime m_NextCurseCircle;
        private DateTime m_NextPsychicRing;
        private DateTime m_NextArcaneCone;
        private DateTime m_NextEldritchPulse;

        // Unique hue (ghostly violet)
        private const int UniqueHue = 1154;

        [Constructable]
        public OneEyedServant()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "One‑Eyed Servant";
            Body = 75;              // Cyclops body
            BaseSoundID = 604;      // Cyclops sounds
            Hue = UniqueHue;

            // Powerful stats
            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(600, 650);
            SetMana(300, 350);
            SetDamage(20, 30);

            // Mixed damage
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Tough resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;

            // Initialize cooldowns to now
            m_NextShadowBeam   = DateTime.UtcNow;
            m_NextCurseCircle  = DateTime.UtcNow;
            m_NextPsychicRing  = DateTime.UtcNow;
            m_NextArcaneCone   = DateTime.UtcNow;
            m_NextEldritchPulse= DateTime.UtcNow;
        }

        public OneEyedServant(Serial serial) : base(serial) { }

        public override int Meat => 10;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 8);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new TreasureMap(5, Map));
        }

        // --- SPECIAL ABILITIES ---

        // 1) Shadow Eye Beam: straight-line energy beam
        private void ShadowEyeBeamAttack()
        {
            if (Map == null || !(Combatant is Mobile target))
                return;

            const int range = 12;
            if (!Utility.InRange(Location, target.Location, range))
                return;

            // Direction vector
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            // Build beam path
            var points = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                var p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (Map.CanFit(p, 16, false, false))
                    points.Add(p);
                else
                {
                    int z2 = Map.GetAverageZ(p.X, p.Y);
                    var p2 = new Point3D(p.X, p.Y, z2);
                    if (Map.CanFit(p2, 16, false, false))
                        points.Add(p2);
                    else break;
                }
            }

            Effects.PlaySound(Location, Map, 0x4F5);                                        // eerie hum
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3728, 8, 8, UniqueHue, 0, 2023, 0);

            foreach (var p in points)
            {
                Effects.SendLocationEffect(p, Map, 0x3818, 12, UniqueHue, 0);
                var mobs = Map.GetMobilesInRange(p, 0);
                foreach (Mobile m in mobs)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 50, 0, 0, 0, 0, 100);
                    }
                }
                mobs.Free();
            }

            m_NextShadowBeam = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 2) Curse Circle: circular poison/necromantic AoE around self
        private void CurseCircle()
        {
            if (Map == null)
                return;

            const int radius = 6;
            Effects.PlaySound(Location, Map, 0x2F3);  // ominous hiss
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3709, 12, 30, UniqueHue, 0, 5029, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!Map.CanFit(p, 16, false, false))
                {
                    int z2 = Map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!Map.CanFit(p, 16, false, false))
                        continue;
                }

                // Spawn poison or necro flames
                if (Utility.RandomBool())
                {
                    // PoisonTile tile = new PoisonTile();
                    // tile.MoveToWorld(p, Map);
                }
                else
                {
                    // NecromanticFlamestrikeTile tile = new NecromanticFlamestrikeTile();
                    // tile.MoveToWorld(p, Map);
                }

                var mobs = Map.GetMobilesInRange(p, 0);
                foreach (Mobile m in mobs)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 30, 0, 0, 100, 0, 0);
                    }
                }
                mobs.Free();
            }

            m_NextCurseCircle = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Psychic Ring: ring‑shaped shockwave spawning vortexes
        private void PsychicRing()
        {
            if (Map == null)
                return;

            const int outer = 8, inner = 5;
            Effects.PlaySound(Location, Map, 0x215);  // psychic pulse
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3779, 1, 30, UniqueHue, 0, 9909, 0);

            for (int x = -outer; x <= outer; x++)
            for (int y = -outer; y <= outer; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
				int dx = p.X - Location.X;
				int dy = p.Y - Location.Y;
				int distSq = dx * dx + dy * dy;

				if (distSq < inner * inner || distSq > outer * outer)
					continue;


                if (!Map.CanFit(p, 16, false, false))
                {
                    int z2 = Map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!Map.CanFit(p, 16, false, false))
                        continue;
                }

                // VortexTile tile = new VortexTile();
                // tile.MoveToWorld(p, Map);

                var mobs = Map.GetMobilesInRange(p, 0);
                foreach (Mobile m in mobs)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 40, 0, 100, 0, 0, 0);
                    }
                }
                mobs.Free();
            }

            m_NextPsychicRing = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Arcane Cone: forward cone of wild magic; leaves lightning storms
        private void ArcaneCone()
        {
            if (Map == null || !(Combatant is Mobile target))
                return;

            const int maxDist = 8;
            // play breath-like cone effect
            Effects.PlaySound(Location, Map, BaseSoundID);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x36B0, 10, 10, UniqueHue, 0, 2023, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= maxDist; i++)
            {
                int width = (int)(i * 0.5);
                for (int off = -width; off <= width; off++)
                {
                    int tx = X + dx * i + (dy == 0 ? off : 0);
                    int ty = Y + dy * i + (dx == 0 ? off : 0);
                    var p = new Point3D(tx, ty, Z);

                    if (!Map.CanFit(p, 16, false, false))
                    {
                        int z2 = Map.GetAverageZ(p.X, p.Y);
                        p = new Point3D(p.X, p.Y, z2);
                        if (!Map.CanFit(p, 16, false, false)) continue;
                    }

                    // LightningStormTile storm = new LightningStormTile();
                    // storm.MoveToWorld(p, Map);

                    var mobs = Map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in mobs)
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 25, 0, 0, 0, 100, 0);
                        }
                    }
                    mobs.Free();
                }
            }

            m_NextArcaneCone = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 5) Eldritch Pulse: short‑range radial stomp spawning landmines
        private void EldritchPulse()
        {
            if (Map == null)
                return;

            const int radius = 4;
            Effects.PlaySound(Location, Map, 0x1F7);  // deep thrum
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x37C4, 1, 10, UniqueHue, 0, 9909, 0);

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!Map.CanFit(p, 16, false, false))
                {
                    int z2 = Map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!Map.CanFit(p, 16, false, false)) continue;
                }

                // LandmineTile mine = new LandmineTile();
                // mine.MoveToWorld(p, Map);

                var mobs = Map.GetMobilesInRange(p, 0);
                foreach (Mobile m in mobs)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 35, 100, 0, 0, 0, 0);
                    }
                }
                mobs.Free();
            }

            m_NextEldritchPulse = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // --- AI LOGIC ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextShadowBeam && InRange(target, 12))
                    ShadowEyeBeamAttack();
                else if (DateTime.UtcNow >= m_NextArcaneCone && InRange(target, 8))
                    ArcaneCone();
                else if (DateTime.UtcNow >= m_NextCurseCircle)
                    CurseCircle();
                else if (DateTime.UtcNow >= m_NextPsychicRing)
                    PsychicRing();
                else if (DateTime.UtcNow >= m_NextEldritchPulse)
                    EldritchPulse();
            }
        }

        // --- DEATH EXPLOSION ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x208);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3728, 16, 20, UniqueHue, 0, 2023, 0);

                const int pieces = 15, radius = 6;
                for (int i = 0; i < pieces; i++)
                {
                    var p = GetRandomSpawnLocation(Location, radius);
                    if (p == Point3D.Zero) continue;

                    // ToxicGasTile gas = new ToxicGasTile();
                    // gas.MoveToWorld(p, Map);

                    Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                  0x3709, 8, 12, UniqueHue, 0, 5052, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper for death spawn
        private Point3D GetRandomSpawnLocation(Point3D center, int radius)
        {
            for (int i = 0; i < 8; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (Map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
                int z2 = Map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);
                if (Map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    return p2;
            }
            return Point3D.Zero;
        }

        // --- SERIALIZATION ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

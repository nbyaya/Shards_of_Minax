using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;     // For Poison Field visuals
using Server.Spells.Seventh;   // For sound IDs / effects
using Server.Multis;           // For tile items

namespace Server.Mobiles
{
    [CorpseName("a tunnel rat corpse")]
    public class TunnelRat : BaseCreature
    {
        private DateTime m_NextCollapseTime;
        private DateTime m_NextSquealTime;
        private DateTime m_NextBurrowTime;
        private Point3D m_LastLocation;

        // Unique ochre‑brown hue for the Tunnel Rat
        private const int UniqueHue = 1175;

        [Constructable]
        public TunnelRat() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name           = "a Tunnel Rat";
            Body           = 0xD7;      // Giant rat body
            BaseSoundID    = 0x188;     // Giant rat sounds
            Hue            = UniqueHue;

            // ——— Stats ———
            SetStr(200, 250);
            SetDex(150, 180);
            SetInt(100, 120);

            SetHits(80, 150);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(10, 15);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison,   60);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   40, 50);

            // ——— Skills ———
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.Poisoning,   120.0, 130.0);
            SetSkill(SkillName.Anatomy,     100.0, 110.0);

            Fame           = 15000;
            Karma          = -15000;
            VirtualArmor   = 70;
            ControlSlots   = 4;

            // ——— Initialize ability cooldowns ———
            m_NextSquealTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextCollapseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextBurrowTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation     = this.Location;

            // ——— Basic Loot ———
            PackItem(new RawRibs(Utility.RandomMinMax(4, 8)));
            PackItem(new Hides(Utility.RandomMinMax(10, 15)));

            // Small chance for specialized drops
            if (Utility.RandomDouble() < 0.05)
                PackItem(new RatcatchersSash());
        }

        // ——— Leave landmines/poison pools as it scurries ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && this.Alive && Utility.RandomDouble() < 0.10)
            {
                // 10% chance per movement to drop a hazard behind
                Point3D drop = oldLocation;
                Map map = this.Map;

                if (map != null && map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                {
                    PoisonTile pool = new PoisonTile();
                    pool.Hue = UniqueHue;
                    pool.MoveToWorld(drop, map);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !this.Alive || this.Map == null || this.Map == Map.Internal)
                return;

            // Sonic Squeal — AoE stun/damage every ~15s
            if (DateTime.UtcNow >= m_NextSquealTime && this.InRange(Combatant.Location, 8))
            {
                SonicSqueal();
                m_NextSquealTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            // Tunnel Collapse — targeted ground quake every ~25s
            else if (DateTime.UtcNow >= m_NextCollapseTime && this.InRange(Combatant.Location, 6))
            {
                TunnelCollapseAttack();
                m_NextCollapseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Subterranean Burrow — disappear and re‑emerge behind target every ~30s
            else if (DateTime.UtcNow >= m_NextBurrowTime && this.InRange(Combatant.Location, 12))
            {
                SubterraneanBurrow();
                m_NextBurrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // ——— Sonic Squeal: Arcane piercing shriek ———
        private void SonicSqueal()
        {
            this.Say("*SQUEEAAAL!*");
            this.PlaySound(0x213);

            var targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);

                // Check for Mobile to access Stam
                if (t is Mobile targetMobile)
                {
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(targetMobile, this, damage, 100, 0, 0, 0, 0);

                    targetMobile.SendMessage(0x22, "The sonic shriek rattles your bones!");
                    // Slow effect: reduce stamina briefly
                    targetMobile.Stam = Math.Max(0, targetMobile.Stam - Utility.RandomMinMax(20, 35));
                    targetMobile.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }

            // Visual echo rings
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x376A, 8, 20, UniqueHue, 0, 5039, 0);
        }

        // ——— Tunnel Collapse: targeted quake + landmines ———
        private void TunnelCollapseAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("**The earth shakes beneath you!*");
            PlaySound(0x1F7);

            Point3D loc = target.Location;
            Map map = this.Map;

            // Pre‑quake flash
            Effects.SendLocationParticles(
                EffectItem.Create(loc, map, EffectItem.DefaultDuration),
                0x3728, 12, 10, UniqueHue, 0, 5039, 0);

            // Delay then spawn earthquake tiles in a 3×3 around the target
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    Point3D drop = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);
                    if (map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                    {
                        EarthquakeTile quake = new EarthquakeTile();
                        quake.Hue = UniqueHue;
                        quake.MoveToWorld(drop, map);
                    }
                }
            });
        }

        // ——— Subterranean Burrow: disappear & re‑spawn behind the target ———
        private void SubterraneanBurrow()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*scurries into the earth*");
            PlaySound(0x592); // burrow sound

            Point3D oldLoc = this.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(oldLoc, this.Map, EffectItem.DefaultDuration),
                0x3779, 10, 15, UniqueHue);

            // Teleport near the target (one tile behind)
            Point3D newLoc = target.Location;
            int tx = newLoc.X + Utility.RandomList(-1, 1);
            int ty = newLoc.Y + Utility.RandomList(-1, 1);
            int tz = newLoc.Z;

            if (!this.Map.CanFit(tx, ty, tz, 16, false, false))
                tz = this.Map.GetAverageZ(tx, ty);

            this.MoveToWorld(new Point3D(tx, ty, tz), this.Map);

            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue);
            PlaySound(0x593);
        }

        // ——— Death: toxic gas clouds ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("*...scurrers... silenced...*");
            PlaySound(0x1F1);

            var map = this.Map;
            var loc = this.Location;

            // Spawn a ring of toxic gas tiles around the corpse
            for (int i = 0; i < 6; i++)
            {
                double angle = i * (Math.PI * 2 / 6);
                int dx = (int)Math.Round(Math.Cos(angle) * 2);
                int dy = (int)Math.Round(Math.Sin(angle) * 2);
                Point3D drop = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                if (map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                {
                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(drop, map);
                }
            }
        }

        // ——— Standard Overrides ———
        public override bool BleedImmune    { get { return true; } }
        public override int  TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus      { get { return 60.0; } }
		
		public TunnelRat(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on load
            m_NextSquealTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextCollapseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextBurrowTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation     = this.Location;
        }
    }
}

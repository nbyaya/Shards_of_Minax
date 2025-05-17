using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a charred black dragon corpse")]
    public class BlackDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime _nextShadowFlameBreath;
        private DateTime _nextVoidSpiral;
        private DateTime _nextNecroticRing;
        private DateTime _nextAbyssalShockwave;
        private DateTime _nextCorruptingRain;

        // Unique Hue (dark purple–black)
        private const int UniqueHue = 1175;

        [Constructable]
        public BlackDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name            = "a black dragon";
            Body            = Utility.RandomList(12, 59);
            BaseSoundID     = 362;
            Hue             = UniqueHue;

            // Stats
            SetStr(1500, 1600);
            SetDex(120, 180);
            SetInt(900, 1000);

            SetHits(1200, 1400);
            SetDamage(40, 55);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire,     40, 55);
            SetResistance(ResistanceType.Cold,     80, 95);
            SetResistance(ResistanceType.Poison,   95, 100);
            SetResistance(ResistanceType.Energy,   80, 95);

            // Skills
            SetSkill(SkillName.EvalInt,     120.0, 140.0);
            SetSkill(SkillName.Magery,      120.0, 140.0);
            SetSkill(SkillName.Meditation,  100.0, 120.0);
            SetSkill(SkillName.MagicResist, 140.0, 180.0);
            SetSkill(SkillName.Tactics,     120.0, 140.0);
            SetSkill(SkillName.Wrestling,   120.0, 140.0);

            Fame         = 35000;
            Karma        = -40000;
            VirtualArmor = 90;

            // Initialize cooldowns to permit immediate use
            _nextShadowFlameBreath = DateTime.UtcNow;
            _nextVoidSpiral         = DateTime.UtcNow;
            _nextNecroticRing       = DateTime.UtcNow;
            _nextAbyssalShockwave   = DateTime.UtcNow;
            _nextCorruptingRain     = DateTime.UtcNow;
        }

        public BlackDragon(Serial serial)
            : base(serial)
        {
        }

        // Basic properties
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;
        public override bool CanFly             => true;

        public override HideType HideType       => HideType.Barbed;
        public override int      Hides          => 60;
        public override int      Meat           => 30;
        public override int      Scales         => 20;
        public override ScaleType ScaleType     => (ScaleType)Utility.Random(4);

        public override Poison PoisonImmune     => Poison.Lethal;
        public override Poison HitPoison        => Poison.Lethal;

        public override int TreasureMapLevel    => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems,      10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new TreasureMap(6, Map));
        }

        // --- Special Abilities ---

        // 1) Shadow Flame Breath: wide cone of dark fire + Cold
        private void ShadowFlameBreath()
        {
            if (Map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int range = 10;
            if (!this.InRange(target, range)) return;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int coneWidth = 5;
            Effects.PlaySound(Location, Map, 0x21F);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3709, 8, 20, UniqueHue, 0, 5023, 0);

            for (int i = 1; i <= range; i++)
            {
                for (int offset = -coneWidth; offset <= coneWidth; offset++)
                {
                    int x = X + i * dx + ((dy == 0) ? offset : 0);
                    int y = Y + i * dy + ((dx == 0) ? offset : 0);
                    Point3D p = new Point3D(x, y, Z);

                    if (!Map.CanFit(p, 16, false, false))
                        p = new Point3D(x, y, Map.GetAverageZ(x, y));

                    Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                  0x3709, 6, 10, UniqueHue, 0, 5024, 0);

                    foreach (Mobile m in Map.GetMobilesInRange(p, 0))
                    {
                        if (m == this || (! (m is PlayerMobile) && ! (m is BaseCreature))) continue;
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 40, 40, 0, 0, 0); // 40% fire, 40% cold
                    }
                }
            }

            _nextShadowFlameBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Void Spiral: spawns a spiral of vortex tiles
        private void VoidSpiral()
        {
            if (Map == null) return;

            Effects.PlaySound(Location, Map, 0x4F1);
            for (int t = 0; t < 12; t++)
            {
                double angle = t * (Math.PI * 2 / 12);
                int radius = 1 + (t / 3);
                int x = X + (int)(Math.Cos(angle) * radius);
                int y = Y + (int)(Math.Sin(angle) * radius);
                Point3D p = new Point3D(x, y, Z);

                if (!Map.CanFit(p, 16, false, false))
                    p = new Point3D(x, y, Map.GetAverageZ(x, y));

                // Spawn VortexTile
                // new VortexTile().MoveToWorld(p, Map);

                Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                              0x3779, 5, 10, UniqueHue, 0, 9910, 0);
            }

            _nextVoidSpiral = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Necrotic Ring: outward shock ring
        private void NecroticRing()
        {
            if (Map == null) return;

            int radius = 6;
            Effects.PlaySound(Location, Map, 0x307);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3728, 10, radius * 2, UniqueHue, 0, 2035, 0);

            foreach (Mobile m in Map.GetMobilesInRange(Location, radius))
            {
                if (m == this || (!(m is PlayerMobile) && !(m is BaseCreature))) continue;
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(25, 35), 0, 0, 50, 50, 0); // 50% poison, 50% energy
                // Spawn PoisonTile
                // new PoisonTile().MoveToWorld(m.Location, Map);
            }

            _nextNecroticRing = DateTime.UtcNow + TimeSpan.FromSeconds(16);
        }

        // 4) Abyssal Shockwave: directional line attack
        private void AbyssalShockwave()
        {
            if (Map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int length = 12;
            if (!this.InRange(target, length)) return;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, Map, 0x5C3);
            for (int i = 1; i <= length; i++)
            {
                Point3D p = new Point3D(X + i * dx, Y + i * dy, Z);
                if (!Map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, Map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                              0x3818, 8, 12, UniqueHue, 0, 2025, 0);

                foreach (Mobile m in Map.GetMobilesInRange(p, 0))
                {
                    if (m == this || (!(m is PlayerMobile) && !(m is BaseCreature))) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, 45, 0, 0, 0, 0, 100); // energy
                }
            }

            _nextAbyssalShockwave = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 5) Corrupting Rain: random poison clouds around target
        private void CorruptingRain()
        {
            if (Map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(target.Location, Map, 0x54F);
            for (int i = 0; i < 20; i++)
            {
                int x = target.X + Utility.RandomMinMax(-5, 5);
                int y = target.Y + Utility.RandomMinMax(-5, 5);
                Point3D p = new Point3D(x, y, target.Z);

                if (!Map.CanFit(p, 16, false, false))
                    p = new Point3D(x, y, Map.GetAverageZ(x, y));

                // new PoisonTile().MoveToWorld(p, Map);

                Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                              0x36B0, 6, 8, UniqueHue, 0, 2032, 0);

                foreach (Mobile m in Map.GetMobilesInRange(p, 0))
                {
                    if (m == this || (!(m is PlayerMobile) && !(m is BaseCreature))) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    // m.ApplyPoison(this, Poison.Deadly);
                }
            }

            _nextCorruptingRain = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= _nextShadowFlameBreath && this.InRange(target, 10))
                    ShadowFlameBreath();
                else if (DateTime.UtcNow >= _nextVoidSpiral)
                    VoidSpiral();
                else if (DateTime.UtcNow >= _nextNecroticRing)
                    NecroticRing();
                else if (DateTime.UtcNow >= _nextAbyssalShockwave && this.InRange(target, 12))
                    AbyssalShockwave();
                else if (DateTime.UtcNow >= _nextCorruptingRain)
                    CorruptingRain();
            }
        }

        // --- OnDeath Explosion ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(Location, Map, 0x214);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3709, 12, 60, UniqueHue, 0, 5027, 0);

            for (int i = 0; i < 30; i++)
            {
                int x = X + Utility.RandomMinMax(-7, 7);
                int y = Y + Utility.RandomMinMax(-7, 7);
                Point3D p = new Point3D(x, y, Z);

                if (!Map.CanFit(p, 16, false, false))
                    p = new Point3D(x, y, Map.GetAverageZ(x, y));

                if (Utility.RandomBool())
                    /* new NecromanticFlamestrikeTile().MoveToWorld(p, Map); */
                    Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                  0x3709, 8, 20, UniqueHue, 0, 5030, 0);
                else
                    /* new VortexTile().MoveToWorld(p, Map); */
                    Effects.SendLocationParticles(EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                                                  0x376A, 5, 15, UniqueHue, 0, 9912, 0);
            }

            base.OnDeath(c);
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

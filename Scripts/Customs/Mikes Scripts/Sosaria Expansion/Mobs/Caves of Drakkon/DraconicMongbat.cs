using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // If you want to use SpellHelper etc.

namespace Server.Mobiles
{
    [CorpseName("a draconic mongbat corpse")]
    public class DraconicMongbat : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextSonicScreech;
        private DateTime m_NextWingGust;
        private DateTime m_NextBloodStorm;
        private DateTime m_NextChaosCross;
        private DateTime m_NextShadowFlare;

        // Unique violet hue for the Draconic Mongbat
        private const int UniqueHue = 1175;

        [Constructable]
        public DraconicMongbat()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Draconic Mongbat";
            Body = 39;
            BaseSoundID = 422;
            Hue = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(300, 350);
            SetInt(400, 450);

            SetHits(600, 650);
            SetStam(300, 350);
            SetMana(400, 450);

            SetDamage(20, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.EvalInt,    90.0, 100.0);
            SetSkill(SkillName.Magery,     90.0, 100.0);
            SetSkill(SkillName.Meditation, 80.0,  90.0);
            SetSkill(SkillName.MagicResist,100.0,120.0);
            SetSkill(SkillName.Tactics,    80.0,  90.0);
            SetSkill(SkillName.Wrestling,  80.0,  90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;

            // Initialize cooldowns
            m_NextSonicScreech = DateTime.UtcNow;
            m_NextWingGust     = DateTime.UtcNow;
            m_NextBloodStorm   = DateTime.UtcNow;
            m_NextChaosCross   = DateTime.UtcNow;
            m_NextShadowFlare  = DateTime.UtcNow;
        }

        public DraconicMongbat(Serial serial)
            : base(serial)
        {
        }

        // --- Basic properties ---
        public override bool CanFly                 { get { return true; } }
        public override bool ReacquireOnMovement    { get { return true; } }
        public override bool AutoDispel             { get { return false; } }

        public override HideType HideType           { get { return HideType.Spined; } }
        public override int Hides                   { get { return 20; } }
        public override int Meat                    { get { return 5; } }
        public override FoodType FavoriteFood       { get { return FoodType.Meat; } }

        public override Poison PoisonImmune         { get { return Poison.Lethal; } }
        public override Poison HitPoison            { get { return Poison.Deadly; } }

        public override int TreasureMapLevel        { get { return 5; } }

        public override int GetIdleSound()          { return BaseSoundID; }
        public override int GetHurtSound()          { return BaseSoundID; }
        public override int GetDeathSound()         { return BaseSoundID; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 8);

            // Very rare unique cloak
            if (Utility.RandomDouble() < 0.005)
                PackItem(new MongbatSummoningCloak());

            // Chance at a level‑5 treasure map
            if (Utility.RandomDouble() < 0.10)
                PackItem(new TreasureMap(5, Map));
        }

        // --- Special Abilities ---

        // 1) Sonic Screech — radial pulse around self
        public void SonicScreechAttack()
        {
            var map = this.Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x376A, 10, 30, UniqueHue, 0, 2061, 0
            );

            int radius = 7, damage = 25;
            foreach (Mobile m in map.GetMobilesInRange(Location, radius))
            {
                if (m == this) continue;
                if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, damage, 50, 0, 0, 50, 0);
                    m.SendMessage("A deafening screech rattles your bones!");
                }
            }

            m_NextSonicScreech = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Wing Gust — cone blast in facing direction
        public void WingGustAttack()
        {
            var map = this.Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3718, 8, 20, UniqueHue, 0, 3601, 0
            );

            // Determine cone
            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            int coneRange = 10, coneWidth = 4, damage = 30;
            var coneLocs = new List<Point3D>();

            for (int i = 1; i <= coneRange; i++)
            {
                int half = (int)(coneWidth * (coneRange - i + 1) / (double)coneRange);
                for (int j = -half; j <= half; j++)
                {
                    int x = X + dx * i, y = Y + dy * i;

                    if (dx == 0)
                        x += j;
                    else if (dy == 0)
                        y += j;
                    else if (dx * dy > 0)
                    {
                        x += j;
                        y -= j;
                    }
                    else
                    {
                        x += j;
                        y += j;
                    }

                    var p = new Point3D(x, y, Z);
                    if (map.CanFit(p, 16, false, false))
                        coneLocs.Add(p);
                    else
                    {
                        int z2 = map.GetAverageZ(x, y);
                        var p2 = new Point3D(x, y, z2);
                        if (map.CanFit(p2, 16, false, false))
                            coneLocs.Add(p2);
                    }
                }
            }

            // Fire the cone
            foreach (var p in coneLocs)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 5, 10, UniqueHue, 0, 2021, 0
                );

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
                e.Free();
            }

            m_NextWingGust = DateTime.UtcNow + TimeSpan.FromSeconds(8);
        }

        // 3) Blood Storm — random poisonous/necromantic droplets
        public void BloodStormAttack()
        {
            var map = this.Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 10, 50, UniqueHue, 0, 2010, 0
            );

            int radius = 12, drops = 25;
            for (int i = 0; i < drops; i++)
            {
                int x = X + Utility.RandomMinMax(-radius, radius);
                int y = Y + Utility.RandomMinMax(-radius, radius);

				if (!Utility.InRange(this.Location, new Point3D(x, y, this.Z), radius))
					continue;


                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                if (!map.CanFit(p, 16, false, false))
                    continue;

                if (Utility.RandomBool())
                    new PoisonTile().MoveToWorld(p, map);
                else
                    new NecromanticFlamestrikeTile().MoveToWorld(p, map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 7, 14, UniqueHue, 0, 2031, 0
                );
            }

            m_NextBloodStorm = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Chaos Cross — plus‑shaped burst of arcane energy
        public void ChaosCrossAttack()
        {
            var map = this.Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(Location, map, 0x5DD);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x375A, 8, 30, UniqueHue, 0, 2102, 0
            );

            int length = 6, damage = 35;

            // Horizontal arm
            for (int i = -length; i <= length; i++)
            {
                int x = X + i, y = Y;
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3818, 6, 12, UniqueHue, 0, 2150, 0
                );

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }
                e.Free();
            }

            // Vertical arm
            for (int i = -length; i <= length; i++)
            {
                int x = X, y = Y + i;
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3818, 6, 12, UniqueHue, 0, 2150, 0
                );

                var e = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in e)
                {
                    if (m == this) continue;
                    if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
                e.Free();
            }

            m_NextChaosCross = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 5) Shadow Flare — targeted burst with webs
        public void ShadowFlareAttack()
        {
            var map = this.Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(target.Location, map, 0x229);
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, map, EffectItem.DefaultDuration),
                0x372A, 12, 24, UniqueHue, 0, 2024, 0
            );

            int burstRadius = 4, damage = 45;

            for (int xOff = -burstRadius; xOff <= burstRadius; xOff++)
            {
                for (int yOff = -burstRadius; yOff <= burstRadius; yOff++)
                {
                    var p = new Point3D(target.X + xOff, target.Y + yOff, target.Z);

                    if (!map.CanFit(p, 16, false, false) || !Utility.InRange(target.Location, p, burstRadius))
                        continue;

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3789, 5, 10, UniqueHue, 0, 2042, 0
                    );

                    var e = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in e)
                    {
                        if (m == this) continue;
                        if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != this.Team))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 0, 50, 50);
                        }
                    }
                    e.Free();

                    // 30% chance to spawn a web tile
                    if (Utility.RandomDouble() < 0.30)
                        new TrapWeb().MoveToWorld(p, map);
                }
            }

            m_NextShadowFlare = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextSonicScreech && this.InRange(target, 7))
                    SonicScreechAttack();
                else if (DateTime.UtcNow >= m_NextWingGust && this.InRange(target, 10))
                    WingGustAttack();
                else if (DateTime.UtcNow >= m_NextBloodStorm)
                    BloodStormAttack();
                else if (DateTime.UtcNow >= m_NextChaosCross && this.InRange(target, 6))
                    ChaosCrossAttack();
                else if (DateTime.UtcNow >= m_NextShadowFlare && this.InRange(target, 12))
                    ShadowFlareAttack();
            }
        }

        // --- OnDeath: scatter poisonous/web tiles ---
        public override void OnDeath(Container c)
        {
            var map = this.Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x214);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 20, 50, UniqueHue, 0, 5052, 0
                );

                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, 5, map);
                    if (p != Point3D.Zero)
                    {
                        if (Utility.RandomBool())
                            new PoisonTile().MoveToWorld(p, map);
                        else
                            new TrapWeb().MoveToWorld(p, map);

                        Effects.SendLocationParticles(
                            EffectItem.Create(p, map, EffectItem.DefaultDuration),
                            0x376A, 10, 20, UniqueHue, 0, 2061, 0
                        );
                    }
                }
            }

            base.OnDeath(c);
        }

        // Helper to find a valid spot within radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int xOff = Utility.RandomMinMax(-radius, radius);
                int yOff = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + xOff, center.Y + yOff, center.Z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int z2 = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);
                if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    return p2;
            }

            return Point3D.Zero;
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

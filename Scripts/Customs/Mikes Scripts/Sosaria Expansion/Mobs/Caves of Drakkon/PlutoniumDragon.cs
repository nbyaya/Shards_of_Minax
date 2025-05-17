using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a plutonium dragon corpse")]
    public class PlutoniumDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextGammaRay;
        private DateTime m_NextPlasmaBeam;
        private DateTime m_NextRadiantNova;
        private DateTime m_NextShockwave;
        private DateTime m_NextVortex;

        // Unique radioactive green hue
        private const int UniqueHue = 1365; // Acid‐green glow

        [Constructable]
        public PlutoniumDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a plutonium dragon";
            Body = Utility.RandomList(12, 59);  // same bodies as GreaterDragon
            BaseSoundID = 362;                 // same sounds
            Hue = UniqueHue;

            // Supersized stats
            SetStr(1500, 1800);
            SetDex(150, 200);
            SetInt(1200, 1500);

            SetHits(2000, 2500);
            SetDamage(40, 60);

            // Resistances skewed to energy & poison
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire,     60, 75);
            SetResistance(ResistanceType.Cold,     60, 75);
            SetResistance(ResistanceType.Poison,   90, 100);
            SetResistance(ResistanceType.Energy,   90, 100);

            // Skills
            SetSkill(SkillName.EvalInt,    120.0, 150.0);
            SetSkill(SkillName.Magery,     120.0, 150.0);
            SetSkill(SkillName.MagicResist,140.0, 170.0);
            SetSkill(SkillName.Tactics,    110.0, 130.0);
            SetSkill(SkillName.Wrestling,  110.0, 130.0);

            Fame = 40000;
            Karma = -40000;
            VirtualArmor = 100;

            Tamable = false;

            // Initialize cooldowns
            m_NextGammaRay   = DateTime.UtcNow;
            m_NextPlasmaBeam = DateTime.UtcNow;
            m_NextRadiantNova= DateTime.UtcNow;
            m_NextShockwave  = DateTime.UtcNow;
            m_NextVortex     = DateTime.UtcNow;
        }

        public PlutoniumDragon(Serial serial)
            : base(serial)
        {
        }

        #region Overrides & Properties
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;

        public override HideType HideType      => HideType.Barbed;
        public override int      Hides         => 60;
        public override int      Meat          => 30;
        public override int      Scales        => 20;
        public override ScaleType ScaleType    => (ScaleType)Utility.Random(4);

        public override Poison   PoisonImmune  => Poison.Lethal;
        public override Poison   HitPoison     => Poison.Deadly;

        public override int      TreasureMapLevel => 7;
        public override bool     CanFly           => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% unique item
            {
                // PackItem(new PlutoniumHeartBreastplate());
            }
            if (Utility.RandomDouble() < 0.10) // 10% high‐level treasure map
            {
                PackItem(new TreasureMap(7, Map));
            }
        }
        #endregion

        #region Special Abilities

        // 1) Gamma‐Ray Breath: a wide cone of poison/energy that spawns ToxicGasTiles
        public void GammaRayBreath()
        {
			if (!(Combatant is Mobile) || Map == null) return;

			Mobile target = (Mobile)Combatant;

            if (!this.InRange(target, 10)) return;

            // Start effect
            Effects.PlaySound(Location, Map, 0x226); // heavy breath sound
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 30, UniqueHue, 0, 5052, 0);

            // Calculate cone
            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);


            int range = 10;
            for (int i = 1; i <= range; i++)
            {
                // width expands
                int halfWidth = (i * 3) / range;
                for (int w = -halfWidth; w <= halfWidth; w++)
                {
                    int x = X + dx * i + (dy == 0 ? w : 0);
                    int y = Y + dy * i + (dx == 0 ? w : 0);

                    var p = new Point3D(x, y, Z);
                    if (!Map.CanFit(p,16,false,false))
                    {
                        p = new Point3D(x, y, Map.GetAverageZ(x,y));
                        if (!Map.CanFit(p,16,false,false)) continue;
                    }

                    // damage & tile
                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                        0x36B0, 10, 10, UniqueHue, 0, 2023, 0);

                    foreach (var m in Map.GetMobilesInRange(p,0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 25, 0,0,100,0,0); // poison damage
                            if (Utility.RandomDouble() < 0.3)
                                m.ApplyPoison(this, Poison.Deadly);
                        }
                    }
                    Map.GetMobilesInRange(p,0).Free();

                    // spawn toxic cloud
                    if (Utility.RandomBool())
                    {
                        // new ToxicGasTile().MoveToWorld(p, Map);
                    }
                }
            }

            m_NextGammaRay = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Plasma Beam: a piercing line of LandmineTiles and high energy damage
        public void PlasmaBeam()
        {
			if (!(Combatant is Mobile) || Map == null) return;
			Mobile target = (Mobile)Combatant;

            if (!this.InRange(target, 15)) return;

            Effects.PlaySound(Location, Map, 0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 2023, 0);

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);


            for (int i = 1; i <= 15; i++)
            {
                int x = X + dx * i;
                int y = Y + dy * i;
                var p = new Point3D(x, y, Z);

                if (!Map.CanFit(p,16,false,false))
                {
                    int avgZ = Map.GetAverageZ(x, y);
                    p = new Point3D(x, y, avgZ);
                    if (!Map.CanFit(p,16,false,false)) break;
                }

                Effects.SendLocationEffect(p, Map, 0x3818, 16, UniqueHue, 0);
                foreach (var m in Map.GetMobilesInRange(p,0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 50, 0,0,0,0,100);
                    }
                }
                Map.GetMobilesInRange(p,0).Free();

                // place a landmine tile
                if (Utility.RandomDouble() < 0.3)
                {
                    // new LandmineTile().MoveToWorld(p, Map);
                }
            }

            m_NextPlasmaBeam = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Radiant Nova: 360° burst of energy leaving LightningStormTiles
        public void RadiantNova()
        {
            if (Map == null) return;

            Effects.PlaySound(Location, Map, 0x2F3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 30, UniqueHue, 0, 5016, 0);

            int radius = 8;
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;
                    if (!Map.CanFit(p,16,false,false))
                    {
                        p = new Point3D(p.X, p.Y, Map.GetAverageZ(p.X, p.Y));
                        if (!Map.CanFit(p,16,false,false)) continue;
                    }

                    // visual
                    Effects.SendLocationEffect(p, Map, 0x3709, 10, UniqueHue, 0);

                    foreach (var m in Map.GetMobilesInRange(p,0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 35, 0,0,50,50,0);
                        }
                    }
                    Map.GetMobilesInRange(p,0).Free();

                    // spawn lightning tile
                    if (Utility.RandomDouble() < 0.25)
                    {
                        // new LightningStormTile().MoveToWorld(p, Map);
                    }
                }
            }

            m_NextRadiantNova = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Nuclear Shockwave: radial EarthquakeTiles + heavy physical damage
        public void NuclearShockwave()
        {
            if (Map == null) return;

            Effects.PlaySound(Location, Map, 0x299);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 40, UniqueHue, 0, 5029, 0);

            int radius = 6;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;
                    if (!Map.CanFit(p,16,false,false))
                    {
                        p = new Point3D(p.X, p.Y, Map.GetAverageZ(p.X, p.Y));
                        if (!Map.CanFit(p,16,false,false)) continue;
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5029, 0);

                    foreach (var m in Map.GetMobilesInRange(p,0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 45, 100,0,0,0,0);
                        }
                    }
                    Map.GetMobilesInRange(p,0).Free();

                    if (Utility.RandomDouble() < 0.3)
                    {
                        // new EarthquakeTile().MoveToWorld(p, Map);
                    }
                }
            }

            m_NextShockwave = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 5) Electromagnetic Vortex: spawns VortexTiles pulling players inward
        public void ElectromagneticVortex()
        {
            if (Map == null) return;

            Effects.PlaySound(Location, Map, 0x1F1);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x37C4, 10, 20, UniqueHue, 0, 9909, 0);

            int radius = 5;
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;
                    if (!Map.CanFit(p,16,false,false))
                    {
                        p = new Point3D(p.X, p.Y, Map.GetAverageZ(p.X, p.Y));
                        if (!Map.CanFit(p,16,false,false)) continue;
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                        0x37CB, 5, 10, UniqueHue, 0, 9909, 0);

                    if (Utility.RandomDouble() < 0.4)
                    {
                        // new VortexTile().MoveToWorld(p, Map);
                    }
                }
            }

            m_NextVortex = DateTime.UtcNow + TimeSpan.FromSeconds(22);
        }

        #endregion

        #region AI Override

        public override void OnThink()
        {
            base.OnThink();

            if (!(Combatant is Mobile)) return;

            // Use abilities when off cooldown & target in range
            if (DateTime.UtcNow >= m_NextGammaRay && this.InRange(Combatant.Location, 10))
                GammaRayBreath();
            else if (DateTime.UtcNow >= m_NextPlasmaBeam && this.InRange(Combatant.Location, 15))
                PlasmaBeam();
            else if (DateTime.UtcNow >= m_NextRadiantNova)
                RadiantNova();
            else if (DateTime.UtcNow >= m_NextShockwave)
                NuclearShockwave();
            else if (DateTime.UtcNow >= m_NextVortex)
                ElectromagneticVortex();
        }

        #endregion

        #region Serialization

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

        #endregion
    }
}

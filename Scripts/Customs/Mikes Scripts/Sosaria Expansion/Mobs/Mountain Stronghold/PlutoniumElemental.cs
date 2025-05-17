using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a plutonium elemental corpse")]
    public class PlutoniumElemental : BaseCreature
    {
        private DateTime m_NextRadiationPulse;
        private DateTime m_NextCoreBurst;
        private Point3D m_LastLocation;

        // A sickly radioactive green
        private const int UniqueHue = 1253;

        [Constructable]
        public PlutoniumElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Plutonium Elemental";
            Body = 0x9E;
            BaseSoundID = 278;
            Hue = UniqueHue;

            // Stats
            SetStr(450, 550);
            SetDex(80, 100);
            SetInt(300, 400);

            SetHits(1000, 1200);
            SetStam(100, 120);
            SetMana(50, 80);

            SetDamage(12, 18);

            // Damage Types
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison,   70);
            SetDamageType(ResistanceType.Energy,   20);

            // Resistances
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics,     85.0, 100.0);
            SetSkill(SkillName.Wrestling,   80.0, 95.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 70;
            ControlSlots = 4;

            // Cooldowns
            m_NextRadiationPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCoreBurst       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation        = this.Location;

            // Basic loot: plutonium shards & reagents
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));        // Represents raw radioactive material
            PackItem(new BlackPearl(Utility.RandomMinMax(8, 12)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(8, 12)));
        }

        // Leave a trail of toxic gas on movement
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Map != null && Utility.RandomDouble() < 0.30)
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Radiation Pulse: AoE around self
            if (DateTime.UtcNow >= m_NextRadiationPulse)
            {
                RadiationPulse();
                m_NextRadiationPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Core Burst: targeted explosion at your feet
            else if (DateTime.UtcNow >= m_NextCoreBurst)
            {
                CoreBurst();
                m_NextCoreBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Radiation Pulse ---
        private void RadiationPulse()
        {
            PlaySound(0x2F5);
            FixedParticles(0x3709, 10, 30, 5064, UniqueHue, 0, EffectLayer.Waist); // glowy green burst

            var targets = new List<Mobile>();
            foreach (var o in Map.GetMobilesInRange(Location, 8))
            {
                if (o != this && CanBeHarmful(o, false) && o is Mobile target)
                    targets.Add(target);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(t, this, dmg, 0, 0, 0, 100, 0); // 100% Poison
                t.ApplyPoison(this, Poison.Lethal);
                t.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Core Burst at Combatant ---
        private void CoreBurst()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Core overload!*");
            PlaySound(0x3D);
            var loc = target.Location;

            // Visual warning
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3709, 8, 20, UniqueHue, 0, 5052, 0);

            // Delay then explode
            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (Map == null) return;
                Effects.PlaySound(loc, Map, 0x208);
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3728, 12, 10, UniqueHue, 0, 5039, 0);

                // Damage and leave poison tile
                foreach (var o in Map.GetMobilesInRange(loc, 4))
                {
                    if (o != this && CanBeHarmful(o, false) && o is Mobile m)
                    {
                        DoHarmful(m);
                        int dmg = Utility.RandomMinMax(40, 60);
                        AOS.Damage(m, this, dmg, 0, 0, 0, 100, 0);
                    }
                }

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
            });
        }

        // --- Meltdown on Death ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Say("*...containment breach...*");
            PlaySound(0x20B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 50, UniqueHue, 0, 5052, 0);

            // Scatter hazardous gas around corpse
            int count = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-5, 5);
                int dy = Utility.RandomMinMax(-5, 5);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune      => true;
        public override Poison HitPoison      => Poison.Greater;
        public override double HitPoisonChance => 1.0;
        public override int TreasureMapLevel  => 6;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus      => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            // 5% chance for a rare Plutonium Core
            if (Utility.RandomDouble() < 0.05)
                PackItem(new HungerEnd());
        }

        public PlutoniumElemental(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextRadiationPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCoreBurst       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}

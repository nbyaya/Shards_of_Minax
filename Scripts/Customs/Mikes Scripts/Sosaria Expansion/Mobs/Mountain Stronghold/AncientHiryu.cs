using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an ancient hiryu corpse")]
    public class AncientHiryu : BaseCreature
    {
        private DateTime m_NextBreathTime;
        private DateTime m_NextRoarTime;
        private DateTime m_NextDiveTime;
        private DateTime m_NextStormTime;
        private Point3D  m_LastLocation;

        private const int UniqueHue = 1157; // A deep, molten shimmer

        [Constructable]
        public AncientHiryu() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name         = "an ancient hiryu";
            Body         = 243;
            Hue          = UniqueHue;
            BaseSoundID  = 0x4FD;

            // Core stats
            SetStr(1500, 1700);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(500, 600);

            SetDamage(30, 40);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire,     80);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     90,100);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.EvalInt,     120.1, 130.0);
            SetSkill(SkillName.Magery,      120.1, 130.0);
            SetSkill(SkillName.MagicResist, 120.1, 130.0);
            SetSkill(SkillName.Tactics,     110.1, 120.0);
            SetSkill(SkillName.Fencing,     100.1, 110.0);
            SetSkill(SkillName.Wrestling,   100.1, 110.0);

            Fame        = 22000;
            Karma       = -22000;
            VirtualArmor = 90;
            ControlSlots = 6;
            Tamable      = false;

            // Initialize ability cooldowns
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextRoarTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextDiveTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextStormTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Reagent loot
            PackItem(new BlackPearl(Utility.RandomMinMax(20, 30)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(20, 30)));
        }

        public AncientHiryu(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            // Movement effect: drop Ice Shards behind
            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    var oldLoc = m_LastLocation;
                    m_LastLocation = this.Location;

                    var shard = new IceShardTile();
                    shard.Hue = UniqueHue;
                    shard.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    m_LastLocation = this.Location;
                }
            }

            if (Combatant is Mobile target && CanBeHarmful(target, false) 
                && Map != null && Map != Map.Internal)
            {
                if (DateTime.UtcNow >= m_NextBreathTime && InRange(target.Location, 10))
                {
                    FireBreathAttack(target);
                    m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
                else if (DateTime.UtcNow >= m_NextRoarTime && InRange(target.Location, 8))
                {
                    AncientRoar();
                    m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }
                else if (DateTime.UtcNow >= m_NextDiveTime && InRange(target.Location, 12))
                {
                    DiveBomb(target);
                    m_NextDiveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                }
                else if (DateTime.UtcNow >= m_NextStormTime)
                {
                    WingStorm();
                    m_NextStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
            }
        }

        private void FireBreathAttack(Mobile target)
        {
            Say("*The ancient flames engulf you!*");
            PlaySound(0x64B);

            // Breath visual
            Effects.SendMovingParticles(
                this, target, 0x36D4, 5, 25, false, true, UniqueHue, 
                0, 9502, 1, 0, EffectLayer.Head, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (!Alive || Map == null) return;

                // Damage in a small radius
                foreach (Mobile m in Map.GetMobilesInRange(target.Location, 2))
                {
                    if (m != this && CanBeHarmful(m, false))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(60, 80), 0,0,0,0,100);
                        m.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
                    }
                }

                // Leave a lingering flame hazard
                var lava = new FlamestrikeHazardTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(target.Location, Map);
            });
        }

        private void AncientRoar()
        {
            Say("*VROAAARRR!*");
            PlaySound(0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 30, UniqueHue, 0, 5015, 0);

            // Stun nearby foes
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile targ)
                {
                    DoHarmful(targ);
                    targ.Freeze(TimeSpan.FromSeconds(2));
                    targ.SendMessage("The roar reverberates, stunning you!");
                }
            }
        }

        private void DiveBomb(Mobile target)
        {
            Say("*I strike from above!*");
            PlaySound(0x20B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x371A, 10, 30, UniqueHue, 0, 5033, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (!Alive || Map == null) return;

                var loc = target.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                Location = loc;
                Effects.PlaySound(loc, Map, 0x208);

                foreach (Mobile m in Map.GetMobilesInRange(loc, 3))
                {
                    if (m != this && CanBeHarmful(m, false))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(50, 70), 100, 0, 0, 0, 0); // 100% physical damage
                    }
                }

                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(loc, Map);
            });
        }

        private void WingStorm()
        {
            Say("*Feel the storm's wrath!*");
            PlaySound(0x2A2);

            // Spawn vortex tiles in a 3×3 around self
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var vortex = new VortexTile();
                vortex.Hue = UniqueHue;
                vortex.MoveToWorld(loc, Map);
            }

            // Damage and message
            foreach (Mobile m in Map.GetMobilesInRange(Location, 4))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile targ)
                {
                    DoHarmful(targ);
                    AOS.Damage(m, this, Utility.RandomMinMax(50, 70), 0, 100, 0, 0, 0); // 100% fire
                    targ.SendMessage("You are buffeted by wing gusts!");
                }
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My age-old slumber reignited!*");
                Effects.PlaySound(Location, Map, 0x64B);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5015, 0);

                // Spawn hot lava hazards
                int count = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-2, 2), dy = Utility.RandomMinMax(-2, 2);
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var lava = new HotLavaTile();
                    lava.Hue = UniqueHue;
                    lava.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus => 70.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new AlchemistsSmockOfStains()); // Unique crafting material
        }

        public override int GetIdleSound()   => 0x4FD;
        public override int GetAngerSound()  => 0x4FE;
        public override int GetAttackSound() => 0x4FC;
        public override int GetHurtSound()   => 0x4FF;
        public override int GetDeathSound()  => 0x4FB;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize cooldowns on load
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextRoarTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextDiveTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextStormTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;
        }
    }
}

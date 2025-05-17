using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // for spell effects
using Server.Spells.Seventh; // for Chain Lightning visuals

namespace Server.Mobiles
{
    [CorpseName("a Warcluck wreckage corpse")]
    public class RoosterUnitC7Warcluck : BaseCreature
    {
        // Timers for its special systems
        private DateTime m_NextOverchargeTime;
        private DateTime m_NextFeatherStormTime;
        private DateTime m_NextGroundShockTime;

        // Unique mechanical hue
        private const int UniqueHue = 2520; // metallic crimson-gold

        [Constructable]
        public RoosterUnitC7Warcluck() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Rooster-Unit C7 \"Warcluck\"";
            Body = 716;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 420);
            SetDex(180, 240);
            SetInt(300, 380);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire,     30);
            SetDamageType(ResistanceType.Energy,   30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   50, 60);

            SetSkill(SkillName.Tactics,    100.0, 110.0);
            SetSkill(SkillName.Wrestling,  100.0, 110.0);
            SetSkill(SkillName.MagicResist,110.0, 120.0);
            SetSkill(SkillName.EvalInt,     90.0, 100.0);
            SetSkill(SkillName.Magery,      90.0, 100.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 70;
            ControlSlots = 6;

            // Cooldowns
            m_NextOverchargeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextFeatherStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGroundShockTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            // Pack reagents & gear
            PackItem(new Gears(Utility.RandomMinMax(5, 10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
        }

        // Sounds
        public override int GetIdleSound()  => 1511;
        public override int GetAngerSound() => 1508;
        public override int GetHurtSound()  => 1510;
        public override int GetDeathSound() => 1509;

        // Always stand its ground once engaged
        public override IDamageable Combatant
        {
            get { return base.Combatant; }
            set
            {
                base.Combatant = value;
                StopFlee();
            }
        }

        // Aura: when players move near, they lose stamina and are briefly slowed
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int stamDrain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage("You feel your legs grow heavy under the Warcluck's gravitic steps!");
                        target.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x227);
                        // brief slow
                        target.Paralyze(TimeSpan.FromSeconds(1.5));
                    }
                }
            }
            base.OnMovement(m, oldLoc);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;
            var range = GetDistanceToSqrt(((Mobile)Combatant).Location);

            if (now >= m_NextOverchargeTime)
            {
                ActivateOvercharge();
                m_NextOverchargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextFeatherStormTime && range <= 10)
            {
                FeatherStorm();
                m_NextFeatherStormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (now >= m_NextGroundShockTime && range <= 12)
            {
                GroundShock();
                m_NextGroundShockTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // 1) Self-buff: Overcharge its systems to boost speed & damage
        private void ActivateOvercharge()
        {
            Say("*Systems Overcharge Engaged!*");
            PlaySound(0x2A8);
            this.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

            double origDamageMin = DamageMin, origDamageMax = DamageMax;
            ActiveSpeed = 0.1;
            PassiveSpeed = 0.3;
            SetDamage((int)(origDamageMin * 1.3), (int)(origDamageMax * 1.3));

            Timer.DelayCall(TimeSpan.FromSeconds(8), () =>
            {
                if (!Alive) return;
                // revert
                ActiveSpeed = this.ActiveSpeed + 0.1;
                PassiveSpeed = this.PassiveSpeed + 0.1;
                SetDamage((int)origDamageMin, (int)origDamageMax);
            });
        }

        // 2) AoE hazard: rains mechanical feathers that poison and burn
        private void FeatherStorm()
        {
            if (!(Combatant is Mobile)) return;

            Say("*Featherstorm Protocol!*");
            PlaySound(0x2E6);

            var location = this.Location;
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    Point3D drop = new Point3D(location.X + x, location.Y + y, location.Z);
                    if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                        drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                    PoisonTile tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(drop, Map);
                }
            }
        }

        // 3) Targeted ground shock: spawns a quake tile under its foe
        private void GroundShock()
        {
            IDamageable dmg = Combatant;
            Point3D targetLoc = dmg.Location;

            Say("*Ground Shock!*");
            PlaySound(0x2D9);
            Effects.SendLocationParticles(EffectItem.Create(targetLoc, Map, EffectItem.DefaultDuration), 0x375A, 8, 20, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                if (!Map.CanFit(targetLoc.X, targetLoc.Y, targetLoc.Z, 16, false, false))
                    targetLoc.Z = Map.GetAverageZ(targetLoc.X, targetLoc.Y);

                EarthquakeTile quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(targetLoc, Map);
            });
        }

        // On death, showers the area with landmines and a lightning storm
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*System Failure... Shutdown!*");
            PlaySound(0x213);

            var center = this.Location;
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                Point3D loc = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                LandmineTile mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                LightningStormTile storm = new LightningStormTile();
                storm.Hue = UniqueHue;
                storm.MoveToWorld(center, Map);
            });
        }

        // Loot & properties
        public override bool BleedImmune         => true;
        public override int TreasureMapLevel     => 5;
        public override double DispelDifficulty  => 130.0;
        public override double DispelFocus       => 65.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(5, 10));

            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new CinderskullCap(1)); // unique mech part
        }

        public RoosterUnitC7Warcluck(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextOverchargeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextFeatherStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGroundShockTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
        }
    }
}

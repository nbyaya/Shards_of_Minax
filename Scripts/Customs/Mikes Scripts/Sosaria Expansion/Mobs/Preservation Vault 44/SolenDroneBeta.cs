using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a solen drone beta corpse")]
    public class SolenDroneBeta : BaseCreature
    {
        private DateTime m_NextIonVolley;
        private DateTime m_NextOverloadBeam;
        private DateTime m_NextDroneSwarm;
        private Point3D m_LastLocation;

        // A metallic emerald-green to distinguish it from typical Solen
        private const int UniqueHue = 1231;

        [Constructable]
        public SolenDroneBeta()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name             = "a Solen Drone Beta";
            Body             = 781;       // same as RedSolenWorker
            BaseSoundID      = 959;
            Hue              = UniqueHue;
            ControlSlots     = 4;

            // Robust but focused more on special attacks than raw hit points
            SetStr(250, 300);
            SetDex(150, 200);
            SetInt(400, 450);

            SetHits(900, 1100);
            SetMana(300, 400);
            SetStam(150, 200);

            SetDamage(10, 15);

            // Mixed damage types—physical plus corrosive toxin
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison,    40);

            // Balanced resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   35, 45);

            // Skills tuned for spells and survivability
            SetSkill(SkillName.Magery,        100.0, 115.0);
            SetSkill(SkillName.EvalInt,       100.0, 115.0);
            SetSkill(SkillName.MagicResist,   110.0, 125.0);
            SetSkill(SkillName.Tactics,       80.0,  90.0);
            SetSkill(SkillName.Wrestling,     75.0,  85.0);

            Fame     = 18000;
            Karma    = -18000;
            VirtualArmor = 70;

            // Initialize timers
            var now = DateTime.UtcNow;
            m_NextIonVolley     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextOverloadBeam  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(16, 22));
            m_NextDroneSwarm    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Loot: high-end reagents and a small chance at a unique Solen tech-core
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new Moonpiercer());
        }

        // Electromagnetic Aura: drains stamina of anyone moving close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || this.Map != m.Map || !m.InRange(this.Location, 3))
                return;

            if (Combatant is Mobile && SpellHelper.ValidIndirectTarget(this, m))
            {
                DoHarmful(m);

                var drain = Utility.RandomMinMax(5, 15);
                if (m.Stam >= drain)
                {
                    m.Stam -= drain;
                    m.SendMessage(0x22, "You feel your stamina sapped by a crackling aura!");
                    m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    m.PlaySound(0x1F8);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave behind Poisonous Corrosion Tiles occasionally
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                var map = this.Map;
                if (map == null || map == Map.Internal) return;

                Point3D spawn = oldLoc;
                if (!map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = map.GetAverageZ(spawn.X, spawn.Y);

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawn, map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // If no combatant or dead, skip
            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Ion Volley: chain lightning across up to 4 targets
            if (now >= m_NextIonVolley && this.InRange(Combatant.Location, 12))
            {
                IonVolleyAttack();
                m_NextIonVolley = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Overload Beam: single powerful beam at current target
            else if (now >= m_NextOverloadBeam && this.InRange(Combatant.Location, 10))
            {
                OverloadBeamAttack();
                m_NextOverloadBeam = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 26));
            }
            // Drone Swarm: releases 3 small Solen Wisp minions
            else if (now >= m_NextDroneSwarm)
            {
                DroneSwarmRelease();
                m_NextDroneSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void IonVolleyAttack()
        {
            this.Say("*Static surge!*");
            this.PlaySound(0x1FA);

            var initial = Combatant as Mobile;
            if (initial == null || !CanBeHarmful(initial, false) || !SpellHelper.ValidIndirectTarget(this, initial))
                return;

            var targets = new List<Mobile> { initial };
            int maxBounces = 4, range = 8;

            for (int i = 1; i < maxBounces; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double closest = double.MaxValue;

                foreach (var m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) && last.InLOS(m))
                    {
                        var dist = last.GetDistanceToSqrt(m);
                        if (dist < closest)
                        {
                            closest = dist;
                            next = m;
                        }
                    }
                }

                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 0x1001, 0, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        int dmg = Utility.RandomMinMax(20, 35);
                        AOS.Damage(dst, this, dmg, 0, 0, 0, 0, 100);
                        dst.FixedParticles(0x3779, 5, 10, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        private void OverloadBeamAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Discharge overload!*");
            this.PlaySound(0x2F3);

            // Beam effect
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x3778, 9, 1, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
            {
                if (CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(60, 85);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Chance to apply a lingering toxin
                    if (0.4 > Utility.RandomDouble())
                        target.ApplyPoison(this, Poison.Lethal);
                }
            });
        }

        private void DroneSwarmRelease()
        {
            this.Say("*Deploying swarm!*");
            this.PlaySound(0x1F3);

            for (int i = 0; i < 3; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-1, 1),
                    Y + Utility.RandomMinMax(-1, 1),
                    Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var wisp = new RedSolenWorker(); // reuse existing smaller Solen
                wisp.Hue = UniqueHue;
                wisp.MoveToWorld(loc, Map);
                wisp.Combatant = this.Combatant; 
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*System failure…*");
            PlaySound(0x2A7);

            // Scattering corrosive mines
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                var offX = Utility.RandomMinMax(-3, 3);
                var offY = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + offX, Y + offY, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var landmine = new LandmineTile();
                landmine.Hue = UniqueHue;
                landmine.MoveToWorld(loc, Map);
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 8, 20, UniqueHue, 0, 5039, 0);
            }
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus    { get { return 60.0; } }

        public SolenDroneBeta(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers
            var now = DateTime.UtcNow;
            m_NextIonVolley    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextOverloadBeam = now + TimeSpan.FromSeconds(Utility.RandomMinMax(16, 22));
            m_NextDroneSwarm   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}

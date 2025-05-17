using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a massive serpent corpse")]
    public class NileSerpent : BaseCreature
    {
        private DateTime m_NextSandstorm;
        private DateTime m_NextVenomSpray;
        private DateTime m_NextSummon;
        private DateTime m_NextTrap;
        private Point3D m_LastLocation;

        private const int UniqueHue = 2100; // Golden‑sand tone

        [Constructable]
        public NileSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name        = "the Nile Serpent";
            Body        = 0x15;
            BaseSoundID = 219;
            Hue         = UniqueHue;

            // —— Stats ——
            SetStr(300, 350);
            SetDex(100, 130);
            SetInt(120, 150);

            SetHits(200, 350);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison,   50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire,     20, 30);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   30, 40);

            SetSkill(SkillName.Poisoning,    90.1, 110.0);
            SetSkill(SkillName.MagicResist,  50.0,  65.0);
            SetSkill(SkillName.Tactics,      80.1,  95.0);
            SetSkill(SkillName.Wrestling,    80.1, 100.0);

            Fame          = 15000;
            Karma         = -15000;
            VirtualArmor  = 70;
            ControlSlots  = 3;

            // initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSandstorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextVenomSpray = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummon    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextTrap      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            m_LastLocation = Location;

            // The serpent carries desert‑themed reagents
            PackItem(new Garlic(     Utility.RandomMinMax(5, 10)));
            PackItem(new Ginseng(    Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade( Utility.RandomMinMax(3,  7)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(3,  7)));
        }

        // —— Abilities on movement: leave a quicksand trap occasionally ——
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if (Deleted || !Alive) return;

            // 20% chance to drop a QuicksandTile at last spot
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var map = Map;
                if (map != null && map.CanFit(m_LastLocation.X, m_LastLocation.Y, m_LastLocation.Z, 16, false, false))
                {
                    var sand = new QuicksandTile { Hue = UniqueHue };
                    sand.MoveToWorld(m_LastLocation, map);
                }
            }

            m_LastLocation = Location;
        }

        // —— Main AI loop triggers special attacks ——
        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            var now = DateTime.UtcNow;
            var rng = Utility.RandomDouble();

            if (now >= m_NextSandstorm && rng < 0.5 && InRange(Combatant.Location, 12))
            {
                SandstormSurge();
                m_NextSandstorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextVenomSpray && InRange(Combatant.Location, 8))
            {
                VenomSpray();
                m_NextVenomSpray = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextSummon)
            {
                SummonLesserSerpents();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 50));
            }
            else if (now >= m_NextTrap && InRange(Combatant.Location, 10))
            {
                QuicksandTrap();
                m_NextTrap = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
        }

        // —— Ability 1: Sandstorm Surge (AoE physical+poison + slow) ——
        public void SandstormSurge()
        {
            PlaySound(0x212);
            FixedParticles(0x3778, 20, 35, 2106, EffectLayer.Waist);

            var victims = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    victims.Add(m);
            }

            foreach (var v in victims)
            {
                DoHarmful(v);
                AOS.Damage(v, this, Utility.RandomMinMax(30, 45), 50, 0, 0, 50, 0);

                var target = v as Mobile;
                if (target != null)
                {
                    int stamDrain = Utility.RandomMinMax(10, 20);
                    target.Stam = Math.Max(0, target.Stam - stamDrain);
                    target.SendMessage("You are slowed by the swirling sands!");
                }
            }
        }

        // —— Ability 2: Venom Spray (cone of poison) ——
        public void VenomSpray()
        {
            var target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target, false))
                return;

            Say("*Hssssss!*");
            PlaySound(0x22B);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 8, 20, UniqueHue, 0, 5032, 0);

            foreach (var m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && InCone(m, 5, 45))
                {
                    DoHarmful(m);
                    (m as Mobile)?.ApplyPoison(this, Poison.Deadly);
                }
            }
        }

        // helper: simple cone check without missing extension methods
        private bool InCone(Mobile m, int radius, int degrees)
        {
            // first check distance
            if (this.GetDistanceToSqrt(m) > radius)
                return false;

            var targetDir = this.GetDirectionTo(m);
            int delta = Math.Abs((int)targetDir - (int)this.Direction);

            // wrap around (so e.g. North vs. North-West is 1 step, not 7)
            if (delta > 4)
                delta = 8 - delta;

            // each direction step is 45°, so delta * 45° must be within half the cone
            return (delta * 45) <= (degrees / 2);
        }

        // —— Ability 3: Summon Lesser Serpents ——
        public void SummonLesserSerpents()
        {
            Say("*Arise, my children!*");
            PlaySound(0x223);

            for (int i = 0; i < 3; i++)
            {
                var spawn = new GiantSerpent
                {
                    Hue = UniqueHue - 50 // slightly different tint
                };

                // pick a random offset around the serpent
                int dx = Utility.RandomMinMax(-2, 2);
                int dy = Utility.RandomMinMax(-2, 2);

                var spawnLoc = new Point3D(
                    X + dx,
                    Y + dy,
                    Z
                );

                spawn.MoveToWorld(spawnLoc, Map);
                spawn.Combatant = Combatant;
            }
        }

        // —— Ability 4: Quicksand Trap beneath target ——
        public void QuicksandTrap()
        {
            var target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target, false))
                return;

            Say("*The sands obey me!*");
            PlaySound(0x22F);

            var loc = target.Location;
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                var trap = new QuicksandTile { Hue = UniqueHue };
                trap.MoveToWorld(loc, Map);
            });
        }

        // —— Drop explosion of flame on death ——
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            PlaySound(0x214);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 40, UniqueHue, 0, 5032, 0);

            for (int i = 0; i < 4; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var flame = new FlamestrikeHazardTile { Hue = UniqueHue };
                flame.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // —— Loot & Resistances ——
        public override Poison PoisonImmune => Poison.Greater;
        public override Poison HitPoison   => Poison.Deadly;
        public override bool   BleedImmune => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 12));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new IronHeartplate()); // your custom artifact
        }

        public override int Meat  => 6;
        public override int Hides => 20;
        public override HideType HideType => HideType.Barbed;

        public NileSerpent(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            // reset timers after load
            var now = DateTime.UtcNow;
            m_NextSandstorm = now + TimeSpan.FromSeconds(15);
            m_NextVenomSpray = now + TimeSpan.FromSeconds(10);
            m_NextSummon    = now + TimeSpan.FromSeconds(25);
            m_NextTrap      = now + TimeSpan.FromSeconds(12);
        }
    }
}

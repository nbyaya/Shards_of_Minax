using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a rocky orcish corpse")]
    public class RockOrc : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextStompTime;
        private DateTime m_NextBoulderTime;
        private DateTime m_NextTrapTime;
        private Point3D m_LastLocation;

        // Unique hue – a stone‐gray tint
        private const int UniqueHue = 1109;

        [Constructable]
        public RockOrc()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Rock Orc";
            Body = 0xB5;            // same body as OrcScout
            BaseSoundID = 0x45A;    // same sounds as OrcScout
            Hue = UniqueHue;

            // — Stats —
            SetStr(300, 350);
            SetDex(80, 100);
            SetInt(50, 70);

            SetHits(900, 1050);
            SetStam(150, 200);
            SetMana(50, 80);

            SetDamage(12, 18);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 100);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 20, 30);

            // — Skills —
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 60.1, 80.0);
            SetSkill(SkillName.Anatomy, 90.1, 100.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextStompTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextTrapTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // — Loot —
            PackItem(new IronIngot(Utility.RandomMinMax(5, 10)));
            PackItem(new Granite(Utility.RandomMinMax(3, 6)));
            PackGem(); // drop a random gem
        }

        public RockOrc(Serial serial) : base(serial)
        {
        }

        // — Passive: Leave LandmineTile when moving —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // 15% chance per move to drop a landmine where it stepped
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15 && this.Map != null)
            {
                var loc = m_LastLocation;
                // ensure tile can fit
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, this.Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;
            // Earthquake Stomp: AoE ground rupture
            if (now >= m_NextStompTime && this.InRange(Combatant.Location, 6))
            {
                EarthquakeStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            // Boulder Throw: ranged single‐target knockback
            else if (now >= m_NextBoulderTime && this.InRange(Combatant.Location, 12))
            {
                BoulderThrow();
                m_NextBoulderTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            // Quicksand Trap: spawn trap under target
            else if (now >= m_NextTrapTime && this.InRange(Combatant.Location, 10))
            {
                QuicksandTrap();
                m_NextTrapTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Ability: Earthquake Stomp ---
        private void EarthquakeStomp()
        {
            this.Say("*ROAR!*");
            this.PlaySound(0x11A); // heavy ground rumble

            // Visual ripple effect
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.0)),
                0x376A, 8, 30, UniqueHue, 0, 5039, 0);

            // Damage and spawn EarthquakeTiles
            var targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (var t in targets)
            {
                DoHarmful(t);
                AOS.Damage(t, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);

                // spawn ground rupture
                var tile = new EarthquakeTile();
                var loc  = t.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
            }
        }

        // --- Ability: Boulder Throw ---
        private void BoulderThrow()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*HURR!*");
            PlaySound(0x2A4); // rock impact sound

            // Send a rock projectile
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.4), () =>
            {
                if (target.Alive && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 100, 0, 0, 0, 0);
                    target.SendMessage(0x22, "You are slammed by a massive boulder!");
                    target.Freeze(TimeSpan.FromSeconds(1.0));
                }
            });
        }

        // --- Ability: Quicksand Trap ---
        private void QuicksandTrap()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*GRRR!*");
            PlaySound(0x58F); // earth swirling

            var loc = target.Location;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

            var sand = new QuicksandTile();
            sand.Hue = UniqueHue;
            sand.MoveToWorld(loc, this.Map);
        }

        // --- Death Effect: Shattering Shockwave ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.PlaySound(0x11B); // rock shatter
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.0)),
                    0x3709, 8, 40, UniqueHue, 0, 5052, 0);

                // Spawn a few shards as EarthquakeTiles
                for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
                {
                    int x = X + Utility.RandomMinMax(-2, 2);
                    int y = Y + Utility.RandomMinMax(-2, 2);
                    int z = Z;
                    var loc = new Point3D(x, y, z);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        loc.Z = Map.GetAverageZ(x, y);

                    var tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(2, 5));
        }

        // Standard Orc properties
        public override bool CanRummageCorpses { get { return true; } }
        public override InhumanSpeech SpeechType  { get { return InhumanSpeech.Orc; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.SavagesAndOrcs; } }
        public override TribeType Tribe { get { return TribeType.Orc; } }
        public override int Meat { get { return 2; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers after load
            var now = DateTime.UtcNow;
            m_NextStompTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextTrapTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation    = this.Location;
        }
    }
}

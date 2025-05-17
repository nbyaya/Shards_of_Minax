using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mine dancer corpse")]
    public class MineDancer : BaseCreature
    {
        // Cooldowns
        private DateTime m_NextTremor, m_NextShard, m_NextPulse, m_NextGas;
        private Point3D m_LastLoc;

        // Glowing ore hue
        private const int OreHue = 2601;

        [Constructable]
        public MineDancer()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a mine dancer";
            Body = 247;
            BaseSoundID = 0x372;
            Hue = OreHue;

            // Stats
            SetStr(350, 420);
            SetDex(180, 240);
            SetInt(100, 140);

            SetHits(800, 950);
            SetStam(200, 240);
            SetMana(100, 150);

            SetDamage(15, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 10);
            SetDamageType(ResistanceType.Energy, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 50, 65);
            SetResistance(ResistanceType.Poison, 40, 55);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Skills
            SetSkill(SkillName.Tactics, 95.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 105.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Ability timers
            var now = DateTime.UtcNow;
            m_NextTremor = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShard  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextPulse  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextGas    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLoc = this.Location;

            // Loot: ore, gems, occasional relic
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new Granite(Utility.RandomMinMax(10, 20)));
            PackGem(5, 8);
            if (Utility.RandomDouble() < 0.05)
                PackItem(new StillwaterUndergarment());  // Rare mining relic
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave landmines behind occasionally
            if (this.Alive && m_LastLoc != this.Location && Utility.RandomDouble() < 0.15)
            {
                var loc = m_LastLoc;
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var mine = new LandmineTile { Hue = OreHue };
                    mine.MoveToWorld(loc, this.Map);
                }
            }

            m_LastLoc = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Earth Tremor: random quake tiles around self
            if (now >= m_NextTremor)
            {
                DoEarthTremor();
                m_NextTremor = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Shard Eruption: ranged burst at Combatant
            else if (now >= m_NextShard && this.InRange(Combatant.Location, 12))
            {
                if (Combatant is Mobile target) DoShardEruption(target);
                m_NextShard = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 16));
            }
            // Magnetic Pulse: draw in & slow
            else if (now >= m_NextPulse && this.InRange(Combatant.Location, 6))
            {
                if (Combatant is Mobile target) DoMagneticPulse(target);
                m_NextPulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            // Toxic Gas: cloud under target
            else if (now >= m_NextGas)
            {
                if (Combatant is Mobile target) DoToxicGas(target.Location);
                m_NextGas = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        private void DoEarthTremor()
        {
            this.Say("*The ground trembles!*");
            PlaySound(0x1F3);
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                var pt = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(pt.X, pt.Y, pt.Z, 16, false, false))
                    pt.Z = Map.GetAverageZ(pt.X, pt.Y);

                var quake = new EarthquakeTile { Hue = OreHue };
                quake.MoveToWorld(pt, Map);
            }
        }

        private void DoShardEruption(Mobile target)
        {
            this.Say("*Feel the jagged shards!*");
            PlaySound(0x55F);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x1CED, 7, 0, false, false, OreHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

            DoHarmful(target);
            int dmg = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

            if (Utility.RandomDouble() < 0.25)
            {
                target.ApplyPoison(this, Poison.Lethal);
                target.SendLocalizedMessage(1070854); // “You’re bleeding profusely!”
            }
        }

        private void DoMagneticPulse(Mobile target)
        {
            this.Say("*Magnetic force!*");
            PlaySound(0x299);
            var inRange = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    inRange.Add(m);
            }

            foreach (var m in inRange)
            {
                if (m is Mobile mob)
                {
                    mob.Stam = Math.Max(0, mob.Stam - Utility.RandomMinMax(20, 30));
                    mob.SendMessage(0x22, "You feel an irresistible pull slow your movements!");
                    mob.FixedParticles(0x373A, 10, 15, OreHue, EffectLayer.Waist);
                    mob.PlaySound(0x207);
                }
            }
        }

        private void DoToxicGas(Point3D loc)
        {
            this.Say("*Breathe deep… if you dare!*");
            PlaySound(0x57);
            var gas = new ToxicGasTile { Hue = OreHue };
            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                gas.MoveToWorld(loc, Map);
            else
            {
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                gas.MoveToWorld(loc, Map);
            }
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);

            if (attacker != null && !attacker.InRange(this, 1) && Utility.RandomDouble() < 0.5)
            {
                // Flamestrike retaliation
                var tile = new FlamestrikeHazardTile { Hue = OreHue };
                tile.MoveToWorld(attacker.Location, attacker.Map);

                Effects.PlaySound(attacker.Location, attacker.Map, 0x208);
                DoHarmful(attacker);
                AOS.Damage(attacker, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && Utility.RandomDouble() < 0.2)
            {
                // Bury (paralyze) for a moment
                attacker.Freeze(TimeSpan.FromSeconds(2.0));
                attacker.SendMessage(0x22, "You’re momentarily buried by rockfall!");
                Effects.SendLocationParticles(
                    EffectItem.Create(attacker.Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, OreHue, 0, 5039, 0);
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender != null && Utility.RandomDouble() < 0.15)
            {
                // Ground slice (minor bleed)
                defender.ApplyPoison(this, Poison.Regular);
                defender.SendLocalizedMessage(1070853); // “Your wounds sting!”
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("*The mine collapses…*");
            Effects.PlaySound(this.Location, this.Map, 0x1F3);

            // Spawn quicksand & landmines around the corpse
            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var pt = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(pt.X, pt.Y, pt.Z, 16, false, false))
                    pt.Z = Map.GetAverageZ(pt.X, pt.Y);

                Server.Items.QuicksandTile qs = new QuicksandTile { Hue = OreHue };
                qs.MoveToWorld(pt, Map);

                if (i % 2 == 0)
                {
                    var mine = new LandmineTile { Hue = OreHue };
                    mine.MoveToWorld(pt, Map);
                }
            }
        }

        // Properties & loot
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus    => 60.0;
		
		public MineDancer(Serial serial) : base(serial) { }

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

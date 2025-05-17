using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Spellweaving;

namespace Server.Mobiles
{
    [CorpseName("the stony root remains")]
    public class StoneboundRoots : BaseCreature
    {
        private DateTime m_NextSnareTime;
        private DateTime m_NextQuakeTime;
        private DateTime m_NextSporeTime;
        private DateTime m_NextSpikeTime;
        private Point3D m_LastLocation;

        // Unique earthy-brown hue
        private const int UniqueHue = 2509;

        [Constructable]
        public StoneboundRoots()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Stonebound Roots";
            Body = 8;
            BaseSoundID = 684;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(80, 100);
            SetInt(50, 60);

            SetHits(800, 900);
            SetStam(150, 180);
            SetMana(0);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics,    80.1, 100.0);
            SetSkill(SkillName.Wrestling,  80.1, 100.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Ability cooldowns
            m_NextSnareTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSporeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSpikeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Loot setup
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            PackGold(2000, 4000);
        }

        // Root Snare: immobilize target briefly and apply poison bleed
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m is Mobile target && Alive && target.Alive && CanBeHarmful(target, false) && !target.Hidden && InRange(target, 6))
            {
                if (DateTime.UtcNow >= m_NextSnareTime && Utility.RandomDouble() < 0.25)
                {
                    m_NextSnareTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

                    target.Frozen = true;
                    target.SendLocalizedMessage(1111641); // “You become entangled in stony roots!”
                    PlaySound(684);
                    Effects.SendLocationParticles(
                        EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration),
                        0x375A, 8, 20, UniqueHue, 0, 5025, 0
                    );

                    // Begin bleed tick (5 ticks, 1s apart)
                    Timer.DelayCall(
                        TimeSpan.FromSeconds(1.0),
                        TimeSpan.FromSeconds(1.0),
                        5,
                        () =>
                        {
                            if (target.Alive && InRange(target, 1))
                            {
                                AOS.Damage(target, this, Utility.RandomMinMax(5, 8), 0, 0, 0, 0, 100);
                                target.SendMessage("Stony thorns tear at you!");
                            }
                        }
                    );

                    // Unfreeze after 5 seconds
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                    {
                        target.Frozen = false;
                        target.SendLocalizedMessage(1111642); // “You break free of the roots.”
                    });
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            Mobile target = Combatant as Mobile;
            if (target == null || Map == null || !Alive)
                return;

            // Earth Shatter: AoE stun & damage
            if (DateTime.UtcNow >= m_NextQuakeTime && InRange(target, 8))
            {
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                EarthShatter();
            }
            // Spore Burst: poisons ground around target
            else if (DateTime.UtcNow >= m_NextSporeTime && InRange(target, 10))
            {
                m_NextSporeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                SporeBurst(target);
            }
            // Root Spike Barrage: landmines of thorns
            else if (DateTime.UtcNow >= m_NextSpikeTime && InRange(target, 12))
            {
                m_NextSpikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                SpikeBarrage(target);
            }

            // Leave jagged root shards behind as it moves
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var tile = new TrapWeb();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(m_LastLocation, Map);
            }
            m_LastLocation = Location;
        }

        private void EarthShatter()
        {
            Say("The earth trembles!");
            PlaySound(0x5A5);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x377A, 12, 20, UniqueHue, 0, 5053, 0
            );

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);
                m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(20, 30));
                m.Freeze(TimeSpan.FromSeconds(2.0));
                m.SendMessage("You are rocked by the quake!");
            }
        }

        private void SporeBurst(Mobile target)
        {
            Say("Feel the decay!");
            PlaySound(0x2E3);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0
            );

            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(target.X + dx, target.Y + dy, target.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
            }
        }

        private void SpikeBarrage(Mobile target)
        {
            Say("Roots of stone, strike!");
            PlaySound(0x59D);

            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-2, 2), dy = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(target.X + dx, target.Y + dy, target.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("The roots... undone...");
            PlaySound(0x5AE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            // Spawn a few leftover spike traps
            for (int i = 0; i < 4; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        // Properties
        public override bool BleedImmune             => true;
        public override Poison PoisonImmune          => Poison.Lesser;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override int TreasureMapLevel         => 5;
        public override double DispelDifficulty      => 120.0;
        public override double DispelFocus           => 55.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            if (Utility.RandomDouble() < 0.03)
                PackItem(new MandrakeRoot(Utility.RandomMinMax(1, 3)));
        }

        public StoneboundRoots(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset cooldowns
            m_NextSnareTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSporeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSpikeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}

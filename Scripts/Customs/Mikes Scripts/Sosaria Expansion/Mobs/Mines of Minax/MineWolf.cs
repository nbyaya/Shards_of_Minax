using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mine wolf carcass")]
    public class MineWolf : BaseCreature
    {
        private DateTime _nextEarthquake;
        private DateTime _nextCoalBreath;
        private DateTime _nextPoisonCloud;
        private Point3D _lastLocation;

        private const int UniqueHue = 2001; // Rusty‐iron hue

        [Constructable]
        public MineWolf()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "a mine wolf";
            Body = Utility.RandomList(25, 27);
            BaseSoundID = 0xE5;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 450);
            SetDex(120, 160);
            SetInt(100, 140);

            SetHits(120, 140);
            SetStam(150, 200);
            SetMana(200, 300);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.Wrestling, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.EvalInt, 80.1, 90.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Ability cooldowns
            var now = DateTime.UtcNow;
            _nextEarthquake = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextCoalBreath = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));

            _lastLocation = this.Location;

            // Starter loot
            PackItem(new IronOre(Utility.RandomMinMax(15, 25)));
            PackItem(new Coal(Utility.RandomMinMax(10, 20)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == Map && Alive && m.InRange(Location, 2) && Utility.RandomDouble() < 0.10)
            {
                // Lay a hidden landmine
                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(oldLocation, Map);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if (now >= _nextCoalBreath && InRange(Combatant.Location, 10))
            {
                CoalBreathAttack();
                _nextCoalBreath = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= _nextEarthquake && InRange(Combatant.Location, 6))
            {
                EarthquakeStomp();
                _nextEarthquake = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (now >= _nextPoisonCloud && InRange(Combatant.Location, 4))
            {
                ReleasePoisonCloud();
                _nextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }

            // Track movement for tile‐leaving
            if (Location != _lastLocation)
                _lastLocation = Location;
        }

        private void CoalBreathAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*ROOAR*");
            PlaySound(0x64A); // Dragon‐like roar

            // Cone of flame: affect target and nearby
            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(target.Location, 3))
            {
                if (m != this && m is Mobile mob && CanBeHarmful(mob, false))
                    targets.Add(mob);
            }

            foreach (var mob in targets)
            {
                DoHarmful(mob);
                AOS.Damage(mob, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0); // 100% fire
                mob.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
                mob.PlaySound(0x208);

                // Spawn hot lava hazard
                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(mob.Location, Map);
            }
        }

        private void EarthquakeStomp()
        {
            Say("*GRRRUMBLE*");
            PlaySound(0x2F3);

            var affected = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && m is Mobile mob && CanBeHarmful(mob, false))
                    affected.Add(mob);
            }

            foreach (var mob in affected)
            {
                DoHarmful(mob);
                AOS.Damage(mob, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0); // 100% physical
                mob.SendMessage(0x22, "The ground shakes violently!");
                mob.FixedParticles(0x3728, 10, 10, 5032, UniqueHue, 0, EffectLayer.Head);

                // Create earthquake hazard tile under them
                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(mob.Location, Map);
            }
        }

        private void ReleasePoisonCloud()
        {
            Say("*HSSIISS*");
            PlaySound(0x224);

            // Spawn multiple poison tiles around self
            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var gas = new PoisonTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Say("*GROOOAAAR*");
            PlaySound(0x56F);

            // Scatter landmines on death
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));
            PackItem(new Emerald(Utility.RandomMinMax(3, 6)));
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));

            if (Utility.RandomDouble() < 0.03)
                PackItem(new Truthrender());  // Unique rare drop
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public MineWolf(Serial serial) : base(serial) { }

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
            var now = DateTime.UtcNow;
            _nextEarthquake = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextCoalBreath = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            _lastLocation = this.Location;
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mine horror corpse")]
    public class MineHorror : BaseCreature
    {
        private DateTime _nextStomp;
        private DateTime _nextGas;
        private DateTime _nextBarrage;
        private Point3D _lastLocation;

        // A dusky iron‐brown hue evoking molten ore and tainted stone
        private const int UniqueHue = 2302;

        [Constructable]
        public MineHorror()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a mine horror";
            Body = 721;
            Hue = UniqueHue;

            SetStr(300, 350);
            SetDex(150, 180);
            SetInt(600, 700);

            SetHits(2000, 2400);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 140.0, 160.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.DetectHidden,120.0, 130.0);
            SetSkill(SkillName.Poisoning,   100.0, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;
            ControlSlots = 4;

            _nextStomp    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            _nextGas      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextBarrage  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _lastLocation = this.Location;

            PackItem(new IronOre(Utility.RandomMinMax(20, 40)));
            PackItem(new GoldOre(Utility.RandomMinMax(10, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && Alive && m.Alive && Map == m.Map && m.InRange(Location, 1) && CanBeHarmful(m, false))
            {
                // 20% chance to leave a quicksand trap at the last location
                if (Utility.RandomDouble() < 0.20 && m is Mobile)
                {
                    var loc = oldLocation;
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(loc, Map);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == Map.Internal)
                return;

            // Rock Stomp: close-range AoE
            if (DateTime.UtcNow >= _nextStomp && InRange(Combatant.Location, 4))
            {
                RockStomp();
                _nextStomp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Toxic Gas: area denial
            else if (DateTime.UtcNow >= _nextGas && InRange(Combatant.Location, 6))
            {
                ReleaseToxicGas();
                _nextGas = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            // Rock Barrage: long–range volley
            else if (DateTime.UtcNow >= _nextBarrage)
            {
                RockBarrage();
                _nextBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        private void RockStomp()
        {
            PlaySound(0x29D);
            FixedParticles(0x36BD, 1, 15, 0x2110, UniqueHue, 0, EffectLayer.Waist);

			var targets = new List<Mobile>();
			IPooledEnumerable eable = Map.GetMobilesInRange(Location, 4);
			try
			{
				foreach (Mobile m in eable)
				{
					if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
						targets.Add(m);
				}
			}
			finally
			{
				eable.Free();
			}


            foreach (var m in targets)
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                }
            }
        }

        private void ReleaseToxicGas()
        {
            Say("**hiss**");
            PlaySound(0x143);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tg = new ToxicGasTile();
                tg.Hue = UniqueHue;
                tg.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        private void RockBarrage()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false) || !SpellHelper.ValidIndirectTarget(this, target))
                return;

            PlaySound(0x2A4);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.3), () =>
                {
                    if (target.Alive)
                    {
                        var from = Location;
                        var to   = target.Location;

                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, from, Map),
                            new Entity(Serial.Zero, to,   Map),
                            0x36D4, 7, 0, false, false,
                            UniqueHue, 0, 0xE10, 0, 0,
                            EffectLayer.Head, 0x100
                        );

                        if (target is Mobile t)
                        {
                            DoHarmful(t);
                            AOS.Damage(t, this, Utility.RandomMinMax(25, 35), 0, 100, 0, 0, 0);
                        }
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            PlaySound(0x2A3);
            FixedParticles(0x36BD, 10, 20, 0x2110, UniqueHue, 0, EffectLayer.CenterFeet);

            // Spawn seismic tremor hazards around the corpse
            for (int i = 0; i < 5; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var eq = new EarthquakeTile();
                eq.Hue = UniqueHue;
                eq.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            PackItem(new Noiseblight());

            if (Utility.RandomDouble() < 0.02)
                PackItem(new SylphshimmerGown());
        }

        public override int GetIdleSound()  => 1553;
        public override int GetAngerSound() => 1550;
        public override int GetHurtSound()  => 1552;
        public override int GetDeathSound() => 1551;

        public override bool BleedImmune       => true;
        public override int  TreasureMapLevel  => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public MineHorror(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            // Re‑initialize cooldowns on load
            _nextStomp   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            _nextGas     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            _nextBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _lastLocation = this.Location;
        }
    }
}

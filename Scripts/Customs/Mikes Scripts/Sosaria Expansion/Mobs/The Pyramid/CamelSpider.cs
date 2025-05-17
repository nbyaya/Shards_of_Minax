using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a camel spider corpse")]
    public class CamelSpider : BaseCreature
    {
        // Ability cooldown timers
        private DateTime _nextBurrow;
        private DateTime _nextVenom;
        private DateTime _nextSandstorm;
        private DateTime _nextWeb;

        // Sandy‑orange hue
        private const int UniqueHue = 2967;

        [Constructable]
        public CamelSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.25, 0.5)
        {
            Name = "a camel spider";
            Body = 737;
            Hue  = UniqueHue;

            // Stats
            SetStr(200, 230);
            SetDex(180, 200);
            SetInt( 80, 100);

            SetHits(500, 600);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison,   50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   40, 50);

            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.Tactics,      90.0, 100.0);
            SetSkill(SkillName.MagicResist,  80.0,  90.0);
            SetSkill(SkillName.Hiding,      120.0, 130.0);
            SetSkill(SkillName.Stealth,     120.0, 130.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // Schedule first uses
            _nextBurrow     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextVenom      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            _nextSandstorm  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextWeb        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            // Loot
            PackItem(new SpidersSilk(Utility.RandomMinMax(20, 30)));
            PackItem(new Bone()); // bones from its prey
            if (Utility.RandomDouble() < 0.05)
                PackItem(new ShellsongVisor()); // rare fang drop
        }

        public override bool BleedImmune   => true;
        public override int  TreasureMapLevel => 5;
        public override double DispelDifficulty => 80.0;
        public override double DispelFocus      => 40.0;

        public override int GetIdleSound()  => 1605;
        public override int GetAngerSound() => 1602;
        public override int GetHurtSound()  => 1604;
        public override int GetDeathSound() => 1603;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && Alive && !Deleted && Map != null)
            {
                var now = DateTime.UtcNow;

                if (now >= _nextBurrow && InRange(target.Location, 1))
                {
                    BurrowAmbush(target);
                    _nextBurrow = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }

                if (now >= _nextVenom && InRange(target.Location, 8))
                {
                    VenomSpray();
                    _nextVenom = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                }

                if (now >= _nextSandstorm)
                {
                    SandstormGust();
                    _nextSandstorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }

                if (now >= _nextWeb)
                {
                    WebTrapField();
                    _nextWeb = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
            }
        }

        private void BurrowAmbush(Mobile target)
        {
            // Camel Spider vanishes into sand
            Hidden = true;
            PlaySound(0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue,
                0,             // renderMode
                5039,          // effect
                (int)EffectLayer.CenterFeet
            );

            // After a short delay, reappear beneath its foe
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (!Alive || Deleted) return;

                Hidden = false;
                MoveToWorld(target.Location, Map);
                PlaySound(0x22F);
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(45, 65), 0, 0, 0, 0, 100);
                target.SendMessage("The camel spider bursts from the sand!");
                target.FixedParticles(
                    0x3779, 10, 25, 5032,
                    UniqueHue,
                    0,                       // renderMode
                    (int)EffectLayer.Head
                );
            });
        }

        private void VenomSpray()
        {
            // Cone of blistering poison
            PlaySound(0x23A);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 1, 30, UniqueHue,
                0,
                5032,
                (int)EffectLayer.CenterFeet
            );

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m.Alive)
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(25, 40), 0, 0, 0, 0, 100);

                if (Utility.RandomDouble() < 0.5)
                    m.ApplyPoison(this, Poison.Deadly);

                m.SendMessage("You are coated in burning venom!");
                m.FixedParticles(
                    0x374A, 10, 15, 5032,
                    UniqueHue,
                    0,
                    (int)EffectLayer.Head
                );
            }

            // Lay down a poison cloud tile
            var tile = new PoisonTile { Hue = UniqueHue };
            tile.MoveToWorld(Location, Map);
        }

        private void SandstormGust()
        {
            Say("*The sands rage!*");
            PlaySound(0x32);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3692, 8, 30, UniqueHue,
                0,
                5039,
                (int)EffectLayer.Waist
            );

            foreach (Mobile m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && m.Alive)
                {
                    DoHarmful(m);
                    int dmg = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);
                    m.SendMessage("The swirling sand lashes you!");

                    var quake = new EarthquakeTile { Hue = UniqueHue };
                    quake.MoveToWorld(m.Location, Map);
                }
            }
        }

        private void WebTrapField()
        {
            PlaySound(0x1B3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3761, 5, 20, UniqueHue,
                0,
                5032,
                (int)EffectLayer.CenterFeet
            );

            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var web = new TrapWeb();
                    web.MoveToWorld(loc, Map);
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            // Last‑ditch quicksand hazards
            PlaySound(0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue,
                0,
                5052,
                (int)EffectLayer.CenterFeet
            );

            int count = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var qs = new QuicksandTile { Hue = UniqueHue };
                    qs.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 5, 20, UniqueHue,
                        0,
                        5039,
                        (int)EffectLayer.Waist
                    );
                }
            }
        }

        public CamelSpider(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset cooldowns on load
            _nextBurrow    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextVenom     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            _nextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextWeb       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;        // for SpellHelper
using Server.Spells.Seventh; // for any spell‐style effects

namespace Server.Mobiles
{
    [CorpseName("a scorched bear corpse")]
    public class DesertBear : BaseCreature
    {
        private DateTime m_NextHeatAuraTime;
        private DateTime m_NextSandstormTime;
        private DateTime m_NextChargeTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1898; // sandy tan

        [Constructable]
        public DesertBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a desert bear";
            Body = 167;
            BaseSoundID = 0xA3;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(80, 100);
            SetInt(50, 60);

            SetHits(250, 350);
            SetStam(80, 100);
            SetMana(0);

            SetDamage(18, 25);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire,     30);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   30, 40);

            // Skills
            SetSkill(SkillName.Wrestling,    90.0, 100.0);
            SetSkill(SkillName.Tactics,      90.0, 100.0);
            SetSkill(SkillName.MagicResist,  80.0,  90.0);
            SetSkill(SkillName.Anatomy,      85.0,  95.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 60;

            // Not tamable
            ControlSlots = 0;

            // Initialize cooldowns
            m_NextHeatAuraTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChargeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            m_LastLocation = this.Location;

            // Some desert loot
            PackItem(new Granite(Utility.RandomMinMax(5, 10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
        }

        // --- Movement: Leave quicksand traps behind ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (Utility.RandomDouble() < 0.15 && this.Map != null) // 15% chance on each tick
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if (this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Think: Trigger special attacks when ready ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextChargeTime && this.InRange(Combatant.Location, 12))
            {
                FrenziedCharge();
                m_NextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextSandstormTime && this.InRange(Combatant.Location, 10))
            {
                SandstormTrap();
                m_NextSandstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            else if (now >= m_NextHeatAuraTime && this.InRange(Combatant.Location, 3))
            {
                HeatAura();
                m_NextHeatAuraTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        // --- Heat Aura: burns all nearby foes ---
        private void HeatAura()
        {
            this.Say("*Gruff roar!*");
            PlaySound(0x2D6); // scorching roar

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3);
            foreach (var o in eable)
            {
                if (o is Mobile target && CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);

                    int dmg = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, dmg, 0, 100, 0, 0, 0); // 100% fire
                    target.SendMessage("The desert bear's scorching aura burns you!");
                    target.FixedParticles(0x3709, 10, 15, 5028, UniqueHue, 0, EffectLayer.Head);
                }
            }
            eable.Free();
        }

        // --- Sandstorm Trap: sprinkles quicksand around your target ---
        private void SandstormTrap()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*The sands obey me!*");
            PlaySound(0x3E7); // swirling sand

            var center = target.Location;
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-1, 1), dy = Utility.RandomMinMax(-1, 1);
                var loc = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, UniqueHue, 0, 5039, 0);
                }
            }
            target.SendMessage("You stagger as quicksand erupts beneath you!");
        }

        // --- Frenzied Charge: leap in, slam & stun ---
        private void FrenziedCharge()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*RROOAAARR!*");
            PlaySound(0x156); // charge roar

            DoHarmful(target);
            int dmg = Utility.RandomMinMax(30, 50);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

            if (target is Mobile m)
            {
                m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(20, 30));
                m.Paralyze(TimeSpan.FromSeconds(1.5));
                m.SendMessage("The desert bear's charge knocks you off your feet!");
                m.FixedParticles(0x3709, 10, 12, 5028, UniqueHue, 0, EffectLayer.Waist);
            }
        }

        // --- Death: fiery sandstorm arises around the corpse ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*...grrr...*");
                PlaySound(0x208);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 30, UniqueHue, 0, 5025, 0);

                int count = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var flame = new FlamestrikeHazardTile();
                        flame.Hue = UniqueHue;
                        flame.MoveToWorld(loc, Map);

                        Effects.SendLocationParticles(
                            EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                            0x376A, 8, 20, UniqueHue, 0, 5039, 0);
                    }
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Misc ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 1);

            if (Utility.RandomDouble() < 0.05) // 5% chance for a claw trophy
                PackItem(new ReapersWeft());
        }

        public override int Meat  => 2;
        public override int Hides => 20;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 4;

        public DesertBear(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‐init timers on server restart
            m_NextHeatAuraTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChargeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
        }
    }
}

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a nile river serpent corpse")]
    public class NileRiverSerpent : BaseCreature
    {
        private DateTime m_NextWaterJet;
        private DateTime m_NextWhirlpool;
        private DateTime m_NextVenomSpray;
        private DateTime m_NextLightningStorm;
        private Point3D m_LastLocation;

        private const int UniqueHue = 0x596; // Jade‑green

        [Constructable]
        public NileRiverSerpent() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Nile River Serpent";
            Body = 150;
            BaseSoundID = 447;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(80, 120);
            SetInt(75, 100);

            SetHits(800, 950);
            SetStam(120, 150);
            SetMana(400, 500);

            SetDamage(20, 30);

            // Damage split
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold,      30);
            SetDamageType(ResistanceType.Poison,    50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.EvalInt,    90.1, 105.0);
            SetSkill(SkillName.Magery,    100.1, 115.0);
            SetSkill(SkillName.MagicResist,110.1, 125.0);
            SetSkill(SkillName.Tactics,    95.1, 110.0);
            SetSkill(SkillName.Wrestling,  95.1, 110.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize ability cooldowns
            m_NextWaterJet      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWhirlpool     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextVenomSpray    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextLightningStorm= DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // Starter loot
            PackItem(new RawFishSteak(Utility.RandomMinMax(5,10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5,10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5,10)));
        }

        // Leave quicksand patches behind
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || !Alive)
                return;

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.10)
            {
                var spot = m_LastLocation;
                if (!Map.CanFit(spot.X, spot.Y, spot.Z, 16, false, false))
                    spot.Z = Map.GetAverageZ(spot.X, spot.Y);

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(spot, this.Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextWaterJet && InRange(Combatant.Location, 12))
            {
                WaterJetAttack();
                m_NextWaterJet = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            else if (now >= m_NextWhirlpool && InRange(Combatant.Location, 10))
            {
                WhirlpoolAttack();
                m_NextWhirlpool = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            else if (now >= m_NextVenomSpray && InRange(Combatant.Location, 4))
            {
                VenomSprayAttack();
                m_NextVenomSpray = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            else if (now >= m_NextLightningStorm)
            {
                LightningStormAttack();
                m_NextLightningStorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // 1) High‑pressure cold water blast
        private void WaterJetAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*The Nile roars!*");
            PlaySound(0x229);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 5, 0, false, true,
                UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, dmg, 0, 0, 0, 30, 70);
                    if (target is Mobile m)
                    {
                        m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(10, 20));
                        m.SendMessage("You’re chilled by the icy torrent!");
                    }
                }
            });
        }

        // 2) Summon a swirling vortex at the target’s feet
        private void WhirlpoolAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Feel the current!*");
            PlaySound(0x658);
            var loc = target.Location;

            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x387D, 10, 20, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (this.Map == null) return;

                var vortex = new VortexTile();
                vortex.Hue = UniqueHue;
                var spot = loc;
                if (!Map.CanFit(spot.X, spot.Y, spot.Z, 16, false, false))
                    spot.Z = Map.GetAverageZ(spot.X, spot.Y);
                vortex.MoveToWorld(spot, this.Map);
            });
        }

        // 3) Cone of toxic venom
        private void VenomSprayAttack()
        {
            Say("*A shower of venom!*");
            PlaySound(0x5C4);

            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && InLOS(m))
                {
                    if (m is Mobile targ)
                    {
                        DoHarmful(targ);
                        int dmg = Utility.RandomMinMax(30, 50);
                        AOS.Damage(targ, this, dmg, 0, 0, 0, 0, 100);
                        targ.ApplyPoison(this, Poison.Greater);
                        targ.SendMessage("You’re envenomed!");
                    }
                }
            }
            eable.Free();
        }

        // 4) Call down electrified storm at random points around the combatant
        private void LightningStormAttack()
        {
            Say("*Storms of the Nile!*");
            PlaySound(0x307);

            if (!(Combatant is Mobile target) || Map == null)
                return;

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(0.3 * i), () =>
                {
                    var loc = new Point3D(
                        target.X + Utility.RandomMinMax(-2, 2),
                        target.Y + Utility.RandomMinMax(-2, 2),
                        target.Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var storm = new LightningStormTile();
                    storm.Hue = UniqueHue;
                    storm.MoveToWorld(loc, this.Map);
                });
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The river returns...*");
                PlaySound(0x228);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn quicksand and poison hazards
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = (Utility.RandomBool() 
                        ? (Item)new QuicksandTile() 
                        : new PoisonTile()) as Item;
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                }
            }

            base.OnDeath(c);
        }

        // Loot & drops
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 10);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new DuskfeastBlouse()); // custom unique fang item
        }

        // Overrides
        public override bool BleedImmune       { get { return true; } }
        public override int  TreasureMapLevel  { get { return 5;    } }
        public override int  Hides             { get { return 15;   } }
        public override int  Scales            { get { return 10;   } }
        public override ScaleType ScaleType    { get { return ScaleType.Green; } }

        public NileRiverSerpent(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on reload
            m_NextWaterJet       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWhirlpool      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextVenomSpray     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextLightningStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}

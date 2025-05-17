using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a doom guardian corpse")]
    public class DoomGuardian : BaseCreature
    {
        // Cooldown timers for unique abilities
        private DateTime m_NextFirestorm, m_NextStomp, m_NextPit, m_NextTeleport;
        private Point3D m_LastLocation;

        // A deep crimson hue for all effects and tiles
        private const int UniqueHue = 1170;

        [Constructable]
        public DoomGuardian()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Doom Guardian";
            Title = null;
            Hue = UniqueHue;
            BaseSoundID = 0x482; // Guardian‑style base sound

            SpeechHue = Utility.RandomDyedHue();

            // --- Body & Name (from original Guardian) ---
            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            // --- Raw Stats ---
            SetStr(400, 450);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(1600, 1800);
            SetStam(200, 250);
            SetMana(500, 700);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 40);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            // --- Skills & Reputation ---
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // --- Initialize ability cooldowns ---
            m_NextFirestorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextStomp      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextPit        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextTeleport   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // --- Standard reagent loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 15)));
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
        }

        // Leave a cloud of toxic gas behind when moving
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                var tile = new ToxicGasTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(oldLocation, this.Map);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Firestorm: spawn FlamestrikeHazardTile around self
            if (DateTime.UtcNow >= m_NextFirestorm)
            {
                DoFirestorm();
                m_NextFirestorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Earthquake Stomp: area EarthquakeTile hazards
            else if (DateTime.UtcNow >= m_NextStomp)
            {
                DoEarthquakeStomp();
                m_NextStomp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Quicksand Pit: trap the target
            else if (DateTime.UtcNow >= m_NextPit)
            {
                DoQuicksandPit();
                m_NextPit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Dark Teleport: blink to combatant, leave teleport tile
            else if (DateTime.UtcNow >= m_NextTeleport)
            {
                DoDarkTeleport();
                m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        private void DoFirestorm()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Feel the inferno!*");
            PlaySound(0x208);
            FixedParticles(0x36BD, 20, 10, 5044, UniqueHue, 0, EffectLayer.Waist);

            // Spawn flamestrikes on everyone in 6‑tile radius
            var eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    var loc = m.Location;
                    var tile = new FlamestrikeHazardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
            eable.Free();
        }

        private void DoEarthquakeStomp()
        {
            Say("*The earth trembles!*");
            PlaySound(0x22C);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x5605, 10, 30, UniqueHue, 0, 5026, 0);

            // Scatter 5 earthquake hazards randomly in 4‑tile radius
            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var p = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                var tile = new EarthquakeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);
            }
        }

        private void DoQuicksandPit()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Sink into despair!*");
            PlaySound(0x2F3);

            var loc = target.Location;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

            var tile = new QuicksandTile();
            tile.Hue = UniqueHue;
            tile.MoveToWorld(loc, Map);
        }

        private void DoDarkTeleport()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            var oldLoc = Location;
            Say("*I return from the abyss!*");
            PlaySound(0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(oldLoc, Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0);

            // Blink near the target
            var dest = target.Location;
            dest.X += Utility.RandomMinMax(-1, 1);
            dest.Y += Utility.RandomMinMax(-1, 1);
            if (!Map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
                dest.Z = Map.GetAverageZ(dest.X, dest.Y);

            MoveToWorld(dest, Map);

            // Leave a chaotic teleport tile behind
            var tile = new ChaoticTeleportTile();
            tile.Hue = UniqueHue;
            tile.MoveToWorld(oldLoc, Map);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;
			
			Say("*Your doom is eternal!*");
            Effects.PlaySound(Location, Map, 0x213);

            // Burst of necromantic flame tiles around the corpse
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var p = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = Map.GetAverageZ(p.X, p.Y);

                var tile = new NecromanticFlamestrikeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // 5% chance for a Doom Core artifact
            if (Utility.RandomDouble() < 0.05)
                PackItem(new SpiralknotObi()); // Define DoomCore elsewhere
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus => 75.0;

        public DoomGuardian(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑initialize cooldowns on load
            m_NextFirestorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextStomp      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextPit        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextTeleport   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;
        }
    }
}

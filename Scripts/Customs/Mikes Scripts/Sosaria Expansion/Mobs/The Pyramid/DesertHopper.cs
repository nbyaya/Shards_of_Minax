using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // for spell effects if needed
using Server.Spells.Seventh; // for Chain Lightning base (unused here)
using Server.Misc;         // for Utility

namespace Server.Mobiles
{
    [CorpseName("a desert hopper corpse")]
    public class DesertHopper : BaseCreature
    {
        private DateTime m_NextSandWave;
        private DateTime m_NextQuicksand;
        private DateTime m_NextSandBurst;
        private Point3D m_LastLocation;

        // A warm, sandy-gold hue
        private const int UniqueHue = 1353;

        [Constructable]
        public DesertHopper()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Desert Hopper";
            Body = 302;
            BaseSoundID = 959;
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(200, 250);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(800, 1000);
            SetStam(200, 250);
            SetMana(100, 150);

            // --- Damage ---
            SetDamage(10, 15);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire,     30); // scorching sand

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   30, 40);

            // --- Skills ---
            SetSkill(SkillName.Tactics,       80.0, 100.0);
            SetSkill(SkillName.Wrestling,     80.0, 100.0);
            SetSkill(SkillName.MagicResist,   75.0,  90.0);

            Fame           = 12000;
            Karma          = -12000;
            VirtualArmor  = 60;
            ControlSlots  = 3;

            // Initialize cooldowns
            m_NextSandWave    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextQuicksand   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSandBurst   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new Granite(Utility.RandomMinMax(5, 10)));
            PackItem(new Gold(Utility.RandomMinMax(200, 400)));
        }

        // Leave quicksand under anyone moving very close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && Alive && m.Alive && m.Map == Map && m.InRange(this.Location, 2) && Utility.RandomDouble() < 0.20)
            {
                if (m is Mobile target)
                {
                    var loc = target.Location;
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var tile = new QuicksandTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            DateTime now = DateTime.UtcNow;

            // Highest priority: Sand Wave
            if (now >= m_NextSandWave && InRange(Combatant.Location, 10))
            {
                SandWave();
                m_NextSandWave = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            else if (now >= m_NextQuicksand)
            {
                QuicksandSummon();
                m_NextQuicksand = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 16));
            }
            else if (now >= m_NextSandBurst && InRange(Combatant.Location, 6))
            {
                SandBurst();
                m_NextSandBurst = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // A sweeping cone of scouring sand
        public void SandWave()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The desert roars!*");
                PlaySound(0x1E1);

                FixedParticles(0x3728, 10, 15, 5033, UniqueHue, 0, EffectLayer.Waist);

                var hits = new List<Mobile>();
                foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
                {
                    if (m != this && CanBeHarmful(m, false) && m.InLOS(this) && Utility.RandomDouble() < 0.75)
                        hits.Add(m);
                }

                foreach (var m in hits)
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
                    if (m is Mobile mv)
                    {
                        mv.SendMessage("You're battered by the scouring sand!");
                        mv.Stam = Math.Max(0, mv.Stam - Utility.RandomMinMax(20, 35));
                    }
                }
            }
        }

        // Summon quicksand pits beneath the combatant
        public void QuicksandSummon()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*The sands shift beneath you!*");
            PlaySound(0x3D);
            var center = target.Location;

            for (int i = 0; i < 3; i++)
            {
                int dx = Utility.RandomMinMax(-1, 1);
                int dy = Utility.RandomMinMax(-1, 1);
                var loc = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
        }

        // Violent burst of sand around itself
        public void SandBurst()
        {
            Say("*Feel the fury of the dunes!*");
            PlaySound(0x2A);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 8, 30, UniqueHue, 0, 5039, 0);

            foreach (Mobile m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
                    if (m is Mobile mv && Utility.RandomDouble() < 0.5)
                    {
                        mv.SendMessage("You're blinded by the swirling sand!");
                        mv.FixedParticles(0x37C4, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                }
            }

            // leave a patch of vortex-sand on the ground
            var tile = new VortexTile();
            tile.Hue = UniqueHue;
            tile.MoveToWorld(Location, Map);
        }

        // Death: explode in a cloud of scalding sand
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*A final whirlwind!*");
                PlaySound(0x1F7);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 15, 50, UniqueHue, 0, 5052, 0);

                // scatter a few quicksand pits
                for (int i = 0; i < 4; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var qs = new QuicksandTile();
                        qs.Hue = UniqueHue;
                        qs.MoveToWorld(loc, Map);
                    }
                }
            }

            base.OnDeath(c);
        }

        // Properties & Loot
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 8));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new AntlerborneVeil());  // unique crafting component
        }

        // Serialization
        public DesertHopper(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init cooldowns
            m_NextSandWave  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextQuicksand = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSandBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation  = this.Location;
        }
    }
}

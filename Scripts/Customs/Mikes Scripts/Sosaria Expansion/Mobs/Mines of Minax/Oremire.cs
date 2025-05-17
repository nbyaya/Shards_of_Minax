using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("an oremire corpse")]
    public class Oremire : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextShockTime;
        private DateTime m_NextCorruptTime;
        private DateTime m_NextMagmaEruptionTime;
        private Point3D m_LastLocation;

        // Unique hue – a dark, metallic green
        private const int UniqueHue = 1365;

        [Constructable]
        public Oremire() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.5)
        {
            Name = "Oremire";
            Body = 789;               // same body as Quagmire
            BaseSoundID = 352;        // same base sound
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(350, 420);
            SetDex(80, 120);
            SetInt(100, 150);

            SetHits(1800, 2200);
            SetStam(150, 200);
            SetMana(200, 300);

            SetDamage(20, 30);

            //  Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Fire, 25);

            //  Resistances
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 20, 35);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 30, 45);

            //  Skills
            SetSkill(SkillName.MagicResist, 100.1, 115.0);
            SetSkill(SkillName.Tactics,     90.1, 100.0);
            SetSkill(SkillName.Wrestling,   95.1, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 85;
            ControlSlots = 4;  // Boss-level

            // Initialize ability cooldowns
            m_NextShockTime         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextMagmaEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation          = this.Location;

            // Starting loot
            PackItem(new IronOre(Utility.RandomMinMax(15, 25)));
            PackItem(new SulfurousAsh (Utility.RandomMinMax(10, 15)));
            PackItem(new WhispersOfChaos());
        }

        // --- Leave corrupting ground as it moves ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Deleted || !this.Alive || this.Map == null)
                return;

            // Drop quicksand tiles occasionally
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D dropLoc = oldLocation;
                int z = this.Map.GetAverageZ(dropLoc.X, dropLoc.Y);
                if (this.Map.CanFit(dropLoc.X, dropLoc.Y, z, 16, false, false))
                {
                    QuicksandTile qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(new Point3D(dropLoc.X, dropLoc.Y, z), this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Shockwave slam (earthquake) in melee range
            if (DateTime.UtcNow >= m_NextShockTime && this.InRange(Combatant.Location, 3))
            {
                EarthquakeSlam();
                m_NextShockTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Corrupting poison cloud at range
            else if (DateTime.UtcNow >= m_NextCorruptTime && this.InRange(Combatant.Location, 10))
            {
                CorruptingCloud();
                m_NextCorruptTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            // Magma eruption around self
            else if (DateTime.UtcNow >= m_NextMagmaEruptionTime)
            {
                MagmaEruption();
                m_NextMagmaEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Earthquake Slam: AoE physical + knockback ---
        private void EarthquakeSlam()
        {
            this.PlaySound(0x2A1); // heavy impact
            this.FixedParticles(0x375A, 10, 30, 5031, UniqueHue, 0, EffectLayer.Waist);

            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

                    // Knockback one tile: show effect then move
                    Effects.SendLocationEffect(
                        target.Location, 
                        target.Map, 
                        0x3728,  // graphic
                        10,      // duration
                        10       // speed
                    );  // :contentReference[oaicite:2]{index=2}

                    // Compute the direction from Oremire to the target, 
                    // then push them one tile that way
                    Direction knockbackDir = this.GetDirectionTo(target.Location);
                    target.Move(knockbackDir);  // :contentReference[oaicite:3]{index=3}
                }
            }
            eable.Free();
        }

        // --- Corrupting Cloud: PoisonTile zone at target ---
        private void CorruptingCloud()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Point3D loc = target.Location;
                this.Say("*Feel the rot!*");
                PlaySound(0x64F); // corrosive hiss

                PoisonTile pt = new PoisonTile();
                pt.Hue = UniqueHue;
                pt.MoveToWorld(loc, this.Map);
            }
        }

        // --- Magma Eruption: HotLavaTiles around self ---
        private void MagmaEruption()
        {
            this.Say("*Burn!*");
            PlaySound(0x15E); // lava rumble

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Map.GetAverageZ(x, y);

                if (Map.CanFit(x, y, z, 16, false, false))
                {
                    HotLavaTile lava = new HotLavaTile
                    {
                        Hue = UniqueHue
                    };
                    lava.MoveToWorld(new Point3D(x, y, z), this.Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(new Point3D(x, y, z), Map, EffectItem.DefaultDuration),
                        0x3709, 8, 20, UniqueHue, 0, 5032, 0
                    );
                }
            }
        }

        // --- Death Effect: Shattering Rupture + Hazard tiles ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.PlaySound(0x2A2);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 12, 25, UniqueHue, 0, 5039, 0
            );

            // Scatter landmine and poison tiles
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Map.GetAverageZ(x, y);

                if (!Map.CanFit(x, y, z, 16, false, false))
                    continue;

                if (Utility.RandomDouble() < 0.5)
                {
                    LandmineTile mine = new LandmineTile { Hue = UniqueHue };
                    mine.MoveToWorld(new Point3D(x, y, z), Map);
                }
                else
                {
                    PoisonTile pt = new PoisonTile { Hue = UniqueHue };
                    pt.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }
        }

        // --- Standard Overrides & Loot ---
        public override bool BleedImmune      => true;
        public override Poison HitPoison      => Poison.Lethal;
        public override double HitPoisonChance => 0.2;  // 20% chance
        public override Poison PoisonImmune   => Poison.Lethal;
        public override int TreasureMapLevel  => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls,   Utility.RandomMinMax(2, 4));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new BlightTouchedWarhelm());    // special shard drop
        }

        public Oremire(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers
            m_NextShockTime         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextMagmaEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation          = this.Location;
        }
    }
}

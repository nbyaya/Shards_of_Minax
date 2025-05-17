using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // for any chain or bolt spells
using Server.Spells.Fifth;   // for FlamestrikeHazardTile


namespace Server.Mobiles
{
    [CorpseName("a pharaoh's unicorn corpse")]
    public class PharaohsUnicorn : BaseCreature
    {
        private DateTime m_NextSandstormTime;
        private DateTime m_NextSolarFlareTime;
        private DateTime m_NextHealingAuraTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 2125; // radiant gold

        [Constructable]
        public PharaohsUnicorn() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Pharaoh's Unicorn";
            Body = 0x7A;
            BaseSoundID = 0x4BC;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(125, 150);
            SetInt(300, 350);

            SetHits(900, 1000);
            SetStam(125, 150);
            SetMana(400, 500);

            SetDamage(25, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire,     25);
            SetDamageType(ResistanceType.Energy,   25);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.EvalInt,    100.1, 110.0);
            SetSkill(SkillName.Magery,     100.1, 110.0);
            SetSkill(SkillName.MagicResist,120.2, 130.0);
            SetSkill(SkillName.Meditation,  90.0, 100.0);
            SetSkill(SkillName.Tactics,     90.1, 100.0);
            SetSkill(SkillName.Wrestling,   80.1,  90.0);

            Fame = 15000;
            Karma = 15000;

            VirtualArmor = 60;

            // Not tamable
            Tamable = false;
            ControlSlots = 0;

            // Cooldowns
            m_NextSandstormTime  = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextSolarFlareTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextHealingAuraTime= DateTime.UtcNow + TimeSpan.FromSeconds(30);

            m_LastLocation = this.Location;

            // Basic loot
            PackGold(1500, 2000);
            PackItem(new NailspikeGrips()); // example special item
        }

        // *** Movement effect: leave quicksand behind to slow pursuers
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Map == this.Map && InRange(m.Location, 3) && Alive && CanBeHarmful(m, false))
            {
                // Drop a quicksand tile at the unicorn's previous spot
                Point3D dropLoc = oldLocation;
                if (Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(dropLoc, this.Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            var now = DateTime.UtcNow;

            // Sandstorm AoE every ~20s (radius 8)
            if (now >= m_NextSandstormTime && InRange(Combatant.Location, 12))
            {
                CastSandstorm();
                m_NextSandstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            // Solar Flare single-target burst every ~25s
            else if (now >= m_NextSolarFlareTime && InRange(Combatant.Location, 10))
            {
                CastSolarFlare();
                m_NextSolarFlareTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Healing Aura when under half health every ~30s
            else if (now >= m_NextHealingAuraTime && Hits < (HitsMax / 2))
            {
                CastHealingAura();
                m_NextHealingAuraTime = now + TimeSpan.FromSeconds(30);
            }
        }

        // --- Sandstorm: creates flame hazards in a circle around target location
        private void CastSandstorm()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The desert whirls!*");
                PlaySound(0x20E); // wind roar

                Point3D ctr = target.Location;
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-5, 5);
                    int dy = Utility.RandomMinMax(-5, 5);
                    Point3D loc = new Point3D(ctr.X + dx, ctr.Y + dy, ctr.Z);

                    // place Flamestrike hazard
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var flame = new FlamestrikeHazardTile();
                    flame.Hue = UniqueHue;
                    flame.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Solar Flare: a line-of-sight fire/energy burst on the target
        private void CastSolarFlare()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*By the sun's fury!*");
                PlaySound(0x208); // fire burst

                DoHarmful(target);
                int damage = Utility.RandomMinMax(45, 65);
                AOS.Damage(target, this, damage, 0, 30, 40, 0, 30);

                target.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Head);

                // leave a hot lava tile at their feet
                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(target.Location, Map);
            }
        }

        // --- Healing Aura: heals self and pulses healing to nearby minions/allies
        private void CastHealingAura()
        {
            Say("*I draw strength from the sands!*");
            PlaySound(0x229); // healing sound
            FixedParticles(0x376A, 10, 30, 5060, UniqueHue, 0, EffectLayer.CenterFeet);

            int heal = Utility.RandomMinMax(100, 150);
            Heal(heal);

            // optional: heal any summoned creatures nearby
            foreach (Mobile m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && m is BaseCreature bc && bc.Controlled && bc.ControlMaster == this)
                {
                    bc.Heal(heal / 2);
                    bc.FixedParticles(0x376A, 8, 20, 5052, UniqueHue, 0, EffectLayer.CenterFeet);
                }
            }

            // leave a brief healing pulse tile underfoot
            var pulse = new HealingPulseTile();
            pulse.Hue = UniqueHue;
            pulse.MoveToWorld(Location, Map);
        }

        // --- Death Explosion
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My reign... ends...*");
                PlaySound(0x214); // explosion
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 15, 60, UniqueHue, 0, 5052, 0
                );

                // Spawn a few toxic gas hazards
                for (int i = 0; i < 4; i++)
                {
                    int dx = Utility.RandomMinMax(-2, 2), dy = Utility.RandomMinMax(-2, 2);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Properties ---
        public override bool BleedImmune     => true;
        public override Poison PoisonImmune  => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus      => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new BeastwardShoulders()); // Unique artifact
        }

        public PharaohsUnicorn(Serial serial) : base(serial) { }

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
            m_NextSandstormTime   = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextSolarFlareTime  = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextHealingAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }
    }
}

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;
using Server.Spells.Seventh;

namespace Server.Mobiles
{
    [CorpseName("an ancient drakon corpse")]
    public class AncientDrakon : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextRoarTime;
        private DateTime m_NextFlameBreathTime;
        private DateTime m_NextEarthquakeTime;
        // Last location for movement effects
        private Point3D m_LastLocation;

        // Unique hue for Ancient Drakon (a weathered, ancient tint)
        private const int UniqueHue = 1175; 

        [Constructable]
        public AncientDrakon() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Ancient Drakon";
            // Using the body/sound from the base dragon, but you can randomize if desired:
            Body = Utility.RandomList(12, 59); 
            BaseSoundID = 362;
            Hue = UniqueHue;

            // --- Significantly Boosted Stats ---
            SetStr(900, 1000);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(2000, 2200);
            // If your shard supports stamina/mana separate from BaseCreature, you can set those too:
            // SetStam(400, 500);
            // SetMana(600, 750);

            // --- Damage Setup (Physical with added fire infliction) ---
            SetDamage(20, 25);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 40);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;
            ControlSlots = 5;
            Tamable = false; // Bosses like this are generally not tamable

            // --- Initialize Ability Timers ---
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFlameBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEarthquakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // --- Base Loot: You can add more special drops as needed ---
            PackItem(new Gold(Utility.RandomMinMax(800, 1200)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
        }

        // --- Terrifying Roar: AoE fear/damage ---
        public void TerrifyingRoarAttack()
        {
            // Play a deep, resonant roar sound and show particles around self
            this.PlaySound(0x2F6); // example roar sound; adjust as needed
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            // Get nearby mobiles within an 8-tile radius
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 8);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            // Process each affected target
            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                // Deal moderate physical damage (e.g., 25-35)
                int damage = Utility.RandomMinMax(25, 35);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // 100% physical damage

                // Provide feedback message if target is Mobile (using safe check)
                if (target is Mobile mobTarget)
                {
                    mobTarget.SendMessage(0x22, "The booming roar leaves you dazed and shaken!");
                    // Optionally, you might apply a brief stun effect here via a custom timer/flag.
                }
            }
        }

        // --- Elder Flame Breath: Cone AoE fire attack with ground hazards ---
        public void ElderFlameBreathAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*The ancient flames surge forth!*");
            PlaySound(0x16E); // A deep, fiery breath sound
            // Particle effect simulating a sweeping cone of flame
            this.FixedParticles(0x36BD, 20, 15, 5052, UniqueHue, 0, EffectLayer.Head);

            // Determine affected area (radius 10)
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 10);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            // Process damage and chance to leave a hazard
            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                // Fire damage between 30 and 50
                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0); // 100% fire damage

                // With a chance, leave behind a Hot Lava hazard near the target
                if (Utility.RandomDouble() < 0.30)
                {
                    Point3D loc = target.Location;
                    if (!this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        loc.Z = this.Map.GetAverageZ(loc.X, loc.Y);
                        if (!this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                            continue;
                    }
                    HotLavaTile hazard = new HotLavaTile();
                    hazard.Hue = UniqueHue;
                    hazard.MoveToWorld(loc, this.Map);
                }
            }
        }

        // --- Seismic Earthquake: Ground quake leaving a hazardous tile ---
        public void SeismicEarthquakeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Target the combatant's location (ensure it's Mobile-safe)
            Point3D targetLocation;
            IDamageable damageable = Combatant;
            if (damageable is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
            {
                targetLocation = targetMobile.Location;
            }
            else
            {
                targetLocation = damageable.Location;
            }

            this.Say("*The very earth shudders beneath you!*");
            PlaySound(0x220); // Earthquake or ground-breaking sound

            // A brief particle effect to warn of the upcoming quake
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // After a short delay, spawn an EarthquakeTile at the targeted location
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;
                
                Point3D spawnLoc = targetLocation;
                if (!this.Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = this.Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!this.Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                EarthquakeTile tile = new EarthquakeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawnLoc, this.Map);
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Movement Effect: Leave behind occasional hazardous debris ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    // For Ancient Drakon, leave behind a brief Earthquake hazard tile
                    EarthquakeTile debris = new EarthquakeTile();
                    debris.Hue = UniqueHue;
                    debris.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile debris = new EarthquakeTile();
                        debris.Hue = UniqueHue;
                        debris.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Check ability cooldowns and execute attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and range
            if (DateTime.UtcNow >= m_NextRoarTime && this.InRange(Combatant.Location, 8))
            {
                TerrifyingRoarAttack();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextFlameBreathTime && this.InRange(Combatant.Location, 10))
            {
                ElderFlameBreathAttack();
                m_NextFlameBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextEarthquakeTime && this.InRange(Combatant.Location, 12))
            {
                SeismicEarthquakeAttack();
                m_NextEarthquakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Death Effect: Drakon's Final Wrath ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My fury... endures beyond death!*");
                PlaySound(0x211);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn multiple hazardous ground tiles around the corpse
                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    // Randomly choose between HotLavaTile and EarthquakeTile for death hazards
                    BaseCreature hazard = null;
                    if (Utility.RandomDouble() < 0.5)
                    {
                        HotLavaTile lava = new HotLavaTile();
                        lava.Hue = UniqueHue;
                        lava.MoveToWorld(hazardLocation, this.Map);
                    }
                    else
                    {
                        EarthquakeTile quake = new EarthquakeTile();
                        quake.Hue = UniqueHue;
                        quake.MoveToWorld(hazardLocation, this.Map);
                    }
                }
            }
            base.OnDeath(c);
        }

        // --- Standard properties and loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            // Very generous loot for a boss-level creature:
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            // Unique drops can be added here with a small chance:
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
        }

        // --- Serialization ---
        public AncientDrakon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_NextRoarTime);
            writer.Write(m_NextFlameBreathTime);
            writer.Write(m_NextEarthquakeTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextRoarTime = reader.ReadDateTime();
            m_NextFlameBreathTime = reader.ReadDateTime();
            m_NextEarthquakeTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if necessary
            if (m_NextRoarTime < DateTime.UtcNow)
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            if (m_NextFlameBreathTime < DateTime.UtcNow)
                m_NextFlameBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            if (m_NextEarthquakeTime < DateTime.UtcNow)
                m_NextEarthquakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
        }
    }
}

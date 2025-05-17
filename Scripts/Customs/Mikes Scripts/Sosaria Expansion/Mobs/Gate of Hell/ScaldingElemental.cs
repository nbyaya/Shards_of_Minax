using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;            // For potential spell effects
using Server.Network;           // For visual and sound effects
using Server.Spells.Fourth;     // For using fire field effects in death explosions

namespace Server.Mobiles
{
    [CorpseName("the scorched remains of a scalding elemental")]
    public class ScaldingElemental : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextSurgeTime;
        private DateTime m_NextMistTime;
        private Point3D m_LastLocation;

        // Unique hue for Scalding Elemental (a vivid fiery tone)
        private const int UniqueHue = 1350;

        [Constructable]
        public ScaldingElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Faster reaction time
        {
            Name = "a Scalding Elemental";
            Body = 158;               // Based on Acid Elemental's body
            BaseSoundID = 263;        // Based on Acid Elemental's sounds
            Hue = UniqueHue;          // Unique fire-hot hue

            // --- Enhanced Stats ---
            SetStr(500, 600);
            SetDex(250, 300);
            SetInt(400, 500);

            SetHits(1500, 1800);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Fire, 85);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills ---
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 6; // Boss-level creature

            // Initialize cooldowns (in seconds; adjust as desired)
            m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMistTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_LastLocation = this.Location;

            // Standard loot + a few extra ingredients
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));
            
            // Maintain a light source
            AddItem(new LightSource());
        }

        // --- Unique Ability: Scalding Surge --- 
        // A burst attack that splashes scalding liquid around the elemental
        public void ScaldingSurgeAttack()
        {
            if (Map == null)
                return;

            // Visual/sound effect for the surge attack
            PlaySound(0x208); // Fireball/explosion sound
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist); // Fiery splash effect

            // Get mobiles in an approximately 6-tile radius
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Stunning visual effect emanating from the elemental
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                // Deal burst damage and simulate burning with a scalding touch.
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70); // Moderate burst damage
                    // 100% fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    
                    // Optionally, you could add a burn effect (like a delayed damage tick) via Timer.DelayCall here.
                }
            }
        }

        // --- Unique Ability: Boiling Mist Attack ---
        // A delayed meteor-like attack that rains down scalding boiling mist onto target areas.
        public void BoilingMistAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Ensure Combatant is a Mobile before proceeding
            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Emitting a cloud of boiling mist!*");
            PlaySound(0x160); // Meteor/impact sound

            int mistCount = Utility.RandomMinMax(4, 7); // Number of mist impacts
            for (int i = 0; i < mistCount; i++)
            {
                // Slight randomization around the target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                // Validate the impact point on the map
                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Launch a mist projectile visual effect from above
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay the damage application to match the visual impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    // Impact effect for the boiling mist
                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration),
                        0x3709, 10, 30, UniqueHue, 0, 2023, 0);

                    // Get targets within a small radius around the impact point
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(30, 40); // Damage per mist impact
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Overriding OnThink to manage ability cooldowns ---
        public override void OnThink()
        {
            base.OnThink();

            // Check if we have moved; leave a trail of boiling liquid if so
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Drop a short-lived field at the previous location.
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Ensure Combatant is valid and on a valid map
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Use Scalding Surge if within 6 tiles and off cooldown
            if (DateTime.UtcNow >= m_NextSurgeTime && InRange(Combatant.Location, 6))
            {
                ScaldingSurgeAttack();
                m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Use Boiling Mist Attack if within 10 tiles and off cooldown
            else if (DateTime.UtcNow >= m_NextMistTime && InRange(Combatant.Location, 10))
            {
                BoilingMistAttack();
                m_NextMistTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Death Explosion ---
        // Upon death, the Scalding Elemental splashes boiling liquid around, leaving hazardous hot puddles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int liquidTilesToDrop = 10; // Number of boiling puddles
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < liquidTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < liquidTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D liquidLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(liquidLocation.X, liquidLocation.Y, liquidLocation.Z, 16, false, false))
                {
                    liquidLocation.Z = Map.GetAverageZ(liquidLocation.X, liquidLocation.Y);
                    if (!Map.CanFit(liquidLocation.X, liquidLocation.Y, liquidLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(liquidLocation);

                // Spawn the HotLavaTile (assumed defined elsewhere) with the elemental’s unique hue
                HotLavaTile droppedLiquid = new HotLavaTile();
                droppedLiquid.Hue = UniqueHue;
                droppedLiquid.MoveToWorld(liquidLocation, this.Map);

                // Create a smaller visual effect at each spawned puddle
                Effects.SendLocationParticles(EffectItem.Create(liquidLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Create a bigger central explosion effect using one of the effect locations
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Chance for rare drops
            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new InfernosEmbraceCloak());
            }
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
            }
        }

        // --- Serialization ---
        public ScaldingElemental(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextSurgeTime);
            writer.Write(m_NextMistTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextSurgeTime = reader.ReadDateTime();
            m_NextMistTime = reader.ReadDateTime();

            // Reinitialize last location upon load
            m_LastLocation = this.Location;
        }
    }
}

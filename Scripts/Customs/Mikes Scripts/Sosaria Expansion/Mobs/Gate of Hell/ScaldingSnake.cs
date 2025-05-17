using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For spell effects if needed
using Server.Network; // For effects and particle systems
using System.Collections.Generic;
using Server.Spells.Fourth; // For using FireFieldItem, etc.

namespace Server.Mobiles
{
    [CorpseName("a scorched snake corpse")]
    public class ScaldingSnake : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextBreathTime;
        private DateTime m_NextSteamTime;
        private Point3D m_LastLocation;

        // Unique Hue for our fiery snake (adjust as desired)
        private const int UniqueHue = 0x497;

        [Constructable]
        public ScaldingSnake() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scalding Snake";
            Body = 52;               // Same body as IceSnake
            BaseSoundID = 0xDB;       // Same base sound as IceSnake
            Hue = UniqueHue;          // Unique firey hue

            // --- Advanced Stats ---
            SetStr(250, 300);
            SetDex(200, 250);
            SetInt(150, 180);

            SetHits(1000, 1200);
            SetStam(250, 300);
            SetMana(200, 250);

            SetDamage(15, 25);

            // Damage is mostly fire
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, -20, 0); // Very vulnerable to cold!
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills ---
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.1, 110.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // Initialize ability cooldowns
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextSteamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 7)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(2, 5)));
        }

        // --- Unique Ability: Lava Trail ---
        // When a mobile moves close to the snake, it leaves behind a trail of scalding steam.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                // For our effect we use a steam/ember-like particle effect.
                int effectID = 0x36BD; // (Example effect ID for steam/heat; adjust if needed)
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int damage = 3;

                // Create a field effect (reuse FireFieldItem to simulate the lava/steam trail)
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(effectID, oldLocation, this, this.Map, duration, damage);

                // Visual and sound effects for the trail
                m.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
                m.PlaySound(0x208);

                // Damage the mobile if within range (fire damage)
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(6, 12), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Override OnThink to manage special ability logic ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a steam trail if we have moved
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int effectID = 0x36BD;
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(effectID, oldLocation, this, this.Map, duration, damage);
            }

            // Check for Combatant existence and valid map conditions
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Use the pattern "if (Combatant is Mobile target)" before accessing Mobile-specific members.
            if (Combatant is Mobile target)
            {
                // --- Scalding Breath Attack (ranged cone attack) ---
                if (DateTime.UtcNow >= m_NextBreathTime && this.InRange(target.Location, 8))
                {
                    ScaldingBreathAttack();
                    m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
                // --- Steam Burst Attack (close-range AoE) ---
                else if (DateTime.UtcNow >= m_NextSteamTime && this.InRange(target.Location, 4))
                {
                    SteamBurstAttack();
                    m_NextSteamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }
            }
        }

        // --- Unique Ability: Scalding Breath Attack ---
        // A fiery cone attack that damages all targets within a 6-tile radius.
        public void ScaldingBreathAttack()
        {
            if (Map == null)
                return;

            // Play sound and display visual effects
            this.PlaySound(0x208);
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Head);

            // Find targets within a 6-tile radius (cone-like area—adjust filtering if desired)
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Send a visual burst effect from the snake outwards
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36BD, 10, 40, UniqueHue, 0, 5029, 0);
                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Deal pure fire damage
                    m.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Steam Burst Attack ---
        // A close-range AoE burst that calls down a series of scalding steam impacts.
        public void SteamBurstAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Boiling steam erupts!*");
            PlaySound(0x160);

            int burstCount = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < burstCount; i++)
            {
                // Randomize impact point near the target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-2, 2),
                    targetLocation.Y + Utility.RandomMinMax(-2, 2),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Send a moving particle effect (using a Flamestrike-style graphic as a proxy)
                Point3D startPoint = new Point3D(
                    impactPoint.X + Utility.RandomMinMax(-1, 1),
                    impactPoint.Y + Utility.RandomMinMax(-1, 1),
                    impactPoint.Z + 20);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                // Delay the damage to simulate impact timing
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration), 0x36BD, 10, 30, UniqueHue, 0, 2023, 0);
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(15, 25);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Death Explosion ---
        // When the Scalding Snake dies, it creates a burst of searing explosions and spawns HotLavaTiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int explosionCount = 6;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < explosionCount; i++)
            {
                int xOffset = Utility.RandomMinMax(-2, 2);
                int yOffset = Utility.RandomMinMax(-2, 2);
                if (xOffset == 0 && yOffset == 0 && i < explosionCount - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D explosionLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(explosionLocation.X, explosionLocation.Y, explosionLocation.Z, 16, false, false))
                {
                    explosionLocation.Z = Map.GetAverageZ(explosionLocation.X, explosionLocation.Y);
                    if (!Map.CanFit(explosionLocation.X, explosionLocation.Y, explosionLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(explosionLocation);

                // Spawn a HotLavaTile (assumed to be defined elsewhere) with the unique hue
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(explosionLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(explosionLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Create a large central explosion effect at one of the points
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
        public override int TreasureMapLevel { get { return 4; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Average, 1);
            if (Utility.RandomDouble() < 0.02)
            {
                // Drop a rare, fire-themed item (assumed to exist)
                PackItem(new MaxxiaScroll());
            }
        }

        public ScaldingSnake(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextSteamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}

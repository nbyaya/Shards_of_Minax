using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For any spell effects
using Server.Network;        // For particle and sound effects
using System.Collections.Generic; // For list support in AoE attacks
using Server.Spells.Fourth;  // If you use fire field effects

namespace Server.Mobiles
{
    [CorpseName("a charred, ghostly corpse")]
    public class ScaldingWraith : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextSteamNovaTime;
        private DateTime m_NextMagmaBurstTime;
        private Point3D m_LastLocation;

        // Unique fire-themed hue (change this value if desired)
        private const int UniqueHue = 0x483; // Example unique hue for a fiery appearance

        [Constructable]
        public ScaldingWraith() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4) // Reaction and range settings
        {
            Name = "a Scalding Wraith";
            Body = 26; // Same as the base wraith body
            BaseSoundID = 0x482; // Same base sound as a normal wraith
            Hue = UniqueHue;


            // --- Advanced Stats ---
            SetStr(500, 600);
            SetDex(250, 300);
            SetInt(300, 350);

            SetHits(1200, 1400); // Much higher health

            // Base damage is increased and split between physical and fire damage
            SetDamage(20, 25);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 70);

            // Resistances – extremely resistant to fire, moderately to physical, vulnerable to cold
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -10, 0);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.2, 130.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5;

            // Initialize cooldown timers (staggered a bit for variety)
            m_NextSteamNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMagmaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // Standard loot items (plus thematic reagents)
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));

            // Optionally, keep a light source for dramatic appearance
            AddItem(new LightSource());
        }

        // --- Unique Ability: Steam Aura on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // If a nearby mobile moves within 2 tiles, leave behind a scalding steam field
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                int itemID = 0x398C; // Use an appropriate field/particle graphic ID
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;

                // Create a fire field item that represents scalding steam
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and audio effect (using Flamestrike-like particles)
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Harm the mobile if valid
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(8, 15), 0, 100, 0, 0, 0); // Fire damage only
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Main Think Loop for Special Abilities ---
        public override void OnThink()
        {
            base.OnThink();

            // If the Scalding Wraith has moved, leave a lingering steam trail at its previous location
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C; // Particle effect for steam trail
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 2;
                var trail = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Do nothing if no combatant exists or map is invalid
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Check if Combatant is a Mobile before accessing its properties
            if (!(Combatant is Mobile target))
                return;

            // If it’s time to use Steam Nova and the target is close enough, execute it
            if (DateTime.UtcNow >= m_NextSteamNovaTime && this.InRange(target.Location, 8))
            {
                SteamNovaAttack();
                m_NextSteamNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            // Else, if it’s time to call down a Magma Burst and the target is within a wider range, execute it
            else if (DateTime.UtcNow >= m_NextMagmaBurstTime && this.InRange(target.Location, 12))
            {
                MagmaBurstAttack();
                m_NextMagmaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Unique Ability: Steam Nova Attack (AoE) ---
        public void SteamNovaAttack()
        {
            if (Map == null)
                return;

            // Play a sound and create a particle burst on self
            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6-tile radius

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Send out a visual effect from the wraith outward
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60); // AoE damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0); // Pure fire damage

                    // Extra visual effect on impacted targets
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Magma Burst Attack (Meteor-Like AoE) ---
        public void MagmaBurstAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Scalding downpour!*"); // Flavor text
            PlaySound(0x160);

            int burstCount = Utility.RandomMinMax(4, 7); // Number of bursts
            for (int i = 0; i < burstCount; i++)
            {
                // Slight randomization of impact area around the target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Simulate a meteor by sending particles from above
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay to match the visual impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 2023, 0);

                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1); // Blast radius of 1 tile
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(25, 35);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Additional Ability: Scalding Touch on Melee Attacks ---
        // This chance-based effect adds extra fire damage on a successful melee hit.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender != null && defender.Alive && Utility.RandomDouble() < 0.20) // 20% chance
            {
                // Display scalding effect on the defender
                defender.FixedParticles(0x3709, 10, 20, 5052, EffectLayer.Waist);
                defender.PlaySound(0x208);

                // Apply additional fire damage
                AOS.Damage(defender, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }
        }

        // --- Death Explosion: Release Scalding Steam and Molten Residue ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int residueTiles = 10; // Number of molten/steam tiles to generate
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < residueTiles; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                // Avoid exact overlap too frequently
                if (xOffset == 0 && yOffset == 0 && i < residueTiles - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D residuePoint = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(residuePoint.X, residuePoint.Y, residuePoint.Z, 16, false, false))
                {
                    residuePoint.Z = Map.GetAverageZ(residuePoint.X, residuePoint.Y);
                    if (!Map.CanFit(residuePoint.X, residuePoint.Y, residuePoint.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(residuePoint);

                // Spawn a HotLavaTile as a stand-in for a scalding residue patch
                HotLavaTile residue = new HotLavaTile(); // Ensure HotLavaTile is defined in your shard
                residue.Hue = UniqueHue;
                residue.MoveToWorld(residuePoint, this.Map);

                // Create a smaller explosion effect at this point
                Effects.SendLocationParticles(EffectItem.Create(residuePoint, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Create a larger central explosion effect at one of the generated points
            Point3D deathEffectLocation = this.Location;
            if (effectLocations.Count > 0)
                deathEffectLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathEffectLocation, this.Map, 0x218); // Explosion sound
            Effects.SendLocationParticles(EffectItem.Create(deathEffectLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } } // High-level treasure map

        // Increased difficulty to dispel this creature’s magical effects
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Rare drop chance for a unique item
            if (Utility.RandomDouble() < 0.01)
                PackItem(new InfernosEmbraceCloak()); // Replace with a custom item if desired
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        // --- Serialization ---
        public ScaldingWraith(Serial serial) : base(serial)
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

            // Re-initialize ability timers upon loading
            m_NextSteamNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMagmaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}

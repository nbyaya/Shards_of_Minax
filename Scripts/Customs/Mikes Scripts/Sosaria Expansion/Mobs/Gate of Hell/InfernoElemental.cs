using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Fourth; //Needed for firefield

namespace Server.Mobiles
{
    [CorpseName("a churning elemental core")] // More impressive corpse name
    public class InfernoElemental : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextMeteorTime;
		private Point3D m_LastLocation;

        // Unique Hue - Example: 1161 is a bright fiery orange/red. Adjust as desired.
        private const int UniqueHue = 1161;

        [Constructable]
        public InfernoElemental() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Faster reaction times
        {
            
			Name = "an Inferno Elemental";
            Body = 15; // Base Fire Elemental body
            BaseSoundID = 838; // Base Fire Elemental sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats ---
            SetStr(450, 550);
            SetDex(210, 260);
            SetInt(380, 450);

            SetHits(1200, 1500); // Much higher health pool
            SetStam(210, 260); // Increased stamina for movement/abilities
            SetMana(380, 450); // Increased mana pool for spells/abilities

            SetDamage(22, 28); // Higher base damage

            // Primarily Fire damage, minimal physical
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 85, 95); // Very high fire resist
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to Cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0); // Added for flavor/potential synergy

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75; // Higher passive defense
            ControlSlots = 5; // Boss-level creature, hard to control

            // Initialize ability cooldowns
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)); // Stagger initial use
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Stagger initial use

            // Standard loot + maybe something extra
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6))); // Added ingredient

            // Keep the light source
            AddItem(new LightSource());
			m_LastLocation = this.Location;

        }

		// --- Unique Ability: Aura of Flame ---
        // Leaves a fire field on the tile it moves OFF of.
		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			// 🔥 AURA EFFECT: Affect other mobiles moving near the elemental
			if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
			{
				// Leave a fire field at the mobile's old location
				int itemID = 0x398C;
				TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
				int damage = 2;

				var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

				// Hit effect: Flamestrike-style
				m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet); // Flamestrike effect
				m.PlaySound(0x208); // Fireball or explosion sound

				// Damage the mobile (fire only)
				if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
				{
					DoHarmful(m);
					AOS.Damage(m, this, Utility.RandomMinMax(8, 15), 0, 100, 0, 0, 0); // Fire-only
				}
			}

			base.OnMovement(m, oldLocation);
		}


        // --- Thinking Process for Special Attacks ---
		public override void OnThink()
		{
			base.OnThink();

			// Check if the elemental has moved
			if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
			{
				Point3D oldLocation = m_LastLocation;
				m_LastLocation = this.Location;

				int itemID = 0x398C;
				TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
				int damage = 2;

				var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
			}

			// Your normal logic for nova/meteor, etc.
			if (Combatant == null || Map == null || Map == Map.Internal)
				return;

			if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 8))
			{
				FlameNovaAttack();
				m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
			}
			else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(Combatant.Location, 12))
			{
				MeteorSwarmAttack();
				m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
			}
		}


        // --- Unique Ability: Flame Nova ---
        public void FlameNovaAttack()
        {
            if (Map == null) return;

            this.PlaySound(0x208); // Fireball sound or similar
            this.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Large fiery explosion effect on self

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

            foreach (Mobile m in eable)
            {
                // Check if the mobile can be harmed and is not the caster itself
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                 // Visually striking effect from caster outwards
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60); // Significant AoE damage
                    // Deal 100% fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Add a visual effect on the target
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head); // Fiery hit effect
                }
            }
        }


        // --- Unique Ability: Meteor Swarm ---
        public void MeteorSwarmAttack()
        {
            if (Combatant == null || Map == null) return;

            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target)) // Ensure target is valid and Mobile
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Incinerating rain!*"); // Optional flavor text
            PlaySound(0x160); // Meteor sound

            int meteorCount = Utility.RandomMinMax(4, 7); // Number of meteors
            for (int i = 0; i < meteorCount; i++)
            {
                // Target slightly randomized locations around the primary target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                // Ensure the impact point is valid on the map
                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue; // Skip if still invalid
                }


                // Visually send a meteor from the sky towards the impact point
                // (Using Flamestrike effect as a proxy for meteor impact)
                // Start high up
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, // Meteor graphic ID (adjust if needed)
                    5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);


                // Delay the damage slightly to match visual impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () => // Stagger impacts slightly
                {
                    if (this.Map == null) return; // Check map validity again inside timer

                    // Send impact visual effect
                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 2023, 0);

                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1); // Small blast radius per meteor
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(25, 35); // Damage per meteor
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Pure fire damage
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Death Explosion ---
        // Using the exact code you provided for spawning HotLavaTiles
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 10; // Number of lava tiles to drop.
            List<Point3D> effectLocations = new List<Point3D>(); // Store locations for main explosion

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3); // Increased spread
                int yOffset = Utility.RandomMinMax(-3, 3);
                // Ensure we don't spawn exactly on the death spot too often, maybe for main effect
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop -1) xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                // Try to find valid ground Z coordinate
                 if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                       continue; // Skip if we can't find a valid spot
                }

                effectLocations.Add(lavaLocation); // Add to list for effects later

                // Spawn the HotLavaTile
                HotLavaTile droppedLava = new HotLavaTile(); // Assuming HotLavaTile is defined elsewhere
                droppedLava.Hue = UniqueHue; // Match the elemental's hue
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                // Create a smaller flamestrike effect at each lava tile's location
                // Use a smaller effect ID maybe, or same with less duration/particles
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0); // Smaller/faster effect
            }

             // Big central explosion effect using collected points
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0) // Use average if points were generated
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218); // Large explosion sound
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion effect


            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } } // Higher level map

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Better base loot
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1); // Chance for scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8)); // More gems

            // Higher chance for the unique cloak or add another item
            if (Utility.RandomDouble() < 0.01) // 1 in 100 chance
            {
                PackItem(new InfernosEmbraceCloak()); // Keep existing special drop
            }
             if (Utility.RandomDouble() < 0.05) // 5% chance for another potential rare drop
            {
                 PackItem(new DaemonBone(Utility.RandomMinMax(3, 5))); // Example rare resource
            }
        }

        // --- Serialization ---
        public InfernoElemental(Serial serial) : base(serial)
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

            // Re-initialize cooldowns on load/restart
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
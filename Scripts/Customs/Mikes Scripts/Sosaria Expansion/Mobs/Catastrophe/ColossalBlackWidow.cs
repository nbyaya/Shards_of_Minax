using System;
using System.Collections.Generic;

using Server.Items;
using Server.Network;
using Server.Spells; // Useful for effects and debuffs

namespace Server.Mobiles
{
    [CorpseName("a colossal black widow corpse")] // Updated corpse name
    public class ColossalBlackWidow : BaseCreature
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public ColossalBlackWidow() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4) // Strong Melee AI
        {
            this.Name = "a Colossal Black Widow"; // Updated name
            this.Body = 0x9D; // Keeping the original body ID
            this.BaseSoundID = 0x388; // Keeping the original sound ID
            this.Hue = 2406; // A unique, dark shadowy purple hue

            // --- Significantly Boosted Stats ---
            this.SetStr(355, 450);
            this.SetDex(155, 200); // Higher Dex for speed/accuracy
            this.SetInt(105, 150); // Cunning predator

            this.SetHits(550, 625); // Significantly more health
            this.Mana = 300; // Give it some mana for abilities if needed later, though focusing on physical/poison

            this.SetDamage(18, 28); // Increased base damage

            // --- Damage Types ---
            this.SetDamageType(ResistanceType.Physical, 50); // Primarily physical
            this.SetDamageType(ResistanceType.Poison, 50);   // Significant poison component

            // --- Resistances ---
            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 25, 35);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 70, 80); // Very high poison resist
            this.SetResistance(ResistanceType.Energy, 30, 40);

            // --- Skills ---
            this.SetSkill(SkillName.Anatomy, 90.1, 105.0);
            this.SetSkill(SkillName.Poisoning, 100.1, 120.0); // Master poisoner
            this.SetSkill(SkillName.MagicResist, 85.1, 100.0);
            this.SetSkill(SkillName.Tactics, 95.1, 110.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 115.0); // Strong grapple

            // --- Fame & Karma ---
            this.Fame = 15000; // High fame
            this.Karma = -15000; // High negative karma

            // --- Other Properties ---
            this.VirtualArmor = 50; // Decent innate armor
            this.Tamable = false;
            this.ControlSlots = 5; // Would take up significant control slots if tamable was true

            // --- Loot ---
            this.PackItem(new SpidersSilk(Utility.RandomMinMax(15, 25))); // More silk
            this.PackItem(new DeadlyPoisonPotion()); // Better poison
            this.PackItem(new DeadlyPoisonPotion());

            m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
        }

        // --- Base Overrides ---
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } } // Immune to deadly poison
        public override Poison HitPoison { get { return Poison.Deadly; } } // Applies deadly poison on hit
        public override int TreasureMapLevel { get { return 5; } } // High-level maps
        public override bool AlwaysMurderer { get { return true; } }
        public override bool Unprovokable { get { return true; } } // Cannot be provoked
        public override bool BardImmune { get { return false; } } // Can potentially be discorded/peace'd (add challenge)
        public override bool CanFlee { get { return false; } } // Doesn't flee

        public ColossalBlackWidow(Serial serial) : base(serial)
        {
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich, 2); // Generous standard loot
            this.AddLoot(LootPack.FilthyRich, 1);
            this.AddLoot(LootPack.Gems, 6);

            // Chance for a unique item (replace with actual item types)
            if (0.02 > Utility.RandomDouble()) // 2% chance for a special drop
            {
                // Example unique items - replace with actual classes later
                switch (Utility.Random(3))
                {
                    case 0: PackItem(new DaemonBone(20)); break; // Placeholder unique resource
                    case 1: PackItem(new Leather(100)); break; // Placeholder unique gear
                    case 2: PackItem(new Gold(5000)); break; // Placeholder valuable item
                }
                // Ideally, use the Unique/Shared SAList approach if your shard supports it fully
                // Example: PackItem( new VenomfangDagger() );
            }
        }

        // --- Unique Ability Triggering ---
        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextAbilityTime && Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive && !target.Deleted && CanBeHarmful(target) && InRange(target, 12) && InLOS(target))
                {
                    ChooseAndUseAbility(target);
                }
                 // Reset timer regardless of successful cast to prevent spamming checks
                m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
            }
        }

		private void ChooseAndUseAbility(Mobile target)
		{
			switch (Utility.Random(4)) // Choose one of 4 abilities
			{
				case 0:
					DoWebBarrage(); // AOE Snare nearby targets
					break;
				case 1:
					DoVenomousSpit(target); // Ranged single target poison attack
					break;
				case 2:
					int nearbyCount = 0;
					foreach (Mobile m in GetMobilesInRange(8))
					{
						if (m != this && m.Alive && CanBeHarmful(m) && InLOS(m))
							nearbyCount++;
					}

					if (nearbyCount > 2) // Only use if multiple enemies are close
						DoToxicCloud(); // AOE poison cloud
					else
						DoCocoonTarget(target); // Fallback to Cocoon if cloud is not ideal
					break;
				case 3:
					DoCocoonTarget(target); // Single target paralyze/immobilize
					break;
			}
		}


        // --- Melee Attack Modifiers ---
		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			// Add Enervating Bite effect (Stamina Drain)
			if (0.25 > Utility.RandomDouble()) // 25% chance on hit
			{
				if (defender is Mobile target)
				{
					target.SendMessage("The widow's fangs drain your energy!");
					target.PlaySound(0x133);
					int staminaDrain = Utility.RandomMinMax(20, 40);
					target.Stam = Math.Max(0, target.Stam - staminaDrain);
					target.FixedParticles(0x374A, 10, 15, 5013, 1153, 2, EffectLayer.Waist);
				}
			}

			// Chance to apply a Web debuff (slow) on hit
			if (0.15 > Utility.RandomDouble()) // 15% chance on hit
			{
				if (defender is Mobile target && !target.Paralyzed && !target.Frozen)
				{
					if (WebbingEffect(target)) // Apply the slow effect
					{
						target.SendMessage("You are momentarily caught in sticky webbing!");
						target.PlaySound(0x108); // Sound of web
					}
				}
			}
		}


        // --- Reaction Ability ---
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // Low chance to spawn a spiderling when hit
            if (0.05 > Utility.RandomDouble() && this.Hits < (this.HitsMax * 0.5)) // 5% chance when below 50% health
            {
                SpawnSpiderling(attacker);
            }
        }

        // --- Ability Implementations ---

        #region Web Barrage (AOE Snare)
        public void DoWebBarrage()
        {
            this.PlaySound(0x108); // Web sound

            // Visual effect originating from the spider
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2007, 0); // Web-like explosion

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile mob in this.GetMobilesInRange(8)) // Affect targets within 8 tiles
            {
                if (mob != this && CanBeHarmful(mob) && mob.Alive && InLOS(mob))
                    targets.Add(mob);
            }

            if (targets.Count > 0)
            {
                this.Say("*Hisses menacingly as webs erupt outwards!*"); // Flavor text

                foreach (Mobile m in targets)
                {
                    if (m is Mobile target) // Safety check
                    {
                        DoHarmful(target);
                        target.SendMessage("Sticky webs cling to you, slowing your movements!");
                        // Apply a short, potent slow effect
                        WebbingEffect(target, TimeSpan.FromSeconds(Utility.RandomMinMax(4, 7)));

                        // Optional minor physical damage
                        //AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0);

                        // Visual effect on target
                        target.FixedParticles(0x376A, 9, 32, 5031, EffectLayer.Waist);
                    }
                }
            }

            targets.Clear();
        }
        #endregion

        #region Webbing Effect Helper
        // Helper to apply a temporary movement/swing speed slow
        private static Dictionary<Mobile, Timer> m_Webbed = new Dictionary<Mobile, Timer>();

        public bool WebbingEffect(Mobile target, TimeSpan duration)
        {
             if (target == null || m_Webbed.ContainsKey(target))
                 return false;

            // Apply debuff (example: reduce swing speed and movement speed)
            // Note: Actual implementation depends heavily on custom shard systems for debuffs.
            // This is a conceptual example. You might need specific methods like:
            // StatMod mod = new StatMod(StatType.Dex, "WebDebuff", -20, duration);
            // target.AddStatMod(mod);
            // TargetSpeedManager.AddSlow(target, "WebDebuff", 0.5, duration); // Example custom speed system

             // Simple approach: Use temporary freeze/paralyze for simplicity if complex debuffs aren't available
             target.Frozen = true; // Freeze movement
             Timer webTimer = new WebbingTimer(target, duration);
             webTimer.Start();
             m_Webbed.Add(target, webTimer);

            return true;
        }

         public bool WebbingEffect(Mobile target)
         {
            return WebbingEffect(target, TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4))); // Shorter duration for the melee proc
         }

        private class WebbingTimer : Timer
        {
            private Mobile m_Target;
            public WebbingTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                // Remove debuff
                 // Note: Need to correctly remove the applied debuff (StatMod, custom system effect, etc.)
                 // Example: target.RemoveStatMod("WebDebuff");
                 // TargetSpeedManager.RemoveSlow(target, "WebDebuff");
                 m_Target.Frozen = false; // Unfreeze

                 if (m_Webbed.ContainsKey(m_Target))
                 {
                    m_Webbed.Remove(m_Target);
                 }

                 m_Target.SendMessage("The sticky webbing dissolves.");
            }
        }
        #endregion


        #region Venomous Spit (Ranged Poison)
        public void DoVenomousSpit(Mobile target)
        {
            if (target == null || !CanBeHarmful(target)) return;

            this.MovingParticles(target, 0x36E4, 7, 0, false, true, 1175, 0, 9502, 4019, 0x163, 0); // Greenish spit effect
            this.PlaySound(0xDD); // Spit sound?

            Timer.DelayCall(TimeSpan.FromSeconds(0.8), new TimerStateCallback(VenomousSpit_Landed), new object[] { target, this });
        }

        private void VenomousSpit_Landed(object state)
        {
             object[] states = (object[])state;
             Mobile target = (Mobile)states[0];
             Mobile source = (Mobile)states[1];

             if (target is Mobile victim && source is Mobile attacker && attacker.CanBeHarmful(victim)) // Safety checks
             {
                attacker.DoHarmful(victim);
                victim.SendMessage("Searing venom burns your skin!");

                // Apply a high-level poison
                victim.ApplyPoison(attacker, Poison.Deadly); // Apply deadly poison directly

                // Optional: Add some direct poison damage on impact
                // AOS.Damage(victim, attacker, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);

                victim.FixedParticles(0x374A, 1, 15, 9900, 1175, 3, EffectLayer.Head); // Green splash effect
             }
        }
        #endregion

        #region Toxic Cloud (AOE Damage)
        public void DoToxicCloud()
        {
            this.PlaySound(0x231); // Gas cloud sound
            this.Say("*Hisses, releasing a cloud of choking venom!*");

            List<Point3D> cloudLocations = new List<Point3D>();
            cloudLocations.Add(this.Location); // Center on self

            // Add surrounding points for a larger cloud
            for (int i = 0; i < 8; ++i)
            {
                 int x = 0, y = 0;
                 Movement.Movement.Offset((Direction)i, ref x, ref y);
                 Point3D p = new Point3D(this.X + x, this.Y + y, this.Z);
                 if (Map.CanFit(p, 16, false, false))
                    cloudLocations.Add(p);
            }


            foreach (Point3D p in cloudLocations)
            {
                // Create temporary field items or just use effects
                Effects.SendLocationParticles(EffectItem.Create(p, this.Map, TimeSpan.FromSeconds(10.0)), 0x3709, 10, 30, 1175, 0, 5029, 0); // Poison field effect
                 // You could also spawn invisible items that pulse damage like the fire field example
                 // For simplicity, we'll use a timer to pulse damage in the area.
            }

             // Start a timer to pulse damage in the area
             Timer cloudTimer = new ToxicCloudTimer(this, this.Map, new Rectangle2D(this.X - 1, this.Y - 1, 3, 3), 5, TimeSpan.FromSeconds(2.0));
             cloudTimer.Start();

             cloudLocations.Clear();
        }

        private class ToxicCloudTimer : Timer
        {
            private Mobile m_Source;
            private Map m_Map;
            private Rectangle2D m_Area;
            private int m_Ticks;
            private int m_MaxTicks;
            private int m_Damage;

            public ToxicCloudTimer(Mobile source, Map map, Rectangle2D area, int maxTicks, TimeSpan interval)
                : base(interval, interval)
            {
                m_Source = source;
                m_Map = map;
                m_Area = area;
                m_MaxTicks = maxTicks;
                m_Damage = Utility.RandomMinMax(8, 12); // Damage per tick
                m_Ticks = 0;
            }

            protected override void OnTick()
            {
                if (m_Source == null || m_Source.Deleted || m_Map == null || m_Map == Map.Internal)
                {
                    Stop();
                    return;
                }

                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = m_Map.GetMobilesInBounds(m_Area);
                foreach (Mobile m in eable)
                {
                    if (m != m_Source && m_Source.CanBeHarmful(m) && m.Alive)
                        targets.Add(m);
                }
                eable.Free();

                foreach (Mobile m in targets)
                {
                    if (m is Mobile target) // Safety check
                    {
                        m_Source.DoHarmful(target);
                        AOS.Damage(target, m_Source, m_Damage, 0, 0, 0, 100, 0); // Poison damage
                        target.SendMessage("You choke on the toxic fumes!");
                        target.FixedParticles(0x374A, 1, 15, 5029, 1175, 0, EffectLayer.Waist); // Small poison effect
                    }
                }

                targets.Clear();

                m_Ticks++;
                if (m_Ticks >= m_MaxTicks)
                {
                    Stop();
                }
            }
        }
        #endregion

        #region Cocoon Target (Paralyze)
        public void DoCocoonTarget(Mobile target)
        {
             if (target == null || !CanBeHarmful(target) || target.Paralyzed || target.Frozen) // Don't cocoon already paralyzed targets
                return;

            this.Say(String.Format("*Shoots thick webbing, ensnaring {0}!*", target.Name));
            this.PlaySound(0x5D5); // Cocoon sound?

            // Visual effect: Webbing flying towards target
            this.MovingParticles(target, 0x376A, 15, 0, false, false, 2007, 0, 9502, 1, 0, EffectLayer.Head, 0);

            DoHarmful(target);
            target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(4, 6))); // Paralyze for 4-6 seconds
            target.SendMessage("You are completely encased in thick, sticky webbing!");

            // Add a persistent visual effect while cocooned (optional)
            // Could use a temporary item attachment or just the initial paralyze
            target.FixedParticles(0x376A, 9, 32, 5031, 2007, 0, EffectLayer.Waist); // Web effect on player
        }
        #endregion

        #region Spawn Spiderling
        public void SpawnSpiderling(Mobile target)
        {
            Map map = this.Map;
            if (map == null)
                return;

            this.Say("*A small, skittering horror emerges!*");
            this.PlaySound(0x388); // Spider sound

            // Try to find a valid location near the Colossal Widow
            Point3D spawnLoc = Point3D.Zero;
            for (int i = 0; i < 10; ++i)
            {
                int x = this.X + Utility.RandomMinMax(-1, 1);
                int y = this.Y + Utility.RandomMinMax(-1, 1);
                int z = map.GetAverageZ(x, y);

                if (map.CanSpawnMobile(new Point2D(x, y), z))
                {
                    spawnLoc = new Point3D(x, y, z);
                    break;
                }
            }

            if (spawnLoc == Point3D.Zero) // Fallback to own location if needed
                spawnLoc = this.Location;

            BaseCreature spiderling = new GiantBlackWidow(); // Spawn the weaker base version
            // --- Customize the spiderling slightly ---
            spiderling.Name = "Colossal Spiderling";
            spiderling.Hue = this.Hue; // Match parent's hue
            spiderling.Team = this.Team; // Ensure it doesn't attack the parent
            spiderling.FightMode = FightMode.Closest;
            // Optionally slightly buff/nerf its stats from the base GiantBlackWidow if desired
            // spiderling.SetHits(30, 45);

            spiderling.MoveToWorld(spawnLoc, map);
            if (target != null && target.Alive)
                spiderling.Combatant = target; // Make it attack the target that hit the parent

            Effects.SendLocationParticles(EffectItem.Create(spiderling.Location, spiderling.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023, 0); // Spawn visual
        }
        #endregion

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timer on load
            m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
        }
    }
}
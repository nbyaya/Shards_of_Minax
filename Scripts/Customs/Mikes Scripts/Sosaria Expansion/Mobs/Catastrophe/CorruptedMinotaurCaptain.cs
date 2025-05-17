using System;
using System.Collections.Generic; // Needed for List
using Server;
using Server.Items;
using Server.Network; // Needed for PublicOverheadMessage

namespace Server.Mobiles
{
    [CorpseName("a corrupted minotaur corpse")]
    public class CorruptedMinotaurCaptain : BaseCreature
    {
        private DateTime _NextGazeAllowed;
        private DateTime _NextStompAllowed;

        [Constructable]
        public CorruptedMinotaurCaptain()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Faster reaction times
        {
            Name = "a corrupted minotaur captain";
            Body = 280; // Minotaur body
            Hue = 1172; // A dark, corrupted purple hue


            // --- Enhanced Stats ---
            SetStr(680, 780);
            SetDex(130, 155);
            SetInt(110, 140);

            SetHits(950, 1150); // Significantly tougher

            SetDamage(28, 38); // Higher base damage

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30); // Adds some poison damage naturally

            // --- Enhanced Resistances ---
            SetResistance(ResistanceType.Physical, 75, 85); // Very high physical resist
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80); // High poison resist due to corruption
            SetResistance(ResistanceType.Energy, 55, 65);

            // --- Enhanced Skills ---
            SetSkill(SkillName.MagicResist, 105.1, 120.0); // Strong magic defense
            SetSkill(SkillName.Tactics, 115.1, 125.0);    // Master tactician
            SetSkill(SkillName.Wrestling, 115.1, 125.0);   // Master grappler
            SetSkill(SkillName.Anatomy, 100.1, 115.0);   // Knows weaknesses
            SetSkill(SkillName.DetectHidden, 80.0, 95.0); // Can detect hidden players nearby

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75; // Significantly higher armor rating

            // --- Abilities ---
            SetWeaponAbility(WeaponAbility.CrushingBlow); // Powerful single hits
            // SetWeaponAbility(WeaponAbility.ParalyzingBlow); // Could keep this too, or replace

            _NextGazeAllowed = DateTime.UtcNow;
            _NextStompAllowed = DateTime.UtcNow;
        }

        public CorruptedMinotaurCaptain(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } } // Higher level map
        public override int Meat { get { return 3; } } // More resources

        // --- Sound Overrides (Using Tormented Minotaur sounds as requested) ---
        public override int GetAngerSound() { return 0x597; }
        public override int GetIdleSound() { return 0x596; }
        public override int GetAttackSound() { return 0x599; }
        public override int GetHurtSound() { return 0x59a; }
        public override int GetDeathSound() { return 0x59c; }

        // --- Special Abilities Logic ---

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            // Check if combatant exists and is a Mobile before trying abilities
            IDamageable combatant = Combatant;
            if (combatant == null || combatant.Deleted || !combatant.Alive || combatant.Map != this.Map || !InRange(combatant.Location, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            // Corrupting Gaze Ability
            if (DateTime.UtcNow >= _NextGazeAllowed && 0.2 > Utility.RandomDouble()) // 20% chance if cooldown is ready
            {
                if (combatant is Mobile target) // Check if the combatant is specifically a Mobile
                {
                    CorruptingGaze(target);
                    _NextGazeAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Cooldown 15-25s
                }
            }

            // Ground Stomp Ability (especially when surrounded or low health)
			if (DateTime.UtcNow >= _NextStompAllowed && (CountMobilesInRange(2) > 2 || Hits < (HitsMax * 0.4)) && 0.3 > Utility.RandomDouble())
            {
                 GroundStomp();
                 _NextStompAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30)); // Cooldown 20-30s
            }
        }

		private int CountMobilesInRange(int range)
		{
			int count = 0;

			foreach (Mobile m in GetMobilesInRange(range))
			{
				if (m != this && m.Alive && CanBeHarmful(m))
					count++;
			}

			return count;
		}


        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Frenzied Charge / Powerful Blow effect
            if (0.25 > Utility.RandomDouble()) // 25% chance on melee hit
            {
                FrenziedBlow(defender);
            }

            // Chance to inflict potent poison on hit
             if (0.15 > Utility.RandomDouble()) // 15% chance on melee hit
             {
                if (defender is Mobile target) // Check if the defender is specifically a Mobile
                {
                    target.ApplyPoison(this, Poison.Deadly); // Apply Deadly poison
                    target.SendLocalizedMessage(1070734); // You feel yourself weakening! (or similar poison message)
                    target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Poison effect visual
                }
             }
        }

        // --- Ability Implementations ---

        public void CorruptingGaze(Mobile target)
        {
            if (!CanBeHarmful(target)) return;

            DoHarmful(target);
            this.MovingParticles(target, 0x375A, 5, 0, false, true, 1172, 0, 9502, 6014, 0x11D, EffectLayer.Head, 0); // Purple bolt visual
            target.PlaySound(0x1FB); // Magic curse sound

            target.SendMessage("The captain's corrupting gaze weakens your resolve!");

            // Effect: Reduce resistances temporarily and drain mana/stamina
            int reduction = Utility.RandomMinMax(15, 25); // Reduce resists by 15-25
            TimeSpan duration = TimeSpan.FromSeconds(10);

            ResistanceMod[] mods = new ResistanceMod[5];
            mods[0] = new ResistanceMod(ResistanceType.Physical, -reduction);
            mods[1] = new ResistanceMod(ResistanceType.Fire, -reduction);
            mods[2] = new ResistanceMod(ResistanceType.Cold, -reduction);
            mods[3] = new ResistanceMod(ResistanceType.Poison, -reduction);
            mods[4] = new ResistanceMod(ResistanceType.Energy, -reduction);

            for (int i = 0; i < mods.Length; i++)
                target.AddResistanceMod(mods[i]);

            // Drain Mana and Stamina
            int drain = Utility.RandomMinMax(30, 50);
            target.Mana = Math.Max(0, target.Mana - drain);
            target.Stam = Math.Max(0, target.Stam - drain);
            target.SendMessage("Your energy feels drained!");

            // Timer to remove the resistance reduction
            Timer.DelayCall(duration, () =>
            {
                for (int i = 0; i < mods.Length; i++)
                    target.RemoveResistanceMod(mods[i]);
                target.SendMessage("You feel your defenses returning to normal.");
            });
        }

         public void GroundStomp()
         {
            if (Map == null || Map == Map.Internal) return;

            PublicOverheadMessage(MessageType.Regular, Hue, false, "*STOMP*"); // Overhead message
            PlaySound(0x207); // Stomp sound

            // Visual effect at the creature's location
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3735, // Ground impact effect
                10, 30, Hue, 0); // Adjust particle count/speed/hue

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile mob in GetMobilesInRange(4)) // Affect mobiles within 4 tiles
            {
                if (mob != this && CanBeHarmful(mob) && mob.AccessLevel == AccessLevel.Player) // Only target players for stomp effects maybe? Adjust if needed.
                    targets.Add(mob);
            }

            if (targets.Count > 0)
            {
                foreach (Mobile m in targets)
                {
                     if (m is Mobile targetMobile) // Check again inside loop for safety
                     {
                        DoHarmful(targetMobile);
                        int damage = Utility.RandomMinMax(30, 50); // AoE damage
                        AOS.Damage(targetMobile, this, damage, 100, 0, 0, 0, 0); // Physical damage

                        // Chance to briefly root/freeze targets
                        if (0.5 > Utility.RandomDouble()) // 50% chance to freeze
                        {
                            targetMobile.Freeze(TimeSpan.FromSeconds(2.0));
                            targetMobile.SendMessage("You are knocked off balance by the impact!");
                        }
                     }
                }
            }
         }

        public void FrenziedBlow(Mobile defender)
        {
            if (!CanBeHarmful(defender)) return;

            // Check if defender is Mobile before accessing Mobile-specific properties
            if (defender is Mobile target)
            {
                DoHarmful(target);
                PlaySound(GetAttackSound()); // Use its normal attack sound for the blow
                target.SendMessage("The minotaur captain strikes with frenzied force!");

                // Deal bonus direct damage (ignores armor to some extent, typical of Crushing Blow style)
                int bonusDamage = Utility.RandomMinMax(25, 40);
                target.Damage(bonusDamage, this); // Direct damage

                // Short stun effect
                target.Freeze(TimeSpan.FromSeconds(1.5));
                target.SendLocalizedMessage(1004014); // You have been stunned!
            }
            else // Handle case where defender is not a Mobile (e.g., a destructible item)
            {
                // Just apply regular damage if it's not a Mobile that can be stunned/messaged
                AOS.Damage(defender, this, Utility.Random(25, 40), 100, 0, 0, 0, 0);
            }
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Generous standard loot

            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1,2)); // Scrolls
            AddLoot(LootPack.Potions, Utility.RandomMinMax(1,3)); // Potions

            // Increased chance for the unique helm
            if (Utility.RandomDouble() < 0.01) // 1 in 100 chance
            {
                PackItem(new HornedCrownOfTheLabyrinth()); // Keep the unique drop
            }
            // Add chance for high-end gems or resources
             if (Utility.RandomDouble() < 0.05) // 5% chance
             {
                 PackItem(new Diamond( Utility.RandomMinMax( 2, 5 ) ) ); // Pack 2-5 diamonds
             }
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

            // Reset cooldowns on server restart/load
             _NextGazeAllowed = DateTime.UtcNow;
             _NextStompAllowed = DateTime.UtcNow;
        }
    }

    // Ensure the HornedCrownOfTheLabyrinth item exists or adjust the drop
    // Example placeholder if it doesn't exist:
    /*
    public class HornedCrownOfTheLabyrinth : PlateHelm
    {
        public override int LabelNumber { get { return 1070823; } } // Horned Crown of the Labyrinth
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public HornedCrownOfTheLabyrinth()
        {
            Hue = 1174; // A fitting hue
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 5;
            Attributes.RegenHits = 2;
            Attributes.AttackChance = 10;
            ArmorAttributes.SelfRepair = 3;
        }

        public HornedCrownOfTheLabyrinth(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
    */
}